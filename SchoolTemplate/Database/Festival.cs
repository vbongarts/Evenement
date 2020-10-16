using System;

namespace SchoolTemplate.Database
{
    public class Festival
    {
        public int Id { get; set; }

        public string Naam { get; set; }

        public string Beschrijving { get; set; }
        public DateTime Datum { get; set; }
        public string Afbeelding { get; set; }
        public string Artists { get; set; }
    }  
}
