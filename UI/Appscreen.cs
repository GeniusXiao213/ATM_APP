using ATM_APP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ATM_APP.UI
{
    public static class Appscreen
    {
        internal static void Welcome()
        {
            Console.Clear();
            Console.Title = "WAN's ATM APP";
            //set the text color
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\n\n---------------------Welcome to My ATM APP!---------------------\n\n");
            Console.WriteLine("Please insert your ATM card");
            Console.WriteLine("Note: Actual ATM machine will accept and validate" +
                " a physical ATM card, read the card number and validate it.");

            Utility.PressEnterToContinue();
        }

        internal static UserAccount UserLoginForm()
        {
            UserAccount tempUserAccount = new UserAccount();
            tempUserAccount.CardNumber = Validator.Convert<long>("your card number.");
            tempUserAccount.CardPin = Convert.ToInt32(Utility.GetSecretInput("Enter your card PIN"));
            return tempUserAccount;
        }

        internal static void LoginProgress()
        {
            Console.WriteLine("\nChecking card number and PIN...");
            Utility.PrintDotAnimation();
        }

        internal static void PrintLockScreen()
        {
            Console.Clear();
            Utility.PrintMessage("Your account is locked. Please go to the nearest branch to unlock your account. Thank you.",true);
            Utility.PressEnterToContinue();
            Environment.Exit(1);

        }

    }
}
