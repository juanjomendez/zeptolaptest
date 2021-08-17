using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class gameScript : MonoBehaviour
{

    List<int[,]> screens;
    public Camera myCamera;
    int WIDTH = 7;
    int HEIGHT = 4;

    public Button exitButton;
    public GameObject scene;

    public int currentScreen;

    void Start()
    {

        exitButton.onClick.AddListener(goToMenu);

        screens = new List<int[,]>();

        readLayouts("Assets/maze/layouts");

        myCamera.transform.localPosition = new Vector3(6f, -3f, -9f);

        instantiateWorld();

    }


    void instantiateWorld()
    {
        int x = Random.Range(0,screens.Count);

        currentScreen = x;
        Shader unlitColorShader = Shader.Find("Unlit/Color");

        int[,] sl = screens[x];

        for (int i = 0;i < WIDTH;i++)
        {
            for (int j = 0; j < HEIGHT; j++)
            {
                if (sl[i, j] == 1)
                {
                    GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
                    quad.transform.SetParent(scene.transform);
                    quad.GetComponent<Renderer>().material.color = Color.white;
                    quad.GetComponent<Renderer>().material.shader = unlitColorShader;
                    quad.transform.localScale = new Vector3(2, 2, 2);
                    quad.transform.position = new Vector3(i*2f, -j*2f, 0);
                }
            }

        }
    }


    void readLayouts(string file_path)
    {
        StreamReader inp_stm = new StreamReader(file_path);
        int[,] newScreen= new int[WIDTH, HEIGHT];

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
                string [] line = inp_ln.Split(",".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries);

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
        
    }
}
