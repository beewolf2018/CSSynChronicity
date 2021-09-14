using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSSynChronicity
{
    static class Utilities
    {
        internal static string FormatTimespan(TimeSpan T)
        {
            int Hours = System.Convert.ToInt32(Math.Truncate(T.TotalHours));
            List<string> Blocks = new List<string>();
            if (Hours != 0)
                Blocks.Add(Hours + " h");
            if (T.Minutes != 0)
                Blocks.Add(T.Minutes.ToString() + " m");
            if (T.Seconds != 0 | Blocks.Count == 0)
                Blocks.Add(T.Seconds.ToString() + " s");
            return string.Join(", ", Blocks.ToArray());
        }

        internal static long GetSize(string File)
        {
            return (new System.IO.FileInfo(File)).Length; // Faster than My.Computer.FileSystem.GetFileInfo().Length (See FileLen_Speed_Test.vb)
        }

        internal static string FormatSize(double Size, int Digits = 2)
        {
            switch (Size)
            {
                case object _ when Size >= (1 << 30):
                    {
                        return Math.Round(Size / (1 << 30), Digits).ToString() + " GB";
                    }

                case object _ when Size >= (1 << 20):
                    {
                        return Math.Round(Size / (1 << 20), Digits).ToString() + " MB";
                    }

                case object _ when Size >= (1 << 10):
                    {
                        return Math.Round(Size / (1 << 10), Digits).ToString() + " kB";
                    }

                default:
                    {
                        return Math.Round(Size, Digits).ToString() + " B";
                    }
            }
        }
    }
}
