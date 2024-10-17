using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using Tesseract;
using Newtonsoft.Json;
using MoniteringBackend.Models; // Ensure this namespace is correct

namespace MoniteringBackend.Services
{
    public class OcrService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public OcrService(HttpClient httpClient, string apiKey)
        {
            _httpClient = httpClient;
            _apiKey = apiKey;
        }

        public string ExtractTextFromPdf(string pdfPath)
        {
            var extractedText = ExtractTextWithiTextSharp(pdfPath);
            return extractedText;
        }

        public string ExtractTextFromImage(string imagePath)
        {
            using var engine = new TesseractEngine("./tessdata", "eng", EngineMode.Default);
            using var img = Pix.LoadFromFile(imagePath);
            using var page = engine.Process(img);
            return page.GetText();
        }

        private string ExtractTextWithiTextSharp(string pdfPath)
        {
            var sb = new System.Text.StringBuilder();

            using (var pdfDoc = new PdfDocument(new PdfReader(pdfPath)))
            {
                for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
                {
                    var page = pdfDoc.GetPage(i);
                    var text = PdfTextExtractor.GetTextFromPage(page);

                    if (!string.IsNullOrWhiteSpace(text))
                    {
                        sb.AppendLine(text);
                    }
                }
            }

            return sb.ToString();
        }

        public async Task<Invoice> ProcessInvoiceText(string extractedText)
        {
            var apiUrl = "https://api.openai.com/v1/chat/completions";

            var requestBody = new
            {
                model = "gpt-3.5-turbo",
                messages = new[] {
            new {
                role = "user",
                content = $"Extract structured data from this invoice in the following JSON format: " +
                          "{{ \"VehicleId\": \"<vehicle_id>\", \"PartNumber\": \"<part_number>\", \"LabourCost\": <labour_cost>, " +
                          "\"HoursWorked\": <hours_worked>, \"RepairType\": \"<repair_type>\", \"ServiceDate\": \"<service_date>\", \"Description\": \"<description>\" }}. " +
                          $"Here is the invoice text: {extractedText}"
            }
        },
                temperature = 0.2
            };

            var json = JsonConvert.SerializeObject(requestBody);
            var requestContent = new StringContent(json, Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _apiKey);

            try
            {
                var response = await _httpClient.PostAsync(apiUrl, requestContent);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();

                    try
                    {
                        // Parse the response into a structured format
                        dynamic result = JsonConvert.DeserializeObject(jsonResponse);
                        var invoiceData = result.choices[0].message.content.ToString();

                        // Convert to Invoice model
                        var invoice = ConvertToInvoiceModel(invoiceData);
                        return invoice;
                    }
                    catch (JsonException)
                    {
                        throw new OcrServiceException("Invalid JSON received from GPT-3.5 response.");
                    }
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    throw new OcrServiceException($"Failed to call GPT-3.5 API: {errorMessage}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new OcrServiceException("An error occurred while processing the invoice.", ex);
            }
        }

        private Invoice ConvertToInvoiceModel(string invoiceData)
        {
            var invoice = new Invoice();

            try
            {
                dynamic structuredData = JsonConvert.DeserializeObject(invoiceData);

                // Safely extract and assign values
                invoice.VehicleId = ExtractField(structuredData.VehicleId, "VehicleId");
                invoice.PartNumber = ExtractField(structuredData.PartNumber, "PartNumber");
                invoice.LabourCost = ParseDecimal(structuredData.LabourCost?.ToString());
                invoice.HoursWorked = ParseDecimal(structuredData.HoursWorked?.ToString());
                invoice.RepairType = ExtractField(structuredData.RepairType, "RepairType");
                invoice.ServiceDate = ParseDate(structuredData.ServiceDate?.ToString());
                invoice.Description = ExtractField(structuredData.Description, "Description");
            }
            catch (Exception ex)
            {
                throw new OcrServiceException("An error occurred while mapping the invoice data.", ex);
            }

            return invoice;
        }

        // Method to safely extract fields or assign "Not Found"
        private string ExtractField(dynamic field, string fieldName)
        {
            return field != null ? field.ToString() : "Not Found";
        }

        // Method to parse decimal values safely
        private decimal ParseDecimal(string value)
        {
            return decimal.TryParse(value, out var result) ? result : 0;
        }

        // Method to parse DateTime values safely
        private DateTime ParseDate(string value)
        {
            return DateTime.TryParse(value, out var result) ? result : DateTime.MinValue;
        }


        // Add this method to your existing OcrService class
        public async Task<string> CallChatGptApi(object requestPayload)
        {
            var apiUrl = "https://api.openai.com/v1/chat/completions";
            var json = JsonConvert.SerializeObject(requestPayload);
            var requestContent = new StringContent(json, Encoding.UTF8, "application/json");

            // Set the authorization header
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _apiKey);

            // Make the API request
            var response = await _httpClient.PostAsync(apiUrl, requestContent);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                dynamic result = JsonConvert.DeserializeObject(jsonResponse);
                return result.choices[0].message.content.ToString(); // Adjust based on actual response structure
            }
            else
            {
                // Log the error or handle it accordingly
                throw new HttpRequestException($"Failed to call ChatGPT API: {response.ReasonPhrase}");
            }
        }

    }
}
