using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace StockWebApp
{
    public static class Log
    {
        public static void Enter(string text = "")
        {
            Debug.WriteLine($"{DateTime.Now} - Enter - {text}");
        }

        public static void Exit(string text = "")
        {
            Debug.WriteLine($"{DateTime.Now} - Exit - {text}");
        }

        public static void Exception(Exception exception, string text = "")
        {
            Debug.WriteLine($"{DateTime.Now} - Exception - {text}");

            if (exception.Message != null)
            {
                Debug.WriteLine($"{DateTime.Now} - Exception - {exception.Message}");

                if (exception.StackTrace != null)
                {
                    Debug.WriteLine($"{DateTime.Now} - Exception StackTrace - {exception.StackTrace}");
                }

            }

            if (exception.InnerException != null)
            {
                Debug.WriteLine($"{DateTime.Now} - InnerException Message - {exception.InnerException.Message}");

                if (exception.InnerException.StackTrace != null)
                {
                    Debug.WriteLine($"{DateTime.Now} - InnerException StackTrace - {exception.InnerException.StackTrace}");
                }

            }

        }
    }
}