# 🐳 Docker Deployment Guide

This guide explains how to build and run the BookList application using Docker containers.

## 📁 Docker Files Overview

### Backend Dockerfile
- **Location**: `backend/BookListApi/Dockerfile`
- **Base Image**: `mcr.microsoft.com/dotnet/sdk:9.0` (build) + `mcr.microsoft.com/dotnet/aspnet:9.0` (runtime)
- **Port**: 8080
- **Features**: Multi-stage build, optimized for production

### Frontend Dockerfile
- **Location**: `frontend/book-list-app/Dockerfile`  
- **Base Image**: `node:20-alpine` (build) + `nginx:alpine` (runtime)
- **Port**: 80
- **Features**: Angular production build, Nginx web server, gzip compression

### Docker Compose
- **Location**: `docker-compose.yml`
- **Services**: Backend API + Frontend Web
- **Network**: Custom bridge network for inter-service communication
- **Health Checks**: Automated health monitoring

## 🚀 Quick Start

### 1. **Clone and Navigate**
```bash
git clone https://github.com/PiotrSzczap/BookList.git
cd BookList
```

### 2. **Configure Environment**
```bash
# Copy environment template
cp .env.example .env

# Edit .env file with your Azure connection strings
nano .env
```

### 3. **Build and Run**
```bash
# Build and start all services
docker-compose up -d --build

# Check status
docker-compose ps

# View logs
docker-compose logs -f
```

### 4. **Access Application**
- **Frontend**: http://localhost (port 80)
- **Backend API**: http://localhost:8080
- **Health Check**: http://localhost:8080/health

## 🔧 Individual Container Commands

### Backend Only
```bash
# Build backend image
docker build -t booklist-api ./backend/BookListApi

# Run backend container
docker run -d \
  --name booklist-api \
  -p 8080:8080 \
  -e ConnectionStrings__CosmosDb="your-cosmos-connection" \
  -e ConnectionStrings__BlobStorage="your-blob-connection" \
  booklist-api
```

### Frontend Only
```bash
# Build frontend image
docker build -t booklist-frontend ./frontend/book-list-app

# Run frontend container
docker run -d \
  --name booklist-frontend \
  -p 80:80 \
  booklist-frontend
```

## 🌐 Production Deployment

### Docker Compose Production
```bash
# Use production compose file
docker-compose -f docker-compose.yml -f docker-compose.prod.yml up -d
```

### Environment Variables
Required environment variables for production:

```bash
COSMOS_DB_CONNECTION_STRING=AccountEndpoint=https://your-cosmosdb.documents.azure.com:443/;AccountKey=your-key;
BLOB_STORAGE_CONNECTION_STRING=DefaultEndpointsProtocol=https;AccountName=your-account;AccountKey=your-key;EndpointSuffix=core.windows.net
```

### Azure Container Instances
```bash
# Create resource group
az group create --name booklist-containers --location eastus

# Deploy backend container
az container create \
  --resource-group booklist-containers \
  --name booklist-api \
  --image your-registry/booklist-api:latest \
  --cpu 1 --memory 1 \
  --ports 8080 \
  --environment-variables \
    ConnectionStrings__CosmosDb="your-cosmos-connection" \
    ConnectionStrings__BlobStorage="your-blob-connection"

# Deploy frontend container
az container create \
  --resource-group booklist-containers \
  --name booklist-frontend \
  --image your-registry/booklist-frontend:latest \
  --cpu 0.5 --memory 0.5 \
  --ports 80
```

## 🔍 Debugging and Monitoring

### View Container Logs
```bash
# All services
docker-compose logs -f

# Specific service
docker-compose logs -f backend
docker-compose logs -f frontend
```

### Enter Container Shell
```bash
# Backend container
docker-compose exec backend bash

# Frontend container  
docker-compose exec frontend sh
```

### Health Checks
```bash
# Backend health
curl http://localhost:8080/health

# Frontend health
curl http://localhost:80
```

## 🛠️ Development with Docker

### Development Override
Create `docker-compose.override.yml`:
```yaml
version: '3.8'
services:
  backend:
    volumes:
      - ./backend/BookListApi:/app
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
  
  frontend:
    volumes:
      - ./frontend/book-list-app/src:/app/src
```

### Hot Reload Development
```bash
# Start with development overrides
docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d
```

## 📊 Container Specifications

### Backend Container
- **CPU**: 1 core
- **Memory**: 1GB
- **Storage**: 512MB
- **Network**: Port 8080
- **Health**: HTTP GET /health

### Frontend Container
- **CPU**: 0.5 core
- **Memory**: 512MB
- **Storage**: 256MB
- **Network**: Port 80
- **Health**: HTTP GET /

## 🚨 Troubleshooting

### Common Issues

**Backend Won't Start**
```bash
# Check environment variables
docker-compose exec backend printenv | grep ConnectionStrings

# Check logs
docker-compose logs backend
```

**Frontend Can't Connect to Backend**
```bash
# Check network connectivity
docker-compose exec frontend curl http://backend:8080/health

# Check CORS configuration
curl -H "Origin: http://localhost" http://localhost:8080/api/books
```

**Database Connection Issues**
```bash
# Verify Cosmos DB connection string
docker-compose exec backend dotnet BookListApi.dll --test-db-connection
```

### Performance Optimization
```bash
# Optimize images
docker system prune -a

# Check resource usage
docker stats

# Scale services
docker-compose up -d --scale backend=2
```

## 🔐 Security Best Practices

### Production Security
1. **Use Azure Key Vault** for connection strings
2. **Enable HTTPS** with SSL certificates
3. **Implement rate limiting** in Nginx
4. **Use non-root users** in containers
5. **Scan images** for vulnerabilities

### Secrets Management
```bash
# Use Docker secrets (Swarm mode)
echo "your-connection-string" | docker secret create cosmos-db-conn -
```

## 📈 Scaling and Load Balancing

### Docker Swarm
```bash
# Initialize swarm
docker swarm init

# Deploy stack
docker stack deploy -c docker-compose.yml booklist
```

### Kubernetes Deployment
```yaml
# See kubernetes/ directory for complete manifests
kubectl apply -f kubernetes/
```

This Docker setup provides a complete containerized environment for the BookList application with production-ready configurations.
