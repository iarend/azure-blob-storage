namespace AzureBlobApiTest.Entity.DTO;

public class ConnectAzureBlobDTO
{
    /// <summary>
    /// Connection string do blob do azure.
    /// </summary>
    public string? ConnectionString { get; set; }

    /// <summary>
    /// Nome do container a ser acessado.
    /// </summary>
    public string? Container { get; set; }
}
