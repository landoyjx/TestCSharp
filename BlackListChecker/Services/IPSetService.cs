using System;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace BlackListChecker.Services
{
    public class IPSetService
    {
        public IPSetService()
        {
        }

        public static int AddToBlackList(string ip)
        {
            Console.WriteLine("Add ip to iptable, ip=" + ip);

            Process p = new Process();
            p.StartInfo.FileName = "ipset";
            p.StartInfo.Arguments = "add " + ip;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();

            StreamReader reader = p.StandardOutput;
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                Console.WriteLine(line);
            }

            p.WaitForExit();
            int ret = p.ExitCode;
            p.Close();

            reader.Close();

            return ret;
        }

        public static int removeFromBlackList(string ip)
        {
            Console.WriteLine("Remove ip from iptable, ip=" + ip);

            Process p = new Process();
            p.StartInfo.FileName = "ipset";
            p.StartInfo.Arguments = "del " + ip;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();

            StreamReader reader = p.StandardOutput;
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                Console.WriteLine(line);
            }

            p.WaitForExit();
            int ret = p.ExitCode;
            p.Close();

            reader.Close();

            return ret;
        }

    }
}
