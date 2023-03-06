using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class GrindManager : MonoBehaviour
{
    public int[,] Matrix;
    Transform parent;

    public List<Box> Boxes = new List<Box>();
    public List<Box> createdBoxes = new List<Box>();

    //public GameObject[,] boxMatrix;

    public List<Box> deletedBoxes = new List<Box>();

    int startXPos = 115;
    int startYPos = 140;
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
    [HideInInspector] public TMP_Text purpleCountText;

    public int scoreCount;
    public TMP_Text scoreCountText;

    void Start()
    {
        parent = this.transform;
        CreateMatrix(colums, rows);
        UpdateBoxCountTexts();
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject go = Reff.i.rayCast.GO();
            if (go.gameObject.tag == "Red" || go.gameObject.tag == "Yellow" || go.gameObject.tag == "Green" || go.gameObject.tag == "Blue" || go.gameObject.tag == "Purple")
            {
                SelectBoxes(go);
            }
        }
    }
    void CreateMatrix(int c, int r)
    {
        Matrix = new int[colums, rows];
        //boxMatrix = new GameObject[colums, rows];
        for (int i = 0; i < c; i++)
        {
            for (int j = 0; j < r; j++)
            {
                Matrix[i, j] = Random.Range(0, 5);
                CreateBoxes(i, j, Matrix[i, j]);
            }
        }
    }

    void CreateBoxes(int x, int y, int value)
    {
        GameObject b = Instantiate(Boxes[value].myBox);
        b.gameObject.transform.SetParent(parent);
        b.transform.position = new Vector3(startXPos + (x * squareSize), startYPos + (y * squareSize));
        b.GetComponent<Box>().xPos = startXPos + (x * squareSize);
        b.GetComponent<Box>().yPos = startYPos + (y * squareSize);
        createdBoxes.Add(b.GetComponent<Box>());

        //boxMatrix[x, y] = b;

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
            Destroy(go.myBox);
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
    void SelectBoxes(GameObject go)
    {
        if (go.GetComponent<Box>().myBros.Count > 0)
        {
            if (!go.GetComponent<Box>().isSelected == true)
            {
                foreach (Box boxy in createdBoxes)
                {
                    boxy.isSelected = false;
                }
                switch (go.gameObject.tag)
                {
                    case "Red":
                        go.GetComponent<Box>().isSelected = true;
                        break;
                    case "Yellow":
                        go.GetComponent<Box>().isSelected = true;
                        break;
                    case "Green":
                        go.GetComponent<Box>().isSelected = true;
                        break;
                    case "Blue":
                        go.GetComponent<Box>().isSelected = true;
                        break;
                    case "Purple":
                        go.GetComponent<Box>().isSelected = true;
                        break;
                }
            }
            else
            {
                DestroySelectedBoxes();
            }
        }
    }
    void DestroySelectedBoxes()
    {
        for (int i = 0; i < createdBoxes.Count; i++)
        {
            if (createdBoxes[i].isSelected)
            {
                deletedBoxes.Add(createdBoxes[i]);
            }
        }
        foreach (Box go in deletedBoxes)
        {
            switch (go.gameObject.tag)
            {
                case "Red":
                    redCount--;
                    break;
                case "Yellow":
                    yellowCount--;
                    break;
                case "Green":
                    greenCount--;
                    break;
                case "Blue":
                    blueCount--;
                    break;
                case "Purple":
                    purpleCount--;
                    break;
            }
            createdBoxes.Remove(go);
            Destroy(go.myBox);
        }
        scoreCount += ((deletedBoxes.Count) * (deletedBoxes.Count - 1));
        deletedBoxes.Clear();
        UpdateBoxCountTexts();
        scoreCountText.SetText(scoreCount.ToString());
        //UpdateBoard();
    }
    public void UpdateBoard()
    {
        for (int i = 0; i < colums; i++)
        {
            for (int j = 0; j < rows; j++)
            {
            }
        }
    }
}