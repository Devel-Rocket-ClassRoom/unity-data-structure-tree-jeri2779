using System;
using UnityEngine;
using UnityEngine.TextCore.Text;
 

public class PriorityQueueTest : MonoBehaviour
{

    PriorityQueue<string, int> queue = new PriorityQueue<string, int>();

    private void Start()
    {
        TestQueue();
        //TestEmptyQueue();
    }

    private void TestQueue()
    {
        queue.Enqueue("Task A", 5);
        queue.Enqueue("Task B", 2);
        queue.Enqueue("Task C", 8);
        queue.Enqueue("Task D", 1);
        queue.Enqueue("Task E", 3);
        queue.Enqueue("Task F", 4);
        queue.Enqueue("Task G", 4);
        while (!queue.IsEmpty())
        {
            Debug.Log(queue.Dequeue());
        }
    }

    private void TestEmptyQueue()
    {
        var pq = new PriorityQueue<string, int>();

        try
        {
            pq.Dequeue();
        }
        catch (InvalidOperationException e)
        {
            Debug.Log($"예외 발생: {e.Message}");
        }
    }

}
