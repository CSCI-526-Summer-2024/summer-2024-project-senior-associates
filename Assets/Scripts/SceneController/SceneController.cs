using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController
{
    void Start()
    {
        SceneUpdate(0);    
    }

    public void SceneUpdate(int scene){
        if(scene==0)
        {
            SceneManager.LoadScene("SampleScene",LoadSceneMode.Single);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("SampleScene"));
        }
        else if(scene==1)
        {
            SceneManager.LoadScene("FinishScene",LoadSceneMode.Single);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("FinishScene"));
        }
    }
}
