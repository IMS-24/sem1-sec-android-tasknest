# Step 1: Use the official Node.js image as the base
FROM node:22

# Step 2: Set the working directory in the container
WORKDIR /app

# Step 3: Copy package.json and package-lock.json (or yarn.lock)
COPY package*.json ./

# Step 4: Install dependencies
RUN npm install

# Step 5: Copy the entire source code to the working directory
COPY . .

# Step 6: Build the project
RUN npm run build

# Step 7: Expose the port that the NestJS application will run on
EXPOSE 3000

# Step 8: Start the application
CMD ["npm", "run", "start:prod"]
