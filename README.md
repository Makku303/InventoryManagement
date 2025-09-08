# 📦 Inventory Management System API

An **Inventory Management System** built with **.NET Core 9**, **Entity Framework Core 9**, and **SQL (Local DB)**.  
It helps businesses track, manage, and optimize stock levels, orders, suppliers, and reports with secure role-based access.  

---

## 🚀 Features

- **Authentication & Authorization**  
  - Secure JWT-based authentication  
  - Role-based access (Admin, Staff)  

- **User Management**  
  - Sign up & login  
  - Manage profiles (update full name & password)  

- **Inventory Management**  
  - Add, update, delete products (name, category, SKU, quantity, price)  
  - Low-stock & out-of-stock alerts  
  - Best-selling products tracking  

- **Stock Movements**  
  - Log **incoming stock** (purchases)  
  - Log **outgoing stock** (sales)  
  - Audit logs for all inventory changes  

- **Suppliers**  
  - Manage suppliers (CRUD)  

- **Reports & Dashboard**  
  - Purchase, sales, and audit log reports  
  - Dashboard with low-stock, out-of-stock, best-sellers, recent activity  

- **Technical Highlights**  
  - Code First + EF Core Migrations  
  - Repository Pattern & Service Layer  
  - Optimistic concurrency handling  
  - Eager & Lazy Loading (with `.Include()`)  
  - `.AsNoTracking()` for read-only queries  
  - Normalized schema & indexed frequently queried columns  
  - Swagger API documentation  
  - Centralized logging & monitoring  

---

## 🛠️ Tech Stack

- **.NET Core 9 (Web API)**  
- **Entity Framework Core 9**  
- **SQL Local DB**  
- **Swagger (API Docs)**  
- **xUnit + Moq (Unit Testing)**  
- **Coverlet + ReportGenerator (Coverage Reports)**  

---

## 📂 Project Structure

```
InventorySystem/
│── InventorySystem.Api/          # Web API project (controllers, startup, swagger)
│── InventorySystem.Core/         # Models, DTOs, repositories, services
│── InventorySystem.Infrastructure/ # EF Core DbContext, repository implementations
│── InventorySystem.Tests/        # Unit tests (xUnit + Moq)
│── TestResults/                  # Coverage reports (generated)
```

---

## ⚙️ Setup & Run

### 1. Clone Repository
```bash
git clone https://github.com/your-repo/inventory-system.git
cd inventory-system
```

### 2. Apply Database Migrations
```bash
dotnet ef database update --project InventorySystem.Infrastructure
```

### 3. Run the API
```bash
dotnet run --project InventorySystem.Api
```

API available at:  
👉 `https://localhost:5001/swagger/index.html`  

---

## 🧪 Testing & Coverage

### Run Tests with Coverage
```bash
dotnet test /p:CollectCoverage=true /p:CoverletOutput=../TestResults/coverage/ /p:CoverletOutputFormat=cobertura
```

### Generate HTML Coverage Report
```bash
reportgenerator -reports:../TestResults/coverage/coverage.cobertura.xml -targetdir:../TestResults/coverage/report -reporttypes:Html
```

Then open:  
```
TestResults/coverage/report/index.html
```

### Enforce Minimum Coverage (80%)
```bash
dotnet test /p:CollectCoverage=true /p:Threshold=80
```

---

## 🔑 Default Roles

- **Admin**  
  - Full access to all features  
- **Staff**  
  - Manage products, suppliers, stock  
  - View reports & dashboard  

---

## 📊 Next Steps (Optional Enhancements)

- Email/SMS notifications for low-stock  
- Export reports (PDF, Excel)  
- Frontend client (React/Angular)  
- CI/CD pipeline with automated testing  

---

## 👨‍💻 Author

Built with ❤️ by **Your Name**
