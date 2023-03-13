using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reff : MonoBehaviour
{
    public static Reff i;
    public uiRaycaster rayCast;
    private void Awake()
    {
        i = this;
    }
}


