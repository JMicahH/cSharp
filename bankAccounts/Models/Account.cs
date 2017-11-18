using System;
using System.Collections.Generic;
using bankAccounts.Models;

namespace bankAccounts.Models
{
    public class Account : BaseEntity
    {
        public int AccountId { get; set; }
        public int UserId { get; set; }
        public double Balance { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<Transaction> Transactions { get; set; }
        public Account()
        {
            Transactions = new List<Transaction>();
        }

    }
}