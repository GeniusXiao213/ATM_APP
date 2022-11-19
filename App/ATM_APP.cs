﻿using ATM_APP.Domain.Entities;
using ATM_APP.Domain.Interfaces;
using ATM_APP.UI;
using System;
using System.Collections.Generic;
using System.Threading;

namespace ATM_APP.App
{
    public class ATM_APP : IUserLogin
    {
        private List<UserAccount> userAccountList;
        private UserAccount selectedAccount;

        public void InitializeData()
        {
            userAccountList = new List<UserAccount>
            {
                new UserAccount{Id=1,FullName="Xiaohan Wan",AccountNumber=123456,CardNumber=321321,CardPin=123123,AccountBalance=50000.00m,IsLocked=false},
                new UserAccount{Id=2,FullName="Anton Yahorau",AccountNumber=456789,CardNumber= 654654, CardPin=456456,AccountBalance=4000.00m,IsLocked=false},
                new UserAccount{Id=3, FullName = "Lanxi Wen",AccountNumber=123555,CardNumber=987987,CardPin=789789,AccountBalance=2000.00m,IsLocked=true}
            };
        }
        public void CheckUserCardNumAndPassword()
        {
            bool isCorrectLogin = false;
            while (isCorrectLogin == false)
            {
                UserAccount inputAccount = Appscreen.UserLoginForm();
                Appscreen.LoginProgress();
                foreach(UserAccount account in userAccountList)
                {
                    selectedAccount=account;
                    if(inputAccount.CardNumber.Equals(selectedAccount.CardNumber))
                    {
                        selectedAccount.TotalLogin++;
                        if(inputAccount.CardPin.Equals(selectedAccount.CardPin))
                        {
                            selectedAccount = account;
                            if(selectedAccount.IsLocked||selectedAccount.TotalLogin>3)
                            {
                                Appscreen.PrintLockScreen();
                            }
                            else
                            {
                                selectedAccount.TotalLogin = 0;
                                isCorrectLogin = true;
                                break;
                            }
                        }
                    }
                    if (isCorrectLogin == false)
                    {
                        Utility.PrintMessage("\nInvalid car number or PIN.", false);
                        selectedAccount.IsLocked = selectedAccount.TotalLogin == 3;
                        if (selectedAccount.IsLocked)
                        {
                            Appscreen.PrintLockScreen();
                        }
                    }
                    Console.Clear();
                }
            }
            
            
        }

        public void Welcome()
        {
            Console.WriteLine($"Welcome back,{selectedAccount.FullName}");
        }
       
    }
}
