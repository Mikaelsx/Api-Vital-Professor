using Azure.Storage.Blobs;

namespace WebAPI.Utils.BlobStorage
{
    public class AzureBlobStorageHelper
    {
        public static async Task<string> UploadImageBlobAsync(IFormFile arquivo, string stringConexão, string nomeContainer)
        {
			try
			{
				// Verifica se existe um arquivo
				if (arquivo != null)
				{
					// Gera um nome unico + extenção do arquivo
					var blobName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(arquivo.FileName);

					// Cria uma instançia do client blob service e passa string de conexão
					var blobServiceCLient = new BlobServiceClient(stringConexão);

					// Obtem um container client usando o nome do container do blob
					var blobContainerClient = blobServiceCLient.GetBlobContainerClient(nomeContainer);

					// Obtem um blob client usando blob name
					var blobClient = blobContainerClient.GetBlobClient(blobName);

					// Para o fluxo de entrada do arquivo 
					using (var stream = arquivo.OpenReadStream())
					{
						// Carrega o arquivo ( Foto ) para o blob storage de forma assincrona
						await blobClient.UploadAsync(stream, true);
					}

					// Retorna a uri do blob como uma string
					return blobClient.Uri.ToString();
				}
				else
				{
					// Retorna a uri de uma imagem padrão caso nenhum arquivo seja enviado
					return "https://vitalmikaelg10.blob.core.windows.net/containervitalg10/user-profile-front-side.jpg";

                }
			}
			catch (Exception)
			{

				throw;
			}
        }
    }
}
