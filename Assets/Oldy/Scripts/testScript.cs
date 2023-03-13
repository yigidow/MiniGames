using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testScript : MonoBehaviour
{
    void Update()
    {
        SelectBoxes();
    }
    void SelectBoxes()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject go = Reff.i.rayCast.GO();

            if (go.CompareTag("Red"))
            {
                Destroy(go);
            }
        }
    }
}
