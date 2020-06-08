using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;

namespace HuaweiBruteforceBootloader {
    class Program {
        static void LaunchApp(string commands) {
            Console.WriteLine("Executing: " + commands);
            string[] initialCommand = commands.Split(' ');

            Process cmd = new Process();
            cmd.StartInfo.FileName = initialCommand[0];
            cmd.StartInfo.Arguments = commands.Replace(initialCommand[0] + " ", "");
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.WaitForExit();
            Console.WriteLine(cmd.StandardOutput.ReadToEnd());
        }

        static void Main(string[] args) {
            long initialValues = 1000000000000000;
            if (args.Length != 0) {
                initialValues = Convert.ToInt64(args[0]);
            }

            int executedTime = 0;

            LaunchApp("adb devices");
            Console.WriteLine("Press any key to launch tool.");
            Console.ReadKey();
            for(; initialValues < 1999999999999999; initialValues++) {
                if (executedTime == 0) {
                    LaunchApp("adb reboot-bootloader");
                    Thread.Sleep(20000);
                }

                LaunchApp("fastboot oem unlock " + initialValues);
                executedTime++;

                if (executedTime == 5) {
                    LaunchApp("fastboot reboot");
                    executedTime = 0;
                    Thread.Sleep(35000);
                }
            }
        }
    }
}
