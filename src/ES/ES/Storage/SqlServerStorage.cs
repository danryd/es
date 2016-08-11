using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ES.Storage
{
    class SqlServerStorage : IStorage
    {
        private readonly SqlConnection connection;
        public SqlServerStorage(string connectionString)
        {
            this.connection = new SqlConnection(connectionString);
        }

        public void Initialize()
        {

            connection.Open();
            var checkForTable = connection.CreateCommand();
            checkForTable.CommandText = SqlServerCommands.TableExistsQuery;
            var tableExists = (int)checkForTable.ExecuteScalar();
            if (tableExists == 0)
            {
                var createTable = connection.CreateCommand();
                createTable.CommandText = SqlServerCommands.CreateTableCommand;
                createTable.ExecuteNonQuery();
            }

            connection.Close();

        }
        public async Task Save(Guid entityId, string eventType, byte[] data)
        {
            connection.Open();
            using (var insert = connection.CreateCommand())
            {
                insert.CommandText = SqlServerCommands.InsertCommand;

                var dataParameter = insert.CreateParameter();
                dataParameter.ParameterName = "data";
                dataParameter.Value = data;
                insert.Parameters.Add(dataParameter);

                var typeParameter = insert.CreateParameter();
                typeParameter.ParameterName = "eventType";
                typeParameter.Value = eventType;
                insert.Parameters.Add(typeParameter);


                var entityIdParameter = insert.CreateParameter();
                entityIdParameter.ParameterName = "entityId";
                entityIdParameter.Value = entityId;
                insert.Parameters.Add(entityIdParameter);


                await insert.ExecuteNonQueryAsync();
                connection.Close();
            }
        }
        public async Task<IEnumerable<byte[]>> Events<T>(Guid entityId)
        {
            connection.Open();
            using (var select = connection.CreateCommand())
            {
                select.CommandText = SqlServerCommands.SelectByEntityAndEventTypeQuery;

                var typeParameter = select.CreateParameter();
                typeParameter.ParameterName = "eventType";
                typeParameter.Value = typeof(T).FullName;
                select.Parameters.Add(typeParameter);


                var entityIdParameter = select.CreateParameter();
                entityIdParameter.ParameterName = "entityId";
                entityIdParameter.Value = entityId;
                select.Parameters.Add(entityIdParameter);


                var result = await ReadResult(@select);
                connection.Close();
                return result;
            }
        }

        private static async Task<IEnumerable<byte[]>> ReadResult(SqlCommand @select)
        {
            using (var reader = await @select.ExecuteReaderAsync())
            {
                var result = new List<byte[]>();
                while (await reader.ReadAsync())
                {
                    var data = reader["Data"] as byte[];
                    result.Add(data);
                }
                return result;
            }
        }

        public async Task<IEnumerable<byte[]>> Events(Guid entityId)
        {
            connection.Open();
            var select = connection.CreateCommand();
            select.CommandText = SqlServerCommands.SelectByEntity;

            var entityIdParameter = select.CreateParameter();
            entityIdParameter.ParameterName = "entityId";
            entityIdParameter.Value = entityId;
            select.Parameters.Add(entityIdParameter);

            var result = await ReadResult(@select);
            connection.Close();
            return result;

        }
    }

}
