namespace TriviaApp.Models
{
    public class TopRatedMovies
    {

        //public class ect
        
            public int page { get; set; }
            public MovieData[] results { get; set; }
            public int total_pages { get; set; }
            public int total_results { get; set; }
        

       

    }
}
