# BookList Application

A full-stack application with a .NET 9 backend API using Azure Cosmos DB and Angular 20 frontend for managing a book collection.

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
