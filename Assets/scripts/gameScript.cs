using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.scripts
{

    public class gameScript : MonoBehaviour
    {

        List<int[,]> screens;
        public Camera myCamera;
        int WIDTH = 9;
        int HEIGHT = 6;

        public Button exitButton;
        public GameObject scene;

        public int currentScreen;
        public GameObject playerPrefab;
        public GameObject player;

        float timeRemaining = 1;
        int timeRemainingInt = 3;
        public Text countDownText;
        Assets.scripts.playerScript myScript;


        void Start()
        {

            exitButton.onClick.AddListener(goToMenu);

            screens = new List<int[,]>();

            readLayouts("Assets/maze/layouts");

            myCamera.transform.localPosition = new Vector3(7.72f, -4.97f, -11.6f);

            instantiateWorld();

            player = Instantiate(playerPrefab);

            player.transform.SetParent(scene.transform);

            myScript = player.GetComponent<Assets.scripts.playerScript>();
            myScript.setStartPosition(screens[currentScreen], WIDTH, HEIGHT);

        }


        void instantiateWorld()
        {
            int x = Random.Range(0, screens.Count);

            currentScreen = x;
            Shader unlitColorShader = Shader.Find("Standard");

            int[,] sl = screens[x];

            for (int i = 0; i < WIDTH; i++)
            {
                for (int j = 0; j < HEIGHT; j++)
                {
                    if (sl[i, j] == 0)
                    {
                        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        cube.transform.SetParent(scene.transform);
                        cube.GetComponent<Renderer>().material.color = Color.white;
                        cube.GetComponent<BoxCollider>().size = new Vector3(1.1f, 1f, 1.1f);//make the collider a little bit bigger than the actual box!
                        //cube.GetComponent<BoxCollider>().isTrigger = true;

                        //Rigidbody gameObjectsRigidBody = cube.AddComponent<Rigidbody>(); // Add the rigidbody.
                        //gameObjectsRigidBody.useGravity = false;

                        cube.transform.localScale = new Vector3(2, 2, 2);
                        cube.transform.position = new Vector3(i * 2f, -j * 2f, 0);
                    }
                }

            }
        }


        void readLayouts(string file_path)
        {
            StreamReader inp_stm = new StreamReader(file_path);
            int[,] newScreen = new int[WIDTH, HEIGHT];

            int index = 0;
            while (!inp_stm.EndOfStream)
            {
                string inp_ln = inp_stm.ReadLine();

                if (inp_ln.Contains("-") == true)
                {
                    newScreen = new int[WIDTH, HEIGHT];
                    screens.Add(newScreen);
                    index = 0;
                }
                else
                {
                    string[] line = inp_ln.Split(",".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries);

                    for (int i = 0; i < WIDTH; i++)
                        newScreen[i, index] = int.Parse(line[i]);

                    screens[screens.Count - 1] = newScreen;
                    index++;
                }

            }

            inp_stm.Close();
        }

        void goToMenu()
        {
            SceneManager.LoadSceneAsync("menu");
        }



        void Update()
        {

            if (timeRemainingInt > 0)
            {
                if (timeRemaining > 0)
                    timeRemaining -= Time.deltaTime;
                else
                {
                    timeRemainingInt--;
                    countDownText.text = timeRemainingInt.ToString();
                    timeRemaining = 1;
                    if (timeRemainingInt == 0)
                    {
                        countDownText.gameObject.SetActive(false);
                        myScript.startTheGame();
                    }
                }
            }
        }
    }
}