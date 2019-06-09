using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ExternalSorting
{
    class MergeFilesUsingArray
    {
        /// <summary>
        /// Most basic way to merge sorted files. Pick min from each file and take min from them.
        /// Output the min to output file and pick the next min from the stream that current min got picked.
        /// Complexity for n files of k size each: 0(n * n * k)
        /// </summary>
        /// <param name="readers"></param>
        /// <param name="writer"></param>
        public static void Merge(ref List<StreamReader> readers, ref StreamWriter writer)
        {
            List<ReaderAndNum> currentMins = new List<ReaderAndNum>();

            // Create list of initial mins from each stream.
            foreach (var reader in readers)
            {
                int currentNo;
                bool isFound = Helper.ReadInt(reader, out currentNo);

                if (isFound)
                    currentMins.Add(new ReaderAndNum(currentNo, reader));
            }

            //write min of all mins to file and get next min from that stream
            while (true)
            {
                ReaderAndNum min = currentMins.First();
                foreach (var pair in currentMins)
                {
                    if (pair.num < min.num)
                        min = pair;
                }

                writer.WriteLine(min.num);
                currentMins.Remove(min);

                if (Helper.ReadInt(min.reader, out int currentNo))
                    currentMins.Add(new ReaderAndNum(currentNo, min.reader));

                if (!currentMins.Any())
                    break;
            }

            
        }

    }
}
