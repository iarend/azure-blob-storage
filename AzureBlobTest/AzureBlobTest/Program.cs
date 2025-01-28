using Azure.Storage.Blobs;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Digite o caminho completo do arquivo (incluindo o nome do arquivo):");
        string caminhoCompleto = Console.ReadLine();

        // Remover aspas se o usuário inserir o caminho entre aspas
        caminhoCompleto = caminhoCompleto.Replace("\"", "").Trim();

        // Verificando se o arquivo existe no caminho fornecido
        if (!File.Exists(caminhoCompleto))
        {
            Console.WriteLine("O arquivo não foi encontrado no caminho especificado.");
            return;
        }

        Console.WriteLine("Digite a string de conexão do Azure Storage Account:");
        string connectionString = Console.ReadLine();

        Console.WriteLine("Digite o nome do container no Azure:");
        string containerName = Console.ReadLine();

        // Criando o cliente do BlobServiceClient
        var blobServiceClient = new BlobServiceClient(connectionString);

        // Obtendo o container
        var blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);

        // Nome do arquivo será o nome do arquivo fornecido no caminho completo
        string arquivoNome = Path.GetFileName(caminhoCompleto);

        // Criando o cliente de blob para o arquivo
        var blobClient = blobContainerClient.GetBlobClient(arquivoNome);

        try
        {
            // Enviando o arquivo para o Azure Storage Blob
            using (var stream = File.OpenRead(caminhoCompleto))
            {
                await blobClient.UploadAsync(stream, overwrite: true);
                Console.WriteLine($"Arquivo {arquivoNome} enviado com sucesso para o container {containerName}.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ocorreu um erro: {ex.Message}");
        }
    }
}
