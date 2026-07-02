// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace TinyUtilities {
    public static class ShellUtility {
        private const string _WSL_PATH = "wsl";
        private const string _POWERSHELL_PATH = "powershell";
        
        [Pure]
        public static async Task<string> Wsl(string command) => await Process(_WSL_PATH, command);
        
        [Pure]
        public static async Task<string> PowerShell(string command) => await Process(_POWERSHELL_PATH, command);
        
        [Pure]
        public static async Task<string> Process(string path, string command) {
            Process process = new Process();
            process.StartInfo.FileName = path;
            process.StartInfo.Arguments = command;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            
            process.Start();
            
            string output = await process.StandardOutput.ReadToEndAsync();
            
            process.WaitForExit();
            
            return output;
        }
        
        [Pure]
        public static Process ProcessSilent(string path, string command, bool isHaveLog = false) {
            Process process = new Process();
            process.StartInfo.FileName = path;
            process.StartInfo.Arguments = command;
            process.StartInfo.RedirectStandardOutput = false;
            process.StartInfo.RedirectStandardError = false;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            
            if (isHaveLog) {
                process.OutputDataReceived += (_, evt) =>
                {
                    if (evt.Data != null) {
                        Console.WriteLine($"{path} : {evt.Data}");
                    }
                };
                
                process.ErrorDataReceived += (_, evt) =>
                {
                    if (evt.Data != null) {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"{path} : {evt.Data}");
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                };
            }
            
            process.Start();
            return process;
        }
    }
}