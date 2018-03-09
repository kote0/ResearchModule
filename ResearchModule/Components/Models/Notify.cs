using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Components.Models
{
    public static class Notify
    {
        private static string info { get; set; }

        private static string error { get; set; }

        public static string Info
        {
            get
            {
                var i = info;
                info = string.Empty;
                return i;
            }
        }

        public static string Error
        {
            get
            {
                var e = error;
                error = string.Empty;
                return e;
            }
        }

        #region public Voids
        public static void SetInfo(string str)
        {
            info = str;
        }

        public static void SetInfo(string str, params object[] obj)
        {
            SetInfo(string.Format(str, obj));
        }
        
        public static void SetError(string str)
        {
            error = str;
        }

        public static void SetError(string str, params object[] obj)
        {
            SetError(string.Format(str, obj));
        }

        public static void SetError(string str, string exp)
        {
            str.Insert(str.Length, "\r\n{0}");
            SetError(string.Format(str, exp));
        }
        #endregion

    }
}
