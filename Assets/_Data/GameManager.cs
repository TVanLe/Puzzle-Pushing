using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static bool NotGetInput { get; private set; }
    private Animator sceneTrans;
    private void Awake()
    {
        NotGetInput = false;
        sceneTrans = GameObject.Find("SceneTrans").GetComponent<Animator>();
    }

    

    public  void NextLevel()
    {
        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        NotGetInput = true;
        sceneTrans.SetTrigger("end");
        yield return new WaitForSeconds(3f);
        if (SceneManager.GetActiveScene().buildIndex + 1 > SceneManager.sceneCountInBuildSettings - 1)
        {
            SceneManager.LoadSceneAsync(0);
        }
        else
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }

    
}
