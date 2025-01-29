using Azure.Storage.Blobs;
using AzureBlobApiTest.Repository.File;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IFileRepository, FileRepository>();

// Obter configurações do appsettings.json ou diretamente no código
string azureBlobConnectionString = builder.Configuration["Azure:ConnectionString"] ?? throw new Exception("ConnectionString não foi informada.");

// Registrar a instância do BlobServiceClient como singleton
builder.Services.AddSingleton(_ => new BlobServiceClient(azureBlobConnectionString));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
