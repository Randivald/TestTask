namespace NASAData_API.DbModels
{
    public class DbMeteorite
    {
        public int      Id  { get; set; }
        public string   Name { get; set; }
        public string   Nametype { get; set; }
        public string   Recclass { get; set; }
        public double   Mass { get; set; }
        public string   Fall { get; set; }
        public DateTime Year { get; set; }
        public string   Type { get; set; }
        public double   Lat { get; set; }
        public double   Lon { get; set; }
    }

}


