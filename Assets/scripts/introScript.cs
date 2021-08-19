using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.scripts
{
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
}