﻿using System;
using System.Text;

namespace Hellion.Core.IO
{
    internal enum LogType
    {
        Info,
        Done,
        Warning,
        Error,
        Debug,
        Loading
    }

    public static class Log
    {
        private static object syncLog = new object();

        static Log()
        {
#if !NET45
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
#endif
        }

        public static void Info(string format, params object[] args)
        {
            lock (syncLog)
                WriteConsole(LogType.Info, string.Format(format, args));
        }

        public static void Done(string format, params object[] args)
        {
            lock (syncLog)
                WriteConsole(LogType.Done, string.Format(format, args));
        }

        public static void Warning(string format, params object[] args)
        {
            lock (syncLog)
                WriteConsole(LogType.Warning, string.Format(format, args));
        }

        public static void Error(string format, params object[] args)
        {
            lock (syncLog)
                WriteConsole(LogType.Error, string.Format(format, args));
        }

        public static void Debug(string format, params object[] args)
        {
#if DEBUG
            lock (syncLog)
                WriteConsole(LogType.Debug, string.Format(format, args));
#endif
        }

        public static void Loading(string format, params object[] args)
        {
            lock (syncLog)
                WriteConsole(LogType.Loading, string.Format(format, args), false);
        }

        private static void WriteConsole(LogType logType, string text, bool newLine = true)
        {
            switch (logType)
            {
                case LogType.Info: Console.ForegroundColor = ConsoleColor.Green; break;
                case LogType.Done: Console.ForegroundColor = ConsoleColor.DarkCyan; break;
                case LogType.Warning: Console.ForegroundColor = ConsoleColor.Yellow; break;
                case LogType.Error: Console.ForegroundColor = ConsoleColor.Red; break;
                case LogType.Debug: Console.ForegroundColor = ConsoleColor.Blue; break;
                case LogType.Loading: Console.ForegroundColor = ConsoleColor.DarkMagenta; break;
            }

            Console.Write("\r");
            Console.Write("[{0}]: ", logType.ToString());
            Console.ResetColor();
            if (newLine)
                Console.WriteLine(text);
            else
                Console.Write(text);
        }
    }
}
