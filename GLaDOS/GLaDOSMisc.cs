using System;
using System.Data.SqlClient;
using System.Diagnostics;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System.Data;
using System.Data.Sql;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLaDOS
{
    class GLaDOSMisc
    {
        private static readonly string ConnectionString = @"Data Source = DESKTOP-BLH070P; Initial Catalog = GLaDOSDatabase; Integrated Security = True";

        private static readonly SqlConnection Connection = new SqlConnection(ConnectionString);

        public static void Welcome()
        {
            try
            {
                Connection.Open();
            }
            catch (SqlException ConnectionFailure)
            {
                GLaDOSAnswer("An error with the database occurred. Please try later :c");
                GLaDOSAnswer("Error: " + ConnectionFailure.Message);
            }

            Console.WriteLine("Welcome to IGgnom's GLaDOS v0.3");
            EnterUsr();
            GLaDOSAnswer("Hello, " + Program.Usr + "!");
            Console.Write(Program.Usr + "> ");
        }

        public static void EnterUsr()
        {
            bool CorrectUsrName = false;

            GLaDOSAnswer("Enter your username:");
            while (!CorrectUsrName)
            {
                Console.Write("> ");
                Program.Usr = Console.ReadLine();
                if (Program.Usr == "")
                    GLaDOSAnswer("Enter your username again.");
                else CorrectUsrName = true;
            }
        }

        public static void EnterPassword()
        {
            string Username = Program.Usr;

            if (Username[0] == '$')
            {
                GLaDOSAnswer("You are already authorized!");
                return;
            }

            string Password;
            string EncPassword;
            string Key;
            string RndKey;
            string CommandString = "SELECT Password, [Key], RndKey FROM [Users] WHERE (Username = '" + Username + "')";
            SqlCommand Command = new SqlCommand(CommandString, Connection);
            Command.ExecuteNonQuery();

            if (Command.ExecuteScalar() != null)
            {
                SqlDataReader Reader = Command.ExecuteReader();
                Reader.Read();
                EncPassword = Reader["Password"].ToString();
                Key = Reader["Key"].ToString();
                RndKey = Reader["RndKey"].ToString();

                GLaDOSAnswer("Enter password:");

                for (int i = 0; i < 3; i++)
                {
                    Console.Write(Program.Usr + "> ");
                    Password = Console.ReadLine();
                    if (Password != GLaDOSSecutity.Decode(EncPassword, Key, Username, @RndKey))
                    {
                        GLaDOSAnswer("Incorrect password. Enter again:");
                    }
                    else
                    {
                        Program.Usr = "$" + Program.Usr;
                        break;
                    }
                }
            }
            else
            {
                GLaDOSAnswer("Current user is not registered.");
            }
        }

        public static void Register()
        {
            string Username = Program.Usr;

            if (Username[0] == '$')
            {
                GLaDOSAnswer("You are already authorized!");
                return;
            }

            string CommandString = "SELECT Password, [Key], RndKey FROM [Users] WHERE (Username = '" + Username + "')";
            SqlCommand Command = new SqlCommand(CommandString, Connection);

            if (Command.ExecuteScalar() != null)
            {
                GLaDOSAnswer("You are already registered!");
                return;
            }

            string RndKey = null;
            string Password;
            string Key;
            byte RndNum;

            Random RandomChar = new Random();

            for (int i = 0; i < 16; i++)
            {
                RndNum = (byte)RandomChar.Next(33, 122);
                RndKey += (char)RndNum;
            }

            GLaDOSAnswer("Create your password:");
            Console.Write(Program.Usr + "> ");
            Password = Console.ReadLine();
            GLaDOSAnswer("Create your secure key:");
            Console.Write(Program.Usr + "> ");
            Key = Console.ReadLine();

            CommandString = "INSERT INTO [Users] VALUES ('" + Username + "','" + GLaDOSSecutity.Encode(Password, Key, Username, @RndKey) + "','" + Key + "','" + @RndKey + "')";
            Command = new SqlCommand(CommandString, Connection);

            try
            {
                Command.ExecuteNonQuery();
            }
            catch (Exception CommandExecutionFailure)
            {
                GLaDOSAnswer("An error occurred. Please try again.");
                GLaDOSAnswer("Error: " + CommandExecutionFailure.Message);
            }
        }

        public static void NeuralNetwork()
        {
            try
            {
                string FileName = @"D:/Programs/PythonProjects/maze.py";
                string Output;

                Process Python = new Process
                {
                    StartInfo = new ProcessStartInfo(@"D:\Python/python.exe", FileName)
                    {
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };
                Python.Start();
                Output = Python.StandardOutput.ReadToEnd();
                GLaDOSAnswer(Output);
            }
            catch (Exception ProcessExecutionFailure)
            {
                GLaDOSAnswer("An error occurred. Please try again.");
                GLaDOSAnswer("Error: " + ProcessExecutionFailure.Message);
            }
        }

        public static void PythonScr()
        {
            string ExecuteScr = null;
            string InputStr = null;

            ScriptEngine Engine = Python.CreateEngine();

            GLaDOSAnswer("Write down your script:");
            while (InputStr != "exec")
            {
                Console.Write("> ");
                InputStr = Console.ReadLine();
                if (InputStr != "exec")
                    ExecuteScr += InputStr + "\n";
            }

            try
            {
                Console.Write("Python> ");
                Engine.Execute(ExecuteScr);
            }
            catch (Exception ScriptExecutionFailure)
            {
                Console.WriteLine("An error occurred. Please try again. Error: " + ScriptExecutionFailure.Message);
            } 
        }

        public static void GetServersList()
        {
            SqlDataSourceEnumerator Instance = SqlDataSourceEnumerator.Instance;
            DataTable Table = Instance.GetDataSources();

            GLaDOSAnswer("Aviable servers list:");

            foreach (DataRow Row in Table.Rows)
            {
                foreach (DataColumn Column in Table.Columns)
                {
                    Console.WriteLine($"\t{Column.ColumnName}: {Row[Column]}");
                }
                Console.WriteLine("");
            }
        }

        public static void GLaDOSAnswer(string Answer)
        {
            Console.WriteLine("GLaDOS> " + Answer);
        }
    }
}
