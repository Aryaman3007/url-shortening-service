# URL Shortening Service

A modern URL shortener built with React + Material-UI frontend and ASP.NET Core backend, featuring a clean interface and PostgreSQL database.

## Features

- 🔗 Create shortened URLs with auto-generated codes
- 📊 Track access counts and usage statistics
- ✏️ Update or delete existing shortened URLs
- 📱 Responsive Material-UI interface
- � Instant copy-to-clipboard with notifications
- 🚀 Fast redirects to original URLs

## Tech Stack

### Frontend
- React 19
- Material-UI v7
- Axios for API calls
- Modern ES6+ JavaScript

### Backend
- ASP.NET Core 9
- Entity Framework Core
- PostgreSQL database
- RESTful API design

## Quick Start

### Prerequisites
- .NET 8 SDK
- Node.js (Latest LTS)
- PostgreSQL 15+

### Environment Setup
1. Copy example environment files:
   ```bash
   # Backend
   cp Backend/.env.example Backend/.env
   # Frontend
   cp Frontend/.env.example Frontend/.env
   ```
2. Update the Backend/.env file with your database credentials:
   ```
   DB_HOST=localhost
   DB_PORT=5432
   DB_NAME=MyDatabase
   DB_USER=your_username
   DB_PASSWORD=your_password
   ```
3. Update the Frontend/.env file if needed:
   ```
   REACT_APP_API_URL=http://localhost:5191
   ```
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=MyDatabase;Username=postgres;Password=your_password"
  }
}
```

### Backend Setup
```bash
cd Backend
dotnet restore
dotnet ef database update
dotnet run
```
The API will run at http://localhost:5191

### Frontend Setup
```bash
cd Frontend
npm install
npm start
```
Frontend runs at http://localhost:3000

## API Endpoints

All endpoints are under the `/shorten` base path:

- `POST /` - Create short URL
  - Body: `{ "url": "https://example.com" }`
  - Returns: Short URL

- `GET /{shortCode}` - Redirect to original URL
  - Automatically increments access count

- `GET /{shortCode}/stats` - Get URL statistics
  - Returns: Creation date, access count, etc.

- `PUT /{shortCode}` - Update existing URL
  - Body: `{ "url": "https://new-url.com" }`

- `DELETE /{shortCode}` - Delete shortened URL

## Development Notes

- Frontend uses proxy configuration to forward API requests
- Database migrations are included in the repository
- Supports both HTTP (5191) and HTTPS (7060) endpoints
- Material-UI theming for consistent design
- Copy-to-clipboard functionality with success notifications

## Common Issues

- If database connection fails, verify PostgreSQL is running and credentials are correct
- For HTTPS development, trust the ASP.NET development certificate:
  ```bash
  dotnet dev-certs https --trust
  ```
- If npm start fails, ensure Node.js is up to date and try clearing npm cache:
  ```bash
  npm cache clean --force
  ```

## License

MIT

## Acknowledgments

- Project inspired by [roadmap.sh](https://roadmap.sh/projects/url-shortening-service)
- Material-UI components and themes
- Entity Framework Core for PostgreSQL
