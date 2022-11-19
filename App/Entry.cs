using ATM_APP.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace ATM_APP.App
{
    internal class Entry
    {
        static void Main(string[] args)
        {
            Appscreen.Welcome();
            long cardNumber = Validator.Convert<long>("your card number");
            Console.WriteLine($"Your card number is {cardNumber}");

            Utility.PressEnterToContinue();
        }
    }
}
