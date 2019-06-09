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
                int currentNo;
                bool isFound = Helper.ReadInt(reader, out currentNo);
                if (isFound)
                    heap.Add(new ReaderAndNum(currentNo, reader));
            }

            while(!heap.IsEmpty())
            {
                ReaderAndNum min = null;
                if (heap.GetMin(out min))
                {
                    writer.WriteLine(min.num);
                    int currentNo;
                    bool isFound = Helper.ReadInt(min.reader, out currentNo);
                    if (isFound)
                        heap.Add(new ReaderAndNum(currentNo, min.reader));

                }
            }
        }
    }


    class Heap
    {
        ReaderAndNum[] data;
        int Size;
        int currentIndex = 0;

        public Heap(int size)
        {
            Size = size;
            data = new ReaderAndNum[size];
        }

        public bool IsEmpty()
        {
            if (currentIndex == 0)
                return true;
            else
                return false;
        }

        int ParentIndex(int index)
        {
            return (index - 1) / 2;
        }

        int LeftChildIndex(int index)
        {
            return index * 2 + 1;
        }

        int RightChildIndex(int index)
        {
            return index * 2 + 2;
        }

        void swap(int index1, int index2)
        {
            T temp = data[index1];
            data[index1] = data[index2];
            data[index2] = temp;
        }

        void HeapifyBottomUp(int index)
        {
            while(index>0 && data[index].num < data[ParentIndex(index)].num)
            {
                swap(index, ParentIndex(index));
            }
        }

        public bool Add(ReaderAndNum no)
        {
            if (currentIndex == Size)
                return false;
            data[currentIndex] = no;
            HeapifyBottomUp(currentIndex);
            currentIndex++;
            return true;
        }

        public bool GetMin(out ReaderAndNum min)
        {
            min = null;
            if (currentIndex == 0)
                return false;

            min = data[0];
            HeapifyRoot();

            currentIndex--;
            return true;
        }

        void HeapifyRoot()
        {
            int currentIndex = 0;
            while(true)
            {
                int minIndex = currentIndex;
                if (LeftChildIndex(currentIndex) >= Size)
                    break;

                if (data[LeftChildIndex(currentIndex)].num < data[minIndex].num)
                    minIndex = LeftChildIndex(currentIndex);

                if(RightChildIndex(currentIndex) < Size && data[RightChildIndex(currentIndex)].num < data[minIndex].num)
                {
                    minIndex = RightChildIndex(currentIndex);
                }

                if (minIndex == currentIndex)
                    break;

                swap(currentIndex, minIndex);
                minIndex = currentIndex;
            }
        }

    }
}
