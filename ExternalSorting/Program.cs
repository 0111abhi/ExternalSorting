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
            MergeFilesUsingArray.Merge(outputSplittedFiles, sortedOutputBlobFilePath);
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
                    
                    if (!Helper.ReadInt(reader, out currentNo))
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
       
    }
}
