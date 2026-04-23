//using NUnit.Framework;
//using UnityEngine;
//using System.Collections.Generic;

//public class PriorityQueueAns<TElement, TPriority>
//{
//    public int Count => heap.Count;
//    private List<PriorityQueue<TElement, TPriority>> heap = new List<PriorityQueue<TElement, TPriority>>();
//    private readonly IComparer<TPriority> comparer;
//    public PriorityQueueAns()
//    {
//        heap = new List<PriorityQueue<TElement, TPriority>>();
//        comparer = Comparer<TPriority>.Default; 
//    }

//    public PriorityQueueAns(IComparer<TPriority> customComparer)
//    {
//        heap = new List<PriorityQueue<TElement, TPriority>>();
//        comparer = customComparer ?? Comparer<TPriority>.Default;
//    }

//    public void Enqueue(TElement elem, TPriority pri)
//    {
//        heap.Add((elem, pri));
//        int currentIndex = heap.Count - 1;
//        while (currentIndex > 0)
//        {
//            int parentIndex = (currentIndex - 1) / 2;
//            if (comparer.Compare(heap[currentIndex].Priority, heap[parentIndex].Priority) >= 0) break;
//            Swap(currentIndex, parentIndex);
//            currentIndex = parentIndex;
//        }
//    }
//    public void Dequeue(TElement element, TPriority priority)
//    {
//        if (IsEmpty())
//        {
//            throw new InvalidOperationException("Queue is empty.");
//        }
//        TElement result = heap[0].Element;

//    }

//    private void HeapifyUp(int index)
//    {
//        while(index > 0)
//        {
//           int parentIndex = (index - 1) / 2;
//            if (comparer.Compare(heap[index].Priority, heap[parentIndex].Priority) >= 0) break;
//            Swap(index, parentIndex);
//            index = parentIndex;    
//        }
         
//    }

//    private void HeapifyDown(int index)
//    {
//        int lastIndex = heap.Count - 1;
//        while (true)
//        {
//            int leftChild = 2 * index + 1;
//            int rightChild = 2 * index + 2;
//            int smallest = index;
//            if (leftChild <= lastIndex && comparer.Compare(heap[leftChild].Priority, heap[smallest].Priority) < 0)
//            {
//                smallest = leftChild;
//            }
//            if (rightChild <= lastIndex && comparer.Compare(heap[rightChild].Priority, heap[smallest].Priority) < 0)
//            {
//                smallest = rightChild;
//            }
//            if (smallest == index) break;
//            Swap(index, smallest);
//            index = smallest;
//        }
//    }


//    private void Swap(int i, int j)
//    {
//        var temp = heap[i];
//        heap[i] = heap[j];
//        heap[j] = temp;
//    }

//}
