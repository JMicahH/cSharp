using System;
using System.Collections.Generic;
using bankAccounts.Models;

namespace bankAccounts.Models
{
    public class Transaction : BaseEntity
    {
        public int AccountId { get ; set; }
        public int TransactionId { get ; set; }
        public double Amount { get ; set; }
        public DateTime CreatedAt { get ; set; }
    }
    
}