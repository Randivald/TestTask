namespace NASAData_API.Models
{
    public class Meteorite
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Nametype { get; set; }
        public string Recclass { get; set; }
        public double Mass { get; set; }
        public string Fall { get; set; }
        public DateTime Year { get; set; }
        public Geolocation Geolocation { get; set; }
    }
}


