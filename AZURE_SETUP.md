# Azure Cosmos DB Setup Guide

## Azure Resources Created

### Resource Group
- **Name**: `book-list-dev`
- **Location**: `East US`

### Cosmos DB Account
- **Name**: `booklist-cosmosdb`
- **API**: `SQL (Core)`
- **Consistency Level**: `Eventual`
- **Location**: `East US`

### Database
- **Name**: `BookListDB`

### Container
- **Name**: `Books`
- **Partition Key**: `/id`

## Connection Details

### Primary Connection String
```
Replace this with your actual Cosmos DB connection string from the Azure portal
```

## Entity Framework Integration

The backend now uses Entity Framework Core with the Cosmos DB provider:

- **NuGet Package**: `Microsoft.EntityFrameworkCore.Cosmos` (v9.0.8)
- **DbContext**: `BookDbContext`
- **Configuration**: Connection string in `appsettings.json` and `appsettings.Development.json`

## API Endpoints

All CRUD operations now interact with Cosmos DB:

- `GET /api/books` - Get all books from Cosmos DB
- `GET /api/books/{id}` - Get specific book by ID (GUID)
- `POST /api/books` - Create new book in Cosmos DB
- `PUT /api/books/{id}` - Update existing book
- `DELETE /api/books/{id}` - Delete book from Cosmos DB
- `POST /api/books/seed` - Seed sample data into Cosmos DB

## Data Model Changes

### Backend (C#)
- `Book.Id` changed from `int` to `string` (GUID)
- Added JSON property name attributes for Cosmos DB serialization
- Added validation attributes

### Frontend (TypeScript)
- `Book.id` changed from `number` to `string`
- Added seed data functionality
- Updated all service methods to handle string IDs

## Azure CLI Commands Used

```bash
# Create resource group
az group create --name book-list-dev --location eastus

# Create Cosmos DB account
az cosmosdb create --name booklist-cosmosdb --resource-group book-list-dev --default-consistency-level Eventual --locations regionName=eastus failoverPriority=0 isZoneRedundant=false

# Create database
az cosmosdb sql database create --account-name booklist-cosmosdb --resource-group book-list-dev --name BookListDB

# Create container
az cosmosdb sql container create --account-name booklist-cosmosdb --resource-group book-list-dev --database-name BookListDB --name Books --partition-key-path "/id"

# Get connection strings
az cosmosdb keys list --name booklist-cosmosdb --resource-group book-list-dev --type connection-strings
```

## Security Notes

- Connection string contains sensitive keys - store securely in production
- Consider using Azure Key Vault for production deployments
- Enable firewall rules for production environments

## Development Setup

1. The connection string is configured in both `appsettings.json` and `appsettings.Development.json`
2. Entity Framework will automatically create the database structure
3. Use the `/api/books/seed` endpoint to populate initial data
4. Both backend and frontend have been updated to work with string-based IDs

## Production Considerations

- Use Azure Key Vault for connection strings
- Configure proper CORS settings
- Set up proper authentication and authorization
- Consider using Azure App Service for hosting
- Enable monitoring and logging
