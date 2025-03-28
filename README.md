# CRM System

This project is a CRM (Customer Relationship Management) system with a backend built using ASP.NET Core and a frontend built using React and TypeScript.

## How to Run the Project

Follow these steps to set up and run the project:

### 1. Start the PostgreSQL Database

The project uses a PostgreSQL database. Use the provided `compose.yml` file to start a PostgreSQL Docker container.

Run the following command in the project root directory:

```sh
docker-compose up -d
```

This will start a PostgreSQL container with the necessary configuration.

### 2. Run the Backend (Web API)

Navigate to the `backend/CRMSystem.Api` directory and run the following commands to start the ASP.NET Core Web API:

```sh
dotnet restore
dotnet build
dotnet run
```

The Web API will start and listen on the configured port (default is http://localhost:3001).

### 3. Run the Frontend Application

Navigate to the `frontend` directory and install the dependencies if you haven't already:

```sh
npm install
```

Then, start the frontend development server:

```sh
npm run dev
```

The frontend application will start and be accessible at http://localhost:3000.

### Notes

- Ensure that you have Docker, .NET SDK, and Node.js installed on your system.
- The backend and frontend should be running simultaneously for the application to function correctly.
- The backend uses a PostgreSQL database, so ensure the database container is running before starting the backend.

### Screenshots

<img width="1008" alt="image" src="https://github.com/user-attachments/assets/c72acc57-d57e-4bba-85b3-e0c8f03c69f9" />

<img width="439" alt="image" src="https://github.com/user-attachments/assets/aa9b85f5-4bfa-4523-b55b-d33fb1e49bd5" />

<img width="991" alt="image" src="https://github.com/user-attachments/assets/14bef605-00a7-4658-9e72-b6705ee66750" />
