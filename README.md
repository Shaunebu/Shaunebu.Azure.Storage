Shaunebu.Azure.AppConfiguration 🌐✨
===================================

![NuGet Version](https://img.shields.io/nuget/v/Shaunebu.Azure.Storage?color=blue&label=NuGet)
![NET Support](https://img.shields.io/badge/.NET%20-%3E%3D8.0-blueviolet) ![NET Support](https://img.shields.io/badge/.NET%20CORE-%3E%3D3.1-blueviolet) ![NET Support](https://img.shields.io/badge/.NET%20MAUI-%20-blueviolet)

A unified library for interacting with Azure Blob Storage and Azure Table Storage.

Provides CRUD operations, querying, metadata management, SAS generation, batch operations, and logging via dependency injection.
This library is designed to simplify working with Azure Storage in multi-platform .NET applications.
* * *

🚀 Installation
---------------

Add the library to your project via NuGet:

`dotnet add package Shaunebu.Azure.Storage`


* * *


🔑 Main Features
----------------

| Feature | Azure Blob Storage | Azure Table Storage |
| --- | --- | --- |
| Upload | ✅ Streams & byte arrays | ❌ N/A |
| Download | ✅ Streams & string content | ❌ N/A |
| Metadata & ContentType | ✅ Supports metadata & content type | ❌ N/A |
| Delete | ✅ Blob deletion | ✅ Entity deletion |
| SAS URI | ✅ Generate SAS URI with expiry & permissions | ❌ N/A |
| CRUD Operations | ❌ Table not supported | ✅ Full CRUD |
| Querying | ❌ | ✅ LINQ & filters, paging, batch upsert |
| Logging | ✅ ILogger integration | ✅ ILogger integration |
| Dependency Injection | ✅ DI ready | ✅ DI ready |
| Multi-platform support | ✅ | ✅ |

* * *


🌐 Shaunebu.Azure.Storage.Blobs
----------------

⚡ Initialization
----------------

Initialize the service using your **connection string** and logger:

```csharp
using Shaunebu.Azure.Storage;
using Shaunebu.Azure.Storage.Abstractions;
using Microsoft.Extensions.Logging;

// Setup logger
var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
var logger = loggerFactory.CreateLogger<AzureBlobStorageService>();

string connectionString = "DefaultEndpointsProtocol=https;AccountName=...;AccountKey=...;EndpointSuffix=core.windows.net";
var blobService = new AzureBlobStorageService(connectionString, logger);
```

* * *

🔑 Main Properties
------------------

| Property | Type | Description |
| --- | --- | --- |
| `_blobServiceClient` | `BlobServiceClient` | Azure SDK client for interacting with blob storage |
| `_logger` | `ILogger` | Logs all operations and errors for better traceability |

* * *

🛠 Blob Operations
------------------

### ➕ Upload Blob (Stream or Bytes)

```csharp
using var stream = new MemoryStream(Encoding.UTF8.GetBytes("Hello World"));
await blobService.UploadBlobAsync("mycontainer", "myblob.txt", stream, contentType: "text/plain");

byte[] data = Encoding.UTF8.GetBytes("Hello Blob Storage");
await blobService.UploadBlobAsync("mycontainer", "myblob2.txt", data, metadata: new Dictionary<string, string> { { "Author", "John" } });
```

### 📥 Download Blob

```csharp
var downloadedStream = await blobService.DownloadBlobAsync("mycontainer", "myblob.txt");
using var reader = new StreamReader(downloadedStream!);
string content = await reader.ReadToEndAsync();
```

### 🗑 Delete Blob

```csharp
await blobService.DeleteBlobAsync("mycontainer", "myblob.txt");
```

* * *

🔑 Generate SAS URI
-------------------

Generate a **read-only SAS URI** valid for a specific duration:

```csharp
string sasUri = await blobService.GetBlobSasUriAsync(
    "mycontainer", 
    "myblob2.txt", 
    TimeSpan.FromHours(1)
);
```

You can also customize permissions:

```csharp
string sasUriWrite = await blobService.GetBlobSasUriAsync(
    "mycontainer", 
    "myblob2.txt", 
    TimeSpan.FromHours(1), 
    BlobSasPermissions.Read | BlobSasPermissions.Write
);
```

* * *

⚖️ Feature Comparison: Shaunebu.Azure.BlobStorage vs Azure SDK
--------------------------------------------------------------

| Feature | Shaunebu.Azure.BlobStorage 📦 | Azure SDK Official ⚡ |
| --- | --- | --- |
| Upload Stream / Bytes | ✅ Supports both with optional metadata & content type | ✅ Supported |
| Metadata & Content Type | ✅ Fully customizable per blob | ✅ Supported but requires manual options |
| Delete Blob | ✅ Simple method with logging | ✅ Supported |
| Download Blob | ✅ Returns Stream, with helper for string content | ✅ Supported |
| SAS URI generation | ✅ Easy, with permission and expiry options | ✅ Supported but manual |
| Logging | ✅ All operations logged via `ILogger` | ❌ Not included |
| Dependency Injection ready | ✅ Clean integration | ❌ Manual client creation needed |
| Container auto-create | ✅ Creates if not exists | ❌ Must call manually |
| Exception handling | ✅ Wraps RequestFailedException with context | ❌ Default SDK exceptions |

> **Tip:** Shaunebu.Azure.BlobStorage simplifies blob handling with metadata, content type, and SAS generation in a **single, DI-ready service**.

* * *

📌 Example Usage
----------------

```csharp
// Upload a blob from string
using var stream = new MemoryStream(Encoding.UTF8.GetBytes("Hello Blob Storage"));
await blobService.UploadBlobAsync("documents", "example.txt", stream, contentType: "text/plain");

// Upload with metadata
await blobService.UploadBlobAsync("documents", "example2.txt", Encoding.UTF8.GetBytes("Data"), metadata: new Dictionary<string, string> { { "Author", "John" } });

// Download blob as stream
var blobStream = await blobService.DownloadBlobAsync("documents", "example.txt");

// Read content as string
string content = await blobService.DownloadBlobAsStringAsync("documents", "example.txt");

// Generate SAS URI
string sasUri = await blobService.GetBlobSasUriAsync("documents", "example.txt", TimeSpan.FromHours(1));

// Delete blob
await blobService.DeleteBlobAsync("documents", "example.txt");
```

* * *

🚀 Platform Support
-------------------

Compatible with **.NET 6+** and **Azure.Storage.Blobs SDK**:

| Platform | Support | Notes |
| --- | --- | --- |
| 🖥 **Windows** | ✅ Full | Console, WPF, WinForms, MAUI |
| 🐧 **Linux** | ✅ Full | Console apps, APIs, microservices |
| 🍏 **macOS** | ✅ Full | Console apps, MAUI |
| 📱 **iOS / Android** | ✅ Full | MAUI apps via DI and backend |
| 🌐 **Blazor Server** | ✅ Full | Safe to use with secret keys |
| 🌐 **Blazor WASM** | ⚠️ Partial | Use backend proxy for connection strings |
| 🌐 **ASP.NET / ASP.NET Core** | ✅ Full | APIs, web apps, microservices |

* * *

✅ **Advantages**
*   Upload streams and byte arrays easily
    
*   Supports metadata and content type per blob
    
*   Generates SAS URIs with permissions and expiry
    
*   Container auto-creation
    
*   Logging integrated via `ILogger`
    
*   Clean dependency injection ready
    
*   Exception context provided for failed operations


<br>

## 🌐 Shaunebu.Azure.Storage.Tables

⚡ Initialization
----------------

Initialize the service using your **connection string** and table name:

```csharp
using Shaunebu.Azure.Storage;
using Shaunebu.Azure.Storage.Abstractions;
using Microsoft.Extensions.Logging;

// Inject logger via DI
var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
var logger = loggerFactory.CreateLogger<AzureTableStorageService<User>>();

string connectionString = "DefaultEndpointsProtocol=https;AccountName=...;AccountKey=...;EndpointSuffix=core.windows.net";
var tableService = new AzureTableStorageService<User>(connectionString, "Users", logger);

// Ensure table exists
await tableService.EnsureTableExistsAsync();
```

* * *

🔑 Main Properties
------------------

| Property | Type | Description |
| --- | --- | --- |
| `_tableClient` | `TableClient` | Azure SDK client for interacting with the table |
| `_logger` | `ILogger` | Logs all operations and errors for better traceability |

* * *

🛠 CRUD Operations
------------------

### ➕ Add Entity

```csharp
var user = new User
{
    PartitionKey = "USERS",
    RowKey = Guid.NewGuid().ToString(),
    Name = "John Doe",
    Email = "john@example.com"
};

await tableService.AddEntityAsync(user);
```

### ✏️ Upsert Entity

```csharp
user.Email = "john.doe@example.com";
await tableService.UpsertEntityAsync(user);
```

### 🗑 Delete Entity

```csharp
await tableService.DeleteEntityAsync(user.PartitionKey, user.RowKey);
```

### 📥 Get Entity

```csharp
var existingUser = await tableService.GetEntityAsync(user.PartitionKey, user.RowKey);
```

* * *

🔍 Query Entities
-----------------

### By Filter (OData syntax)

```csharp
var allUsers = await tableService.QueryEntitiesAsync("PartitionKey eq 'USERS'");
```

### By Expression

```csharp
var filteredUsers = await tableService.QueryEntitiesAsync(u => u.Name.Contains("John"));
```

### Paged Query

```csharp
var (items, continuationToken) = await tableService.QueryPagedAsync("PartitionKey eq 'USERS'", pageSize: 50);
```

* * *

⚡ Batch Operations
------------------

Supports batch upserts (max 100 entities per transaction):

```csharp
var users = new List<User> { user1, user2, user3 };
await tableService.BatchUpsertAsync(users);
```

* * *

⚖️ Feature Comparison: Shaunebu.Azure.TableStorage vs Azure SDK
---------------------------------------------------------------

| Feature | Shaunebu.Azure.TableStorage 🗄️ | Azure SDK Official ⚡ |
| --- | --- | --- |
| CRUD operations | ✅ Add, Upsert, Delete, Get | ✅ Basic CRUD |
| Query filters | ✅ Supports OData and LINQ expressions | ✅ Supported but requires more code |
| Batch operations | ✅ Upsert batches with 100 entities per transaction | ❌ Must implement manually |
| Paged queries | ✅ Built-in support with continuation tokens | ❌ Must implement manually |
| Logging | ✅ Logs all operations via `ILogger` | ❌ Not included |
| Dependency Injection ready | ✅ Clean DI integration | ❌ Manual client creation needed |
| Multi-environment usage | ✅ Pass different connection strings/tables via DI | ❌ Manual setup |

> **Tip:** Shaunebu.Azure.TableStorage simplifies working with Azure Table Storage, especially for batch operations, filtering, and multi-environment enterprise scenarios.

* * *

📌 Example Usage
----------------

```csharp
// Initialize service
await tableService.EnsureTableExistsAsync();

// Add user
await tableService.AddEntityAsync(user);

// Update user
user.Email = "new.email@example.com";
await tableService.UpsertEntityAsync(user);

// Query users
var johnUsers = await tableService.QueryEntitiesAsync(u => u.Name.Contains("John"));

// Batch upsert
await tableService.BatchUpsertAsync(new List<User> { user1, user2 });

// Delete user
await tableService.DeleteEntityAsync(user.PartitionKey, user.RowKey);
```

* * *

🚀 Platform Support
-------------------

Compatible with **.NET 6+** and **Azure.Data.Tables SDK**:

| Platform | Support | Notes |
| --- | --- | --- |
| 🖥 **Windows** | ✅ Full | Console, WPF, WinForms, MAUI |
| 🐧 **Linux** | ✅ Full | Console apps, APIs, microservices |
| 🍏 **macOS** | ✅ Full | Console apps, MAUI |
| 📱 **iOS / Android** | ✅ Full | MAUI apps via DI and backend |
| 🌐 **Blazor Server** | ✅ Full | Safe to use with secret keys |
| 🌐 **Blazor WASM** | ⚠️ Partial | Use backend proxy for connection strings |
| 🌐 **ASP.NET / ASP.NET Core** | ✅ Full | APIs, web apps, microservices |

* * *

✅ **Advantages**
*   Full CRUD support, including batch operations
    
*   Supports LINQ and OData filters
    
*   Continuation token support for large tables
    
*   Logging built-in via `ILogger`
    
*   Clean dependency injection ready
    
*   Multi-environment enterprise friendly