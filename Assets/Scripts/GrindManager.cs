using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class GrindManager : MonoBehaviour
{
    public int index;
    public int[,] Matrix;
    Transform parent;
    
    public List<Box> Boxes = new List<Box>();
    public List<Box> createdBoxes = new List<Box>();

    public List<GameObject> selectedBoxes = new List<GameObject>();

    int startXPos = 105;
    int startYPos = 180;
    int squareSize = 85;

    int colums = 20;
    int rows = 10;

    [HideInInspector] public int redCount;
    [HideInInspector] public TMP_Text redCountText;
    [HideInInspector] public int yellowCount;
    [HideInInspector] public TMP_Text yellowCountText;
    [HideInInspector] public int greenCount;
    [HideInInspector] public TMP_Text greenCountText;
    [HideInInspector] public int blueCount;
    [HideInInspector] public TMP_Text blueCountText;
    [HideInInspector] public int purpleCount;
    [HideInInspector]public TMP_Text purpleCountText;


    void Start()
    {
        parent = this.transform;
        CreateMatrix(colums, rows);
        UpdateBoxCountTexts();
    }

 
    void Update()
    {
        //SelectBoxes();
    }
    void CreateMatrix(int c, int r)
    {
        Matrix = new int [colums, rows];
        for (int i = 0; i < c; i++)
        {
            for(int j = 0; j < r; j++)
            {
                Matrix[i, j] = Random.Range(0, 5);
                CreateBoxes(i, j, Matrix[i, j]);
                createdBoxes[0].xIndex = 10;
            }
        }
    }

    void CreateBoxes(int x, int y, int value)
    {
        GameObject b = Instantiate(Boxes[value].boxy);
        b.gameObject.transform.SetParent(parent);
        b.transform.position = new Vector3(startXPos + (x * squareSize), startYPos + (y * squareSize));

        createdBoxes.Add(Boxes[value]);
        switch (b.gameObject.tag)
        {
            case "Red":
                redCount++;
                break;
            case "Yellow":
                yellowCount++;
                break;
            case "Green":
                greenCount++;
                break;
            case "Blue":
                blueCount++;
                break;
            case "Purple":
                purpleCount++;
                break;
        }
    }

    public void RefreshBoxes()
    {
        foreach (Box go in createdBoxes)
        {
            Destroy(go.boxy);
        }
        createdBoxes.Clear();
        redCount = 0; blueCount = 0; yellowCount = 0; greenCount = 0; purpleCount = 0;
        CreateMatrix(colums, rows);
        UpdateBoxCountTexts();
    }

    void UpdateBoxCountTexts()
    {
        redCountText.SetText(redCount.ToString());
        blueCountText.SetText(blueCount.ToString());
        yellowCountText.SetText(yellowCount.ToString());
        greenCountText.SetText(greenCount.ToString());
        purpleCountText.SetText(purpleCount.ToString());
    }
    //void SelectBoxes()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        GameObject go = Reff.i.rayCast.GO();

    //        switch (go.gameObject.tag)
    //        {
    //            case "Red":
    //                index = createdBoxesGameobj.IndexOf(go);
    //                selectedBoxes.Add(go);
    //                GameObject child = go.gameObject.transform.GetChild(0).gameObject;
    //                child.SetActive(false);
    //                while (index < rows * colums && createdBoxesGameobj[index + rows].CompareTag(go.tag))
    //                    if (createdBoxesGameobj[index + rows].CompareTag(go.tag))
    //                    {
    //                        selectedBoxes.Add(go);
    //                        createdBoxesGameobj[index + rows].gameObject.transform.GetChild(0).gameObject.SetActive(false);
    //                        index += rows;
    //                    }
    //                break;

    //            case "Yellow":
    //                index = createdBoxesGameobj.IndexOf(go);
    //                    break;
    //            case "Green":
    //                index = createdBoxesGameobj.IndexOf(go);
    //                break;
    //            case "Blue":
    //                index = createdBoxesGameobj.IndexOf(go);
    //                break;
    //            case "Purple":
    //                index = createdBoxesGameobj.IndexOf(go);
    //                break;

    //        }
    //    }
    //}

    [System.Serializable]
    public class Box
    {
        public GameObject boxy;
        public int xIndex;
        public int yIndex;
    }

}
 