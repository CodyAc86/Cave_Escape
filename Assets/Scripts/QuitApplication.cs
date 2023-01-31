using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitApplication : MonoBehaviour
{
    
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("we pushed Escape");
            QuitGame();
        }
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
