using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoWriteLevel : MonoBehaviour
{
    private TextMeshProUGUI textMeshProText;

    private void Start()
    {
        textMeshProText = GetComponent<TextMeshProUGUI>();
        int index = SceneManager.GetActiveScene().buildIndex;
        textMeshProText.text = "Level " + index;
    }
    
}
