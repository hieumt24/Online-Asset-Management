# AssetManagement

## Overview

AssetManagement is a comprehensive solution designed to streamline the management of various assets within an organization. The project includes both backend and frontend components, facilitating efficient tracking, updating, and maintenance of assets.

## Features

- **Asset Tracking**: Monitor the status and location of assets.
- **Maintenance Scheduling**: Plan and track maintenance activities.
- **User Management**: Assign roles and permissions for different users.
- **Reporting**: Generate detailed reports on asset performance and status.

## Technology Stack

- **Backend**: C#
- **Frontend**: TypeScript, React
- **Database**: SQL Server

## Installation

### Prerequisites

- .NET SDK
- Node.js
- SQL Server

### Steps

1. **Clone the repository**:

   ```bash
   git clone https://github.com/phamthanhhungpro/AssetManagement.git
   cd AssetManagement

   ```

2. **Backend Setup**:

   ```bash
   cd backend
   cd src
   dotnet restore
   dotnet build
   dotnet run

   ```

3. **Frontend Setup**:

   ```bash
   cd frontend
   bun install or npm install
   bun run dev or npm start

   ```

4. **Database Setup**:

- Configure the connection string in `appsettings.Development.json`.
- Run the database migrations

Usage

- Access the application at http://localhost:3000.
- Log in with your credentials.
- Navigate through the dashboard to manage assets.
