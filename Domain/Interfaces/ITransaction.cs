using ATM_APP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ATM_APP.Domain.Interfaces
{
    public interface ITransaction
    {
        void InsertTransaction(long _UserBankAccountId, TransactionType _transType, decimal _tranAmount, string _desc);
        void ViewTransaction();

    }
}
