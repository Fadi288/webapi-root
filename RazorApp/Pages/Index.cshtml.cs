using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _factory;

        public string WeatherForecast { get; set; } = string.Empty;
        public IndexModel(IHttpClientFactory factory)
        {
            _factory = factory;
        }

        public async Task OnGet()
        {
            var client  = _factory.CreateClient("WebApi");
            var response = await client.GetAsync("weatherforecast");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                WeatherForecast = content.ToString();
            }
            else
            {
                WeatherForecast = "Error: " + response.StatusCode.ToString();
            }
        }
    }
}
