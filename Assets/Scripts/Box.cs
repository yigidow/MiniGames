using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public GameObject myBox;
    public GameObject shade;

    public bool isSelected;

    //public BoxCollider2D boxCollder;

    public List<Box> myBros = new List<Box>();

    // Update is called once per frame
    void Update()
    {
        if(isSelected == true)
        {
            shade.gameObject.SetActive(false);
            foreach (Box bro in myBros)
            {
                bro.isSelected = true;
                //bro.shade.gameObject.SetActive(false);
            }
        }
        else
        {
            shade.gameObject.SetActive(true);
        }
    }

    void OnCollisionEnter2D(Collision2D  other)
    {
        if (other.gameObject.tag == this.gameObject.tag)
        {
            myBros.Add(other.gameObject.GetComponent<Box>());
        }
    }
}
