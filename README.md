# BookList Application

# 📚 BookList - Azure-Powered Book Management System

A complete full-stack application for managing books with cloud storage capabilities, built with **.NET 9** and **Angular 20**.

## 🚀 Features

### **Backend (.NET 9 Web API)**
- **Entity Framework Core** with Azure Cosmos DB integration
- **Azure Blob Storage** for large file handling (book content)
- Automatic database seeding with sample data
- RESTful API with CRUD operations
- File upload/download/delete endpoints
- CORS configuration for cross-origin requests
- Comprehensive error handling and logging

### **Frontend (Angular 20)**
- **Standalone components** architecture
- Responsive CSS Grid layout
- File upload with drag-and-drop support
- Real-time content management UI
- Download functionality with proper file naming
- Visual indicators for content availability
- Error handling and loading states

### **Azure Integration**
- **Cosmos DB** for book metadata storage
- **Blob Storage** for book content files (PDF, EPUB, TXT, DOC, DOCX)
- Resource Group management
- Connection string security best practices

## 🛠️ Getting Started

### **Prerequisites**
- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Node.js 20+](https://nodejs.org/)
- [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli)
- Azure subscription with Cosmos DB and Storage Account

### **Azure Setup**

1. **Login to Azure CLI**
   ```bash
   az login
   ```

2. **Create Resource Group**
   ```bash
   az group create --name book-list-dev --location eastus
   ```

3. **Create Cosmos DB Account**
   ```bash
   az cosmosdb create --name booklist-cosmosdb --resource-group book-list-dev --kind GlobalDocumentDB --default-consistency-level Eventual
   ```

4. **Create Storage Account**
   ```bash
   az storage account create --name booklist2617dev --resource-group book-list-dev --location eastus --sku Standard_LRS
   ```

5. **Create Blob Container**
   ```bash
   az storage container create --name book-content --account-name booklist2617dev
   ```

### **Backend Setup**

1. **Navigate to backend directory**
   ```bash
   cd backend/BookListApi
   ```

2. **Restore NuGet packages**
   ```bash
   dotnet restore
   ```

3. **Configure connection strings**
   
   Create `appsettings.Development.json`:
   ```json
   {
     "ConnectionStrings": {
       "CosmosDb": "AccountEndpoint=https://your-cosmosdb.documents.azure.com:443/;AccountKey=your-key;",
       "BlobStorage": "DefaultEndpointsProtocol=https;AccountName=your-storage;AccountKey=your-key;EndpointSuffix=core.windows.net"
     }
   }
   ```

4. **Run the backend**
   ```bash
   dotnet run
   ```

   Backend will be available at: `http://localhost:5095`

### **Frontend Setup**

1. **Navigate to frontend directory**
   ```bash
   cd frontend/book-list-app
   ```

2. **Install npm packages**
   ```bash
   npm install
   ```

3. **Start development server**
   ```bash
   npm start
   ```

   Frontend will be available at: `http://localhost:4200`

## 📁 Project Structure

```
BookList/
├── backend/
│   └── BookListApi/
│       ├── Controllers/         # API controllers
│       ├── Models/             # Data models
│       ├── Services/           # Business logic services
│       ├── Data/               # Database context
│       └── Properties/         # Launch settings
├── frontend/
│   └── book-list-app/
│       └── src/
│           └── app/
│               ├── components/  # Angular components
│               ├── services/   # HTTP services
│               └── models/     # TypeScript models
└── docs/                       # Documentation
```

## 🔧 API Endpoints

### **Books**
- `GET /api/books` - Get all books
- `GET /api/books/{id}` - Get specific book
- `POST /api/books` - Create new book
- `PUT /api/books/{id}` - Update book
- `DELETE /api/books/{id}` - Delete book

### **Content Management**
- `POST /api/books/{id}/content` - Upload book content
- `GET /api/books/{id}/content` - Download book content
- `DELETE /api/books/{id}/content` - Delete book content
- `GET /api/books/{id}/content/url` - Get content download URL

## 🎨 UI Features

- **📖 Book Grid Display** - Responsive card layout
- **📤 File Upload** - Click to select files for upload
- **📥 Download** - Direct file download with original names
- **🗑️ Content Management** - Delete content or entire books
- **📄 Content Indicators** - Visual status of content availability
- **⚡ Real-time Updates** - Automatic refresh after operations

## 🔒 Security

- **Environment-based Configuration** - Sensitive data in Development settings
- **Azure Key Vault Ready** - Easy integration for production secrets
- **CORS Policy** - Configured for localhost development
- **File Type Validation** - Restricted to document formats

## 🚧 Development

### **Running in Development**
Both applications support hot reload:
- Backend: File changes trigger automatic rebuild
- Frontend: Component changes reflect immediately

### **Building for Production**
```bash
# Backend
cd backend/BookListApi
dotnet publish -c Release

# Frontend  
cd frontend/book-list-app
npm run build
```

## 📊 Technologies Used

| Component | Technology | Version |
|-----------|------------|---------|
| Backend API | .NET | 9.0 |
| Frontend | Angular | 20.0 |
| Database | Azure Cosmos DB | SQL API |
| File Storage | Azure Blob Storage | v12 |
| ORM | Entity Framework Core | 9.0 |
| HTTP Client | Angular HttpClient | 20.0 |
| Styling | CSS Grid + Flexbox | - |

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Test thoroughly
5. Submit a pull request

## 📝 License

This project is licensed under the MIT License - see the LICENSE file for details.

---

**Made with ❤️ using .NET 9 and Angular 20**

## Project Structure

```
BookList/
├── backend/
│   └── BookListApi/           # .NET 9 Web API with Entity Framework & Cosmos DB
└── frontend/
    └── book-list-app/         # Angular 20 Application
```

## Features

- **Backend (.NET 9 Web API)**:
  - RESTful API for book management
  - Entity Framework Core with Azure Cosmos DB
  - CRUD operations (Create, Read, Update, Delete)
  - CORS enabled for Angular frontend
  - Data seeding endpoint
  - Async/await pattern for database operations

- **Frontend (Angular 20)**:
  - Modern Angular application with standalone components
  - Responsive design with CSS Grid layout
  - Book list display with grid layout
  - Form for adding new books
  - Seed data functionality
  - Real-time updates after operations
  - Error handling and loading states

## Azure Resources

- **Resource Group**: `book-list-dev`
- **Cosmos DB Account**: `booklist-cosmosdb`
- **Database**: `BookListDB`
- **Container**: `Books` (partition key: `/id`)

## Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Node.js](https://nodejs.org/) (version 18 or higher)
- [Angular CLI](https://angular.io/cli) version 20
- [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli) (for Azure resources)
- Azure subscription

## Getting Started

### Running the Backend (.NET 9 API)

1. Navigate to the backend directory:
   ```bash
   cd backend/BookListApi
   ```

2. Restore dependencies:
   ```bash
   dotnet restore
   ```

3. Run the API:
   ```bash
   dotnet run
   ```

   The API will be available at:
   - HTTP: `http://localhost:5000`
   - HTTPS: `https://localhost:5001`

4. Seed initial data (optional):
   ```bash
   curl -X POST https://localhost:5001/api/books/seed
   ```

### Running the Frontend (Angular 20)

1. Navigate to the frontend directory:
   ```bash
   cd frontend/book-list-app
   ```

2. Install dependencies:
   ```bash
   npm install
   ```

3. Start the development server:
   ```bash
   ng serve
   ```

   The application will be available at `http://localhost:4200`

## API Endpoints

- `GET /api/books` - Get all books from Cosmos DB
- `GET /api/books/{id}` - Get a specific book by GUID
- `POST /api/books` - Create a new book in Cosmos DB
- `PUT /api/books/{id}` - Update a book
- `DELETE /api/books/{id}` - Delete a book from Cosmos DB
- `POST /api/books/seed` - Seed sample data into Cosmos DB

## Book Model

```json
{
  "id": "550e8400-e29b-41d4-a716-446655440000",
  "title": "Book Title",
  "author": "Author Name",
  "isbn": "978-0-123456-78-9",
  "publishedDate": "2023-01-01T00:00:00Z",
  "genre": "Fiction",
  "price": 19.99,
  "description": "Book description"
}
```

## Development

### Backend Development

The backend includes:
- Entity Framework Core with Cosmos DB provider
- Book model with GUID IDs and JSON serialization
- Async database operations
- DbContext configuration for Cosmos DB
- Error handling and logging
- CORS configuration for Angular frontend

### Frontend Development

The frontend includes:
- Book model interface (TypeScript)
- Book service for API communication with Azure backend
- Book list component for displaying books
- Book form component for adding new books
- Seed data functionality for easy testing
- Responsive CSS styling
- Error handling and loading states

## Azure Setup

See [AZURE_SETUP.md](AZURE_SETUP.md) for detailed information about:
- Azure resources created
- Connection configuration
- Entity Framework integration
- Security considerations

## Technologies Used

**Backend:**
- .NET 9
- ASP.NET Core Web API
- Entity Framework Core
- Azure Cosmos DB
- System.Text.Json

**Frontend:**
- Angular 20
- TypeScript
- RxJS
- Angular HttpClient
- CSS Grid and Flexbox

**Cloud:**
- Azure Cosmos DB (SQL API)
- Azure Resource Groups

## License

This project is for demonstration purposes.
