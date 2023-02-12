using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//class is taken from Unit tutorial "Making a Main Menu"

public class QuitGame : MonoBehaviour
{
    public void Quit()
    {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();

#endif
    }
}

