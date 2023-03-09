using Dapper;
using Microsoft.EntityFrameworkCore;
using Test.Infrastructure.Persistence;
using System.Data;
using System.Data.SqlClient;
using static Dapper.SqlMapper;


namespace Test.Infrastructure.Repositories.Base
{
    public abstract class BaseRepository
    {
        protected string connectionString = null;
        public BaseRepository(DatabaseContext dbContext)
        {
            connectionString = dbContext.Database.GetDbConnection().ConnectionString;
        }
        protected void Initialize(string connectionString)
        {
            this.connectionString = connectionString;
        }

        protected IDbConnection Connection()
        {
            try
            {
                return new SqlConnection(this.connectionString);
            }
            catch (SqlException ex)
            {
                //TODO
                //IList<ValidationFailure> messages = new List<ValidationFailure>();
                //for (int i = 0; i < ex.Errors.Count; i++)
                //{
                //    messages.Add(new ValidationFailure(Convert.ToString(ex.ErrorCode), ex.Errors[i].Message));
                //}
                //throw new ValidationException(messages);
                throw new Exception("Error Occur");
            }
            catch (Exception)
            {
                throw;
            }

        }

        protected static int SqlTimeout
        {
            get { return 30; }
        }

        protected async Task<int> ExecuteScalar(string queryName, object param = null)
        {
            try
            {
                using IDbConnection dbConnection = Connection();
                int returnVal = -1;
                returnVal = await dbConnection.QueryFirstOrDefaultAsync<int>(queryName,
                                                    param: param,
                                                    commandType: CommandType.StoredProcedure,
                                                    commandTimeout: SqlTimeout);
                return returnVal;
            }
            catch (SqlException ex)
            {
                //IList<ValidationFailure> messages = new List<ValidationFailure>();
                //for (int i = 0; i < ex.Errors.Count; i++)
                //{
                //    messages.Add(new ValidationFailure(Convert.ToString(ex.ErrorCode), ex.Errors[i].Message));
                //}
                //throw new ValidationException(messages);
                throw new Exception("Error Occur");
            }
            catch (Exception)
            {
                throw;
            }

        }

        protected async void ExecuteQuery(string queryName, object param = null)
        {
            try
            {
                using IDbConnection dbConnection = Connection();
                await dbConnection.QueryAsync(queryName, param: param, commandType: CommandType.StoredProcedure, commandTimeout: SqlTimeout);
            }
            catch (SqlException ex)
            {
                //IList<ValidationFailure> messages = new List<ValidationFailure>();
                //for (int i = 0; i < ex.Errors.Count; i++)
                //{
                //    messages.Add(new ValidationFailure(Convert.ToString(ex.ErrorCode), ex.Errors[i].Message));
                //}
                //throw new ValidationException(messages);
                throw new Exception("Error Occur");
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected async Task<T> ExecuteQueryMultiResult<T, K>(string queryName, object param = null)
        {
            try
            {
                using IDbConnection dbConnection = Connection();
                object returnVal;
                returnVal = await dbConnection.QueryAsync<K>(queryName, param: param, commandType: CommandType.StoredProcedure, commandTimeout: SqlTimeout);
                return (T)Convert.ChangeType(returnVal, typeof(T));
            }
            catch (SqlException ex)
            {
                //IList<ValidationFailure> messages = new List<ValidationFailure>();
                //for (int i = 0; i < ex.Errors.Count; i++)
                //{
                //    messages.Add(new ValidationFailure(Convert.ToString(ex.ErrorCode), ex.Errors[i].Message));
                //}
                //throw new ValidationException(messages);
                throw new Exception("Error Occur");
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected async Task<T> ExecuteQuerySingleResult<T>(string queryName, object param = null)
        {
            try
            {
                using IDbConnection dbConnection = Connection();
                T returnVal;
                returnVal = await dbConnection.QueryFirstAsync<T>(queryName, param: param, commandType: CommandType.StoredProcedure, commandTimeout: SqlTimeout);
                return returnVal;
            }
            catch (SqlException ex)
            {
                //IList<ValidationFailure> messages = new List<ValidationFailure>();
                //for (int i = 0; i < ex.Errors.Count; i++)
                //{
                //    messages.Add(new ValidationFailure(Convert.ToString(ex.ErrorCode), ex.Errors[i].Message));
                //}
                //throw new ValidationException(messages);
                throw new Exception("Error Occur");
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected async Task<IDataReader> executeReader(string queryName, object param = null)
        {
            try
            {
                using IDbConnection dbConnection = Connection();
                return await dbConnection.ExecuteReaderAsync(queryName, param: param, commandType: CommandType.StoredProcedure, commandTimeout: SqlTimeout);
            }
            catch (SqlException ex)
            {
                //IList<ValidationFailure> messages = new List<ValidationFailure>();
                //for (int i = 0; i < ex.Errors.Count; i++)
                //{
                //    messages.Add(new ValidationFailure(Convert.ToString(ex.ErrorCode), ex.Errors[i].Message));
                //}
                //throw new ValidationException(messages);
                throw new Exception("Error Occur");
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected async Task<SqlMapper.GridReader> QueryMultiple(string queryName, object param = null)
        {
            try
            {
                using IDbConnection dbConnection = Connection();
                return await dbConnection.QueryMultipleAsync(queryName, param: param, commandType: CommandType.StoredProcedure, commandTimeout: SqlTimeout);
            }
            catch (SqlException ex)
            {
                //IList<ValidationFailure> messages = new List<ValidationFailure>();
                //for (int i = 0; i < ex.Errors.Count; i++)
                //{
                //    messages.Add(new ValidationFailure(Convert.ToString(ex.ErrorCode), ex.Errors[i].Message));
                //}
                //throw new ValidationException(messages);
                throw new Exception("Error Occur");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<object>> GettMultipleResultSet(string queryName, object param, params Func<GridReader, object>[] readerFuncs)
        {
            var returnResults = new List<object>();

            try
            {
                using IDbConnection dbConnection = Connection();
                var gridReader = await dbConnection.QueryMultipleAsync(queryName, param: param, commandType: CommandType.StoredProcedure, commandTimeout: SqlTimeout);
                //var gridReader = await QueryMultiple(queryName, param);
                foreach (var readerFunc in readerFuncs)
                {
                    var obj = readerFunc(gridReader);
                    returnResults.Add(obj);
                }
            }
            catch (SqlException ex)
            {
                //IList<ValidationFailure> messages = new List<ValidationFailure>();
                //for (int i = 0; i < ex.Errors.Count; i++)
                //{
                //    messages.Add(new ValidationFailure(Convert.ToString(ex.ErrorCode), ex.Errors[i].Message));
                //}
                //throw new ValidationException(messages);
                throw new Exception("Error Occur");
            }
            catch (Exception)
            {
                throw;
            }

            return returnResults;
        }

    }
}
