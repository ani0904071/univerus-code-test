FROM node:18-alpine

WORKDIR /app

# Copy both files
COPY package.json package-lock.json ./

# Use npm ci for clean install
RUN npm ci

# Optional: install serve globally
RUN npm install -g serve

# Copy the rest of the code
COPY . .

# Build the app
RUN npm run build

EXPOSE 3000

CMD [ "serve", "-s", "dist" ]
