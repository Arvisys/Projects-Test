using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebjetClientApp.Models
{
    public class Movie
    {
        /* Properties */
        public string Title { get; set; }
        public int Year { get; set; }
        public string ID { get; set; }
        public string Type { get; set; }
        public string Poster { get; set; }
        public string Provider { get; set; }

        //Details
        public string Rated { get; set; }
        public string Released { get; set; }
        public string Runtime { get; set; }
        public string Genre { get; set; }
        public string Director { get; set; }
        public string Writer { get; set; }
        public string Actors { get; set; }
        public string Plot { get; set; }
        public float  Price { get; set; }
        public float Rating { get; set; }


    }

    public class MovieCollection{
        public List<Movie>  Movies { get; set; }
    }

    


}