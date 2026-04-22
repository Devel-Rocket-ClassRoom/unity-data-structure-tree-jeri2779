using UnityEngine;

public class TreeNode<TKey, Tvalue>

{
    public TKey Key { get; set; }
    public Tvalue Value { get; set; }   
    public int Height { get; set; }

    public TreeNode<TKey, Tvalue> Left { get; set; }
    public TreeNode<TKey, Tvalue> Right { get; set; }

    public TreeNode(TKey key, Tvalue value)
    {
        Key = key;
        Value = value;
        Height = 1;  
        
    }
}
