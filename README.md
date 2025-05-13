# 🌿 Agri-Energy Connect Platform

Welcome to my **Agri-Energy Connect Platform** - this is a working prototype built using ASP.NET Core, Entity Framework Core, and SQL Server. The system is designed to help manage farmers and their products securly utilizing role based access for the roles: **Farmers** and **Employees** 

---

## 📋 Table of Contents

- [🔧 What you'll need](#-prerequisites)
- [⚙️ Setup Development Environment](#️-setup-instructions)
- [🗄️ Database Setup](#️-database-initialization)
- [▶️ Running the Application](#️-running-the-application)
- [🔐 Default Login Credentials](#-default-login-details)
- [👥 User Role Functionalities](#-user-role-functionalities)
  - [👨‍🌾 Farmer](#-farmer)
  - [👩‍💼 Employee](#-employee)
- [🖥️ User Interface](#️-user-interface)
- [✅ Data Validation and Error Handling](#-data-validation-and-error-handling)
- [🧪 Testing and Development](#-testing-and-development)
- [📎 Additional Notes](#-notes)
- [📫 Contact](#-contact)

---

## 🔧 Prerequisites

Make sure the following tools are installed before you start:

- [Visual Studio 2022+](https://visualstudio.microsoft.com/) with **ASP.NET and web development** workload
- [.NET 6 SDK or later](https://dotnet.microsoft.com/)
- [SQL Server Management Studio (SSMS)](https://aka.ms/ssmsfullsetup)
- SQL Server (LocalDB or full instance)

---

## ⚙️ Setup Instructions

1. **Clone the Project**
   ```bash
   git clone [https://github.com/VCPTA/bca3-prog7311-part-2-ST10268524.git]
   cd agri-energy-connect
   ```

2. **Open the Solution**
   Open the .sln file in Visual Studio.

3. **Update the Database Connection**

  - Open appsettings.json and update the connection string with your local SQL Server name:
    ```bash
    "ConnectionStrings": {
      "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=AgriEnergyConnectDB;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
    }
    ```
    
4. **Restore Dependencies**

  - Visual Studio usually restores NuGet packages automatically.

  If not, go to:

    Tools > NuGet Package Manager > Manage NuGet Packages for Solution
---
    
## 🗄️ Database Initialization
Initialize the database using Entity Framework Core by running the following in the **Package Manager** Console:

    Update-Database
    
This command will:

 - Create the ***AgriEnergyDB*** database.

 - Seed it with the default roles (Farmer, Employee), users, and sample data.

---

## ▶️ Running the Application
1. Press F5 or click Start Debugging in Visual Studio.

2. The browser will open to https://localhost:{port}.

3. You’ll land on the Login/Register page to begin.

---
## 🔐 Default Login Details
  ### 👨‍🌾 Farmer
  - **Email**: farmer@test.com
  - **Password**: Farmer123!

### 👩‍💼 Employee
  - **Email**: employee@test.com
  - **Password**: Employee123!

> ⚠️ These accounts are for development/testing only.
---

## 👥 User Role Functionalities
### 👨‍🌾 Farmer
  - Register/login as a Farmer
  - Add new products with:
    - Product Name
    - Category
    - Production Date
  - View and manage your own products

### 👩‍💼 Employee
  - Register/login as an Employee
  - Add new Farmer profiles
  - View all products
  - Filter products:
      - Filter by farmer
      - Filter by date range
      - Filter by product category

Role-based navigation and permissions are enforced using ASP.NET Identity.

---

## 🖥️ User Interface

- A simple, easy-to-use design with distinct views for farmers and employees
- Both desktop and mobile devices may use responsive design.
- Data grids, input validation, and clear labeling

---

## ✅ Data Validation and Error Handling

- Validation on both the client and server sides:
  - Required fields
  - Appropriate forms (date, dropdowns, etc.)
- Users' error feedback messages
- Prevents the submission of incorrect data - Secures pages according to user roles

--- 

## 🧪 Testing and Development

  - Iterative development with feature-based testing
  -   Manual testing was done for:
      - CRUD operations
      - role managementA
      - Authentication.
  -   UX testing is carried out as necessary.

---

## 📎 Notes

  - To reset the database:
      1. Drop AgriEnergyDB in SSMS
      2. Run Update-Database again in Package Manager Console
  - All source code, scripts, and assets are included in this repository

---

## 📫 Contact
- Developer: Derik Korf
- Email: st10268524@vcconnect.edu.za
