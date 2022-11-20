using System;
using System.Collections.Generic;
using System.Text;

namespace ATM_APP.Domain.Interfaces
{
    public interface IUserAccountActions
    {
        void CheckBalance();
        void PlaceDeposit();
        void MakeWithDrawal();

    }
}
