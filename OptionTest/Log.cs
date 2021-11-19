using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OptionTest
{
    class Log : Result
    {
        private static string _LogPath = GetProcessLogPath();

        public enum PauseOption : int
        {
            NoPause,
            OnePause,
            LoopPause
        }

        public static string LogPath { get { return _LogPath; } set { _LogPath = value; } }

        public Log()
        {
            _LogPath = @"\Working\Seqloga.txt";
        }

        public Log(string logPath)
        {
            _LogPath = logPath;

        }

        private static void RunPauseOption(PauseOption option)
        {
            switch (option)
            {
                case PauseOption.NoPause:
                    break;
                case PauseOption.OnePause:
                    Pause();
                    break;
                case PauseOption.LoopPause:
                    PauseLoop();
                    break;
            };
        }

        public static void Pause()
        {
            Console.WriteLine("Press any Key to continues");
            Console.ReadKey();
        }

        public static void PauseLoop()
        {
            while (true)
            {
                Pause();
            }
        }
        public static void PauseLoop(string message)
        {
            while (true)
            {
                Pause();
            }
        }
        public static void PauseLoop(Exception exception)
        {
            while (true)
            {
                WriteLog(exception.Message.ToString());
                WriteLog(exception.StackTrace.ToString());
                Pause();
            }
        }

        public static void Exit(int exitCode)
        {
            Environment.Exit(exitCode);
        }
        public static void Exit(string Message, int exitCode)
        {
            WriteLog(Message);
            CreateFailFlag(Message, exitCode);
            Environment.Exit(exitCode);
        }
        public static void Exit(Exception exception, int exitCode)
        {
            WriteLog(exception.Message.ToString());
            WriteLog(exception.StackTrace.ToString());
            CreateFailFlag(exception, exitCode);
            Environment.Exit(exitCode);
        }
        public static void Exit(int exitCode , PauseOption Option)
        {
            RunPauseOption(Option);
            Environment.Exit(exitCode);
        }
        public static void Exit(string Message, int exitCode, PauseOption Option)
        {
            WriteLog(Message);
            CreateFailFlag(Message, exitCode);
            RunPauseOption(Option);
            Environment.Exit(exitCode);
        }
        public static void Exit(Exception exception, int exitCode, PauseOption Option)
        {
            WriteLog(exception.Message.ToString());
            WriteLog(exception.StackTrace.ToString());
            CreateFailFlag(exception, exitCode);
            RunPauseOption(Option);
            Environment.Exit(exitCode);
        }

            


        public static void PrintError(string message)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Clear();
            //Console.WriteLine(message);
            WriteLog();
            WriteLog(message);
            WriteLog();
            WriteLog("Please Contact Engineer Team");
            PauseLoop(message);
        }

        public static void PrintError(string format, params object[] arg)
        {
            try
            {
                string newString = format;
                for (int i = 0; i < arg.Length; i++)
                {
                    if (arg[i] == null) arg[i] = string.Empty;
                    newString = newString.Replace("{" + i + "}", arg[i].ToString());
                }

                PrintError(newString);
            }
            catch (Exception ex)
            {
                PauseExit(ex, 2);
            }
        }
        public static void PrintError(string format, object arg0)
        {
            try
            {
                string newString = format;
                newString = newString.Replace("{" + 0 + "}", arg0.ToString());
                PrintError(newString);
            }
            catch (Exception ex)
            {
                PauseExit(ex, 2);
            }
        }


        public static void WriteLog()
        {
            try
            {
                Console.WriteLine();

                System.IO.File.AppendAllText(_LogPath, "\n");
            }
            catch (Exception ex)
            {
                PauseExit(ex, 1);
            }
        }
        public static void WriteLog(string input)
        {
            try
            {
                Console.WriteLine();
                Console.WriteLine(input);

                System.IO.File.AppendAllText(_LogPath, input + "\n");
            }
            catch (Exception ex)
            {
                PauseExit(ex, 1);
            }
        }
        public static void WriteLog(string format, params object[] arg)
        {
            try
            {
                string newString = format;
                for (int i = 0; i < arg.Length; i++)
                {
                    if (arg[i] == null) arg[i] = string.Empty;
                    newString = newString.Replace("{" + i + "}", arg[i].ToString());
                }

                WriteLog(newString);
            }
            catch (Exception ex)
            {
                PauseExit(ex, 1);
            }
        }
        public static void WriteLog(string format, object arg0)
        {
            try
            {
                string newString = format;
                newString = newString.Replace("{" + 0 + "}", arg0.ToString());
                WriteLog(newString);
            }
            catch (Exception ex)
            {
                PauseExit(ex, 1);
            }
        }

        private static string GetProcessLogPath()
        {
            try
            {
                string IniFilePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\Setting.ini";
                if (!File.Exists(IniFilePath))
                {
                    string errormessage = "FlashBIOS Error: Cannot find Setting File";
                    Console.WriteLine(errormessage);
                    Exit(20);
                }

                IniFile ini = new IniFile(IniFilePath);

                if (ini.KeyExists("ProcessLog", "Path"))
                {
                    if (!string.IsNullOrEmpty(ini.Read("ProcessLog", "Path")))
                    {
                        return ini.Read("ProcessLog", "Path");
                    }
                }
                //return @"\BIOS\BIOSCSV.csv";

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                Console.WriteLine(ex.StackTrace.ToString());
                Exit(20);
            }
            return @"NOT FOUND";
        }
    }
}
