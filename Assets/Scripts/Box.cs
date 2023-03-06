using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public GameObject myBox;
    public GameObject shade;

    //public Rigidbody2D myRigidbody;

    public bool isSelected;
    public bool gotMyBros;

    public int xPos;
    public int yPos;

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

        gameObject.transform.position = new Vector3(xPos, yPos, transform.position.z);
    }

    void OnCollisionEnter2D(Collision2D  other)
    {
     
        if (other.gameObject.tag == this.gameObject.tag)
        {
            myBros.Add(other.gameObject.GetComponent<Box>());
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
        }
    }
}
