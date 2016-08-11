using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using ES.Activation;
using ES.Serialization;
using ES.Storage;

namespace ES
{
    public class StoreFactory
    {
        private static readonly string DefaultConnection = "Server=.;Database=ES;Trusted_Connection=True;";
        private IDictionary<string,IStore> stores = new Dictionary<string, IStore>();

        private static void Initialize(string connectionString)
        {
           
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IStore OpenStore()
        {
            if (!stores.ContainsKey(DefaultConnection))
            {
                var storage = new SqlServerStorage(DefaultConnection);
                storage.Initialize();
                var store =  new Store(new BinarySerializer(),storage, new Activator());
               
                stores.Add(DefaultConnection,store);
            }


            return stores[DefaultConnection];
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IStore OpenInMemoryStore()
        {
            var storage = new InMemoryStorage();
            storage.Initialize();
            var store = new Store(new BinarySerializer(), storage, new Activator());
            return store;
        }
        private static SqlConnection SqlConnection(string connectionString)
        {
            var connection = new SqlConnection();
            connection.ConnectionString = connectionString;
            return connection;
        }
    }
}
