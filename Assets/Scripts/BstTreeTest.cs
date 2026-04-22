using System.Collections.Generic;
using UnityEngine;

public class BstTreeTest : MonoBehaviour
{
    public BstTreeVisualizer visualizer;
    public int nodeCount = 10;       
    public int minKey = 1;
    public int maxKey = 1000;

    private BinarySearchTree<int, string> bst;
    private List<int> insertedKeys;  // 삽입 순서 기록 (제거 시 역순 사용)
 
    // Space      : 노드 n개 추가
    // Backspace  : 노드 n개 제거 (삽입 역순)
    // Alpha1     : SpacingType → Pow
    // Alpha2     : SpacingType → LevelOrder
    // Alpha3     : SpacingType → InOrder

    void Start()
    {
        bst = new BinarySearchTree<int, string>();
        insertedKeys = new List<int>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AddNodes();
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            RemoveNodes();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            visualizer.spacingType = BstTreeVisualizer.SpacingType.Pow;
            Debug.Log("SpacingType: Pow");
            visualizer.DrawTree(bst.Root);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            visualizer.spacingType = BstTreeVisualizer.SpacingType.LevelOrder;
            Debug.Log("SpacingType: LevelOrder");
            visualizer.DrawTree(bst.Root);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            visualizer.spacingType = BstTreeVisualizer.SpacingType.InOrder;
            Debug.Log("SpacingType: InOrder");
            visualizer.DrawTree(bst.Root);
        }
    }
 
    private void AddNodes()
    {
        int added = 0;
        int attempts = 0;
        int maxAttempts = nodeCount * 10;

        while (added < nodeCount && attempts < maxAttempts)
        {
            int key = Random.Range(minKey, maxKey + 1);
            if (!bst.ContainsKey(key))
            {
                bst.Add(key, key.ToString());
                insertedKeys.Add(key);
                added++;
            }
            attempts++;
        }

        if (added < nodeCount)
        {
            Debug.LogWarning($"요청한 {nodeCount}개 중 {added}개 추가됨.");
        }
        else
        {
            Debug.Log($"노드 {added}개 추가. 총 노드 수: {bst.Count}");
        }

        visualizer.DrawTree(bst.Root);
    }

   
    private void RemoveNodes()
    {
        if (insertedKeys.Count == 0)
        {
            Debug.LogWarning("제거할 노드 없음");
            return;
        }

        int removeCount = Mathf.Min(nodeCount, insertedKeys.Count);

        for (int i = 0; i < removeCount; i++)
        {
            int key = insertedKeys[insertedKeys.Count - 1];
            insertedKeys.RemoveAt(insertedKeys.Count - 1);
            bst.Remove(key);
        }

        Debug.Log($"노드 {removeCount}개 제거. 총 노드 수: {bst.Count}");

        visualizer.DrawTree(bst.Root);
    }
}
