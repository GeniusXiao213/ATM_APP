using System;
using System.Collections.Generic;
using System.Text;

namespace ATM_APP.Domain.Enums
{
    public enum AppMenu
    {
        CheckBalance=1, //defalut would be starts from 0
        PlaceDeposit,
        MakeWithdrawal,
        InternalTransfer,
        ViewTransaction,
        Logout
    }
}
