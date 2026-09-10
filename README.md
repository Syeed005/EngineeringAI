# EngineeringAI — Azure Enterprise RAG & Agent Integration

## Overview

EngineeringAI is an enterprise-focused AI application that demonstrates how existing engineering data and documents can be integrated with Microsoft Azure to provide secure, grounded, AI-assisted access to organizational knowledge.

The project addresses a common enterprise challenge:

> How can an organization introduce AI capabilities across existing engineering data and applications without replacing the systems that already contain valuable business information?

Rather than treating the LLM as the application, this project focuses on the complete system around it:

- enterprise data integration;
- relational engineering data;
- document ingestion;
- REST APIs;
- cloud deployment;
- retrieval and search;
- Retrieval-Augmented Generation (RAG);
- agentic workflows;
- security;
- testing;
- CI/CD;
- telemetry and observability.

The objective is to demonstrate how traditional enterprise software engineering and modern AI capabilities can be combined into a maintainable architecture.

---

## The Business Problem

Engineering organizations often store critical information across multiple systems and formats.

Examples include:

- SQL databases;
- equipment records;
- project information;
- supplier data;
- engineering deliverables;
- PDF documents;
- technical specifications;
- procedures and manuals;
- engineering applications;
- external enterprise systems.

A user trying to answer a relatively simple engineering question may need to search several applications, databases, and documents manually.

Traditional enterprise applications solve part of this problem through APIs, reports, search interfaces, and system integrations.

Generative AI introduces another possibility:

> Allow users to ask questions naturally while grounding the response in authoritative enterprise data.

However, connecting an LLM directly to enterprise information introduces architectural challenges involving retrieval quality, security, permissions, system integration, observability, reliability, and data governance.

EngineeringAI was built to explore those challenges as an end-to-end engineering system rather than as a standalone chatbot.

---

## Project Goals

The project was designed around the following goals:

1. Model realistic engineering data in SQL Server.
2. Expose enterprise data through maintainable ASP.NET Core REST APIs.
3. Integrate the application with Microsoft Azure.
4. Support both structured engineering data and unstructured engineering documents.
5. Build a document ingestion and retrieval pipeline.
6. Implement semantic/vector search for engineering knowledge.
7. Use RAG to generate source-grounded answers.
8. Extend retrieval into multi-step agentic workflows.
9. Apply enterprise security patterns.
10. Implement automated testing and CI/CD.
11. Add application telemetry and observability.
12. Maintain clear boundaries between application, infrastructure, retrieval, and AI components.

---

## Architecture

The solution combines traditional enterprise application architecture with an AI retrieval layer.

```text
                   Existing Engineering Environment
                              |
             +----------------+----------------+
             |                                 |
             v                                 v
     Structured Data                     Documents
     SQL / Engineering DB                PDF / Files
             |                                 |
             v                                 v
      ASP.NET Core APIs                 Blob Storage
             |                                 |
             |                          Text Extraction
             |                                 |
             |                              Chunking
             |                                 |
             |                            Embeddings
             |                                 |
             |                                 v
             |                         Azure AI Search
             |                                 |
             +---------------+-----------------+
                             |
                             v
                    Retrieval / Context
                             |
                             v
                   Azure OpenAI / LLM
                             |
                             v
                    RAG / AI Services
                             |
                             v
                    Agent / Tool Layer
                             |
                             v
                   Enterprise Application
```

The architecture intentionally separates:

```text
Enterprise Data
      ↓
Integration
      ↓
Retrieval
      ↓
AI Reasoning
      ↓
Application
```

This makes it possible to change individual components without redesigning the complete solution.

---

## Technology Stack

### Backend

- C#
- .NET 10
- ASP.NET Core
- Entity Framework Core
- REST APIs
- Dependency Injection
- FluentValidation

### Data

- SQL Server
- Azure SQL Database
- Entity Framework Core
- Relational data modeling

### Azure

