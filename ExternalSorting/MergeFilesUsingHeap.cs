using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ExternalSorting
{
    class MergeFilesUsingHeap
    {
        public static void Merge(ref List<StreamReader> readers, ref StreamWriter writer)
        {
            Heap heap = new Heap(readers.Count);
            foreach(var reader in readers)
            {
                AddCurrentToHeap(reader, heap);
            }

            while(!heap.IsEmpty())
            {
                ReaderAndNum min = null;
                if (heap.GetMin(out min))
                {
                    writer.WriteLine(min.num);
                    AddCurrentToHeap(min.reader, heap);

                }
            }
        }

        public static void AddCurrentToHeap(StreamReader reader, Heap heap)
        {
            if (Helper.ReadInt(reader, out int currentNo))
                heap.Add(new ReaderAndNum(currentNo, reader));
        }
    }
}
