using AzureBlobApiTest.Repository.File;
using Microsoft.AspNetCore.Mvc;

namespace AzureBlobApiTest.Controller;

[ApiController]
[Route("api/v1/[controller]")]
public class FileController : ControllerBase
{
    private readonly IFileRepository _fileRepository;

    public FileController(IFileRepository fileRepository)
    {
        _fileRepository = fileRepository ?? throw new ArgumentNullException(nameof(fileRepository));
    }

    [HttpGet("view/{fileName}")]
    public async Task<IActionResult> GetFileUrl(string fileName) => Ok(await _fileRepository.GetFileUrl(fileName));

    [HttpGet("download/{fileName}")]
    public async Task<IActionResult> DownloadFile(string fileName)
    {
        var (content, contentType) = await _fileRepository.DownloadFileAsync(fileName);

        return File(content, contentType, fileName);
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadFile([FromBody] IFormFile file, [FromHeader] bool ignoreInapropriate) => Ok(await _fileRepository.UploadFile(file, ignoreInapropriate));
}