using UnityEditor;
using UnityEngine;

public class TreeVisuallizer : MonoBehaviour
{
    public enum SpacingType
    {
        InOrder,
        PreOrder,
        PostOrder,
        LevelOrder
    }
    public GameObject NodePrefab;
    public SpacingType spacingType;
    public float VerticalSpacing = 2f;
    public float HorizontalSpacing = 2f;    


}
