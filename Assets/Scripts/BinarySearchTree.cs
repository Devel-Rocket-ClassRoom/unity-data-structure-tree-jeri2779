using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using Unity.IO.LowLevel.Unsafe;
using System.Linq;
//이진 탐색 트리(Binary Search Tree)는 각 노드가 최대 두 개의 자식 노드를 가지는 트리 자료구조입니다. 각 노드는 키와 값을 저장하며, 왼쪽 서브트리의 모든 키는 부모 노드의 키보다 작고, 오른쪽 서브트리의 모든 키는 부모 노드의 키보다 큽니다. 이 구조는 검색, 삽입, 삭제 등의 연산을 효율적으로 수행할 수 있게 해줍니다.

public class BinarySearchTree<TKey, TValue> : IDictionary<TKey, TValue> where TKey : IComparable<TKey>
{
    protected TreeNode<TKey, TValue> root;
    public TreeNode<TKey, TValue> Root => root;//트리의 루트 노드를 반환하는 프로퍼티
    public BinarySearchTree()
    {
        root = null;
    }
    public TValue this[TKey key] //인덱서로 키를 사용하여 값을 가져오거나 설정할 수 있도록 구현
    {
        get
        {
            if (TryGetValue(key, out TValue value))//키가 트리에 존재하는지 확인
            {
                return value;//존재하면 해당 값을 반환
            }
            else
            {
                throw new KeyNotFoundException($"키 {key} 없음");//존재하지 않으면 예외를 던짐
            }
        }
        set
        {
            root = AddOrUpdate(root, key, value);//키-값 쌍을 트리에 추가하거나 업데이트
        }
    }

    public ICollection<TKey> Keys =>  InOrderTraversal().Select(kvp => kvp.Key).ToList();//트리를 중위 순회하여 키를 반환하는 컬렉션
 

    public ICollection<TValue>  Values => InOrderTraversal().Select(kvp => kvp.Value).ToList();//트리를 중위 순회하여 값을 반환하는 컬렉션
    

    public int Count => CountNodes(root);//트리의 노드 수를 반환하는 메서드

    protected virtual int CountNodes(TreeNode<TKey, TValue> node)
    {
        if (node == null)
        {
            return 0;//노드가 null이면 0을 반환
        }
        return 1 + CountNodes(node.Left) + CountNodes(node.Right);//현재 노드 + 왼쪽 서브트리의 노드 수 + 오른쪽 서브트리의 노드 수
    }

    public bool IsReadOnly => false;

    public void Add(TKey key, TValue value)
    {
        root = Add(root, key, value);//트리에 새로운 키-값 쌍을 추가하는 메서드.
    }

    public void Add(KeyValuePair<TKey, TValue> item)
    {
        Add(item.Key, item.Value);
    }

    protected virtual TreeNode<TKey, TValue> Add(TreeNode<TKey,TValue> node, TKey key, TValue value)
    {
        if(node == null)
        {
            return new TreeNode<TKey, TValue>(key, value);//새로운 노드를 생성하여 반환
        }
        int compare = key.CompareTo(node.Key);
        if(compare < 0)
        {
            node.Left = Add(node.Left, key, value);//key가 node.Key보다 작으면 왼쪽 서브트리+
        }
        else if(compare > 0)
        {
            node.Right = Add(node.Right, key, value);//key가 node.Key보다 크면 오른쪽 서브트리+
        }
        else
        {
            throw new ArgumentException($"키 {key} 이미 존재");
        }
        UpdateHeight(node);
        return node;
    }

    protected virtual TreeNode<TKey, TValue> AddOrUpdate(TreeNode<TKey, TValue> node, TKey key, TValue value)
    {
        if (node == null)
        {
            return new TreeNode<TKey, TValue>(key, value);//새로운 노드를 생성하여 반환
        }
        int compare = key.CompareTo(node.Key);
        if (compare < 0)
        {
            node.Left = AddOrUpdate(node.Left, key, value);//key가 node.Key보다 작으면 왼쪽 서브트리+
        }
        else if (compare > 0)
        {
            node.Right = AddOrUpdate(node.Right, key, value);//key가 node.Key보다 크면 오른쪽 서브트리+
        }
        else
        {
            node.Value = value;//key가 node.Key와 같으면 해당 노드의 값을 업데이트
        }
        UpdateHeight(node);
        return node;
    }

