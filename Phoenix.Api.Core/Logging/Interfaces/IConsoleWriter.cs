using System;

namespace Phoenix.Api.Core.Logging.Interfaces
{
    public interface IConsoleWriter
    {
        void Write(string format, params object[] args);
        void WriteLine(string format, params object[] args);
        void WriteLineInColor(string message, ConsoleColor textColor,
            ConsoleColor backgroundColor = ConsoleColor.Black);
        void WriteInColor(string message, ConsoleColor textColor,
            ConsoleColor backgroundColor = ConsoleColor.Black);
    }
}
