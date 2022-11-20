using System;
using System.Collections.Generic;
using System.Text;

namespace ATM_APP.Domain.Entities
{
    public class InternalTransfer
    {
        public decimal TransferAmount { get; set; }
        public long RecipeintBankAccountNumber { get; set; }
        public string RecipeintBankAccountName { get; set; }
    }
}
