using System.Net.Http.Headers;

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

        public async Task<T?> GetMovieDetails<T>()
        {

            HttpClient client = _httpClientFactory.CreateClient();
            String authKey = "eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiJjOGM4YWEzOWIwZGYyYWJjMTAyOTBmMTgxZmM5MDdkZiIsIm5iZiI6MTc2MDI5OTg3My40ODE5OTk5LCJzdWIiOiI2OGVjMGI2MWZiYjAxZjJiNmFkNjhjN2IiLCJzY29wZXMiOlsiYXBpX3JlYWQiXSwidmVyc2lvbiI6MX0.S32htx2mHmcIrj-vTGSCKq38vfC46v5r_h681yGM6Ks";
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authKey);
            Uri requestUri = new Uri("https://api.themoviedb.org/3/movie/top_rated?language=en-US&page=1");
            var response = await client.GetAsync(requestUri);


            //var result =  await client.GetFromJsonAsync<TriviaAnswer[]>(requestUri);
            if (!response.IsSuccessStatusCode)
                throw new Exception($"{response.StatusCode}");
            //return null;
            var body = await response.Content.ReadFromJsonAsync<T>();

            return body;
                




        }
    }
}