- Azure App Service
- Azure Functions
- Azure SQL
- Azure Blob Storage
- Azure AI Search
- Azure OpenAI / Microsoft Foundry
- Azure Key Vault
- Managed Identity
- Azure RBAC
- Application Insights

### Integration

- REST APIs
- Azure Functions
- Event-driven integration patterns
- Enterprise data services

### AI

- Large Language Models
- Embeddings
- Vector Search
- Retrieval-Augmented Generation (RAG)
- Semantic retrieval
- Context grounding
- Citations
- Function/tool calling
- Agentic workflows

### DevOps & Quality

- Git
- GitHub
- GitHub Actions
- CI/CD
- Unit Testing
- Integration Testing
- Functional Testing
- OpenTelemetry
- Application Insights

---

## Engineering Data Model

The application models structured engineering information using relational data.

Representative entities include:

```text
Projects
   |
   +---- Equipment
   |
   +---- Suppliers
   |
   +---- Deliverables
```

These entities represent the type of structured information commonly found in engineering and enterprise systems.

The purpose of including relational engineering data is important:

**Enterprise AI rarely operates only on documents.**

Real business questions may require information from:

- relational databases;
- APIs;
- documents;
- engineering systems;
- external services.

The architecture therefore treats structured data and unstructured knowledge as complementary sources.

---

## Enterprise REST API Layer

ASP.NET Core APIs provide controlled access to engineering data.

The API layer includes patterns such as:

- GET endpoints;
- filtering;
- sorting;
- pagination;
- POST;
- PUT;
- DELETE;
- validation;
- duplicate detection;
- structured error handling;
- ProblemDetails;
- dependency injection;
- logging.

Example conceptual request:

```http
GET /api/equipment?projectId=1001&status=Active
```

Instead of allowing AI components to directly manipulate databases, APIs can provide a controlled boundary between AI workflows and enterprise systems.

This becomes especially important when agentic workflows are introduced.

---

## Why Use APIs Instead of Direct Database Access?

An AI agent technically could be given direct access to a database.

That does not mean it should be.

A controlled API/tool boundary allows the application to enforce:

- validation;
- authorization;
- business rules;
- logging;
- input constraints;
- auditing;
- predictable outputs;
- separation of concerns.

The design principle is:

> The model can decide which approved capability is needed, but application code remains responsible for executing that capability safely.

---

## Cloud Migration and Integration

A major part of the project was moving from a local development environment toward a cloud-hosted enterprise architecture.

The development path included:

```text
Local SQL Server
      ↓
ASP.NET Core Application
      ↓
Azure SQL
      ↓
Azure App Service
      ↓
Application Insights / Telemetry
```

This required addressing practical deployment concerns such as:

- database configuration;
- Entity Framework migrations;
- application configuration;
- cloud connectivity;
- deployment failures;
- telemetry;
- health monitoring;
- environment-specific settings.

The objective was not simply to deploy an application to Azure, but to understand how the application behaves once it leaves the developer workstation.

---

## Document RAG Pipeline

Structured SQL data solves only part of the enterprise information problem.

Engineering organizations also maintain significant knowledge in documents.

The RAG pipeline extends the architecture to support those sources.

```text
Engineering Documents
        ↓
Azure Blob Storage
        ↓
Document Download
        ↓
Text Extraction
        ↓
Text Cleanup
        ↓
Chunking
        ↓
Embedding Generation
        ↓
Azure AI Search
        ↓
Vector / Hybrid Retrieval
        ↓
Context Construction
        ↓
LLM
        ↓
Grounded Response + Citation
```

The application retrieves relevant enterprise content before asking the LLM to generate the final answer.

This reduces reliance on the model's general pretrained knowledge for organization-specific questions.

---

## Why RAG?

Without retrieval:

```text
User Question
      ↓
LLM
      ↓
General Model Answer
```

The model may provide a fluent answer without access to the organization's actual engineering information.

With RAG:

