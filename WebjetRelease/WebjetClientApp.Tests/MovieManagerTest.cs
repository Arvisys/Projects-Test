using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;
using System.Collections.Generic;

namespace WebjetClientApp.Tests
{
    [TestClass]
    public class MovieManagerTest
    {
        [TestMethod]
        public void GetLowestPriceMovieTest()
        {
            WebjetClientApp.Models.Movie expected;
            WebjetClientApp.Models.Movie result;
            WebjetClientApp.Models.Movie cm = new WebjetClientApp.Models.Movie();
            cm.Title = "Movie1";
            cm.Price = 4.20F;

            WebjetClientApp.Models.Movie fm = new WebjetClientApp.Models.Movie();
            fm.Title = "Movie1";
            fm.Price = 5.1F;

            WebjetClientApp.Manager.MovieManager mgr = new WebjetClientApp.Manager.MovieManager();

            result = mgr.GetLowestPriceMovie(cm, fm);

            expected = cm; //lowest price 

            Assert.IsTrue(expected == result);
        }

        [TestMethod]
        public void GetLowestPriceMovieOneNullTest()
        {
            WebjetClientApp.Models.Movie expected;
            WebjetClientApp.Models.Movie result;

            WebjetClientApp.Models.Movie fm = new WebjetClientApp.Models.Movie();
            fm.Title = "Movie1";
            fm.Price = 5.1F;

            WebjetClientApp.Manager.MovieManager mgr = new WebjetClientApp.Manager.MovieManager();

            result = mgr.GetLowestPriceMovie(null, fm);

            expected = fm; //lowest price 

            Assert.IsTrue(expected == result);
        }


        [TestMethod]
        public void GetLowestPriceMovieBothNullTest()
        {
            WebjetClientApp.Models.Movie expected;
            WebjetClientApp.Models.Movie result;

            WebjetClientApp.Manager.MovieManager mgr = new WebjetClientApp.Manager.MovieManager();

            result = mgr.GetLowestPriceMovie(null, null);

            expected = null; //lowest price 

            Assert.IsTrue(expected == result);
        }

        [TestMethod]
        public void GetMovieNamesJsonTest()
        {


            StringBuilder result;
            StringBuilder expected = new StringBuilder();
            List<WebjetClientApp.Models.Movie> cinemaMovies = new List<WebjetClientApp.Models.Movie>();
            WebjetClientApp.Models.Movie cm = new WebjetClientApp.Models.Movie();
            cm.Title = "Movie1";
            cm.ID = "ID1";
            cinemaMovies.Add(cm);
            cm = new WebjetClientApp.Models.Movie();
            cm.Title = "Movie2";
            cm.ID = "ID2";
            cinemaMovies.Add(cm);
            cm = new WebjetClientApp.Models.Movie();
            cm.Title = "Movie3";
            cm.ID = "ID3";
            cinemaMovies.Add(cm);
            expected.Append("var tags = [");
            string comma = "";
            foreach (var item in cinemaMovies)
            {
                expected.Append(comma + "{\'label': '" + item.Title + "', 'value': '" + item.ID + "'}");
                comma = ",";
            }
            expected.Append("];\n");

            WebjetClientApp.Manager.MovieManager mgr = new WebjetClientApp.Manager.MovieManager();

            result = mgr.GetMovieNamesJson(cinemaMovies, null);

            Assert.IsTrue(expected.ToString() == result.ToString());
        }

        [TestMethod]
        public void GetMovieNamesJsonNullTest()
        {
            StringBuilder result;
            StringBuilder expected = null;

            WebjetClientApp.Manager.MovieManager mgr = new WebjetClientApp.Manager.MovieManager();
            result = mgr.GetMovieNamesJson(null, null);

            Assert.IsTrue(expected == result);
        }
    }
}
