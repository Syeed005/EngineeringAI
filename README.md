# EngineeringAI

An enterprise AI solution that augments existing engineering systems without requiring source-system replacement. EngineeringAI integrates and migrates relevant customer and engineering data into Azure, exposes data through .NET APIs, and implements retrieval-augmented generation (RAG) with agentic workflows for intelligent multi-step data retrieval and interaction.

The solution is built with production-ready patterns including authentication, security, telemetry, automated testing, and CI/CD pipelines.

## Solution Overview

Rather than replacing existing systems, EngineeringAI provides a non-disruptive bridge that:

- **Preserves existing infrastructure** by integrating with current engineering and customer systems
- **Consolidates data** through a managed migration and extraction pipeline into Azure
- **Exposes APIs** for programmatic access to engineering data via .NET
- **Augments intelligence** using RAG (Retrieval-Augmented Generation) with Azure AI Search and Azure OpenAI
- **Enables workflows** through agentic patterns for complex, multi-step retrieval and data interaction
- **Operates at scale** with enterprise-grade authentication, security, observability, and reliability patterns

## Architecture

```
Existing Customer/Engineering Systems
↓
Integration / Migration Layer
↓
Azure SQL + Blob Storage
↓
Data Extraction & Chunking
↓
Embeddings & Azure AI Search
↓
RAG Pipeline
↓
Agentic Workflows
↓
Enterprise Application & APIs
```

## Tech Stack

- **Runtime:** .NET 10
- **Framework:** ASP.NET Core
- **Database:** Azure SQL
- **Storage:** Azure Blob Storage
- **Search:** Azure AI Search
- **AI/LLM:** Azure OpenAI
- **Hosting:** Azure App Service
- **Monitoring:** Azure Monitor
- **Architecture:** Clean Architecture
- **Testing:** Automated Unit & Integration Tests
- **CI/CD:** Automated Pipelines

## Key Capabilities

**Data Integration & Migration**
- Non-disruptive extraction from existing engineering systems
- Managed data pipelines into Azure SQL and Blob Storage
- Structured chunking for optimal RAG performance

**RAG & Search**
- Vector embeddings via Azure OpenAI
- Semantic search powered by Azure AI Search
- Context-aware retrieval for accurate responses

**Agentic Workflows**
- Multi-step reasoning and retrieval patterns
- Tool use and dynamic data interaction
- Stateful conversation management

**Enterprise Readiness**
- Authentication and authorization patterns
- Secure credential and secret management
- Comprehensive telemetry and logging
- Automated testing and deployment pipelines