```text
User Question
      ↓
Enterprise Search
      ↓
Relevant Engineering Context
      ↓
LLM
      ↓
Grounded Answer
```

The AI system becomes an interface over enterprise knowledge rather than simply a general-purpose chatbot.

---

## Vector and Hybrid Search

Semantic retrieval allows the system to locate relevant information even when the user's wording does not exactly match the source document.

Conceptually:

```text
User Question
      ↓
Embedding
      ↓
Vector Similarity Search
      ↓
Relevant Chunks
```

However, vector similarity is not always sufficient.

Enterprise retrieval can benefit from combining:

```text
Keyword Search
      +
Vector Search
      +
Metadata Filtering
      +
Semantic Ranking
```

This creates a hybrid retrieval strategy that can handle both semantic meaning and exact engineering terminology.

Examples of exact terminology that may matter include:

- equipment identifiers;
- project numbers;
- document numbers;
- supplier names;
- engineering tags;
- revision numbers.

---

## Metadata-Aware Retrieval

Enterprise documents contain business context beyond their text.

Useful metadata can include:

```text
DocumentId
Title
DocumentType
Project
Revision
Version
EffectiveDate
EquipmentTag
Source
PageNumber
SecurityClassification
```

Metadata enables retrieval to answer not only:

> Which text is semantically similar?

but also:

> Which information is relevant, current, authoritative, and appropriate for this user?

This becomes increasingly important as the knowledge base grows.

---

## Agentic Workflows

RAG primarily answers questions using retrieved context.

Some enterprise tasks require actions across multiple systems.

For example:

```text
User Request
     ↓
Agent
     ↓
Determine Required Steps
     ↓
Retrieve Project Information
     ↓
Call Equipment API
     ↓
Validate Result
     ↓
Retrieve Supporting Document
     ↓
Generate Response
```

The project explores multi-step AI workflows where the model can select approved tools for specific tasks.

Potential tools can include:

```text
SearchDocuments()
GetProject()
GetEquipment()
GetSupplier()
GetDeliverables()
```

The important distinction is that the LLM does not directly receive unrestricted access to enterprise infrastructure.

Instead:

```text
LLM
 ↓
Tool Request
 ↓
Application Validation
 ↓
Authorized API / Service
 ↓
Result
 ↓
LLM
```

This provides a safer boundary for enterprise AI.

---

## Security Architecture

Enterprise AI security requires more than protecting an API key.

The architecture explores Azure security patterns including:

- Managed Identity;
- Azure RBAC;
- Azure Key Vault;
- application authorization;
- controlled service access;
- secure configuration;
- separation of application and AI permissions.

A preferred pattern is:

```text
Application
     ↓
Managed Identity
     ↓
Azure Resource
```

rather than:

```text
Application
     ↓
Hard-Coded Secret
     ↓
Azure Resource
```

Secrets that are required should be stored in an appropriate secret-management system rather than source code or configuration committed to Git.

---

## Authorization and AI

Authentication answers:

> Who is the user?

Authorization answers:

> What is the user allowed to access?

This distinction becomes particularly important with RAG.

A search index may contain documents from multiple departments or projects.

The fact that a document exists in the index does not mean every user should be allowed to retrieve it.

A production architecture should therefore consider:

```text
User Identity
      ↓
Authorization Context
      ↓
Search Filter
      ↓
Permitted Documents
      ↓
RAG
```

rather than retrieving everything and asking the LLM not to reveal restricted information.

---

## CI/CD

The project uses GitHub Actions to automate software delivery activities.

The pipeline includes concepts such as:

```text
Code Commit
     ↓
Build
     ↓
Automated Tests
     ↓
Artifact
     ↓
Authentication to Azure
     ↓
Deployment
```

The project also explores OIDC-based Azure authentication to reduce dependence on long-lived deployment credentials.

CI/CD is treated as part of the application architecture rather than an activity performed only after development is complete.

---

## Testing Strategy

Enterprise AI applications still require traditional software testing.

