using SECRYPT;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Data;

namespace DATAACCESS
{
    public class DataAccess : IDataAccess
    {
        private readonly DBOptions _dbOptions;
        private readonly ManagerSecrypt _secrypt;
        public DataAccess(DBOptions dbOptions, SecryptOptions secryptOptions) { this._dbOptions = dbOptions; this._secrypt = new ManagerSecrypt(secryptOptions.Semilla); }
        private string Connection(DBOptionItems _dataBase)
        {
            string _password = this._secrypt.Desencriptar(_dataBase.password);
            return $"Persist Security Info=True;User ID={_dataBase.user};Pwd={_password};Server={_dataBase.server};Database={_dataBase.dataBase};Application Name={_dbOptions.name};TrustServerCertificate=true;";
        }

        public async Task<HealthCheckResult> Check(string _name)
        {
            var _response = new HealthCheckResult();
            try
            {
                var item = this._dbOptions.connections.FirstOrDefault(x => x.name == _name);
                if (item == null) { throw new InvalidOperationException($"No database configuration found for '{_name}'."); }
                try
                {
                    var _connection = new SqlConnection(Connection(item));
                    await _connection.OpenAsync();
                    await _connection.CloseAsync();
                    await Task.Run(() => _response = HealthCheckResult.Healthy($"BATABASE: {item.dataBase}; SERVER: {item.server}; USER: {item.user}"));
                }
                catch (Exception ex)
                {
                    return HealthCheckResult.Unhealthy($"COULD NOT CONNECT TO DATABASE: {item.dataBase} SERVER: {item.server}; USER: {item.user}; EXCEPTION: {ex.Message.ToUpper()}");
                }
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy($"CONFIG PARMETER DATABASE: {_name}; EXCEPTION: {ex.Message.ToUpper()}");
            }
            return _response;
        }
        public async Task<bool> ExecuteStoredProcedure(string _name, string _query, List<SqlParameter> _parameter)
        {
            var dbOptionItem = this._dbOptions.connections.FirstOrDefault(x => x.name == _name);
            if (dbOptionItem == null) { throw new InvalidOperationException($"No database configuration found for '{_name}'."); }
            using (SqlConnection _connection = new SqlConnection(Connection(dbOptionItem)))
            {
                try
                {
                    SqlCommand _command = new SqlCommand(_query, _connection) { CommandType = CommandType.StoredProcedure, CommandTimeout = 600000 };
                    _parameter.ForEach(item => { if (item.Value == null) item.Value = DBNull.Value; _command.Parameters.Add(item); });
                    await _connection.OpenAsync();
                    await _command.ExecuteNonQueryAsync();
                    await _connection.CloseAsync();
                    return true;
                }
                catch (Exception)
                {
                    await _connection.CloseAsync();
                    SqlConnection.ClearAllPools();
                    throw;
                }
            }
        }
        public async Task<DataTable> SelectStoredProcedure(string _name, string _query, List<SqlParameter> _parameter)
        {
            DataTable _consultation = new();
            var dbOptionItem = this._dbOptions.connections.FirstOrDefault(x => x.name == _name);
            if (dbOptionItem == null) { throw new InvalidOperationException($"No database configuration found for '{_name}'."); }
            using (SqlConnection _connection = new SqlConnection(Connection(dbOptionItem)))
            {
                try
                {
                    SqlDataAdapter _command = new SqlDataAdapter(_query, _connection);
                    _command.SelectCommand.CommandType = CommandType.StoredProcedure;
                    _command.SelectCommand.CommandTimeout = 3600000;
                    _parameter.ForEach(item => { if (item.Value == null) item.Value = DBNull.Value; _command.SelectCommand.Parameters.Add(item); });
                    await _connection.OpenAsync();
                    await Task.Run(() => _command.Fill(_consultation));
                    await _connection.CloseAsync();
                }
                catch (Exception)
                {
                    await _connection.CloseAsync();
                    SqlConnection.ClearAllPools();
                    throw;
                }
            }
            return _consultation;
        }
        public async Task<DataTable> Select(string _name, string _query)
        {
            DataTable _consultation = new();
            var dbOptionItem = this._dbOptions.connections.FirstOrDefault(x => x.name == _name);
            if (dbOptionItem == null) { throw new InvalidOperationException($"No database configuration found for '{_name}'."); }
            using (SqlConnection _connection = new SqlConnection(Connection(dbOptionItem)))
            {
                try
                {
                    SqlDataAdapter _command = new SqlDataAdapter(_query, _connection);
                    _command.SelectCommand.CommandType = CommandType.Text;
                    _command.SelectCommand.CommandTimeout = 3600000;
                    await _connection.OpenAsync();
                    await Task.Run(() => _command.Fill(_consultation));
                    await _connection.CloseAsync();
                }
                catch (Exception)
                {
                    await _connection.CloseAsync();
                    SqlConnection.ClearAllPools();
                    throw;
                }
            }
            return _consultation;
        }
    }
}
