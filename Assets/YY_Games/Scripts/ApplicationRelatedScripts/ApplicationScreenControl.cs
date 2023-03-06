    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YY_Games_Scripts
{
    public class ApplicationScreenControl : MonoBehaviour
    {

        void Start()
        {
            Screen.SetResolution(1280, 720, FullScreenMode.FullScreenWindow);
            Application.targetFrameRate = 60;
        }

    }
}

