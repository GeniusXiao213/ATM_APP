using System;
using System.Collections.Generic;
using System.Text;

namespace ATM_APP.UI
{
    public static class Utility
    {
        public static void PrintMessage(string msg, bool success)
        {
            
            if (success) { Console.ForegroundColor = ConsoleColor.Yellow; }
            else
            {
                Console.ForegroundColor= ConsoleColor.Red;
            }
            Console.WriteLine(msg);
            //Console.ResetColor(); change to default color grey
            Console.ForegroundColor = ConsoleColor.White;
            PressEnterToContinue();
        }
        public static string GetUserInput(string prompt)
        {
            Console.WriteLine($"Enter {prompt}");
            return Console.ReadLine();
        }
        public static void PressEnterToContinue()
        {
            Console.WriteLine("\n\nPress Enter to continue...\n");
            Console.ReadLine();
        }
    }
}
