# Use Node.js 20 as the base image for building
FROM node:20-alpine AS build

# Set the working directory
WORKDIR /app

# Copy package.json and package-lock.json
COPY package*.json ./

# Install dependencies (including devDependencies for build)
RUN npm ci

# Copy the rest of the application code
COPY . ./

# Build the Angular application for production
RUN npm run build

# Use Nginx to serve the built application
FROM nginx:alpine AS runtime

# Copy the built Angular app from the build stage to Nginx's html directory
COPY --from=build /app/dist/book-list-app /usr/share/nginx/html

# Copy custom Nginx configuration
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Expose port 80
EXPOSE 80

# Start Nginx
CMD ["nginx", "-g", "daemon off;"]
