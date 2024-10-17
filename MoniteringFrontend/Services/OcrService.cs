using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;

namespace MoniteringFrontend.Services
{
    public class OcrService
    {
        private readonly HttpClient _httpClient;

        public OcrService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> UploadImageAndExtractText(IBrowserFile imageFile)
        {
            var content = new MultipartFormDataContent();
            var fileContent = new StreamContent(imageFile.OpenReadStream());
            content.Add(fileContent, "imageFile", imageFile.Name); // Change "file" to "imageFile"

            var response = await _httpClient.PostAsync("ocr/Extract", content); // Change to "ocr/Extract"
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<OcrResponse>();
            return result.Text;
        }


    }

    public class OcrResponse
    {
        public string Text { get; set; }
    }
}
