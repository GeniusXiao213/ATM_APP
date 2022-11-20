using ATM_APP.Domain.Entities;
using ATM_APP.Domain.Enums;
using ATM_APP.Domain.Interfaces;
using ATM_APP.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Transactions;
using Transaction = ATM_APP.Domain.Entities.Transaction;

namespace ATM_APP.App
{
    
    public class ATM_APP : IUserLogin,IUserAccountActions,ITransaction
    {
        private List<UserAccount> userAccountList;
        private UserAccount selectedAccount;
        private List<Transaction> _listOfTransactions;
        private const decimal mininumKeptAmount = 500;

        private readonly Appscreen screen;

        public ATM_APP()
        {
            screen = new Appscreen(); //instantiate the screen so that the non-static method can be accessed
        }

        public void Run()
        {

            Appscreen.Welcome();
            CheckUserCardNumAndPassword();
            Appscreen.WelcomeCustomer(selectedAccount.FullName);
            while(true)
            {
                Appscreen.DisplayAppMenu();
                ProcessMenuOption();
            }
        }
        public void InitializeData()
        {
            userAccountList = new List<UserAccount>
            {
                new UserAccount{Id=1,FullName="Xiaohan Wan",AccountNumber=123456,CardNumber=321321,CardPin=123123,AccountBalance=50000.00m,IsLocked=false},
                new UserAccount{Id=2,FullName="Anton Yahorau",AccountNumber=456789,CardNumber= 654654, CardPin=456456,AccountBalance=4000.00m,IsLocked=false},
                new UserAccount{Id=3, FullName = "Lanxi Wen",AccountNumber=123555,CardNumber=987987,CardPin=789789,AccountBalance=2000.00m,IsLocked=true}
            };
            _listOfTransactions = new List<Transaction>();
        }
        public void CheckUserCardNumAndPassword()
        {
            bool isCorrectLogin = false;
            while (isCorrectLogin == false)
            {
                UserAccount inputAccount = Appscreen.UserLoginForm();
                Appscreen.LoginProgress();
                foreach(UserAccount account in userAccountList)  //?
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

        private void ProcessMenuOption()
        {
            switch (Validator.Convert<int>("an option:"))
            {
                case (int)AppMenu.CheckBalance:
                    CheckBalance();
                    break;
                case (int)AppMenu.PlaceDeposit:
                    PlaceDeposit();
                    break;
                case (int)AppMenu.MakeWithdrawal:
                    MakeWithDrawal();
                    break;
                case (int)AppMenu.InternalTransfer:
                    //var internalTransfer = Appscreen.InternalTransferForm();
                    //non-static can not be accessed
                    var internalTransfer = screen.InternalTransferForm();
                    ProcessInternalTransfer(internalTransfer);
                    break;
                case (int)AppMenu.ViewTransaction:
                    ViewTransaction();
                    break;
                case (int)AppMenu.Logout:
                    Appscreen.LogOutProgress();
                    Utility.PrintMessage("You have successfully logged out. Please collect your" +
                        " ATM card.",true);
                    break;

                default:
                    Utility.PrintMessage("Invalid Option.", false);
                    break;

            }
            
        }

        public void CheckBalance()
        {
            Utility.PrintMessage($"Your account balance is: {Utility.FormatAmount(selectedAccount.AccountBalance)}",true);
        }

        public void PlaceDeposit()
        {
            Console.WriteLine("\nOnly multiples of 500 and 1000 dollar allowed.\n");
            var transaction_amt = Validator.Convert<int>($"amount {Appscreen.cur}");

            Console.WriteLine("\nChecking and Counting bank notes.");
            Utility.PrintDotAnimation();
            Console.WriteLine("");

            if(transaction_amt<=0)
            {
                Utility.PrintMessage("Amount needs to be greater than zero. Try again.", false);
                return;
            }
            if(transaction_amt%500!=0)
            {
                Utility.PrintMessage($"Enter deposit amount in multiple of 500 or 1000. Try again.", false);
                return;
            }

            if(PreviewBankNotesCount(transaction_amt)==false)
            {
                Utility.PrintMessage($"You have cancelled your action.", false);
                return;
            }

            //bind transaction details to transaction object
            InsertTransaction(selectedAccount.Id, TransactionType.Deposit, transaction_amt, "");

            //update account balance
            selectedAccount.AccountBalance += transaction_amt;

            Utility.PrintMessage($"Your deposit of {Utility.FormatAmount(transaction_amt)} was successful.",true);

        }

        public void MakeWithDrawal()
        {
            var transaction_amt = 0;
            int selectedAmount = Appscreen.SelectedAmount();
            if(selectedAmount==-1)
            {
                MakeWithDrawal();
                return;
            }
            else if(selectedAmount!=0)
            {
                transaction_amt= selectedAmount;
            }
            else
            {
                transaction_amt= Validator.Convert<int>($"amount: {Appscreen.cur}");
            }

            //input validation
            if(transaction_amt<=0)
            {
                Utility.PrintMessage("Amount needs to be greater than zero. Try again.", false);
                return;
            }
            if(transaction_amt%500!=0)
            {
                Utility.PrintMessage("You can only withdraw amount in multiples of 500 or 1000 dollar. Try again.", false);
                return;
            }

            //Business logic validation
            if(transaction_amt>selectedAccount.AccountBalance)
            {
                Utility.PrintMessage($"Withdrawal failed. Your balance is too low to withdrawl" +
                    $"{Utility.FormatAmount(transaction_amt)}", false);
                return;
            }
            if((selectedAccount.AccountBalance-transaction_amt)<mininumKeptAmount)
            {
                Utility.PrintMessage($"Withdrawal failed. Your account needs to have" +
                    $"minimum{Utility.FormatAmount(mininumKeptAmount)}", false);
                return;
            }
            InsertTransaction(selectedAccount.Id,TransactionType.Withdrawal,-transaction_amt,"");
            //update account balance message

            selectedAccount.AccountBalance-= transaction_amt;

            Utility.PrintMessage($"You have successfully withdraw" +
                $"{Utility.FormatAmount(transaction_amt)}", true);
            return;

        }

        private bool PreviewBankNotesCount(int amount)
        {
            int thousandNotesCount = amount / 1000;
            int fiveHundredNotesCount = (amount % 1000) / 500;

            Console.WriteLine("\nSummary");
            Console.WriteLine("------");
            Console.WriteLine($"{Appscreen.cur}1000 X {thousandNotesCount}={1000 * thousandNotesCount}");
            Console.WriteLine($"{Appscreen.cur}500 X {fiveHundredNotesCount}={500*fiveHundredNotesCount}");
            Console.WriteLine($"Total amount: {Utility.FormatAmount(amount)}\n\n");

            int opt = Validator.Convert<int>("1 to confirm");
            return opt.Equals(1);
        }

        public void InsertTransaction(long _UserBankAccountId, TransactionType _transType, decimal _tranAmount, string _desc)
        {

            var transaction = new Transaction()
            {
                TransactionId = Utility.GetTransactionId(),
                UserBankAccountId = _UserBankAccountId,
                TransactionDate = DateTime.Now,
                TransactionType = _transType,
                TransactionAmount = _tranAmount,
                Description = _desc

            };
            //add transaction object to the list
            _listOfTransactions.Add(transaction);

        }

        public void ViewTransaction()
        {
            var filteredTransactionList=_listOfTransactions.Where(t=>t.UserBankAccountId==selectedAccount.Id).ToList();
            //check if there is a transaction
            if(filteredTransactionList.Count <=0 ) 
            {
                Utility.PrintMessage("You have no transaction yet.", true);

            }
            else
            {
                /*var table = new ConsoleTable("Id", "Transaction Date", "Type", "Descriptions", "Amount " + Appscreen.cur);
                foreach(var tran in filteredTransactionList)
                {
                    table.AddRow(tran.TransactionId,tran.TransactionDate,tran.TransactionType,tran.Description,tran.TransactionAmount);
                }
                table.Options.EnableCount = false;
                table.Write();
                Utility.PrintMessage($"You have {filteredTransactionList.Count} transaction(s)", true);
                   */

            }
        }


        private void ProcessInternalTransfer(InternalTransfer internalTransfer)
        {
            if(internalTransfer.TransferAmount<=0 )
            {
                Utility.PrintMessage("Amount needs to be more than zero. Try again.", false);
                return;
            }
            if(internalTransfer.TransferAmount>selectedAccount.AccountBalance)
            {
                Utility.PrintMessage($"Transfer failed. You do not have enough balance" +
                    $"to transfer {Utility.FormatAmount(internalTransfer.TransferAmount)}", false);
                return;
            }

            if((selectedAccount.AccountBalance-mininumKeptAmount)<mininumKeptAmount)
            {
                Utility.PrintMessage($"Transfer failed. Your account needs to have minimum" +
                    $"{Utility.FormatAmount(mininumKeptAmount)}", false);
                return;
            }

            var selectedBankAccountReciever=(from userAcc in userAccountList
                                             where userAcc.AccountNumber==internalTransfer.RecipeintBankAccountNumber
                                             select userAcc).FirstOrDefault();
            if(selectedBankAccountReciever==null)
            {
                Utility.PrintMessage("Transfer failed. Reciever bank account number is invalid.", false);
                return;
            }

            //check receiver's name
            if(selectedBankAccountReciever.FullName!= internalTransfer.RecipeintBankAccountName)
            {
                Utility.PrintMessage("Transfer failed. Recipients's bank account name does noe match.", false);
                return;
            }

            //add transaction to transactions record-sender
            InsertTransaction(selectedAccount.Id, TransactionType.Transfer, -internalTransfer.TransferAmount, "transfer" +
                $"to {selectedBankAccountReciever.AccountNumber} ({selectedBankAccountReciever.FullName})");
            //update sender's account balance

            selectedAccount.AccountBalance-=internalTransfer.TransferAmount;

            //add transaction record-reciever
            InsertTransaction(selectedBankAccountReciever.Id,TransactionType.Transfer,internalTransfer.TransferAmount,"Transfered from " +
                $"{selectedAccount.AccountNumber} ({selectedAccount.FullName})");

            selectedBankAccountReciever.AccountBalance += internalTransfer.TransferAmount;

            //print success msg
            Utility.PrintMessage($"You have successfully transfered" +
                $"{Utility.FormatAmount(internalTransfer.TransferAmount)} to " +
                $"{internalTransfer.RecipeintBankAccountName}", true);

        }
    }
}
