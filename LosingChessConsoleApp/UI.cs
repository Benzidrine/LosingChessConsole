using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosingChessConsoleApp
{
    public class UI
    {
        public string FooBar = "";

        public UI() 
        {

        }

        public static string Welcome(String Name)
        {
            string WelcomeMessage = "";
            WelcomeMessage = "Hello " + Name + " " + FooBar;
            return WelcomeMessage;
        }
    }
}
