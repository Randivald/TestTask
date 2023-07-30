using Dapper;
using MySqlConnector;
using NASAData_API.Models;
using NASAData_API.DbModels;

namespace NASAData_API.Repositories
{
    public  interface IMeteoriteRepository: IRepository<DbMeteorite>
    {
        MeteoriteView[] GetAllByFilter(MeteoriteFilter filter);
    }
}
