using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Text;
using System.Threading.Tasks;

namespace GLaDOS
{
    class GlaDOSCommands
    {
        public static void Command(string UsrCommand)
        {
            switch (UsrCommand.ToLower())
            {
                case string Case when Case == "exit" || Case == "bye":
                    GLaDOSMisc.GLaDOSAnswer("Goodbye!");
                    Thread.Sleep(100);
                    Environment.Exit(0);
                    break;
                case string Case when Case == "time":
                    GLaDOSMisc.GLaDOSAnswer("Current time is: " + DateTime.Now.ToLongTimeString() + ".");
                    break;
                case string Case when Case == "date":
                    GLaDOSMisc.GLaDOSAnswer("Current date is: " + DateTime.Now.ToShortDateString() + ". " + DateTime.Now.DayOfWeek + ".");
                    break;
                case string Case when Case == "help" || Case == "info" || Case == "?":
                    GLaDOSMisc.GLaDOSAnswer("Information missing :(");
                    break;
                case string Case when Case == "sudo" || Case == "su" || Case == "admin":
                    GLaDOSMisc.EnterPassword();
                    break;
                case string Case when Case == "register" || Case == "reg" || Case == "newusr":
                    GLaDOSMisc.Register();
                    break;
                case string Case when Case == "neural" || Case == "neuralnet":
                    GLaDOSMisc.NeuralNetwork();
                    break;
                case string Case when Case == "clr" || Case == "clrscr" || Case == "clear":
                    Console.Clear();
                    break;
                case string Case when Case == "python" || Case == "py":
                    GLaDOSMisc.PythonScr();
                    break;
                case string Case when Case == "list" || Case == "li":
                    GLaDOSMisc.GetServersList();
                    goto default;
                case string Case when Case.Contains("+") || Case.Contains("-") || Case.Contains("*") || Case.Contains("/") || Case.Contains("!"):
                    GLaDOSCalc.CalculatorActionHandler(Case);
                    break;
                case string Case when Case == "rnd":
                    Random Randomize = new Random();
                    GLaDOSMisc.GLaDOSAnswer(Convert.ToString(Randomize.Next(0, 9)));
                    break;
                default:
                    GLaDOSMisc.GLaDOSAnswer("Error. Enter command again or type \"?\" for information.");
                    break;
            }
        }
    }
}
