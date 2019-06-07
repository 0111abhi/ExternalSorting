using System;
using System.IO;
using System.Reflection.Metadata;

namespace ExternalSorting
{
    public class Helper
    {
        public static int intSize = 4;

        public static int CountNosFitInSizeMb(int sizeInMb)
        {
            return (sizeInMb * 1024 * 1024 / intSize);
        }

        /// <summary>
        /// Gets a single int from stream. Returs false if it didn't find any num in the stream
        /// </summary>
        /// <param name="reader">The reader stream to search for nums</param>
        /// <param name="firstNum">output first num found in stream</param>
        /// <returns></returns>
        public static bool ReadInt(StreamReader reader, out int firstNum)
        {
            string currentStr = "";
            bool isNumFound = false;
            firstNum = 0;
            while (!reader.EndOfStream)
            {
                char c = (char)reader.Read();
                if (c == ',' || c == ' ')
                {
                    bool res = int.TryParse(currentStr, out firstNum);
                    if (res == false)
                    {
                        // Current string was not a number. So empty it.
                        currentStr = "";
                        continue;
                    }
                    else
                    {
                        isNumFound = true;
                        break;
                    }
                }
                else
                    currentStr += c;
            }

            return isNumFound;
        }
    }
}
