# Hacker News REST API Service

This is a REST API service for fetching and caching Hacker News stories. The service provides endpoints to get the best stories, individual stories by ID, and the top stories based on their scores.

## How to Run the Application

### Clone the Repository

```pwsh
git clone https://github.com/yourusername/hacker-news.git
cd hacker-news
```

### Build the Project

```pwsh
dotnet build src/hacker-news.sln
```

### Run the Application

```pwsh
dotnet run --project src/hacker-news/hacker-news.csproj
```

### Run the Test

#### Unit

```pwsh
dotnet test .\src\hacker-news.nunit\hacker-news.nunit.csproj
```

#### Integration

```pwsh
dotnet test .\src\hacker-news.nunit.integration\hacker-news.nunit.integration.csproj
```
### Access the API

The API will be available at [http://localhost:8080](http://localhost:8080). You can access the Swagger UI for API documentation at [http://localhost:8080/swagger](http://localhost:8080/swagger).

## Assumptions

- The Hacker News API endpoints are stable and provide the required data in the expected format.
- The Hacker News API endpoints handle properly incorrect and invalid responses (BadRequest, NotFound etc.).
- The application is running in a development environment with access to the internet to fetch data from the Hacker News API.
- The caching mechanism is sufficient for the current load and usage patterns.
- The first query will take longer as we have nothing in cache.

## Enhancements and Changes

Given more time, the following enhancements and changes could be made:
 - Proper error handling (probably a middleware to handle all possible error scenarios)
 - Confugrations extended (cache expiration, rate limiting, hacker-news API config)
 - Improved caching - auto cache refresh, distributed caching
 - Logging added
 - More tests added to cover all possible scenarios Hacker-News API has (I didn't go though all edge cases)
 - Performance Tests
 - If Hacker-news had multiple independent services we might have automated fallback configuration.
 - We can add rety policy (e.g. network issues)
 - Add monitoring 
 - Add authentication and authorization to secure the API endpoints.

## API Endpoints

- `GET /News/bestIds`: Returns the IDs of the best stories.
- `GET /News/{id}`: Returns the details of a story by its ID.
- `GET /News/top/{number}`: Returns the top N stories ordered by score.
