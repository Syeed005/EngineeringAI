# EngineeringAI

EngineeringAI is a cloud-based engineering application built with ASP.NET Core, .NET 10, Azure SQL, Azure App Service, and Azure monitoring services.

The project is being developed using Clean Architecture principles with automated testing and CI/CD.

## Architecture

```
Existing Customer/Engineering Systems
↓
Integration / Migration Layer
↓
Azure SQL + Blob/Documents
↓
Extraction & Chunking
↓
Embeddings / Azure AI Search
↓
RAG
↓
Agent / Tools
↓
Enterprise Application
```

## Tech Stack

- **Runtime:** .NET 10
- **Framework:** ASP.NET Core
- **Database:** Azure SQL
- **Storage:** Azure Blob Storage
- **Search:** Azure AI Search
- **Hosting:** Azure App Service
- **Monitoring:** Azure Monitor
- **Architecture:** Clean Architecture
- **Testing:** Automated Testing
- **CI/CD:** Automated Pipelines
