// API test layer demonstration using JSONPlaceholder (jsonplaceholder.typicode.com)
// In a TherapyNotes context, these patterns would apply to internal API routes
// with authentication headers added via client.DefaultRequestHeaders.Authorization

using Xunit;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;

namespace TherapyNotesUITests.Tests
{
    public class ApiTests
    {
        private readonly HttpClient client;

        public ApiTests()
        {
            client = new HttpClient();
            client.BaseAddress = new System.Uri("https://jsonplaceholder.typicode.com");
        }

        [Fact]
        public async Task GetPost_ReturnsSuccessStatusCode()
        {
            var response = await client.GetAsync("/posts/1");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetPost_ReturnsExpectedFields()
        {
            var response = await client.GetAsync("/posts/1");
            var body = await response.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(body);
            Assert.True(doc.RootElement.TryGetProperty("id", out _));
            Assert.True(doc.RootElement.TryGetProperty("title", out _));
            Assert.True(doc.RootElement.TryGetProperty("body", out _));
        }

        [Fact]
        public async Task GetInvalidPost_Returns404()
        {
            var response = await client.GetAsync("/posts/99999");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task CreatePost_ReturnsCreatedStatus()
        {
            var payload = new StringContent(
                JsonSerializer.Serialize(new { title = "Test Post", body = "Test Body", userId = 1 }),
                System.Text.Encoding.UTF8,
                "application/json"
            );
            var response = await client.PostAsync("/posts", payload);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }
    }
}