 
 
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;


public class BstTreeVisualizer : MonoBehaviour
{
    public GameObject nodePrefab;  
    public float horizontalSpacing = 2f;  // 노드 간 가로 간격
    public float verticalSpacing = 2f;   // 노드 간 세로 간격
    public float cameraPadding = 5f;    

    public enum SpacingType
    {
        InOrder,
        Pow,
        LevelOrder
    }
    public SpacingType spacingType = SpacingType.InOrder;

    private readonly Dictionary<object, Vector3> nodePositions = new Dictionary<object, Vector3>(); // Node key to position mapping
    private readonly Dictionary<object, GameObject> nodeObjects = new Dictionary<object, GameObject>(); // Node key to GameObject mapping
     
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void DrawTree<TKey, TValue>(TreeNode<TKey, TValue> root)
        where TKey : IComparable<TKey>
    {
        Clear();
        if (root == null) return;


        // 레이아웃 방식에 따라 좌표 계산
        switch (spacingType)
        {
            case SpacingType.Pow:
                AssignPositionsPow(root, Vector3.zero, root.Height);
                break;
            case SpacingType.LevelOrder:
                AssignPositionsLevelOrder(root);
                break;
            case SpacingType.InOrder:
                int xIndex = 0;
                AssignPositionsInOrder(root, 0, ref xIndex);
                break;
        }

        InstantiateSubtree(root);
        FitCamera();
    }
    private void AssignPositionsPow<TKey, TValue>(
     TreeNode<TKey, TValue> node,
     Vector3 position,
     int height)
     where TKey : IComparable<TKey>
    {
        if (node == null) return;

        nodePositions[node.Key] = position;

        float offset = Mathf.Pow(2, height - 1) * horizontalSpacing / 2;
        Vector3 childBase = position + Vector3.down * verticalSpacing;

        AssignPositionsPow(node.Left, childBase + Vector3.left * offset, height - 1);
        AssignPositionsPow(node.Right, childBase + Vector3.right * offset, height - 1);
    }
    //In-order 방식으로 좌표 할당
    private void AssignPositionsInOrder<TKey, TValue>(
        TreeNode<TKey, TValue> node,
        int depth,
        ref int xIndex)
        where TKey : IComparable<TKey>
    {
        if (node == null) return;
        AssignPositionsInOrder(node.Left, depth + 1, ref xIndex);
        Vector3 position = new Vector3(xIndex * horizontalSpacing, -depth * verticalSpacing, 0);
        nodePositions[node.Key] = position;
        xIndex++;
        AssignPositionsInOrder(node.Right, depth + 1, ref xIndex);
    }
    private void AssignPositionsLevelOrder<TKey, TValue>(TreeNode<TKey, TValue> root)
    where TKey : IComparable<TKey>
    {
        var queue = new Queue<(TreeNode<TKey, TValue> node, int depth)>();
        var levels = new List<List<TreeNode<TKey, TValue>>>();

        queue.Enqueue((root, 0));
        
        while (queue.Count > 0)
        {
            var (node, depth) = queue.Dequeue();
            while(levels.Count <= depth)
            {
                levels.Add(new List<TreeNode<TKey, TValue>>());
            }
            levels[depth].Add(node);

            if(node.Left != null) queue.Enqueue((node.Left, depth + 1));
            if(node.Right != null) queue.Enqueue((node.Right, depth + 1));  
        }

        for (int depth = 0; depth < levels.Count; depth++)
        {
            float yPos = -depth * verticalSpacing;
            var row = levels[depth];

            for (int i = 0; i < row.Count; i++)
            {
                float xPos = i * horizontalSpacing;
                nodePositions[row[i].Key] = new Vector3(xPos, yPos, 0f);
            }
        }

    }
    private void InstantiateSubtree<TKey, TValue>(TreeNode<TKey, TValue> node)
    where TKey : IComparable<TKey>
    {
        if (node == null) return;
        Vector3 pos = nodePositions[node.Key];
        GameObject nodeObj = Instantiate(nodePrefab, pos, Quaternion.identity, transform);

        var tmp = nodeObj.GetComponentInChildren<TextMeshPro>();
        if(tmp != null)
        {
            tmp.text = node.Key.ToString();
        }
        nodeObjects[node.Key] = nodeObj;

        if (node.Left != null)
        {
            var edgeLeft = nodeObj.transform.Find("EdgeLeft").GetComponent<LineRenderer>();
            if (edgeLeft != null)
            {
                edgeLeft.enabled = true;
                edgeLeft.positionCount = 2;
                edgeLeft.SetPosition(0, pos + Vector3.forward * 0.1f);
                edgeLeft.SetPosition(1, nodePositions[node.Left.Key] + Vector3.forward * 0.1f);
            }
        }
        else
        {
            var edgeLeft = nodeObj.transform.Find("EdgeLeft").GetComponent<LineRenderer>();
            if (edgeLeft != null)
            {
                edgeLeft.enabled = false;
            }
        }

        if (node.Right != null)
        {
            var edgeRight = nodeObj.transform.Find("EdgeRight").GetComponent<LineRenderer>();
            if (edgeRight != null)
            {
                edgeRight.enabled = true;
                edgeRight.positionCount = 2;
                edgeRight.SetPosition(0, pos + Vector3.forward * 0.1f);
                edgeRight.SetPosition(1, nodePositions[node.Right.Key] + Vector3.forward * 0.1f);
            }

        }
        else
        {
            var edgeRight = nodeObj.transform.Find("EdgeRight").GetComponent<LineRenderer>();
            if (edgeRight != null)
            {
                edgeRight.enabled = false;
            }
        }

        InstantiateSubtree(node.Left);
        InstantiateSubtree(node.Right);
    }
    private void FitCamera()//요구사항엔 없으나 노드가 화면 밖으로 나가지 않게 하는 메서드.
    {
        
        if (nodePositions.Count == 0) return;

        float minX = float.MaxValue, maxX = float.MinValue;
        float minY = float.MaxValue, maxY = float.MinValue;

        foreach (var pos in nodePositions.Values)
        {
            //x,y값만 반영
            if (pos.x < minX) minX = pos.x;
            if (pos.x > maxX) maxX = pos.x;
            if (pos.y < minY) minY = pos.y;
            if (pos.y > maxY) maxY = pos.y;
        }

        //  중앙으로 카메라 이동
        float centerX = (minX + maxX) * 0.5f;
        float centerY = (minY + maxY) * 0.5f;
        Camera.main.transform.position = new Vector3(centerX, centerY, Camera.main.transform.position.z);

        // 세로/가로 중 큰 쪽으로 orthographicSize 설정
        float screenRatio = (float)Screen.width / Screen.height;
        float requiredVertical = (maxY - minY) * 0.5f + cameraPadding;
        float requiredHorizontal = (maxX - minX) * 0.5f / screenRatio + cameraPadding;

        Camera.main.orthographicSize = Mathf.Max(requiredVertical, requiredHorizontal);

    }

    public void Clear()
    {
        foreach (var obj in nodeObjects.Values)
        {
            Destroy(obj);
        }
        nodeObjects.Clear();
        nodePositions.Clear();
    }
}