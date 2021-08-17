﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class introScript : MonoBehaviour
{

    public Button playButton;
    void Start()
    {
        playButton.onClick.AddListener(goToPlay);
    }


    void goToPlay()
    {
        SceneManager.LoadSceneAsync("game");
    }
}
