using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using MoniteringBackend.Models;

namespace MoniteringFrontend.Services
{
    public class VehicleService
    {
        private readonly HttpClient _httpClient;

        public VehicleService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Fetch all vehicles
        public async Task<List<Vehicle>> GetVehiclesAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<Vehicle>>("Vehicle"); // Adjusted to use the correct endpoint
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error fetching vehicles: {ex.Message}");
                return new List<Vehicle>(); // Return an empty list in case of error
            }
        }

        // Fetch a single vehicle by ID
        public async Task<Vehicle> GetVehicleByIdAsync(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<Vehicle>($"Vehicle/{id}"); // Adjusted to use the correct endpoint
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error fetching vehicle by ID {id}: {ex.Message}");
                return null; // Return null or handle accordingly
            }
        }

        // Create a new service record
        public async Task<ServiceRecord> CreateServiceRecordAsync(ServiceRecord serviceRecord)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("ServiceRecord", serviceRecord);
                response.EnsureSuccessStatusCode(); // Throw if not a success code.

                return await response.Content.ReadFromJsonAsync<ServiceRecord>();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error creating service record: {ex.Message}");
                return null; // Return null or handle accordingly
            }
        }
    }
}
