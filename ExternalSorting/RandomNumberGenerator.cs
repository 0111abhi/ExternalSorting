using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Reflection.Metadata;

namespace ExternalSorting
{
    class RandomNumberGenerator
    {
        public static void GenerateRandomNos(string filePath, int sizeInMB, int min = -2147483648, int max = 2147483647)
        {
            long count = Helper.CountNosFitInSizeMb(sizeInMB);
            Random r = new Random();
            int number = 0;
            using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                for (long i = 0; i < count; i++)
                {
                    number = r.Next(min, max);
                    writer.Write(number + " ");
                }
            }
        }
    }
}
