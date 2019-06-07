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
    }
}
