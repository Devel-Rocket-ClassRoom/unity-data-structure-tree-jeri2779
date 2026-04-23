using UnityEngine;
using System.Collections.Generic;

public class TreeVisualizeTest : MonoBehaviour
{
    public TreeVisualizer visualizer;
    public int nodeCount = 10;
    public int minKey = 1;
    public int maxKey = 1000;

    private BinarySearchTree<int, string> bst;
    private AVLTree<int, string> avl;
    private List<int> insertedKeys;

 
    // 1 :  BST
    // 2 :  AVL
    // Q :  InOrder
    // W :  Pow
    // E :  LevelOrder
    // Space :  노드 n개 추가
    // Backspace :  노드 n개 제거  

    void Start()
    {
        bst = new BinarySearchTree<int, string>();
        avl = new AVLTree<int, string>();
        insertedKeys = new List<int>();
    }

    void Update()
    {
        // TreeType 변경
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // BST로 변경 시 기존 데이터 초기화
            visualizer.treeType = TreeVisualizer.TreeType.BST;
            bst = new BinarySearchTree<int, string>();
            insertedKeys.Clear();
            visualizer.Clear();
            Debug.Log("TreeType: BST");
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            // AVL로 변경 시 기존 데이터 초기화
            visualizer.treeType = TreeVisualizer.TreeType.AVL;
            avl = new AVLTree<int, string>();
            insertedKeys.Clear();
            visualizer.Clear();
            Debug.Log("TreeType: AVL");
        }

        // SpacingType 변경
        if (Input.GetKeyDown(KeyCode.Q))
        {
            visualizer.spacingType = TreeVisualizer.SpacingType.InOrder;
            Debug.Log("SpacingType: InOrder");
            Redraw();
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            visualizer.spacingType = TreeVisualizer.SpacingType.Pow;
            Debug.Log("SpacingType: Pow");
            Redraw();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            visualizer.spacingType = TreeVisualizer.SpacingType.LevelOrder;
            Debug.Log("SpacingType: LevelOrder");
            Redraw();
        }

        // 노드 추가
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AddNodes();
        }

        // 노드 제거
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            RemoveNodes();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetTree();
            Debug.Log("트리 초기화");
        }
    }

     
    private void Redraw()
    {
        if (visualizer.treeType == TreeVisualizer.TreeType.BST)
        {
            visualizer.DrawTree(bst.Root);
        }
        else
        {
            visualizer.DrawTree(avl.Root);
        }
    }

 
    private void AddNodes()
    {
        int added = 0;
        int attempts = 0;
        int maxAttempts = nodeCount * 10;

        if (visualizer.treeType == TreeVisualizer.TreeType.BST)
        {
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
            Debug.Log($"BST노드 {added}개 추가.");
            visualizer.DrawTree(bst.Root);
        }
        else
        {
            while (added < nodeCount && attempts < maxAttempts)
            {
                int key = Random.Range(minKey, maxKey + 1);
                if (!avl.ContainsKey(key))
                {
                    avl.Add(key, key.ToString());
                    insertedKeys.Add(key);
                    added++;
                }
                attempts++;
            }
            Debug.Log($"AVL노드 {added}개 추가.");
            visualizer.DrawTree(avl.Root);
        }

        if (added < nodeCount)
        {
            Debug.LogWarning($"{added}+");
        }
    }

   
    private void RemoveNodes()
    {
        if (insertedKeys.Count == 0)
        {
            Debug.LogWarning("제거할 노드 없음");
            return;
        }

        int removeCount = Mathf.Min(nodeCount, insertedKeys.Count);

        if (visualizer.treeType == TreeVisualizer.TreeType.BST)
        {
            for (int i = 0; i < removeCount; i++)
            {
                int key = insertedKeys[insertedKeys.Count - 1];
                insertedKeys.RemoveAt(insertedKeys.Count - 1);
                bst.Remove(key);
            }
            Debug.Log($"BST노드 {removeCount}개 제거.");
            visualizer.DrawTree(bst.Root);
        }
        else
        {
            for (int i = 0; i < removeCount; i++)
            {
                int key = insertedKeys[insertedKeys.Count - 1];
                insertedKeys.RemoveAt(insertedKeys.Count - 1);
                avl.Remove(key);
            }
            Debug.Log($"AVL노드 {removeCount}개 제거.");
            visualizer.DrawTree(avl.Root);
        }
    }
    private void ResetTree()
    {
        bst = new BinarySearchTree<int, string>();
        avl = new AVLTree<int, string>();
        insertedKeys.Clear();
        visualizer.Clear();
    }
}

