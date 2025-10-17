using Azure;
using Azure.Data.Tables;
using Microsoft.Extensions.Logging;
using Shaunebu.Azure.Storage.Services;
using System.Text;

// Table Storage Configuration 
string tableConnectionString = "DefaultEndpointsProtocol=yourUrl";
string tableName = "Users";


Console.WriteLine("🚀 Starting Azure Table Storage Example...");

var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
var blobLogger = loggerFactory.CreateLogger<AzureTableStorageService<Users>>();

// Initialize table service
var usersTable = new AzureTableStorageService<Users>(tableConnectionString, tableName, blobLogger);

// Ensure table exists
await usersTable.EnsureTableExistsAsync();

// Insert a new user
var user = new Users
{
    PartitionKey = "Users",
    RowKey = Guid.NewGuid().ToString(),
};

try
{
    await usersTable.AddEntityAsync(user);
    Console.WriteLine("✅ User inserted successfully");
}
catch (Exception ex)
{
    Console.WriteLine($"❌ Error inserting user: {ex.Message}");
}

// Query and print all users
var users = await usersTable.QueryEntitiesAsync();

foreach (var u in users)
{
    Console.WriteLine($"User: PartitionKey={u.PartitionKey}, RowKey={u.RowKey}");
}



// Blob Storage Configuration 
string blobConnectionString = "DefaultEndpointsProtocol=yourUrl";
string container = "test-container";
string blobName = "hello.txt";

var tableLogger = loggerFactory.CreateLogger<AzureBlobStorageService>();

var blobService = new AzureBlobStorageService(blobConnectionString, tableLogger);

try
{
    // Upload blob
    using var stream = new MemoryStream(Encoding.UTF8.GetBytes("Hello from Blob Storage!"));
    await blobService.UploadBlobFromStreamAsync(container, blobName, stream, overwrite: true, metadata: null, contentType: null);
    Console.WriteLine("✅ Blob uploaded successfully");

    // Download blob content
    var content = await blobService.DownloadBlobAsStringAsync(container, blobName);
    Console.WriteLine($"Blob content: {content}");

    // Delete the blob
    //await blobService.DeleteBlobAsync(container, blobName);
    //Console.WriteLine("✅ Blob deleted successfully");
}
catch (Exception ex)
{
    Console.WriteLine($"❌ Blob operation error: {ex.Message}");
}


Console.ReadLine();

public class Users : ITableEntity
{
    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
}