The project includes multiple testing levels:

### Unit Tests

Used to validate isolated business/application logic.

### Integration Tests

Used to verify interactions between application components and data layers.

### Functional Tests

Used to validate API behavior through the running application boundary.

AI features introduce additional evaluation requirements because model outputs are probabilistic.

This means a mature AI application requires both:

```text
Traditional Software Testing
             +
AI Evaluation
```

Traditional tests answer questions such as:

> Did the API return the expected status code?

AI evaluation asks different questions:

> Was the answer grounded?

> Was the correct source retrieved?

> Did the model use the appropriate tool?

> Was the response useful and accurate?

---

## Observability

Once an application is deployed, successful execution cannot be assumed.

The project uses Application Insights and OpenTelemetry concepts to observe application behavior.

Useful telemetry includes:

- requests;
- failures;
- exceptions;
- dependency calls;
- latency;
- database operations;
- external service calls;
- AI request latency;
- retrieval behavior.

For an AI system, observability eventually needs to extend across the complete pipeline:

```text
User Request
     ↓
Retrieval
     ↓
Retrieved Chunks
     ↓
Prompt / Context
     ↓
Model
     ↓
Tool Calls
     ↓
Response
```

This helps distinguish between different failure modes.

For example, a poor answer may originate from:

- poor source data;
- bad extraction;
- weak chunking;
- incorrect retrieval;
- stale documents;
- prompt construction;
- tool failure;
- model behavior.

---

## Engineering Challenges

### 1. Integrating Existing Enterprise Data

Enterprise systems rarely start as clean greenfield environments.

Data already exists in:

- databases;
- APIs;
- files;
- documents;
- legacy engineering applications.

The architecture therefore needs to integrate with existing systems rather than assume they can simply be replaced.

---

### 2. Structured vs. Unstructured Data

SQL data and documents require different retrieval strategies.

A relational query may be appropriate for:

> Show equipment belonging to Project X.

RAG may be more appropriate for:

> What does the engineering procedure say about equipment inspection?

An agentic workflow may eventually combine both.

---

### 3. Retrieval Quality

Generating embeddings does not automatically produce a good RAG system.

Retrieval quality depends on:

- extraction;
- chunking;
- embedding model;
- metadata;
- search configuration;
- filters;
- ranking;
- query formulation.

Poor retrieval creates poor context even when the LLM itself is capable.

---

### 4. Security Boundaries

Giving an AI system access to enterprise tools creates a new security boundary.

The architecture therefore separates:

```text
Model Reasoning
```

from:

```text
Permission to Execute
```

A model requesting an operation should not itself constitute authorization to perform that operation.

---

### 5. Production Troubleshooting

Moving from local development to Azure introduced issues that do not necessarily appear during local execution.

Examples include:

- environment configuration;
- database connectivity;
- deployment configuration;
- application startup failures;
- telemetry configuration;
- cloud resource permissions.

Troubleshooting these issues was an important part of the project because production engineering involves more than writing application code.

---

## Architecture Trade-Offs

The project deliberately evaluates when to use managed Azure services and when simpler application components are sufficient.

### Benefits of the Azure Architecture

- Managed application hosting
- Managed relational database
- Scalable document storage
- Enterprise search capabilities
- Integrated identity/security
- Centralized telemetry
- CI/CD integration
- Easier evolution toward production scale

### Costs and Constraints

Cloud architecture also introduces:

- service cost;
- configuration complexity;
- identity and permission management;
- dependency on cloud services;
- operational governance;
- environment management.

For example, a dedicated enterprise search service may be justified for a large production knowledge base but unnecessary for a very small prototype.

Architecture should therefore be driven by actual requirements rather than by maximizing the number of cloud services used.

---

## Cost-Aware Engineering

An important part of this project is evaluating architecture under realistic resource constraints.

AI applications can incur costs from:

- LLM tokens;
- embeddings;
- search infrastructure;
- application hosting;
- databases;
- storage;
- telemetry.

