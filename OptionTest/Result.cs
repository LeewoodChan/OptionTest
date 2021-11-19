using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OptionTest
{
    class Result
    {
        /// <summary>
        /// Get the Output path from Setting, then create a new text file with the New Refurb FeatureByte store inside.
        /// </summary>
        /// <param name="Message">New Refurb FeatureByte After Add and Remove Requirement Component</param>
        public void CreateOutput(string Message)
        {
            string ErrorPath = GetErrorOutputPath();
            string OutputPath = GetResultOutputPath();
            //it will delete the exist file and create a new file.
            RemovePreviousOutput(ErrorPath, OutputPath);

            using (StreamWriter Writer = new StreamWriter(OutputPath))
            {
                //Console.WriteLine(NewFeatureByte);
                Writer.WriteLine(Message);
            }


        }


        /// <summary>
        /// Get the ouput path from Setting, then create a new flag file with ErrorMessage inside
        /// It will exit and return the ErrorLevel and Print ErrorMessage
        /// </summary>
        /// <param name="ErrorLevel">ErrorLevel need to return after EXIT the tool</param>
        /// <param name="ErrorMessage">ErrorMessage print before EXIT the tool</param>
        public static void CreateFailFlag(string ErrorMessage, int ErrorLevel)
        {
            try
            {
                string ErrorPath = GetErrorOutputPath();
                string OutputPath = GetResultOutputPath();
                //it will delete the exist file and create a new file.
                RemovePreviousOutput(ErrorPath, OutputPath);
                using (StreamWriter Writer = new StreamWriter(ErrorPath))
                {
                    //Console.WriteLine("ErrorLevel {0} = {1}", ErrorLevel, ErrorMessage);
                    Writer.WriteLine("ErrorLevel {0} = {1}", ErrorLevel, ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                Log.Exit(ex, 21);

            }
        }
        /// <summary>
        /// Get the ouput path from Setting, then create a new flag file with ErrorMessage inside
        /// It will exit and return the ErrorLevel and Print ErrorMessage
        /// </summary>
        /// <param name="ErrorLevel">ErrorLevel need to return after EXIT the tool</param>
        /// <param name="except">ErrorMessage from Except print before EXIT the tool</param>
        public static void CreateFailFlag(Exception except, int ErrorLevel)
        {
            try
            {
                string ErrorPath = GetErrorOutputPath();
                string OutputPath = GetResultOutputPath();
                //it will delete the exist file and create a new file.
                RemovePreviousOutput(ErrorPath, OutputPath);
                using (StreamWriter Writer = new StreamWriter(ErrorPath))
                {
                    //Console.WriteLine("ErrorLevel {0} = {1}", ErrorLevel, ErrorMessage);
                    Writer.WriteLine("ErrorLevel {0}:\n", ErrorLevel);
                    Writer.WriteLine(except.Message.ToString());
                    Writer.WriteLine(except.StackTrace.ToString());

                }
            }
            catch (Exception ex)
            {
                Log.Exit(ex, 21);

            }
        }

        /// <summary>
        /// Get the Output path from the Setting.ini file.
        /// If there is no path found, return the path folder of tool with filename Ouput.txt
        /// </summary>
        /// <returns></returns>
        private static string GetResultOutputPath()
        {

            string IniFilePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\Setting.ini";

            if (!File.Exists(IniFilePath))
            {
                Console.WriteLine("FlashBIOS Error: Cannot find Setting File");
                Environment.Exit(3); //Exist this application or tool.
            }

            IniFile ini = new IniFile(IniFilePath);

            if (ini.KeyExists("Output", "Path"))
            {
                if (!string.IsNullOrEmpty(ini.Read("Output", "Path")))
                {
                    return ini.Read("Output", "Path");
                }
            }
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\Output.txt";
        }

        /// <summary>
        /// Get the Error path from the Setting.ini file.
        /// If there is no path found, return the path folder of tool with filename Error.flg
        /// </summary>
        /// <returns></returns>
        private static string GetErrorOutputPath()
        {

            string IniFilePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\Setting.ini";

            if (!File.Exists(IniFilePath))
            {
                Console.WriteLine("FlashBIOS Error: Cannot find Setting File");
                Environment.Exit(4); //Exist this application or tool.
            }

            IniFile ini = new IniFile(IniFilePath);

            if (ini.KeyExists("Error", "Path"))
            {
                if (!string.IsNullOrEmpty(ini.Read("Error", "Path")))
                {
                    return ini.Read("Error", "Path");
                }
            }
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\Error.txt";
        }
        /// <summary>
        /// Remove all the create output and error
        /// </summary>
        private static void RemovePreviousOutput(string OutputPath, string ErrorPath)
        {
            if (File.Exists(OutputPath))
            {
                File.Delete(OutputPath);
            }

            if (File.Exists(ErrorPath))
            {
                File.Delete(ErrorPath);
            }
        }
    }
}
