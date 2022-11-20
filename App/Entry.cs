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
            ATM_APP atmApp=new ATM_APP();
            atmApp.InitializeData();
            atmApp.Run();
            
        }
    }
}
