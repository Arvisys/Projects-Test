using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

// After installing the nuget package Microsoft.AspNet.WebApi.Client
using System.Net.Http;
using System.Net.Http.Headers;
//dependency of WebApi client
using Newtonsoft.Json;


namespace WebjetClientApp.Manager
{
    public class MovieManager
    {

        /// <summary>
        /// contructor
        /// </summary>
        public MovieManager()
        {

        }

        /// <summary>
        /// Get the collection of movies from the Film world API
        /// </summary>
        /// <returns></returns>
        public List<Models.Movie> GetFilmWorldMovies()
        {
            try
            {
                HttpClient client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(10); //Assumption : Time out 10sec

                client.BaseAddress = new Uri("http://webjetapitest.azurewebsites.net/"); //set the base address for http client
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("x-access-token", "sjd1HfkjU83ksdsm3802k");

                HttpResponseMessage response = client.GetAsync("/api/filmworld/movies").Result; // blocking call

                if (response.IsSuccessStatusCode)
                {
                    Models.MovieCollection col = response.Content.ReadAsAsync<Models.MovieCollection>().Result;

                    return col.Movies;// returns list of movies of film world
                }
                else
                {
                    throw new Exception("Error code: " + response.StatusCode.ToString() + "Message: " + response.ReasonPhrase);//todo if service not avaiable
                }
            }
            catch
            {
                //Any Error return null
                return null;
            }
        }

        /// <summary>
        /// Get movie details from the Film world API 
        /// </summary>
        /// <param name="flmMovID"></param>
        /// <returns></returns>
        public Models.Movie GetFilmWorldMoviesDetails(string flmMovID)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(10);

                client.BaseAddress = new Uri("http://webjetapitest.azurewebsites.net/"); //set the base address for http client
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("x-access-token", "sjd1HfkjU83ksdsm3802k");

                HttpResponseMessage response = client.GetAsync("/api/filmworld/movie/" + flmMovID).Result; // blocking call

