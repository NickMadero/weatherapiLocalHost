using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/new-jersey", async context =>
{
    // first getting my api Key and creating a string with the url I want
    String apiKey = "b3d734cf5e18adcd52b4c7cebdce3ee5";
    string apiUrl = $"https://api.openweathermap.org/data/2.5/weather?q=New Jersey&appid={apiKey}&units=imperial";

    //new client 
    HttpClient client = new HttpClient();
    
        // get a response from the api url 
        var response = await client.GetAsync(apiUrl);

        // check if the response is good
        if (response.IsSuccessStatusCode)
        {

            // if good we will then make the content of the body into a string
            string responseBody = await response.Content.ReadAsStringAsync();

           // parse to a json format 
            var jsonDocument = JsonDocument.Parse(responseBody);

            // grabbed specific data to display.
            var temperature = jsonDocument.RootElement.GetProperty("main").GetProperty("temp").GetDouble();
            var weatherDescription = jsonDocument.RootElement.GetProperty("weather")[0].GetProperty("description").GetString();
            var humidity = jsonDocument.RootElement.GetProperty("main").GetProperty("humidity").GetDouble();
          

            // formatting the data to fit one output.
            string formattedResponse = $"Temperature: {temperature} \nWeather: {weatherDescription} \nhumidity: {humidity}";

            await context.Response.WriteAsync(formattedResponse);
        }
        // in case the api fails
        else
        {
            await context.Response.WriteAsync($"API call failed with status code: {response.StatusCode}");
        }
    
});

app.Run();
