# 📝 MyMVCBlog

A modern, full-featured blog application built with **ASP.NET Core MVC** and **.NET 10**, showcasing professional web development practices, clean architecture, and user authentication.

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=flat&logo=dotnet)
![C#](https://img.shields.io/badge/C%23-14.0-239120?style=flat&logo=csharp)
![SQL Server](https://img.shields.io/badge/SQL%20Server-Database-CC2927?style=flat&logo=microsoftsqlserver)
![Bootstrap](https://img.shields.io/badge/Bootstrap-5-7952B3?style=flat&logo=bootstrap)

---

## 🚀 Features

- ✅ **User Authentication** – Secure registration and login with ASP.NET Core Identity
- ✅ **Blog Post Management** – Full CRUD operations (Create, Read, Update, Delete)
- ✅ **Category Filtering** – Organize posts by Technology, Health, and Lifestyle
- ✅ **Image Upload** – Feature images with validation (JPG, PNG, GIF, WebP, AVIF)
- ✅ **Comments System** – Add comments to blog posts with real-time updates
- ✅ **Responsive UI** – Mobile-first design with Bootstrap 5
- ✅ **Entity Framework Core** – Code-first approach with migrations
- ✅ **Clean Architecture** – Separation of concerns with ViewModels and Controllers

---

## 🛠️ Tech Stack

| Technology | Purpose |
|------------|---------|
| **ASP.NET Core MVC** | Web framework |
| **.NET 10** | Runtime environment |
| **Entity Framework Core** | ORM for database operations |
| **SQL Server** | Relational database |
| **ASP.NET Core Identity** | User authentication & authorization |
| **Bootstrap 5** | Responsive UI framework |
| **Razor Views** | Server-side rendering |

---

## 📸 Screenshots

### 🏠 Home Page
![Home](docs/home.jpeg)

### 📝 Posts List
![Posts](docs/posts.jpeg)

### ➕ Create Post
![Create](docs/create.jpeg)

### 📄 Post Details
![Details](docs/details.jpeg)

### 🔐 Login Page
![Login](docs/login.jpeg)

---

## 📂 Project Structure

```
MyMVCBlog/
├── Controllers/
│   ├── HomeController.cs       # Landing page
│   ├── PostController.cs       # Blog CRUD operations
│   └── AuthController.cs       # Authentication logic
├── Models/
│   ├── Post.cs                 # Blog post entity
│   ├── Category.cs             # Category entity
│   ├── Comment.cs              # Comment entity
│   └── ViewModels/             # DTOs for views
│       ├── PostViewModel.cs
│       ├── EditViewModel.cs
│       ├── LoginViewModel.cs
│       └── RegisterViewModel.cs
├── Views/
│   ├── Home/
│   │   └── Index.cshtml        # Landing page
│   ├── Post/
│   │   ├── Index.cshtml        # Post listing
│   │   ├── Details.cshtml      # Single post view
│   │   ├── Create.cshtml       # Create post form
│   │   ├── Edit.cshtml         # Edit post form
│   │   └── Delete.cshtml       # Delete confirmation
│   ├── Auth/
│   │   ├── Login.cshtml
│   │   └── Register.cshtml
│   └── Shared/
│       ├── _Layout.cshtml      # Main layout
│       └── _Navbar.cshtml      # Navigation bar
├── Data/
│   └── ApplicationDbContext.cs # EF Core DbContext
├── Migrations/                 # Database migrations
└── wwwroot/
    ├── css/                    # Stylesheets
    ├── js/                     # Client-side scripts
    └── images/                 # Uploaded images
```

---

## 🏗️ Architecture & Patterns

### **MVC Pattern**
- **Models** – Domain entities and ViewModels for data transfer
- **Views** – Razor pages with Bootstrap for responsive UI
- **Controllers** – Business logic and request handling

### **Key Design Decisions**
1. **ViewModels** – Prevent over-posting attacks by separating DTOs from entities
2. **AsNoTracking()** in Edit – Load original entity without tracking, then explicitly update with `Update()`
3. **Image Upload Validation** – File extension and size checks before saving
4. **Soft Layout** – Consistent form styling with Bootstrap cards and utility classes
5. **Category Dropdown** – Navigation with dropdown menu for category filtering

---

## ⚙️ Setup & Installation

### Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [SQL Server](https://www.microsoft.com/sql-server) or SQL Server Express/LocalDB
- [Visual Studio 2025](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/)

### Installation Steps

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/MyMVCBlog.git
   cd MyMVCBlog
   ```

2. **Update connection string**
   
   Edit `appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=MyMVCBlogDb;Trusted_Connection=True;MultipleActiveResultSets=true"
     }
   }
   ```

3. **Apply database migrations**
   ```bash
   dotnet ef database update
   ```

4. **Run the application**
   ```bash
   dotnet run
   ```

5. **Open in browser**
   ```
   https://localhost:5001
   ```

---

## 🎯 Key Functionality Highlights

### 1️⃣ **User Registration & Authentication**
- Secure password hashing with ASP.NET Core Identity
- Email validation and duplicate email prevention
- Login/logout functionality

### 2️⃣ **Blog Post CRUD**
- **Create** – Add new posts with title, content, author, category, and feature image
- **Read** – View all posts or filter by category
- **Update** – Edit existing posts, optionally update feature image
- **Delete** – Remove posts with confirmation dialog

### 3️⃣ **Image Handling**
- Validates file extensions (`.jpg`, `.jpeg`, `.png`, `.gif`, `.webp`, `.avif`)
- Stores images in `wwwroot/images/` with GUID-based filenames
- Automatically deletes old images when updating posts

### 4️⃣ **Comments**
- AJAX-based comment submission
- Real-time comment display without page reload
- Timestamp formatting (`MMM dd yyyy`)

### 5️⃣ **Category System**
- Seed data: Technology, Health, Lifestyle
- Dropdown navigation in navbar
- Post filtering by `categoryId`

---

## 📊 Database Schema

### **Posts**
| Column | Type | Description |
|--------|------|-------------|
| Id | int | Primary key |
| Title | nvarchar | Post title |
| Content | nvarchar(max) | Post content |
| Author | nvarchar | Author name |
| PublishedDate | datetime2 | Publication date |
| CategoryId | int | Foreign key to Categories |
| FeatureImagePath | nvarchar | Image URL |

### **Categories**
| Column | Type | Description |
|--------|------|-------------|
| Id | int | Primary key |
| Name | nvarchar | Category name |
| Description | nvarchar | Category description |

### **Comments**
| Column | Type | Description |
|--------|------|-------------|
| Id | int | Primary key |
| UserName | nvarchar | Commenter name |
| Content | nvarchar | Comment text |
| CommentDate | datetime2 | Comment timestamp |
| PostId | int | Foreign key to Posts |

---

## 🧪 Testing the Application

### Sample Workflow
1. **Register** a new account at `/Auth/Register`
2. **Login** with your credentials at `/Auth/Login`
3. **Create** a blog post at `/Post/Create`
4. **View** all posts at `/Post/Index` or home page
5. **Filter** posts by category using navbar dropdown
6. **Edit** or **Delete** posts using action buttons
7. **Add comments** to posts in the details view

---

## 🔐 Security Features

- **Anti-Forgery Tokens** – CSRF protection on all forms
- **File Upload Validation** – Whitelist of allowed image extensions
- **Model Validation** – Data annotations for input validation
- **Password Hashing** – ASP.NET Core Identity secure storage
- **Input Sanitization** – Razor engine automatic HTML encoding

---

## 🎨 UI/UX Highlights

- **Bootstrap 5** for responsive design
- **Bootstrap Icons** for visual enhancements
- **Card-based layouts** for consistent form styling
- **Dropdown navigation** for category filtering
- **Alerts & validation messages** with contextual colors
- **Mobile-first design** with responsive grid system

---

## 📝 Lessons Learned

This project demonstrates:
- ✅ ASP.NET Core MVC architecture and routing
- ✅ Entity Framework Core with Code-First migrations
- ✅ ASP.NET Core Identity for authentication
- ✅ Form handling with validation and model binding
- ✅ File upload and server-side storage
- ✅ Razor views with tag helpers and partial views
- ✅ Bootstrap integration for professional UI
- ✅ Separation of concerns with ViewModels
- ✅ CRUD operations with EF Core tracking and no-tracking queries

---

## 👤 Author

**Michał Bąkiewicz**  
📧 [thekcr85@gmail.com](mailto:thekcr85@gmail.com)  
💼 [LinkedIn](https://www.linkedin.com/in/bakiewicz-michal/) | 🐙 [GitHub](https://github.com/thekcr85)

---

## 📄 License

This project is licensed under the MIT License. Feel free to use it for learning and portfolio purposes.

---

## 🙏 Acknowledgments

- Built with ❤️ using ASP.NET Core MVC
- UI powered by Bootstrap 5
- Icons from Bootstrap Icons

---

**⭐ If you find this project helpful, please give it a star!**
