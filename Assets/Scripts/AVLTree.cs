using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class AVLTree<TKey, TValue> : BinarySearchTree<TKey, TValue> where TKey : System.IComparable<TKey>
{
    public AVLTree() : base() { }

    protected override TreeNode<TKey, TValue> Add(TreeNode<TKey, TValue> node, TKey key, TValue value)
    {
        node = base.Add(node, key, value);
        return Balance(node);
    }
    protected override TreeNode<TKey, TValue> AddOrUpdate(TreeNode<TKey, TValue> node, TKey key, TValue value)
    {
        node = base.AddOrUpdate(node, key, value);
        return Balance(node);
    }
    protected override TreeNode<TKey, TValue> Remove(TreeNode<TKey, TValue> node, TKey key)
    {
        node = base.Remove(node, key);
        if(node == null) return node;
        return Balance(node);
    }


    protected int BalanceFactor(TreeNode<TKey, TValue> node)
    {
        if (node == null) return 0;
        return node == null ? 0 : Height(node.Left) - Height(node.Right);   
        return Height(node.Left) - Height(node.Right);
    }

    protected TreeNode<TKey, TValue> RotateRight(TreeNode<TKey, TValue> node)
    {
        var leftChild = node.Left;
        var rightSubTreeOfLeftChild = leftChild.Right;
        leftChild.Right = node;
        node.Left = rightSubTreeOfLeftChild;
        UpdateHeight(node);
        UpdateHeight(leftChild);
        return leftChild;
    }
    protected TreeNode<TKey, TValue> RotateLeft(TreeNode<TKey, TValue> node)
    {
        var rightChild = node.Right;
        var leftSubTreeOfRightChild = rightChild.Left;
        rightChild.Left = node;
        node.Right = leftSubTreeOfRightChild;
        UpdateHeight(node);
        UpdateHeight(rightChild);
        return rightChild;
    }

    protected TreeNode<TKey, TValue> Balance(TreeNode<TKey, TValue> node)
    {
        if (node == null) return null;
        int balance = BalanceFactor(node);  
        if(balance > 1)
        {
            if (BalanceFactor(node.Left) < 0)
            {
                node.Left = RotateLeft(node.Left);
            }
            //node = RotateRight(node);
            return RotateRight(node);   
        }
        else if (balance < -1)
        {
            if (BalanceFactor(node.Right) > 0)
            {
                node.Right = RotateRight(node.Right);
            }
            //node = RotateLeft(node);
            return RotateLeft(node);
        }
        return node;
    }

    public bool IsBalanced()
    {
        return IsBalanced(Root);
    }

    private bool IsBalanced(TreeNode<TKey, TValue> node)
    {
        if (node == null) return true;
        int balance = BalanceFactor(node);
        if (Mathf.Abs(balance) > 1) return false;
        return IsBalanced(node.Left) && IsBalanced(node.Right);
        
    }
}