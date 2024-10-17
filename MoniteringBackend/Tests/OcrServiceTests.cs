using Moq;
using Moq.Protected;
using MoniteringBackend.Services;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MoniteringBackend.Tests
{
    public class OcrServiceTests
    {
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private readonly HttpClient _httpClient;
        private readonly OcrService _ocrService;

        public OcrServiceTests()
        {
            // Mock the HttpMessageHandler used in OcrService
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            var apiKey = "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"; // Set a mock API key
            _ocrService = new OcrService(_httpClient, apiKey);
        }

        [Fact]
        public async Task ChatGptApi_ShouldReturnExpectedResponse()
        {
            // Arrange
            string expectedResponse = "This is a test response from ChatGPT API.";
            var requestPayload = new { prompt = "Test prompt" }; // Adjust as per your API requirements

            // Mock the ChatGPT API response
            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    It.IsAny<HttpRequestMessage>(),
                    It.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(expectedResponse)
                });

            // Act
            var response = await _ocrService.CallChatGptApi(requestPayload); // Assume this method exists

            // Assert
            Assert.Equal(expectedResponse, response);
        }
    }
}
