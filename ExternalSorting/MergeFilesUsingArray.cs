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
        /// Reader and current min in it.
        /// </summary>
        class ReaderAndNum
        {
            public int num;
            public StreamReader reader;

            public ReaderAndNum(int _num, StreamReader _reader)
            {
                num = _num;
                reader = _reader;
            }
        }

        /// <summary>
        /// Most basic way to merge sorted files. Pick min from each file and take min from them.
        /// Output the min to output file and pick the next min from the stream that current min got picked.
        /// Complexity for n files of k size each: 0(n * n * k)
        /// </summary>
        /// <param name="inputFilesPath"></param>
        /// <param name="outputFilePath"></param>
        public static void Merge(string inputFilesPath, string outputFilePath)
        {
            List<StreamReader> readers = new List<StreamReader>();

            foreach (var file in Directory.GetFiles(inputFilesPath))
            {
                readers.Add(new StreamReader(file));
            }

            List<ReaderAndNum> currentMins = new List<ReaderAndNum>();
            foreach (var reader in readers)
            {

                int currentNo;
                bool isFound = Helper.ReadInt(reader, out currentNo);

                if (isFound)
                    currentMins.Add(new ReaderAndNum(currentNo, reader));
            }

            StreamWriter writer = new StreamWriter(outputFilePath);

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

            writer.Close();
            foreach (var reader in readers)
                reader.Close();
        }

    }
}
