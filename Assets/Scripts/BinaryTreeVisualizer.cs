using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using TMPro;


public class BinaryTreeVisualizer : MonoBehaviour
{
    public GameObject NodePrefab;
    public GameObject LinePrefab;
    public float VerticalSpacing = 2f;
    public float HorizontalSpacing = 2f;
    private int xPosCount = 0;
    public enum SpacingType
    {
        InOrder,
        Pow,
        LevelOrder
    }
    public SpacingType spacingType = SpacingType.InOrder;

    private BinarySearchTree<int, string> bst;

    // 노드 키 → 화면 위치
    private Dictionary<int, Vector2> layout = new Dictionary<int, Vector2>();
    // 노드 키 → 게임오브젝트
    private Dictionary<int, GameObject> nodeMap = new Dictionary<int, GameObject>();

    private List<GameObject> nodeObjects = new List<GameObject>();
    private List<GameObject> lineObjects = new List<GameObject>();


    public void SetTree(BinarySearchTree<int, string> tree)
    {
        bst = tree;
        DrawTree(); 
    }
    public void DrawTree()
    {
        if(bst == null) return;
        Clear();
        layout.Clear();
        xPosCount = 0;

        switch (spacingType)
        {
            case SpacingType.InOrder:
                CalculayteLayout(bst.Root, 0);
                CenterByRoot();
                break;
            case SpacingType.Pow:
                CalcLayoutPow(bst.Root, 0, 0, bst.Root.Height);
                break;
        }
         
        CreateNodes(bst.Root);
        CreateEdges(bst.Root);

    }
    public void CalculayteLayout(TreeNode<int, string> node, int depth)
    {
        if (node == null) return;
        CalculayteLayout(node.Left, depth + 1);

        float xPos = xPosCount * HorizontalSpacing;
        float yPos = -depth * VerticalSpacing;
         
        layout[node.Key] = new Vector2(xPos, yPos);
        xPosCount++;
        CalculayteLayout(node.Right, depth + 1);
    }
    void CalcLayoutPow(TreeNode<int, string> node, float x, float y, int height)
    {
        if (node == null) return;

        layout[node.Key] = new Vector2(x, y);

        float offset = HorizontalSpacing * Mathf.Pow(2, height - 1);

        CalcLayoutPow(node.Left, x - offset, y - VerticalSpacing, height - 1);
        CalcLayoutPow(node.Right, x + offset, y - VerticalSpacing, height - 1);
    }
    public void CenterByRoot()
    {
        float rootX = layout[bst.Root.Key].x;

        // 카메라 월드 좌표 기준으로 중앙 설정
        float camX = Camera.main.transform.position.x;

        var keys = new List<int>(layout.Keys);
        foreach (int key in keys)
        {
            layout[key] = new Vector2(
                layout[key].x - rootX + camX,  // 루트를 카메라 중앙에 맞춤
                layout[key].y
            );
        }
    }
    public void CreateNodes(TreeNode<int, string> node)
    {
        if (node == null) return;
        Vector2 pos2D = layout[node.Key]; 
        Vector3 pos3D = new Vector3(pos2D.x, pos2D.y, 0);

        GameObject nodeObj = Instantiate(NodePrefab, pos3D, Quaternion.identity, transform);
        nodeObj.name = $"Node_{node.Key}";

        var text = nodeObj.GetComponentInChildren<TextMeshPro>();
        if (text != null)
        {
            text.text = node.Value;
        }

        nodeObjects.Add(nodeObj);
        nodeMap[node.Key] = nodeObj;
        CreateNodes(node.Left);
        CreateNodes(node.Right);
    }
    public void CreateEdges(TreeNode<int, string> node)
    {
        if (node == null) return;

        if (node.Left != null) DrawLine(node.Key, node.Left.Key);
        if (node.Right != null) DrawLine(node.Key, node.Right.Key);
        
        CreateEdges(node.Left);
        CreateEdges(node.Right);

    }
    public void DrawLine(int startKey , int endKey)
    {
        GameObject lines = Instantiate(LinePrefab,transform);
        var lineRender = lines.GetComponent<LineRenderer>();

        Vector3 startPos = nodeMap[startKey].transform.position; 
        Vector3 endPos = nodeMap[endKey].transform.position; 

        lineRender.positionCount = 2;
        lineRender.SetPosition(0, startPos + Vector3.forward * 0.1f);
        lineRender.SetPosition(1, endPos + Vector3.forward * 0.1f);

        lineObjects.Add(lines);
    }
    public void Clear()
    {
        foreach(var node in nodeObjects)
        {
            Destroy(node);
        }
        foreach(var line in lineObjects)
        {
            Destroy(line);
        }
        nodeObjects.Clear();
        lineObjects.Clear();
        nodeMap.Clear();

    }
}
