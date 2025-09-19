using Core.IRepositories;
using Core.IServices;
using Core.Models;
using Core.Models.Enums;
using DataAccessLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class InventoryService : IInventoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        public InventoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region Products services
        public Task<Product> AddProductAsync(Product product, string userId) { 
            if(product == null)
                throw new ArgumentNullException(nameof(product));
            
            _unitOfWork.Products.AddAsync(product);
            _unitOfWork.SaveChangesAsync();
            return Task.FromResult(product);
        }
        public Task<Product> UpdateProductAsync(Product product, string userId)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            var existingProduct = _unitOfWork.Products.GetByIdAsync(product.Id).Result;
            if (existingProduct == null)
                throw new KeyNotFoundException($"Product with ID {product.Id} not found.");

            _unitOfWork.Products.Update(product);
            _unitOfWork.SaveChangesAsync();
            return Task.FromResult(product);
        }
        public Task<bool> DeleteProductAsync(int productId, string userId)
        {
            var existingProduct = _unitOfWork.Products.GetByIdAsync(productId).Result;
            if (existingProduct == null)
                throw new KeyNotFoundException($"Product with ID {productId} not found.");
            _unitOfWork.Products.Delete(existingProduct);
            _unitOfWork.SaveChangesAsync();
            return Task.FromResult(true);
        }
        public Task<Product> GetProductByIdAsync(int id)
        {
            var product = _unitOfWork.Products.GetByIdAsync(id).Result;
            if (product == null)
                throw new KeyNotFoundException($"Product with ID {id} not found.");
            return Task.FromResult(product);
        }
        public Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            var products = _unitOfWork.Products.ListAsync().Result;
            return Task.FromResult(products);
        }
        public Task<Product> GetProductBySKUAsync(string sku)
        {
            if (string.IsNullOrWhiteSpace(sku))
                throw new ArgumentException("SKU cannot be null or empty.", nameof(sku));
            var product = _unitOfWork.Products.GetBySKUAsync(sku).Result;
            if (product == null)
                throw new KeyNotFoundException($"Product with SKU {sku} not found.");
            return Task.FromResult(product);
        }
        public Task<IEnumerable<Product>> GetLowStockProductsAsync(int threshold)
        {
            var products = _unitOfWork.Products.GetLowStockAsync(threshold).Result;
            return Task.FromResult(products);
        }
        public Task<IEnumerable<Product>> GetOutOfStockProductsAsync()
        {
            var products = _unitOfWork.Products.GetOutOfStockAsync().Result;
            return Task.FromResult(products);
        }
        public Task<IEnumerable<Product>> GetBestSellingProductsAsync(DateTime from, DateTime to, int topN)
        {
            if (from > to)
                throw new ArgumentException("The 'from' date must be earlier than or equal to the 'to' date.");
            if (to > DateTime.Now)
                throw new ArgumentException("The 'to' date cannot be in the future.");
            var products = _unitOfWork.Products.GetBestSellingAsync(from, to, topN).Result;
            return Task.FromResult(products);
        }
        #endregion

        #region Purchase services
        public Task<Purchase> AddPurchaseAsync(Purchase purchase, string userId)
        {
            if (purchase == null)
                throw new ArgumentNullException(nameof(purchase));
            _unitOfWork.Purchases.AddAsync(purchase);
            _unitOfWork.SaveChangesAsync();
            return Task.FromResult(purchase);
        }
        public Task<Purchase> UpdatePurchaseAsync(Purchase purchase, string userId)
        {
            if (purchase == null)
                throw new ArgumentNullException(nameof(purchase));
            var existingPurchase = _unitOfWork.Purchases.GetByIdAsync(purchase.Id).Result;
            if (existingPurchase == null)
                throw new KeyNotFoundException($"Purchase with ID {purchase.Id} not found.");
            _unitOfWork.Purchases.Update(purchase);
            _unitOfWork.SaveChangesAsync();
            return Task.FromResult(purchase);
        }
        public Task<bool> DeletePurchaseAsync(int purchaseId, string userId)
        {
            var existingPurchase = _unitOfWork.Purchases.GetByIdAsync(purchaseId).Result;
            if (existingPurchase == null)
                throw new KeyNotFoundException($"Purchase with ID {purchaseId} not found.");
            _unitOfWork.Purchases.Delete(existingPurchase);
            _unitOfWork.SaveChangesAsync();
            return Task.FromResult(true);
        }
        public Task<IEnumerable<Purchase>> GetAllPurchasesAsync()
        {
            var purchases = _unitOfWork.Purchases.ListAsync().Result;
            return Task.FromResult(purchases);
        }
        public Task<Purchase> GetPurchaseByIdAsync(int id)
        {
            var purchase = _unitOfWork.Purchases.GetByIdAsync(id).Result;
            if (purchase == null)
                throw new KeyNotFoundException($"Purchase with ID {id} not found.");
            return Task.FromResult(purchase);
        }
        public Task<Purchase> GetPurchaseByIdWithItemsAsync(int id)
        {
            var purchase = _unitOfWork.Purchases.GetByIdWithItemsAsync(id).Result;
            if (purchase == null)
                throw new KeyNotFoundException($"Purchase with ID {id} not found.");
            return Task.FromResult(purchase);
        }
        public async Task<Purchase> AddPurchaseAsync(Purchase purchase, IEnumerable<PurchaseItem> items, Guid userId)
        {
            using var transaction = await (_unitOfWork as UnitOfWork)?._context.Database.BeginTransactionAsync();
            try
            {
                if (purchase == null)
                    throw new ArgumentNullException(nameof(purchase));
                if (items == null || !items.Any())
                    throw new ArgumentException("Purchase must have at least one item.", nameof(items));

                purchase.CreatedById = userId;
                purchase.TotalAmount = items.Sum(i => i.LineTotal);

                await _unitOfWork.Purchases.AddPurchaseAsync(purchase, items);
                foreach (var item in items)
                {
                    var product = await _unitOfWork.Products.GetByIdAsync(item.ProductId);
                    if (product == null) throw new Exception($"Product {item.ProductId} not found");

                    int oldQty = product.QuantityOnHand;
                    product.QuantityOnHand += item.Quantity;

                    _unitOfWork.Products.Update(product);

                    var transactionLog = new InventoryTransaction
                    {
                        ProductId = product.Id,
                        ChangeQuantity = item.Quantity,
                        TransactionType = TransactionType.Purchase,
                        ReferenceId = purchase.Id,
                        PreviousQuantity = oldQty,
                        NewQuantity = product.QuantityOnHand,
                        PerformedById = userId,
                        Notes = $"Purchase {purchase.InvoiceNumber}"
                    };

                    await _unitOfWork.InventoryTransactions.AddAsync(transactionLog);
                }

                await _unitOfWork.SaveChangesAsync();
                await transaction.CommitAsync();
                return purchase;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        public Task<IEnumerable<Purchase>> GetPurchasesByDateRangeAsync(DateTime from, DateTime to)
        {
            if (from > to)
                throw new ArgumentException("The 'from' date must be earlier than or equal to the 'to' date.");
            if (to > DateTime.Now)
                throw new ArgumentException("The 'to' date cannot be in the future.");
            var purchases = _unitOfWork.Purchases.GetByDateRangeAsync(from, to).Result;
            return Task.FromResult(purchases);
        }
        #endregion

        #region Sales services
        public Task<Sale> AddSaleAsync(Sale sale, string userId)
        {
            if (sale == null)
                throw new ArgumentNullException(nameof(sale));
            _unitOfWork.Sales.AddAsync(sale);
            _unitOfWork.SaveChangesAsync();
            return Task.FromResult(sale);
        }
        public Task<Sale> UpdatePurchaseAsync(Sale sale, string userId)
        {
            if (sale == null)
                throw new ArgumentNullException(nameof(sale));
            var existingSale = _unitOfWork.Sales.GetByIdAsync(sale.Id).Result;
            if (existingSale == null)
                throw new KeyNotFoundException($"Sale with ID {sale.Id} not found.");
            _unitOfWork.Sales.Update(sale);
            _unitOfWork.SaveChangesAsync();
            return Task.FromResult(sale);
        }
        public Task<bool> DeleteSaleAsync(int saleId, string userId)
        {
            var existingSale = _unitOfWork.Sales.GetByIdAsync(saleId).Result;
            if (existingSale == null)
                throw new KeyNotFoundException($"Sale with ID {saleId} not found.");
            _unitOfWork.Sales.Delete(existingSale);
            _unitOfWork.SaveChangesAsync();
            return Task.FromResult(true);
        }
        public Task<IEnumerable<Sale>> GetAllSalesAsync()
        {
            var sales = _unitOfWork.Sales.ListAsync().Result;
            return Task.FromResult(sales);
        }
        public Task<Sale> GetSaleByIdAsync(int id)
        {
            var sale = _unitOfWork.Sales.GetByIdAsync(id).Result;
            if (sale == null)
                throw new KeyNotFoundException($"Sale with ID {id} not found.");
            return Task.FromResult(sale);
        }
        public Task<Sale> GetSaleByIdWithItemsAsync(int id)
        {
            var sale = _unitOfWork.Sales.GetByIdWithItemsAsync(id).Result;
            if (sale == null)
                throw new KeyNotFoundException($"Sale with ID {id} not found.");
            return Task.FromResult(sale);
        }
        public async Task<Sale> AddSaleAsync(Sale sale, IEnumerable<SaleItem> items, Guid userId)
        {
            using var transaction = await(_unitOfWork as UnitOfWork)?._context.Database.BeginTransactionAsync();
            try
            {
                if (sale == null)
                    throw new ArgumentNullException(nameof(sale));
                if (items == null || !items.Any())
                    throw new ArgumentException("Sale must have at least one item.", nameof(items));
                sale.CreatedById = userId;
                sale.TotalAmount = items.Sum(i => i.LineTotal);

                await _unitOfWork.Sales.AddSaleAsync(sale, items);

                foreach (var item in items)
                {
                    var product = await _unitOfWork.Products.GetByIdAsync(item.ProductId);
                    if (product == null) throw new Exception($"Product {item.ProductId} not found");

                    if (product.QuantityOnHand < item.Quantity)
                        throw new InvalidOperationException($"Insufficient stock for product {product.SKU}");

                    int oldQty = product.QuantityOnHand;
                    product.QuantityOnHand -= item.Quantity;

                    _unitOfWork.Products.Update(product);

                    var transactionLog = new InventoryTransaction
                    {
                        ProductId = product.Id,
                        ChangeQuantity = -item.Quantity,
                        TransactionType = TransactionType.Sale,
                        ReferenceId = sale.Id,
                        PreviousQuantity = oldQty,
                        NewQuantity = product.QuantityOnHand,
                        PerformedById = userId,
                        Notes = $"Sale {sale.InvoiceNumber}"
                    };

                    await _unitOfWork.InventoryTransactions.AddAsync(transactionLog);
                }

                await _unitOfWork.SaveChangesAsync();
                await transaction.CommitAsync();
                return sale;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        public Task<IEnumerable<Sale>> GetSalesByDateRangeAsync(DateTime from, DateTime to)
        {
            if (from > to)
                throw new ArgumentException("The 'from' date must be earlier than or equal to the 'to' date.");
            if (to > DateTime.Now)
                throw new ArgumentException("The 'to' date cannot be in the future.");
            var sales = _unitOfWork.Sales.GetByDateRangeAsync(from, to).Result;
            return Task.FromResult(sales);
        }
        #endregion
    }
}
