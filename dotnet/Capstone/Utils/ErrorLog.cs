using System.Collections.Generic;
using System.IO;
using System;
using System.Runtime.CompilerServices;

namespace Capstone.Utils
{
    public class ErrorLog
    {
        /// <summary>
        /// Error log writer - to help debug issues. A new log file will be generated each day an error occurs.
        /// </summary>
        /// <param name="action">What method or action are you performing?</param>
        /// <param name="item">What item is being passed to the method? Helpful if you pass this string into the error log</param>
        /// <param name="methodName">The method's name</param>
        /// <param name="text">Any other helpful text - e.g. exception text. Optional</param>
        public static void WriteLog(string action, string item, string methodName = null, string text = null)
        {
            try
            {
                DateTime date = DateTime.UtcNow;
                int year = date.Year;
                int month = date.Month;
                int day = date.Day;
                using (StreamWriter sw = new StreamWriter($"Log/CreationUpdateLog_{year}_{month}_{day}.txt", true))
                {
                    sw.WriteLine($"{action} | {item} | {methodName} | {text} | {DateTime.UtcNow}");
                    sw.WriteLine("--- END OF ACTION ---");
                    sw.WriteLine("");
                }
            }
            catch (Exception e)
            {
            }
        }
    }
}
