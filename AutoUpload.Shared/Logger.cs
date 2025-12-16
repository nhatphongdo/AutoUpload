using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoUpload.Shared
{
    public static class Logger
    {
        public static readonly string NewLine = char.ConvertFromUtf32(13) + char.ConvertFromUtf32(10);

        public static Stream LogStream { get; set; }

        public static void WriteLog(string message)
        {
            if (LogStream == null || !LogStream.CanWrite)
            {
                return;
            }

            message = NewLine + "---------- On " + DateTime.Now.ToString() + " ----------" + NewLine + message + NewLine;
            var buffer = Encoding.UTF8.GetBytes(message);
            LogStream.Write(buffer, 0, buffer.Length);
            LogStream.Flush();
        }

        public static void WriteLog(Exception exc, string moreDetail = "")
        {
            if (LogStream == null || !LogStream.CanWrite)
            {
                return;
            }

            var message = NewLine + "---------- On " + DateTime.Now.ToString() + " ----------" + NewLine + exc.Message + NewLine;
            message += moreDetail + NewLine;
            message += "Stack trace: " + NewLine;
            message += exc.StackTrace + NewLine;
            if (exc.InnerException != null)
            {
                message += "Inner Exception: " + NewLine;
                message += exc.InnerException.ToString() + NewLine;
            }

            var buffer = Encoding.UTF8.GetBytes(message);
            LogStream.Write(buffer, 0, buffer.Length);
            LogStream.Flush();
        }
    }
}