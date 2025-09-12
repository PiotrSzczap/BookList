# ☸️ Kubernetes Deployment Guide

This guide explains how to deploy the BookList application to Azure Kubernetes Service (AKS) using GitHub Actions CI/CD pipeline.

## 🏗️ Infrastructure Overview

### **Azure Resources**
- **Resource Group**: `booklist-k8s`
- **Container Registry**: `booklistregistry2617.azurecr.io`
- **AKS Cluster**: `booklist-aks` (2 nodes, Standard_B2s)
- **Integration**: ACR attached to AKS for seamless image pulling

### **Kubernetes Components**
- **Namespace**: `booklist`
- **Deployments**: Backend API (2 replicas) + Frontend (2 replicas)
- **Services**: ClusterIP services for internal communication
- **Ingress**: Nginx ingress for external access
- **LoadBalancer**: For development/testing access
- **HPA**: Horizontal Pod Autoscaling for both services

## 🚀 Deployment Pipeline

### **GitHub Actions Workflow** (`.github/workflows/deploy-to-aks.yml`)

#### **Build Stage**
1. **Checkout Code**: Get latest source code
2. **Docker Build**: Multi-stage builds for both services
3. **Push to ACR**: Tagged with `latest` and commit SHA
4. **Cache**: GitHub Actions cache for faster builds

#### **Deploy Stage**
1. **Azure Login**: Using service principal credentials
2. **AKS Access**: Get cluster credentials
3. **Secret Injection**: Replace placeholders with actual values
4. **K8s Deploy**: Apply all manifests in sequence
5. **Health Checks**: Verify deployments are ready

### **Required Secrets**
| Secret | Description | Example |
|--------|-------------|---------|
| `AZURE_CREDENTIALS` | Service principal JSON | `{"clientId": "...", ...}` |
| `ACR_USERNAME` | Container registry username | `booklistregistry2617` |
| `ACR_PASSWORD` | Container registry password | `encrypted_password` |
| `COSMOS_DB_CONNECTION_STRING` | Cosmos DB connection | `AccountEndpoint=https://...` |
| `BLOB_STORAGE_CONNECTION_STRING` | Storage connection | `DefaultEndpointsProtocol=https...` |

## 📁 Kubernetes Manifests

### **00-namespace-config.yaml**
- Creates `booklist` namespace
- ConfigMap with connection strings
- Secrets for sensitive data

### **01-backend-deployment.yaml**
- Deployment with 2 replicas
- Resource limits: 1GB RAM, 0.5 CPU
- Health checks on `/health` endpoint
- Environment variables from secrets

### **02-frontend-deployment.yaml**
- Deployment with 2 replicas
- Resource limits: 256MB RAM, 0.2 CPU
- Nginx-based static file serving
- Health checks on `/` endpoint

### **03-ingress.yaml**
- Nginx ingress controller
- Routes `/api` to backend, `/` to frontend
- SSL/TLS support ready
- LoadBalancer service for direct access

### **04-hpa.yaml**
- Horizontal Pod Autoscaler
- CPU/Memory based scaling
- Backend: 2-10 replicas
- Frontend: 2-8 replicas

## ⚡ Quick Deployment

### **Prerequisites**
```bash
# Install Azure CLI
curl -sL https://aka.ms/InstallAzureCLIDeb | sudo bash

# Install kubectl
az aks install-cli

# Login to Azure
az login
```

### **Manual Deployment**
```bash
# Get AKS credentials
az aks get-credentials --resource-group booklist-k8s --name booklist-aks

# Deploy all manifests
kubectl apply -f k8s/

# Check status
kubectl get all -n booklist

# Get external IP
kubectl get service booklist-loadbalancer -n booklist
```

### **Automated Deployment**
```bash
# Push to main branch triggers automatic deployment
git push origin main

# Monitor workflow
gh run watch

# Check deployment status
kubectl get pods -n booklist -w
```

## 🔍 Monitoring and Debugging

### **Pod Status**
```bash
# Check all pods
kubectl get pods -n booklist

# Describe problematic pod
kubectl describe pod <pod-name> -n booklist

# View logs
kubectl logs -f <pod-name> -n booklist
```

### **Service Discovery**
```bash
# Check services
kubectl get services -n booklist

# Test internal connectivity
kubectl exec -it <pod-name> -n booklist -- curl http://booklist-api-service/health
```

### **Ingress Debugging**
```bash
# Check ingress
kubectl get ingress -n booklist

# Describe ingress
kubectl describe ingress booklist-ingress -n booklist

# Check nginx controller logs
kubectl logs -n ingress-nginx -l app.kubernetes.io/name=ingress-nginx
```

