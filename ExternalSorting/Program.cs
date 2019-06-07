using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;

namespace ExternalSorting
{
    class Program
    {

        static string filePath = @"C:\Users\abhisagg\Documents\rand.txt";

        static void Main(string[] args)
        {
            //RandomNumberGenerator.GenerateRandomNos(filePath, 10);
            //ReadBlob(1, @"C:\Users\abhisagg\Documents\output");
            MergeSortedFiles(@"C:\Users\abhisagg\Documents\output", @"C:\Users\abhisagg\Documents\merged.txt");
        }

        public static void ReadBlob(int nodeSizeMb, string outputPath)
        {
            int nosCount = Helper.CountNosFitInSizeMb(nodeSizeMb);


            using (StreamReader reader = new StreamReader(filePath))
            {
                int outputFilesCount = 0;
                List<int> allNumsForNode = new List<int>();
                while (!reader.EndOfStream)
                {
                    string currentStr = "";
                    while (!reader.EndOfStream)
                    {
                        char c = (char)reader.Read();
                        if (c == ' ' || c == ',')
                            break;
                        else
                            currentStr += c;
                    }

                    if (currentStr == "")
                        continue;

                    bool res = int.TryParse(currentStr, out int currentNo);

                    if (res == false)
                        continue;
                    
                    if (allNumsForNode.Count + 1 <= nosCount)
                        allNumsForNode.Add(currentNo);
                    else
                    {
                        outputFilesCount++;
                        allNumsForNode.Sort();
                        string sortedNums = string.Join(",", allNumsForNode);
                        File.WriteAllText(Path.Combine(outputPath, outputFilesCount.ToString()), sortedNums);

                        allNumsForNode.Clear();
                        allNumsForNode.Add(currentNo);
                    }
                }
            }
        }

        class NumReader
        {
            public NumReader(int _num, StreamReader _reader)
            {
                num = _num;
                reader = _reader;
            }
            public int num;
            public StreamReader reader;
        }


        public static void MergeSortedFiles(string inputFilesPath, string outputFilePath)
        {
            List<StreamReader> readers = new List<StreamReader>();
           
            foreach(var file in Directory.GetFiles(inputFilesPath))
            {
                readers.Add(new StreamReader(file));
            }

            List<NumReader> currentList = new List<NumReader>();
            foreach(var reader in readers)
            {

                int currentNo;
                bool isFound = ReadInt(reader, out currentNo);

                if(isFound)
                    currentList.Add(new NumReader(currentNo, reader));
            }

            StreamWriter writer = new StreamWriter(outputFilePath);

            while(true)
            {
                NumReader min = currentList.First();
                foreach(var pair in currentList)
                {
                    if (pair.num < min.num)
                        min = pair;

                }

                writer.WriteLine(min.num);
                currentList.Remove(min);

                if (ReadInt(min.reader, out int currentNo))
                    currentList.Add(new NumReader(currentNo, min.reader));

                if (!currentList.Any())
                    break;
            }

            writer.Close();
            foreach (var reader in readers)
                reader.Close();
        }

        public static bool ReadInt(StreamReader reader, out int currentNo)
        {
            string currentStr = "";
            bool isFound = false;
            currentNo = 0;
            while (!reader.EndOfStream)
            {
                char c = (char)reader.Read();
                if (c == ',')
                {
                    bool res = int.TryParse(currentStr, out currentNo);
                    if (res == false)
                    {
                        currentStr = "";
                        continue;
                    }
                    else
                    {
                        isFound = true;
                        break;
                    }
                }
                else
                    currentStr += c;
            }

            return isFound;
        }
       
    }
}