    public void Clear()
    {
        root = null;//트리를 초기화하여 모든 노드를 제거
    }

    public bool Contains(KeyValuePair<TKey, TValue> item)//
    {
        return TryGetValue(item.Key, out TValue value) && EqualityComparer<TValue>.Default.Equals(value, item.Value);//키가 트리에 존재하는지 확인하고,값이 일치하는지 확인
    }

    public bool ContainsKey(TKey key)//키가 트리에 존재하는지 확인하는 메서드
    {
        return TryGetValue(key, out _);//TryGetValue 메서드 이용. 키가 존재 확인,있다면 true
    }

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
         foreach(var item in this)
        {
            if(arrayIndex >= array.Length)
            {
                throw new ArgumentException("배열의 크기가 충분하지 않습니다.");
            }
            array[arrayIndex++] = item;//트리를 중위 순회하여 키-값 쌍을 배열에 복사
        }
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()//트리를 중위 순회하여 키-값 쌍을 반환하는 열거자 메서드
    {
        return  InOrderTraversal().GetEnumerator();

    }
    
    public virtual IEnumerable<KeyValuePair<TKey, TValue>> InOrderTraversal()//트리를 레벨 순회하여 키-값 쌍을 반환하는 열거자 메서드
    {
        return InOrderTraversal(root);
    }
    protected virtual  IEnumerable<KeyValuePair<TKey, TValue>> InOrderTraversal(TreeNode<TKey, TValue> node)
    {
        if(node != null)
        {
            foreach(var kvp in InOrderTraversal(node.Left))//왼쪽 서브트리를 중위 순회하여 키-값 쌍을 반환
            {
                yield return kvp;//왼쪽 서브트리의 키-값 쌍을 반환
            }

            yield return new KeyValuePair<TKey, TValue>(node.Key, node.Value);//현재 노드를 반환
            foreach(var kvp in InOrderTraversal(node.Right))//오른쪽 서브트리를 중위 순회하여 키-값 쌍을 반환
            {
                yield return kvp;//오른쪽 서브트리의 키-값 쌍을 반환
            }
        }
      
    }
    public virtual IEnumerable<KeyValuePair<TKey, TValue>> LevelOrderTraversal()//트리를 레벨 순회하여 키-값 쌍을 반환하는 열거자 메서드
    {
        return LevelOrderTraversal(root);
    }
    protected virtual IEnumerable<KeyValuePair<TKey, TValue>> LevelOrderTraversal(TreeNode<TKey, TValue> node)
    {
        if (node == null) yield break;

        var queue = new Queue<TreeNode<TKey, TValue>>();
        queue.Enqueue(node);                

        while (queue.Count > 0)
        {
            var current = queue.Dequeue(); // 큐 앞에서 꺼냄
            yield return new KeyValuePair<TKey, TValue>(current.Key, current.Value);

            if (current.Left != null) 
            {
                queue.Enqueue(current.Left);  // 왼쪽 자식 큐에 추가
            }
            if (current.Right != null) //
            {
                queue.Enqueue(current.Right); // 오른쪽 자식 큐에 추가
            }
        }
    }
    public virtual IEnumerable<KeyValuePair<TKey, TValue>> PreOrderTraversal()//트리를 전위 순회하여 키-값 쌍을 반환하는 열거자 메서드
    {
        return PreOrderTraversal(root);
    }   
    protected virtual IEnumerable<KeyValuePair<TKey, TValue>> PreOrderTraversal(TreeNode<TKey, TValue> node)
    {
        if (node != null)
        {
            yield return new KeyValuePair<TKey, TValue>(node.Key, node.Value);//현재 노드를 반환
            foreach (var kvp in PreOrderTraversal(node.Left))//왼쪽 서브트리를 전위 순회하여 키-값 쌍을 반환
            {
                yield return kvp;//왼쪽 서브트리의 키-값 쌍을 반환
            }

            foreach (var kvp in PreOrderTraversal(node.Right))//오른쪽 서브트리를 전위 순회하여 키-값 쌍을 반환
            {
                yield return kvp;//오른쪽 서브트리의 키-값 쌍을 반환
            }
        }
    }
    public virtual IEnumerable<KeyValuePair<TKey, TValue>> PostOrderTraversal()//트리를 후위 순회하여 키-값 쌍을 반환하는 열거자 메서드
    {
        return PostOrderTraversal(root);
    }   
    protected virtual IEnumerable<KeyValuePair<TKey, TValue>> PostOrderTraversal(TreeNode<TKey, TValue> node)
    {
        if (node != null)
        {
            foreach (var kvp in PostOrderTraversal(node.Left)) 
            {
                yield return kvp;//왼쪽 서브트리의 키-값 쌍을 반환
            }

            foreach (var kvp in PostOrderTraversal(node.Right)) 
            {
                yield return kvp;//오른쪽 서브트리의 키-값 쌍을 반환
            }
            yield return new KeyValuePair<TKey, TValue>(node.Key, node.Value);//현재 노드를 반환
        }
         
    }

