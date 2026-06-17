# Summary.API

A lightweight ASP.NET Core Web API for abstractive text summarization using Azure AI Language (Text Analytics).

## What this project does

- Accepts text input via HTTP API.
- Validates incoming requests.
- Splits large text into chunks when needed.
- Calls Azure Language Service to generate summaries.
- Returns a unified response model: `GeneralResponseT<T>`.
- Secures endpoints with API Key authentication (`X-Api-Key`).

## Project structure

- `src/Summary.API` - Web API layer (controllers, middleware, auth, startup).
- `src/Summary.Application` - application/business logic (services, validators).
- `src/Summary.Core` - contracts, models, exceptions.
- `src/Summary.Infrastructure` - Azure Text Analytics integration.

## Configuration

Set values in `src/Summary.API/appsettings.json` (prefer User Secrets or environment variables in real environments):

- `ApiKey:Key` - API key required to access this API.
- `TextAnalyticsClient:Endpoint` - Azure Language Service endpoint.
- `TextAnalyticsClient:ApiKey` - Azure Language Service API key.
- `Summarize:MaxInputLength` - maximum allowed input text length.
- `AzureLanguageService:DocumentSizeLimit` - chunk size limit for processing.
- `ApplicationInsights:ConnectionString` - optional, for telemetry.

## Run locally

### 1. Prerequisites

- .NET SDK 10

### 2. Restore and build

```bash
dotnet restore Summary.API.slnx
dotnet build Summary.API.slnx
```

### 3. Run the API

```bash
dotnet run --project src/Summary.API/Summary.API.csproj
```

After startup:

- Swagger (Development only): `https://localhost:<port>/swagger`
- Root endpoint: `GET /`

## API usage

### Authentication header

Include this header in requests:

```http
X-Api-Key: <your-api-key>
```

### Main endpoint

`POST /Summarize`

Request body: plain string.

### Alternative endpoint

`POST /api/Summarize`

Example request body:

```json
{
  "data": {
    "inputText": "Your large text to summarize..."
  }
}
```

## Notes

- Swagger is disabled by default outside Development.
- All controller endpoints except `GET /` require API key authorization.
