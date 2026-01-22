using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Data;

namespace DATAACCESS
{
    public interface IDataAccess
    {
        Task<bool> ExecuteStoredProcedure(string _name, string _query, List<SqlParameter> _parameter);

        Task<DataTable> Select(string _name, string _query);

        Task<DataTable> SelectStoredProcedure(string _name, string _query, List<SqlParameter> _parameter);

        Task<HealthCheckResult> Check(string _name);
    }
}
