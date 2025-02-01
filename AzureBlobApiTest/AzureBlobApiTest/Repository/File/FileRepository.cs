using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace AzureBlobApiTest.Repository.File;

public class FileRepository : IFileRepository
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly BlobContainerClient _blobContainerClient;
    private readonly string _blobContainerName = "test";

    public FileRepository(BlobServiceClient blobServiceClient)
    {
        _blobServiceClient = blobServiceClient;
        _blobContainerClient = _blobServiceClient.GetBlobContainerClient(_blobContainerName);
    }


    public async Task<string> UploadFile(IFormFile file, bool ignoreInapropriate)
    {
        // Verificar se o arquivo foi enviado
        if (file == null || file.Length == 0) throw new Exception("Nenhum arquivo foi enviado.");
        Console.WriteLine("Verificou arquivo");

        await IsSafeFile(file, ignoreInapropriate);
        
        // Nome do arquivo original
        string fileName = Path.GetFileName(file.FileName);
        Console.WriteLine("Buscou o nome do arquivo.");

        // Criar um BlobClient para o arquivo enviado
        BlobClient blobClient = _blobContainerClient.GetBlobClient(fileName);
        Console.WriteLine("Criou o blob do arquivo");

        // Fazer o upload do arquivo
        using (var stream = file.OpenReadStream())
        {
            await blobClient.UploadAsync(stream, overwrite: true);
        }
        Console.WriteLine("Realizou upload do arquivo.");

        // Retorna o url da imagem.
        return blobClient.Uri.ToString();
    }

    public async Task<string> GetFileUrl(string fileName)
    {
        BlobClient blobClient = _blobContainerClient.GetBlobClient(fileName);
        
        return await Task.FromResult(blobClient.Uri.ToString());
    }

    public async Task<(Stream content, string contentType)> DownloadFileAsync(string fileName)
    {
        BlobClient blobClient = _blobContainerClient.GetBlobClient(fileName);
        Console.WriteLine("Buscou blob do arquivo.");

        BlobDownloadInfo downloadInfo = await blobClient.DownloadAsync();

        return (downloadInfo.Content, downloadInfo.ContentType);
    }

    private async Task IsSafeFile(IFormFile file, bool ignoreInapropriate)
    {   
        // 1. Verificar tipo de arquivo
        var allowedExtensions = new[] { ".jpg", ".png", ".pdf", ".txt" };
        var extension = Path.GetExtension(file.FileName).ToLower();
        if (!allowedExtensions.Contains(extension))
        {
            throw new Exception("O formato do arquivo informado não é permitido.");
        }

        // 2. Verificar tipo MIME
        var mimeType = file.ContentType;
        if (!mimeType.StartsWith("image/") && mimeType != "application/pdf" && !mimeType.StartsWith("text/"))
        {
            throw new Exception("O tipo do arquivo informado não é permitido.");
        }

        // 3. Verificar tamanho do arquivo (limitar a 10MB)
        long maxSize = 10485760;

        if (file.Length > maxSize)
        {
            throw new Exception($"O tamanho do arquivo precisa ser inferior a {maxSize}.");
        }

        // 4. Verificar o arquivo quanto a malware (usando uma ferramenta externa ou serviço)
        bool isMalicious = await CheckForMalware(file);
        if (isMalicious)
        {
            throw new Exception("O arquivo parece conter conteúdo malicioso.");
        }

        // 5. Moderar conteúdo se for uma imagem
        if (mimeType.StartsWith("image/"))
        {
            if (!ignoreInapropriate)
            {
                var isInappropriate = await ModerateImageContent(file);
                if (isInappropriate)
                {
                    throw new Exception("O arquivo parece conter conteúdo inapropriado.");
                }
            }
        }
    }


    private async Task<bool> CheckForMalware(IFormFile file)
    {
        return await Task.FromResult(true);
    }

    
    private async Task<bool> ModerateImageContent(IFormFile file)
    {
        return await Task.FromResult(true);
    }
}