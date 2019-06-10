using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ExternalSorting
{
    /// <summary>
    /// Instead of writing numbers one by one, write a batch of nums that can fit in memory along with nums
    /// from each stream reader.
    /// </summary>
    class MergeFilesUsingHeapAsBatch
    {
        public static void Merge(ref List<StreamReader> readers, ref StreamWriter writer, int nodeRamSizeMb)
        {
            int sizeMb = FindWriteSize(readers.Count, nodeRamSizeMb);
            int countNos = Helper.CountNosFitInSizeMb(sizeMb);
            Heap heap = new Heap(readers.Count * countNos);
            foreach(var reader in readers)
            {
                for(int i = 0;i<countNos;i++)
                    AddCurrentToHeap(reader, heap);
            }

            while(!heap.IsEmpty())
            {
                ReaderAndNum min = null;
                int minsCount = 0;
                string mins = "";
                while (heap.GetMin(out min) && minsCount < countNos)
                {
                    minsCount++;
                    mins += min.num + "\n";
                    AddCurrentToHeap(min.reader, heap);
                }

                writer.Write(mins);
            }
        }

        /// <summary>
        /// Total Ram size is nodeRamSizeMb. Each node will give x mins and we will get x mins overall that
        /// can be outputted. So we need memory of x mins from each node and amount x to be stored as output.
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="nodeRamSizeMb"></param>
        /// <returns>Returns how many Mb of data can we process at a time and will write
        /// this much amount of data</returns>
        public static int FindWriteSize(int nodes, int nodeRamSizeMb)
        {
            return (nodeRamSizeMb / (nodes + 1));
        }

        public static void AddCurrentToHeap(StreamReader reader, Heap heap)
        {
            if (Helper.ReadInt(reader, out int currentNo))
                heap.Add(new ReaderAndNum(currentNo, reader));
        }
    }
}
