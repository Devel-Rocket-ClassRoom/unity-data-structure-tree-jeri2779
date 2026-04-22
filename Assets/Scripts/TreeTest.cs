using UnityEngine;

public class TreeTest : MonoBehaviour
{
    //BinarySearchTree 테스트코드 작성
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        var bst = new BinarySearchTree<string, string>();
        bst["50"] = "Apple";
        bst["30"] = "Candy";
        bst["70"] = "Banana";
        bst["20"] = "Onion";
        bst["40"] = "Lettuce";

        //foreach(var pair in bst)
        //{
        //    Debug.Log(pair);

        //}
        //Debug.Log($"123 Contains: {bst.ContainsKey("123")}");
        //bst.Remove("123");
        //Debug.Log($"123 Contains: {bst.ContainsKey("123")}");


        //foreach (var pair in bst.InOrderTraversal())
        //{
        //    Debug.Log(pair);

        //}
        //foreach(var pair in bst.PreOrderTraversal())
        //{
        //    Debug.Log(pair);
        //}
        //foreach(var pair in bst.PostOrderTraversal())
        //{
        //    Debug.Log(pair);
        //}
        foreach (var pair in bst.LevelOrderTraversal())
        {
            //Debug.Log(pair.Key);
        }
    }

     
}
