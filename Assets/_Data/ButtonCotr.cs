using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonCotr : MonoBehaviour
{
    
    private void Update()
    {
        if (InputManger.Instance.restart)
        {
            Restart();
        }

        if (InputManger.Instance.back)
        {
            Back();
        }
    }

    public void Restart()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    public void Back()
    {
        SceneManager.LoadSceneAsync(0);
    }
}