The project therefore treats cost as an architectural concern.

Potential optimization strategies include:

- chunk only when required;
- avoid regenerating unchanged embeddings;
- retrieve only the context needed;
- select models according to task complexity;
- control prompt/context size;
- monitor token consumption;
- scale infrastructure according to actual workload.

A technically sophisticated architecture is not automatically a good architecture if its operating cost is unjustified by the business value it produces.

---

## What This Project Demonstrates

This project demonstrates experience across the complete path from traditional enterprise software to applied AI:

- C# / .NET development
- ASP.NET Core REST APIs
- SQL Server and Azure SQL
- Relational data modeling
- Enterprise system integration
- Microsoft Azure
- Azure App Service
- Azure Functions
- Blob Storage
- Azure AI Search
- Azure OpenAI / Microsoft Foundry
- RAG
- Embeddings
- Vector and hybrid retrieval
- Agentic workflows
- Function/tool calling
- Managed Identity
- RBAC
- Key Vault
- CI/CD
- GitHub Actions
- Unit/integration/functional testing
- Application Insights
- OpenTelemetry
- Production troubleshooting
- Architecture trade-off analysis

---

## What I Learned

The primary lesson from this project is that enterprise AI is primarily a **systems engineering problem**, not simply an LLM problem.

A useful enterprise AI solution requires coordination between:

```text
Business Requirements
        +
Existing Systems
        +
Enterprise Data
        +
APIs / Integrations
        +
Retrieval
        +
AI Models
        +
Security
        +
Testing
        +
Deployment
        +
Observability
```

The LLM is only one component of that system.

Another important lesson is that AI should not automatically replace existing enterprise architecture.

In many situations, the better approach is:

> Understand the existing environment, preserve systems that already work, introduce controlled integration boundaries, add AI where it creates value, measure the result, and evolve the architecture as requirements become clearer.

---

## Project Evolution

The project has been developed incrementally rather than attempting to build every capability at once.

```text
Enterprise Data Model
        ↓
REST APIs
        ↓
Testing
        ↓
Azure SQL
        ↓
Azure Deployment
        ↓
Security
        ↓
CI/CD
        ↓
Observability
        ↓
Document Ingestion
        ↓
Embeddings
        ↓
Enterprise Search
        ↓
RAG
        ↓
Agentic Workflows
```

This incremental approach allows each architectural layer to be validated before additional complexity is introduced.

---

## Future Improvements

Potential future work includes:

- multi-document RAG;
- hybrid keyword/vector retrieval;
- metadata filtering;
- semantic reranking;
- authorization-aware retrieval;
- structured + unstructured data orchestration;
- additional agent tools;
- human-in-the-loop approval for sensitive actions;
- retrieval and groundedness evaluation;
- prompt/model observability;
- cost and latency monitoring;
- multi-agent workflow evaluation;
- Docker/container deployment;
- reusable enterprise connectors.

---

## Why I Built This

I built EngineeringAI to explore how AI can be introduced into an existing enterprise engineering environment without treating the customer's current systems as obstacles that must first be replaced.

The project starts with conventional enterprise software engineering — relational data, APIs, integration, testing, deployment, security, and observability — and then adds retrieval and AI capabilities on top of those foundations.

The objective is not simply to demonstrate that an LLM can answer a question.

The objective is to demonstrate how an engineer can move from:

```text
Existing Enterprise Systems
           ↓
Understand the Data
           ↓
Build Integration Boundaries
           ↓
Move / Expose Required Data
           ↓
Deploy Secure Cloud Services
           ↓
Add Retrieval
           ↓
Add AI
           ↓
Add Agentic Capabilities
           ↓
Evaluate and Observe
           ↓
Deliver Useful Enterprise Outcomes
```

This reflects the engineering approach I would use in a real customer environment:

**understand what already exists, identify the business problem, integrate rather than unnecessarily replace, deliver value incrementally, and build a path from prototype to production.**
