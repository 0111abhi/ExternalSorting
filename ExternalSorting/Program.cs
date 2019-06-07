using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;

namespace ExternalSorting
{
    class Program
    {

        static string inputBlobFilePath = @"C:\Users\abhisagg\Documents\rand.txt";
        static string outputSplittedFiles = @"C:\Users\abhisagg\Documents\output";
        static string sortedOutputBlobFilePath = @"C:\Users\abhisagg\Documents\merged.txt";

        static int sizeOfRandBlobToBeGeneratedMb = 10;
        static int nodeRamSizeMb = 1;

        static void Main(string[] args)
        {
            RandomNumberGenerator.GenerateRandomNos(inputBlobFilePath, sizeOfRandBlobToBeGeneratedMb);
            SplitBlob(nodeRamSizeMb, outputSplittedFiles);
            MergeSortedFiles(outputSplittedFiles, sortedOutputBlobFilePath);
        }

        /// <summary>
        /// Read numbers from blob and split it into files that can fit into memory
        /// Also sorts them before writing splits to file.
        /// </summary>
        /// <param name="nodeSizeMb">The memory size of each node</param>
        /// <param name="outputPath">The output where splitted files will be stored</param>
        public static void SplitBlob(int nodeSizeMb, string outputPath)
        {
            int nosCount = Helper.CountNosFitInSizeMb(nodeSizeMb);

            using (StreamReader reader = new StreamReader(inputBlobFilePath))
            {
                int outputFilesCount = 0;
                List<int> allNumsForNode = new List<int>();
                while (!reader.EndOfStream)
                {
                    int currentNo = 0;
                    
                    if (!ReadInt(reader, out currentNo))
                        continue;

                    if (allNumsForNode.Count + 1 > nosCount)
                    {
                        outputFilesCount++;
                        allNumsForNode.Sort();
                        string sortedNums = string.Join(",", allNumsForNode);
                        File.WriteAllText(Path.Combine(outputPath, outputFilesCount.ToString()), sortedNums);
                        allNumsForNode.Clear();
                    }

                    allNumsForNode.Add(currentNo);
                }
            }
        }

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


        public static void MergeSortedFiles(string inputFilesPath, string outputFilePath)
        {
            List<StreamReader> readers = new List<StreamReader>();
           
            foreach(var file in Directory.GetFiles(inputFilesPath))
            {
                readers.Add(new StreamReader(file));
            }

            List<ReaderAndNum> currentMins = new List<ReaderAndNum>();
            foreach(var reader in readers)
            {

                int currentNo;
                bool isFound = ReadInt(reader, out currentNo);

                if(isFound)
                    currentMins.Add(new ReaderAndNum(currentNo, reader));
            }

            StreamWriter writer = new StreamWriter(outputFilePath);

            while(true)
            {
                ReaderAndNum min = currentMins.First();
                foreach(var pair in currentMins)
                {
                    if (pair.num < min.num)
                        min = pair;
                }

                writer.WriteLine(min.num);
                currentMins.Remove(min);

                if (ReadInt(min.reader, out int currentNo))
                    currentMins.Add(new ReaderAndNum(currentNo, min.reader));

                if (!currentMins.Any())
                    break;
            }

            writer.Close();
            foreach (var reader in readers)
                reader.Close();
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
