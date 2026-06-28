// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System;
using System.Diagnostics;

namespace TinyUtilities.Logger {
    public static class DebugDisplay {
        public static void WriteToConsole(string message, LogType type) {
            ConsoleColor current = Console.ForegroundColor;
            
            Console.ForegroundColor = type.ToColor();
            Console.WriteLine(message);
            Console.ForegroundColor = current;
        }
        
        public static void SendToSystem(string message, LogType type) {
            Debug.WriteLine(message, type.ToString());
        }
        
        private static ConsoleColor ToColor(this in LogType type) {
            switch (type) {
                case LogType.Log: return Console.ForegroundColor;
                case LogType.Warning: return ConsoleColor.Yellow;
                case LogType.Error: return ConsoleColor.Red;
                default: return Console.ForegroundColor;
            }
        }
    }
}