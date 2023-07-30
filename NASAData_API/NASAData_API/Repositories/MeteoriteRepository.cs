using Dapper;
using MySqlConnector;
using NASAData_API.DbModels;
using NASAData_API.Helpers;
using NASAData_API.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace NASAData_API.Repositories
{
    public class MeteoriteRepository : IMeteoriteRepository
    {
        private string _connectionString { get; set; }
        private string _tableName { get; set; } = "meteorites";

        public MeteoriteRepository(string connectionString) 
        {
            _connectionString = connectionString;
        }
        public DbMeteorite[] GetAll()
        {
            using (IDbConnection db = new MySqlConnection(_connectionString))
            {
                return db.Query<DbMeteorite>($"SELECT * FROM {_tableName} ").ToArray();                   
            }
        }
        public MeteoriteView[] GetAllByFilter(MeteoriteFilter filter)
        {

            using (IDbConnection db = new MySqlConnection(_connectionString))
            {
                StringBuilder builder = new StringBuilder($"SELECT Year, COUNT(*) as Count, SUM(Mass) as SumMass FROM {_tableName} WHERE ");
                builder.Append($"Year BETWEEN @FromYears AND @ToYears ");
                if (filter.ClassSelect != "")
                {
                    builder.Append($"AND Recclass = @ClassSelect ");
                }
                if (filter.SearchName != "")
                {
                    builder.Append($"AND Name LIKE CONCAT('%',@SearchName,'%') ");
                }
                builder.Append($"GROUP BY Year ");
                if (filter.SortBy != "")
                {
                    builder.Append($"ORDER BY @SortBy @SortDesc");
                }
               

                string sqlQuery = builder.ToString();

                MeteoriteView[] groupedMeteorites = db.Query<MeteoriteView>(sqlQuery, filter).Select(x => 
                {  
                    x.Year = DataTimeExtension.ParseDateTimeToYear(x.Year); 
                    return x; 
                }).ToArray();


                return groupedMeteorites;
            }
        }
        public void InsertRange(DbMeteorite[] meteorites)
        {
            using (IDbConnection db = new MySqlConnection(_connectionString))
            {
                string sqlQuery = $"INSERT INTO {_tableName} (Id, Name, Nametype, Recclass, Mass, Fall, Year, Type, Lat, Lon) " +
                                           "VALUES(@Id, @Name, @Nametype, @Recclass, @Mass, @Fall, @Year, @Type, @Lat, @Lon)";

                db.Execute(sqlQuery, meteorites);
            }
        }

        public void DeleteRange(DbMeteorite[] meteorites)
        {
            using (IDbConnection db = new MySqlConnection(_connectionString))
            {
                var sqlQuery = $"DELETE FROM {_tableName} WHERE Id = @id";
                db.Execute(sqlQuery, meteorites);
            }
        }

        public void DeleteAll()
        {
            using (IDbConnection db = new MySqlConnection(_connectionString))
            {
                var sqlQuery = $"DELETE FROM {_tableName} WHERE Id > -1";
                db.Execute(sqlQuery);
            }
        }

    }
}