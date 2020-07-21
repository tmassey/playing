using System;
using Phoenix.Api.Core.Logging.Interfaces;

namespace Phoenix.Api.Core.Logging.ConsoleLogger
{
    public class ConsoleWriter : IConsoleWriter
    {
        private static readonly object ThisLock = new object();


        public void Write(string format, params object[] args)
        {
            Console.Write(format, args);
        }

        public void WriteLine(string format, params object[] args)
        {
            Console.WriteLine(format, args);
        }

        public void WriteLineInColor(string message, ConsoleColor textColor, 
            ConsoleColor backgroundColor = ConsoleColor.Black)
        {
            lock (ThisLock)
            {
                var currentTextColor = Console.ForegroundColor;
                var currentBackColor = Console.BackgroundColor;
                Console.ForegroundColor = textColor;
                Console.BackgroundColor = backgroundColor;
                Console.WriteLine(message);
                Console.ForegroundColor = currentTextColor;
                Console.BackgroundColor = currentBackColor;
            }
        }

        public void WriteInColor(string message, ConsoleColor textColor, 
            ConsoleColor backgroundColor = ConsoleColor.Black)
        {
            lock (ThisLock)
            {
                var currentTextColor = Console.ForegroundColor;
                var currentBackColor = Console.BackgroundColor;
                Console.ForegroundColor = textColor;
                Console.BackgroundColor = backgroundColor;
                Console.Write(message);
                Console.ForegroundColor = currentTextColor;
                Console.BackgroundColor = currentBackColor;
            }
        }
    }
}
