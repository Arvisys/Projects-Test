using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace WebjetClientApp.Controllers
{
    public class MovieController : Controller
    {
        // GET: Movie
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Cinema()
        {
            ViewBag.Title = "Cinema world";
            // create manager object
            Manager.MovieManager mgr = new Manager.MovieManager();

            List<Models.Movie> movies = mgr.GetCinemaWorldMovies();
            if (movies != null)
            {
                return View(movies);
            }
            else
            {
                ViewBag.Error = "Service unavailable. Please try again.";
                return View();
            }
        }

        public ActionResult Film()
        {
            try
            {
                ViewBag.Title = "Film World";
                Manager.MovieManager mgr = new Manager.MovieManager();
                List<Models.Movie> movies = mgr.GetFilmWorldMovies();

                if (movies != null)
                {
                    return View(movies);
                }
                else
                {
                    ViewBag.Error = "Service unavailable. Please try again.";
                    return View();
                }
            }
            catch (Exception)
            {
                return View();
            }
        }


        // GET: Price
        [HttpGet]
        public ActionResult Price()
        {
            ViewBag.Title = "Get Cheapest price for the movie.";
            Manager.MovieManager mgr = new Manager.MovieManager();

            ViewBag.Json = mgr.GetMovieNamesJson();

            if (ViewBag.Json == null)
            {
                ViewBag.Error = "Unable to retrieve the Movie list. Please try again.";
            }
            return View();
        }


        // Post: Price
        [HttpPost]
        public ActionResult Price(string hdnMovieID)
        {
            ViewBag.Title = "Get Cheapest price for the movie.";
            Manager.MovieManager mgr = new Manager.MovieManager();
            ViewBag.Json = mgr.GetMovieNamesJson();

            if (ViewBag.Json == null)
            {
                ViewBag.Error = "Unable to retrieve the Movie list. Please try again.";
                return View();
            }

            string movieID = hdnMovieID;

            Models.Movie mv;
            List<Models.Movie> movies = new List<Models.Movie>();
            mv = mgr.GetLowestPriceMovie(movieID);
            if (mv == null)
            {
                ViewBag.Error = "Service unavailable. Please try again.";
                return View();
            }
            else
            {
                movies.Add(mv);
                return View(movies);
            }
        }



        [HttpGet]
        public ActionResult MovieDetails(string movieID, string provider)
        {
            ViewBag.Title = "Get Cheapest price for the movie.";
            Manager.MovieManager mgr = new Manager.MovieManager();

            Models.Movie mv;
            List<Models.Movie> movies = new List<Models.Movie>();
            if (provider == "Cinema")
            {
                mv = mgr.GetCinemaWorldMoviesDetails(movieID);
            }
            else
            {
                mv = mgr.GetFilmWorldMoviesDetails(movieID);
            }

            if (mv == null)
            {
                ViewBag.Error = "Service unavailable. Please try again.";
                return View();
            }
            else
            {
                movies.Add(mv);
                return View(movies);
            }
        }
    }
}