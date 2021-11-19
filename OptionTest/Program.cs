using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptionTest
{
    class Program : Log
    {
        static void Main(string[] args)
        {
            Exit("TEST", 3, PauseOption.OnePause);
        }
    }
}
