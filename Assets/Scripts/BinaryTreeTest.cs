using UnityEngine;

public class BinaryTreeTest : MonoBehaviour
{
    public BinaryTreeVisualizer treeVisualizer;
    public int nodeCount = 100;
    public int minKey = 1;
    public int maxKey = 1000;

    void Start()
    {
        GenerateTree();
    }

    public void GenerateTree()
    {
        BinarySearchTree<int, string> bst = new BinarySearchTree<int, string>();

        int count = 0;
        int attempts = 0;

        while (count < nodeCount && attempts < nodeCount * 10)
        {
            int key = Random.Range(minKey, maxKey + 1);   
            if (!bst.ContainsKey(key))                   
            {
                bst.Add(key, key.ToString());
                count++;
            }
            attempts++;
        }

        treeVisualizer.SetTree(bst);   
    }
}