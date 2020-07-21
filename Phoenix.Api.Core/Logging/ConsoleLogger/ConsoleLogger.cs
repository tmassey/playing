using System;
using System.Net;
using Phoenix.Api.Core.Logging.Enums;
using Phoenix.Api.Core.Logging.Interfaces;
using Phoenix.Api.Core.Logging.Models;

namespace Phoenix.Api.Core.Logging.ConsoleLogger
{
    public class ConsoleLogger : PhoenixLoggerBase, IPhoenixLogger
    {
        private readonly IConsoleWriter _consoleWriter;
        private static readonly object ThisLock = new object();

        public ConsoleLogger(IConsoleWriter consoleWriter)
        {
            _consoleWriter = consoleWriter;
        }

        public void Debug(string message, UserDetails userDetails = null)
        {
            if (LogLevel < Severity.Debug) return;
            lock (ThisLock)
            {
                _consoleWriter.WriteInColor("DEBUG", ConsoleColor.Black, ConsoleColor.White);
                _consoleWriter.WriteInColor($"   {message}", ConsoleColor.White);
                WriteUserDetails(userDetails);
            }
        }

        public void Info(string message, UserDetails userDetails = null)
        {
            if (LogLevel < Severity.Info) return;
            lock (ThisLock)
            {
                _consoleWriter.WriteInColor("INFO", ConsoleColor.Black, ConsoleColor.Cyan);
                _consoleWriter.WriteInColor($"    {message}", ConsoleColor.Cyan);
                WriteUserDetails(userDetails);
            }
        }

       public void Warning(string message, UserDetails userDetails = null)
        {
            if (LogLevel < Severity.Warning) return;
            lock (ThisLock)
            {
                _consoleWriter.WriteInColor("WARN", ConsoleColor.Black, ConsoleColor.Yellow);
                _consoleWriter.WriteInColor($"    {message}", ConsoleColor.Yellow);
                WriteUserDetails(userDetails);
            }
        }

        public void Error(string message, UserDetails userDetails = null)
        {
            if (LogLevel < Severity.Error) return;
            lock (ThisLock)
            {
                _consoleWriter.WriteInColor("ERROR", ConsoleColor.Black, ConsoleColor.Magenta);
                _consoleWriter.WriteInColor($"   {message}", ConsoleColor.Magenta);
                WriteUserDetails(userDetails);
            }
        }

        public void Network(string method, HttpStatusCode responseCode, string path, int duration, string serviceId = null, UserDetails userDetails = null)
        {
            if (LogLevel < Severity.Network) return;
            var color = ConsoleColor.DarkMagenta;
            if (((int)responseCode >= 200) && ((int)responseCode < 300)) color = ConsoleColor.Green;

            lock (ThisLock)
            {
                _consoleWriter.WriteInColor("NETWORK", ConsoleColor.Black, ConsoleColor.Green);
                _consoleWriter.WriteInColor($" {method}: {path} ==> ", ConsoleColor.Green);
                _consoleWriter.WriteInColor($"{(int)responseCode} : {responseCode} ", color);
                _consoleWriter.WriteInColor($"[{duration}ms]", ConsoleColor.Green);
                WriteUserDetails(userDetails);
            }
        }

        public void Fatal(Exception ex, UserDetails userDetails = null)
        {
            var inner = ex;

            while (inner.InnerException != null) inner = inner.InnerException;


            lock (ThisLock)
            {
                _consoleWriter.WriteInColor("FATAL", ConsoleColor.White, ConsoleColor.Red);
                _consoleWriter.WriteInColor($"   {inner}", ConsoleColor.Red);
                WriteUserDetails(userDetails);
            }
        }


        private void WriteUserDetails(UserDetails userDetails)
        {
            lock (ThisLock)
            {
                if (userDetails == null)
                {
                    _consoleWriter.WriteLine("");
                    return;
                }
                _consoleWriter.WriteLineInColor($" - {userDetails.ClientId} : {userDetails.Email}", ConsoleColor.DarkGray);
            }
        } 

        public void Dispose()
        {
        }

    }
}

