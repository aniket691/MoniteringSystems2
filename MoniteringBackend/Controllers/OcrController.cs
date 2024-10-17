using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoniteringBackend.Models; // Ensure this namespace is correct
using MoniteringBackend.Services;
using System.IO;
using System.Threading.Tasks;

namespace MoniteringBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OcrController : ControllerBase
    {
        private readonly OcrService _ocrService;

        public OcrController(OcrService ocrService)
        {
            _ocrService = ocrService;
        }

        // POST: api/Ocr/ExtractPdf
        [HttpPost("ExtractPdf")]
        public async Task<IActionResult> ExtractTextFromPdf(IFormFile pdfFile)
        {
            if (pdfFile == null || pdfFile.Length == 0)
                return BadRequest("No file uploaded.");

            // Save the PDF file to a temporary location
            var tempFilePath = Path.GetTempFileName();
            using (var stream = System.IO.File.Create(tempFilePath))
            {
                await pdfFile.CopyToAsync(stream);
            }

            // Extract text using OCR service
            var extractedText = _ocrService.ExtractTextFromPdf(tempFilePath);

            Console.WriteLine(extractedText);

            // Process the extracted text as an invoice
            var invoice = await _ocrService.ProcessInvoiceText(extractedText);

            // Clean up the temporary file
            System.IO.File.Delete(tempFilePath);

            // Return the processed invoice
            if (invoice != null)
            {
                return Ok(invoice);
            }

            return BadRequest("Failed to process the invoice.");
        }

        // POST: api/Ocr/ExtractImage
        [HttpPost("ExtractImage")]
        public async Task<IActionResult> ExtractTextFromImage(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
                return BadRequest("No file uploaded.");

            // Save the image file to a temporary location
            var tempFilePath = Path.GetTempFileName();
            using (var stream = System.IO.File.Create(tempFilePath))
            {
                await imageFile.CopyToAsync(stream);
            }

            // Extract text using OCR service
            var extractedText = _ocrService.ExtractTextFromImage(tempFilePath);

            // Process the extracted text as an invoice
            var invoice = await _ocrService.ProcessInvoiceText(extractedText);

            // Clean up the temporary file
            System.IO.File.Delete(tempFilePath);

            // Return the processed invoice
            if (invoice != null)
            {
                return Ok(invoice);
            }
            return BadRequest("Failed to process the invoice.");
        }
    }
}