                if (response.IsSuccessStatusCode)
                {
                    Models.Movie mv = response.Content.ReadAsAsync<Models.Movie>().Result;
                    mv.Provider = "Film World";
                    return mv;
                }
                else
                {
                    //Any Error return null 
                }
            }
            catch
            {
                //Any Error return null
            }
            return null;
        }

        /// <summary>
        /// Get the collection of movies from the Cinema world API
        /// </summary>
        /// <returns></returns>
        public List<Models.Movie> GetCinemaWorldMovies()
        {
            try
            {
                HttpClient client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(10);

                client.BaseAddress = new Uri("http://webjetapitest.azurewebsites.net/"); //set the base address for http client
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("x-access-token", "sjd1HfkjU83ksdsm3802k");

                HttpResponseMessage response = client.GetAsync("/api/cinemaworld/movies").Result; // blocking call

                if (response.IsSuccessStatusCode)
                {
                    Models.MovieCollection col = response.Content.ReadAsAsync<Models.MovieCollection>().Result;

                    return col.Movies;
                }
                else
                {
                    throw new Exception("Error code: " + response.StatusCode.ToString() + "Message: " + response.ReasonPhrase);//todo if service not avaiable
                }
            }
            catch 
            {
                //Any Error return null
                return null;
            }
        }


        /// <summary>
        /// Get movie details from the Cinema world API 
        /// </summary>
        /// <param name="movieID"></param>
        /// <returns></returns>
        public Models.Movie GetCinemaWorldMoviesDetails(string movieID)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(10);
                client.BaseAddress = new Uri("http://webjetapitest.azurewebsites.net/"); //set the base address for http client
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("x-access-token", "sjd1HfkjU83ksdsm3802k");

                HttpResponseMessage response = client.GetAsync("/api/cinemaworld/movie/" + movieID).Result; // blocking call

                if (response.IsSuccessStatusCode)
                {
                    Models.Movie mv = response.Content.ReadAsAsync<Models.Movie>().Result;
                    mv.Provider = "Cinema World";
                    return mv;
                }
                else
                {
                    //Any Error return null
                }
            }
            catch
            {
                //Any Error return null
            }
            return null;
        }

        /// <summary>
        /// /// Creates and returns a JSON format array of movie title and id.
        /// if it is already stored in the session then return it else gets from the API and store in the session (for performance)
        /// The movie title and ID are return as a json format to be used by the autocomplete in the Price view.
        /// </summary>
        /// <returns></returns>
        public StringBuilder GetMovieNamesJson()
        {
            if ((StringBuilder)System.Web.HttpContext.Current.Session["MoviesNames"] != null)
            {
                return (StringBuilder)System.Web.HttpContext.Current.Session["MoviesNames"];
            }
            List<Models.Movie> cinemaMovies = GetCinemaWorldMovies();
            List<Models.Movie> filmMovies = GetFilmWorldMovies();
            return GetMovieNamesJson(cinemaMovies, filmMovies);
        }

        /// <summary>
        /// Creates and returns a JSON format array of movie title and id.
        /// The list of movies of cinema and film are supplied as the parameters.
        /// When any API is unavaiable then the correcsponding parameter (list of movies) will be supplied as a null object.
        /// /// </summary>
        /// <param name="cinemaMovies"></param>
        /// <param name="filmMovies"></param>
        /// <returns></returns>
        public StringBuilder GetMovieNamesJson(List<Models.Movie> cinemaMovies, List<Models.Movie> filmMovies)
        {
            StringBuilder sbJSon = new StringBuilder();

            List<Models.Movie> movies = new List<Models.Movie>();

            if (cinemaMovies == null && filmMovies == null)
            {
                return null;// vrtodo both service unavailbe
            }

            //Assumption : The title of a movie is same in both the providers (Cinema world and film world)
            //so that the user can search  by the movie title and get the cheapest price from the 2 providers 

            if (filmMovies != null)
            {
                foreach (var film in filmMovies)
                {
                    movies.Add(film);
                }
            }
            else
            {
                //create empty film movie collection when Fil API is not available
                filmMovies = new List<Models.Movie>();
            }

            if (cinemaMovies != null)
            {
                foreach (var cinema in cinemaMovies)
                {
                    //only unique Movie title are added to the list
                    var matches = filmMovies.Where(m => m.Title == cinema.Title);
                    if (matches.Count() == 0)
                    {
                        movies.Add(cinema);
                    }
                }
            }
            
            sbJSon.Append("var tags = [");
            string comma = "";
            foreach (var item in movies)
            {
                sbJSon.Append(comma + "{\'label': '" + item.Title + "', 'value': '" + item.ID + "'}");
                comma = ",";
            }
            sbJSon.Append("];\n");
            
            //store as session variable for future use
            if (System.Web.HttpContext.Current != null)
            {
                System.Web.HttpContext.Current.Session["MoviesNames"] = sbJSon;
            }
            return sbJSon;
        }


        /// <summary>
        /// Return the lowest movie price of the suppied movie ID
        /// </summary>
        /// <param name="movieID"></param>
        /// <returns></returns>
        public Models.Movie GetLowestPriceMovie(string movieID)
        {
            string id;
            //Assumption : Movie ID of the 2 provider (Cinema and film) are same Eg :"0076759"
            //except that the movie ID will be prefixed with "cw" for Cinema World Eg: "cw0076759"
            // and prefixed with "fw" for film World Eg : "fw0076759"
            id = movieID.Replace("cw", "");
            id = id.Replace("fw", "");
            Models.Movie cinemaMovie = GetCinemaWorldMoviesDetails("cw" + id);
            Models.Movie filmMovie = GetFilmWorldMoviesDetails("fw" + id);
            return GetLowestPriceMovie(cinemaMovie, filmMovie);
        }

        /// <summary>
        ///  /// Return the lowest movie price of the suppied film movie and cinema movie
        /// </summary>
        /// <param name="cinemaMovie"></param>
        /// <param name="filmMovie"></param>
        /// <returns></returns>
        public Models.Movie GetLowestPriceMovie(Models.Movie cinemaMovie, Models.Movie filmMovie)
        {
            ///Assumption: movie price will be greater than 0 (done for error handling)
            float cPrice = 0;
            float fPrice = 0;
           
            if (cinemaMovie == null && filmMovie == null)
            {
                return null;//vrtodo2; Error is grtting price from both the providers
            }

            //Assumption: Comparing movie prices between the 2 providers.
            // When both the provider are available then the movie of the cheapest price will be returned.
            //But when any one provider (Eg : Film world ) is unavailable and the other provider (Eg:Cinema world) is available 
            //then the price of the movie from the available provider (Eg: Cinema world) will be returned as the cheapest price - because it is the only movie price available at that time.
            if (cinemaMovie != null)
            {
                cPrice = cinemaMovie.Price;
            }
            if (filmMovie != null)
            {
                fPrice = filmMovie.Price;
            }

            if (cPrice == 0) //Assumption price cannot be 0 
            {
                return filmMovie;
            }

            if (fPrice == 0) //Assumption price cannot be 0 
            {
                return cinemaMovie;
            }

            if (cPrice < fPrice) //return the lowest price as the cheapest
            {
                return cinemaMovie;
            }
            else
            {
                return filmMovie;
            }
        }


    }

}