            using System.Net.Http.Headers;
using System.Text.Json;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using TriviaApp.Models;

namespace TriviaApp
{
    public class TriviaFetchService
    {

        private readonly IHttpClientFactory _httpClientFactory = null!;

        private readonly IConfiguration _configuration = null!;
       // private readonly ILogger<TriviaFetchService> _logger = null!;



        public TriviaFetchService(
      IHttpClientFactory httpClientFactory,
      IConfiguration configuration
      ) =>
      (_httpClientFactory, _configuration) =
          (httpClientFactory, configuration);

        //public MovieData GetMovieDetails() { 
        
        //}

        public async Task<T? > GetMovieDetails<T>() {

            HttpClient client = _httpClientFactory.CreateClient();
            String authKey = "eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiJjOGM4YWEzOWIwZGYyYWJjMTAyOTBmMTgxZmM5MDdkZiIsIm5iZiI6MTc2MDI5OTg3My40ODE5OTk5LCJzdWIiOiI2OGVjMGI2MWZiYjAxZjJiNmFkNjhjN2IiLCJzY29wZXMiOlsiYXBpX3JlYWQiXSwidmVyc2lvbiI6MX0.S32htx2mHmcIrj-vTGSCKq38vfC46v5r_h681yGM6Ks";
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",authKey);
            Uri requestUri = new Uri("https://api.themoviedb.org/3/movie/top_rated?language=en-US&page=1");
            var response = await client.GetAsync(requestUri);


            //var result =  await client.GetFromJsonAsync<TriviaAnswer[]>(requestUri);
            if (!response.IsSuccessStatusCode)
                throw new Exception($"{response.StatusCode}");
            //return null;
            var body = await response.Content.ReadFromJsonAsync<T>();

            return body;



    //        var request = new HttpRequestMessage
    //        {

            //            Method = HttpMethod.Get,
            //            RequestUri = new Uri("https://api.themoviedb.org/3/discover/movie?include_adult=false&include_video=false&language=en-US&page=1&primary_release_year=2025&sort_by=popularity.desc"),
            //            Headers =
            //{
            //    { "accept", "application/json" },
            //    { "Authorization", "Bearer eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiJjOGM4YWEzOWIwZGYyYWJjMTAyOTBmMTgxZmM5MDdkZiIsIm5iZiI6MTc2MDI5OTg3My40ODE5OTk5LCJzdWIiOiI2OGVjMGI2MWZiYjAxZjJiNmFkNjhjN2IiLCJzY29wZXMiOlsiYXBpX3JlYWQiXSwidmVyc2lvbiI6MX0.S32htx2mHmcIrj-vTGSCKq38vfC46v5r_h681yGM6Ks" },
            //},
            //        };
    }
            //var options = JsonSerializerOptions.Web;



            //foreach (var spy in result)
            //{
            //    Console.WriteLine($"Firstname: {spy.FirstName}, Surname: {spy.Surname}, Age: {spy.Age}");
            //}



            //using (var response = await client.SendAsync(request))
            //{
            //    response.EnsureSuccessStatusCode();
            //    var body = await response.Content.ReadAsStringAsync();
            //    Console.WriteLine(body);
            //}

        }
}