## 📊 Scaling and Performance

### **Manual Scaling**
```bash
# Scale backend
kubectl scale deployment booklist-api -n booklist --replicas=5

# Scale frontend
kubectl scale deployment booklist-frontend -n booklist --replicas=3
```

### **Auto-scaling Metrics**
```bash
# Check HPA status
kubectl get hpa -n booklist

# View HPA details
kubectl describe hpa booklist-api-hpa -n booklist
```

### **Resource Usage**
```bash
# Node resource usage
kubectl top nodes

# Pod resource usage
kubectl top pods -n booklist
```

## 🔐 Security Best Practices

### **Network Policies**
```yaml
apiVersion: networking.k8s.io/v1
kind: NetworkPolicy
metadata:
  name: booklist-network-policy
  namespace: booklist
spec:
  podSelector: {}
  policyTypes:
  - Ingress
  - Egress
  ingress:
  - from:
    - namespaceSelector:
        matchLabels:
          name: ingress-nginx
  egress:
  - {}
```

### **Pod Security Context**
```yaml
securityContext:
  runAsNonRoot: true
  runAsUser: 1000
  fsGroup: 2000
  capabilities:
    drop:
    - ALL
```

### **Resource Limits**
- Always set resource requests and limits
- Use LimitRanges for namespace-level controls
- Monitor resource usage regularly

## 🌐 External Access Options

### **1. LoadBalancer (Current)**
- **URL**: `http://<EXTERNAL_IP>`
- **Pros**: Simple, direct access
- **Cons**: Costs money, no SSL termination

### **2. Ingress with Domain**
- **URL**: `https://booklist.yourdomain.com`
- **Setup**: Update ingress host, configure DNS
- **Pros**: Professional, SSL support
- **Cons**: Requires domain and certificate

### **3. Azure Application Gateway**
- **Integration**: AKS + Application Gateway
- **Features**: WAF, SSL offloading, path-based routing
- **Pros**: Enterprise features
- **Cons**: More complex, higher cost

## 🔄 CI/CD Pipeline Customization

### **Environment-specific Deployments**
```yaml
# Add to workflow for staging environment
- name: Deploy to Staging
  if: github.ref == 'refs/heads/develop'
  run: |
    sed -i 's/booklist/booklist-staging/g' k8s/*.yaml
    kubectl apply -f k8s/
```

### **Blue-Green Deployment**
```yaml
- name: Blue-Green Deployment
  run: |
    kubectl patch deployment booklist-api -n booklist -p '{"spec":{"template":{"metadata":{"labels":{"version":"green"}}}}}'
    kubectl rollout status deployment/booklist-api -n booklist
```

### **Canary Releases**
```yaml
- name: Canary Deployment
  run: |
    kubectl patch deployment booklist-api -n booklist -p '{"spec":{"replicas": 1}}'
    # Monitor metrics, then scale up if successful
```

## 🚨 Troubleshooting Guide

### **Common Issues**

#### **Pods Stuck in Pending**
```bash
# Check resource constraints
kubectl describe pod <pod-name> -n booklist

# Check node capacity
kubectl describe nodes
```

#### **Image Pull Errors**
```bash
# Verify ACR integration
az aks check-acr --resource-group booklist-k8s --name booklist-aks --acr booklistregistry2617

# Check image exists
az acr repository show-tags --name booklistregistry2617 --repository booklist-api
```

#### **Service Connection Issues**
```bash
# Test DNS resolution
kubectl exec -it <pod-name> -n booklist -- nslookup booklist-api-service

# Check endpoints
kubectl get endpoints -n booklist
```

### **Performance Issues**
```bash
# Check resource usage
kubectl top pods -n booklist

# Review HPA metrics
kubectl get hpa -n booklist --watch

# Examine logs for errors
kubectl logs -f deployment/booklist-api -n booklist
```

## 📈 Production Readiness

### **Before Going Live**
- [ ] Configure custom domain and SSL
- [ ] Set up monitoring and alerting
- [ ] Implement backup strategies
- [ ] Configure log aggregation
- [ ] Set up disaster recovery
- [ ] Perform security scanning
- [ ] Load testing
- [ ] Set up monitoring dashboards

### **Monitoring Stack** (Optional)
```bash
# Install Prometheus + Grafana
helm repo add prometheus-community https://prometheus-community.github.io/helm-charts
helm install prometheus prometheus-community/kube-prometheus-stack -n monitoring --create-namespace
```

This Kubernetes deployment provides a robust, scalable, and production-ready environment for the BookList application!