    public bool Remove(TKey key)//키가 트리에 존재하는지 확인하고, 존재한다면 해당 노드를 제거하는 메서드
    {
        int initialCount = Count;//현재 트리의 노드 수를 저장
        root = Remove(root, key);
        return Count < initialCount;//노드 수가 줄어들었는지 확인하여 제거 성공 여부 반환
    }

    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
        return Remove(item.Key);
    }

    protected virtual TreeNode<TKey, TValue> Remove(TreeNode<TKey, TValue> node, TKey key) 
    {
        if (node == null)
        {
            return node;//노드가 null이면 키가 트리에 존재하지 않으므로 null 반환
        }
        int compare = key.CompareTo(node.Key);
        if(compare < 0)
        {
            node.Left = Remove(node.Left, key);//key가 node.Key보다 작으면 왼쪽 서브트리에서 제거
        }
        else if(compare > 0)
        {
            node.Right = Remove(node.Right, key);//key가 node.Key보다 크면 오른쪽 서브트리에서 제거
        }
        else
        {
            if(node.Left == null)//제거할 노드가 왼쪽 자식이 없는 경우
            {
                return node.Right;//오른쪽 자식을 반환하여 제거된 노드를 대체
            }
            else if(node.Right == null)//제거할 노드가 오른쪽 자식이 없는 경우
            {
                return node.Left;//왼쪽 자식을 반환하여 제거된 노드를 대체
            }
            
            TreeNode<TKey, TValue> minNode = FindMin(node.Right);//오른쪽 서브트리에서 가장 작은 키를 가진 노드를 찾음

            node.Key = minNode.Key;//현재 노드의 키를 minNode의 키로 대체
            node.Value = minNode.Value;//현재 노드의 값을 minNode의 값으로 대체

            node.Right = Remove(node.Right, minNode.Key);//minNode를 오른쪽 서브트리에서 제거
             
        }
        UpdateHeight(node);
        return node;
    }

    protected virtual TreeNode<TKey, TValue> FindMin(TreeNode<TKey, TValue> node)
    {
        while (node.Left != null)
        {
            node = node.Left;//왼쪽 자식 노드로 이동하여 가장 작은 키를 가진 노드를 찾음
        }
        return node;
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        return TryGetValue(root, key, out value);
    }
    protected bool TryGetValue(TreeNode<TKey, TValue> node, TKey key, out TValue value)//재귀적으로 트리를 탐색하여 키를 찾는 메서드
    {
        if(node == null)//노드가 null이면 키가 트리에 존재하지 않으므로 false를 반환
        {
            value = default;
            return false;
        }

        int compare = key.CompareTo(node.Key);
        if (compare == 0)//key가 node.Key와 같으면 해당 노드의 값을 반환
        {
            value = node.Value;
            return true;

        }
        else if (compare < 0)//key가 node.Key보다 작으면 왼쪽 서브트리로 이동
        {
            return TryGetValue(node.Left, key, out value);
        }
        else//key가 node.Key보다 크면 오른쪽 서브트리로 이동
        {
            return TryGetValue(node.Right, key, out value);
        }

    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    protected virtual void UpdateHeight(TreeNode<TKey, TValue> node)
    {
        //int leftHeight = node.Left != null ? node.Left.Height : 0;
        //int rightHeight = node.Right != null ? node.Right.Height : 0;
        //node.Height = 1 + Math.Max(leftHeight, rightHeight);
        node.Height = 1 + Math.Max(Height(node.Left), Height(node.Right));
    }

    protected int Height(TreeNode<TKey, TValue> node)
    {
        return node != null ? node.Height : 0;
        //return node == null ? 0 : node.Height;    
    }
}
