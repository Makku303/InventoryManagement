using Core.IRepositories;
using Core.IServices;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<InventoryService> _logger;

        public InventoryService(IUnitOfWork unitOfWork, ILogger<InventoryService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Product> AddProductAsync(Product product)
        {
            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation($"Product added: {product.Name} (SKU: {product.SKU})");
            return product;
        }

        public async Task<Product> UpdateProductAsync(Product product)
        {
            try
            {
                _unitOfWork.Products.Update(product);
                await _unitOfWork.SaveChangesAsync();
                _logger.LogInformation($"Product updated: {product.Id}");
                return product;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogWarning(ex, $"Concurrency conflict while updating product {product.Id}");
                throw;
            }
        }

        public async Task<bool> DeleteProductAsync(int productId)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(productId);
            if (product == null)
                return false;

            // Soft delete
            product.IsActive = false;
            _unitOfWork.Products.Update(product);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation($"Product deactivated: {productId}");
            return true;
        }

        public async Task<Purchase> AddPurchaseAsync(Purchase purchase, IEnumerable<PurchaseItem> items, Guid userId)
        {
            //using var transaction = await (_unitOfWork as UnitOfWork)?._context.Database.BeginTransactionAsync();
            //try
            //{
            //    purchase.CreatedById = userId;
            //    purchase.TotalAmount = items.Sum(i => i.LineTotal);

            //    await _unitOfWork.Purchases.AddPurchaseAsync(purchase, items);

            //    foreach (var item in items)
            //    {
            //        var product = await _unitOfWork.Products.GetByIdAsync(item.ProductId);
            //        if (product == null) throw new Exception($"Product {item.ProductId} not found");

            //        int oldQty = product.QuantityOnHand;
            //        product.QuantityOnHand += item.Quantity;

            //        _unitOfWork.Products.Update(product);

            //        var transactionLog = new InventoryTransaction
            //        {
            //            ProductId = product.Id,
            //            ChangeQuantity = item.Quantity,
            //            TransactionType = TransactionType.Purchase,
            //            ReferenceId = purchase.Id,
            //            PreviousQuantity = oldQty,
            //            NewQuantity = product.QuantityOnHand,
            //            PerformedById = userId,
            //            Notes = $"Purchase {purchase.InvoiceNumber}"
            //        };

            //        await _unitOfWork.InventoryTransactions.AddAsync(transactionLog);
            //    }

            //    await _unitOfWork.SaveChangesAsync();
            //    await transaction.CommitAsync();

            //    _logger.LogInformation($"Purchase created: {purchase.Id}, Items: {items.Count()}");
            //    return purchase;
            //}
            //catch (Exception ex)
            //{
            //    await transaction.RollbackAsync();
            //    _logger.LogError(ex, "Failed to add purchase");
            //    throw;
            //}
            throw new NotImplementedException();
        }

        public async Task<Sale> AddSaleAsync(Sale sale, IEnumerable<SaleItem> items, Guid userId)
        {
            //using var transaction = await (_unitOfWork as UnitOfWork)?._context.Database.BeginTransactionAsync();
            //try
            //{
            //    sale.CreatedById = userId;
            //    sale.TotalAmount = items.Sum(i => i.LineTotal);

            //    await _unitOfWork.Sales.AddSaleAsync(sale, items);

            //    foreach (var item in items)
            //    {
            //        var product = await _unitOfWork.Products.GetByIdAsync(item.ProductId);
            //        if (product == null) throw new Exception($"Product {item.ProductId} not found");

            //        if (product.QuantityOnHand < item.Quantity)
            //            throw new InvalidOperationException($"Insufficient stock for product {product.SKU}");

            //        int oldQty = product.QuantityOnHand;
            //        product.QuantityOnHand -= item.Quantity;

            //        _unitOfWork.Products.Update(product);

            //        var transactionLog = new InventoryTransaction
            //        {
            //            ProductId = product.Id,
            //            ChangeQuantity = -item.Quantity,
            //            TransactionType = TransactionType.Sale,
            //            ReferenceId = sale.Id,
            //            PreviousQuantity = oldQty,
            //            NewQuantity = product.QuantityOnHand,
            //            PerformedById = userId,
            //            Notes = $"Sale {sale.InvoiceNumber}"
            //        };

            //        await _unitOfWork.InventoryTransactions.AddAsync(transactionLog);
            //    }

            //    await _unitOfWork.SaveChangesAsync();
            //    await transaction.CommitAsync();

            //    _logger.LogInformation($"Sale created: {sale.Id}, Items: {items.Count()}");
            //    return sale;
            //}
            //catch (Exception ex)
            //{
            //    await transaction.RollbackAsync();
            //    _logger.LogError(ex, "Failed to add sale");
            //    throw;
            //}
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Product>> GetLowStockProductsAsync(int threshold)
        {
            return await _unitOfWork.Products.GetLowStockAsync(threshold);
        }

        public async Task<IEnumerable<Product>> GetOutOfStockProductsAsync()
        {
            return await _unitOfWork.Products.GetOutOfStockAsync();
        }

        public async Task<IEnumerable<Product>> GetBestSellingProductsAsync(DateTime from, DateTime to, int topN)
        {
            return await _unitOfWork.Products.GetBestSellingAsync(from, to, topN);
        }

        public Task<Product> GetProductByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetBestSellingProductsAsync(int top)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Sale>> GetAllSalesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Sale> GetSaleByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Purchase>> GetAllPurchasesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Purchase> GetPurchaseByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddPurchaseAsync(Purchase purchase)
        {
            throw new NotImplementedException();
        }

        public Task DeletePurchaseAsync(Purchase purchase)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddSaleAsync(Sale sale)
        {
            throw new NotImplementedException();
        }

        public Task DeleteSaleAsync(Sale sale)
        {
            throw new NotImplementedException();
        }
    }
}
