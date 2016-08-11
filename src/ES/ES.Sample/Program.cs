using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Sample
{
    class Program
    {
        static void Main(string[] args)
        {

            var store = new StoreFactory().OpenInMemoryStore(
                );
            var id = Guid.NewGuid();
            store.Save(id, new AccountCreated {AccountNo = 123456, Owner = "Franz Jaeger"}).Wait();
            store.Save(id, new Transaction() { Amount = 100 }).Wait();
            store.Save(id, new Transaction { Amount = -22 }).Wait();
            var account = store.Entity<Account>(id).Result;
            
        
            Console.WriteLine(account.Balance);

        }
    }

    public class Account
    {
        public Account()
        {
            transactions = new List<Transaction>();
        }
        public Guid AccountId { get; set; }
        public int AccountNo { get; set; }

        public string Owner { get; set; }


        public decimal Balance { get { return transactions.Sum(tx => tx.Amount); } }

        private readonly List<Transaction> transactions;

        public void Apply(Transaction transaction)
        {
            transactions.Add(transaction);
        }
        public void Apply(AccountCreated accountCreated)
        {
            AccountNo = accountCreated.AccountNo;
            Owner = accountCreated.Owner;
        }
    }

    [Serializable]
    public class Transaction
    {
        public decimal Amount { get; set; }
        public Guid Id { get; set; }
    }
    [Serializable]
    public class AccountCreated
    {
        public int AccountNo { get; set; }
       public string Owner { get; set; }
    }
}
