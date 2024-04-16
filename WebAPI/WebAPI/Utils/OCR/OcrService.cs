using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System.Security.AccessControl;

namespace WebAPI.Utils.OCR
{
    public class OcrService
    {
        private readonly string _subscriptKey = "971fe13740a442439cb13642c7137f81";

        private readonly string _endpoint = "https://computervisiong10t.cognitiveservices.azure.com/";

        public async Task<string> RecognizeTextAsync(Stream imageStream)
        {
            try
            {
                var client = new ComputerVisionClient( new ApiKeyServiceClientCredentials( _subscriptKey ) )
                {
                    Endpoint = _endpoint
                };

                var ocrResult = await client.RecognizePrintedTextInStreamAsync(true, imageStream);

                return ProcessRecognitionResult( ocrResult );
            }
            catch (Exception ex)
            {
                return "Erro ao reconhecre texto" + ex.Message;
            }
        }

        private static string ProcessRecognitionResult(OcrResult result)
        {
            try
            {
                string recognizedText = "";

                foreach (var region in result.Regions)
                {
                    foreach (var line in region.Lines)
                    {
                        foreach (var word in line.Words)
                        {
                            recognizedText += word.Text + " ";
                        }

                        recognizedText += "/n";
                    }
                }

                return recognizedText;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
