using System;
using System.Diagnostics;
using System.Security;

namespace RunAs
{
    class Program
    {
        static int Main(string[] args)
        {
            string username = "", password = "", domain = "", apppath = "", arguments = "";

            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i].ToLower())
                {
                    case "-u":
                        username = args[++i];
                        break;
                    case "-p":
                        password = args[++i];
                        break;
                    case "-d":
                        domain = args[++i];
                        break;
                    default:
                        if (apppath == "")
                            apppath = args[i];
                        else
                            arguments += args[i] + (i < args.Length ? " " : "");
                        break;
                }
            }

            if (args.Length == 0 || apppath == "")
            {
                Console.WriteLine("\nCommand line syntax:\nRunAs [-u user] [-p password] [-d domain] Program.exe  [Program params (if exsit)]\n");
                return 1;
            }
            Console.WriteLine(arguments);
            return RunAs(apppath, arguments, domain, username, password);

        }
        static int RunAs(string apppath, string arguments, string domain, string username, string password)
        {
            try
            {
                var userProcess = Process.Start(apppath, arguments, username, GetSecure(password), domain);
                while (userProcess != null && !userProcess.HasExited)
                {
                }
                if (userProcess != null) return userProcess.ExitCode;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return -1;
        }

        static SecureString GetSecure(string str)
        {
            SecureString secureStr = new SecureString();
            foreach (var c in str)
            {
                secureStr.AppendChar(c);
            }

            return secureStr;
        }
    }
}