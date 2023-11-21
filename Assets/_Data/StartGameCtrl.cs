using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameCtrl : MonoBehaviour
{
    public GameObject start;
    public GameObject level;
    public GameObject sceneTrans;
    private float timeDelay = 3f;
    private float timeCounter = 0f;

    private void Start()
    {
        timeCounter = 0f;
    }

    private void Update()
    {
        if (StartGame())
        {
            
            StartCoroutine(SceneTrans());
        }

        timeCounter += Time.deltaTime;
    }

    protected virtual bool StartGame()
    {
        if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) && start.activeSelf && timeCounter >= timeDelay)
        {
            Debug.Log("start game");
            return true;
        }
        else
            return false;

    }

    IEnumerator SceneTrans()
    {
        sceneTrans.GetComponent<Animator>().SetTrigger("end");
        yield return new WaitForSeconds(3f);
        start.SetActive(false);
        level.SetActive(true);
        sceneTrans.GetComponent<Animator>().SetTrigger("start");
        yield return new WaitForSeconds(3f);
        sceneTrans.SetActive(false);
    }

    public void SelectLV(int name)
    {
        StartCoroutine(SelectLevel(name));
    }
    
    IEnumerator SelectLevel(int name)
    {
        sceneTrans.SetActive(true);
        sceneTrans.GetComponent<Animator>().SetTrigger("end");
        yield return new WaitForSeconds(3f);
        SceneManager.LoadSceneAsync(name);
    }
}
