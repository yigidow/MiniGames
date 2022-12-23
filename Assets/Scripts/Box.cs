using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public GameObject myBox;
    public GameObject shade;
    public int indexX;
    public int indexY;

    public bool isSelected;

    public List<Box> myBros = new List<Box>();
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isSelected == true)
        {
            shade.gameObject.SetActive(false);
            foreach (Box bro in myBros)
            {
                bro.isSelected = true;
                bro.shade.gameObject.SetActive(false);
            }
        }
    }
}
