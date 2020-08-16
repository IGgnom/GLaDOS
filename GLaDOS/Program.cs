using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace GLaDOS
{
    class Program
    {
        public static string Usr = "";

        static void Main(string[] args)
        {
            bool IsWorking = true;

            GLaDOSMisc.Welcome();
            while (IsWorking)
            {
                GlaDOSCommands.Command(Console.ReadLine());
                Console.Write(Usr + "> ");
            }
        }
    }
}
