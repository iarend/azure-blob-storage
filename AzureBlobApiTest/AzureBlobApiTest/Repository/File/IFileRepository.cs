namespace AzureBlobApiTest.Repository.File;

public interface IFileRepository
{
    /// <summary>
    /// Faz o upload de um arquivo.
    /// </summary>
    /// <param name="file">Entidade do arquivo.</param>
    /// <returns></returns>
    Task<string> UploadFile(IFormFile file, bool ignoreInapropriate);

    /// <summary>
    /// Método para obter a URL pública do arquivo.
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    Task<string> GetFileUrl(string fileName);

    /// <summary>
    /// Faz o download de um arquivo.
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    Task<(Stream content, string contentType)> DownloadFileAsync(string fileName);
}