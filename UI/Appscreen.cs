using System;
using System.Collections.Generic;
using System.Text;

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

        
    }
}
