using Claims.Application.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;

namespace Claims.Tests
{
    public class ClaimsControllerTests
    {
        [Fact]
        public async Task Get_Claims()
        {
            var application = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(_ =>
                {});

            var client = application.CreateClient();

            var response = await client.GetAsync("/Claims");

            response.EnsureSuccessStatusCode();

            var responseStrContent = await response.Content.ReadAsStringAsync();
            var claims = JsonConvert.DeserializeObject<List<ClaimModel>>(responseStrContent);
        }

    }
}
