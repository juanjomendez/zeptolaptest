using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerScript : MonoBehaviour
{

    Texture2D[] texturesRight;

    Material myMaterial;

    string[] bitmapsRight = { "1", "2", "3", "4", "5", "6", "7", "8"};


    void Start()
    {

        texturesRight = new Texture2D[8];

        transform.localPosition = new Vector3(-6f, -3f - 1f + 0.25f, 8.99f);

        for (int i = 0;i < 8;i++)
            texturesRight[i] = Resources.Load(bitmapsRight[i]) as Texture2D;

        myMaterial = GetComponent<Renderer>().material;

        myMaterial.mainTexture = texturesRight[0];

    }

    
    void Update()
    {
        transform.position = new Vector3(transform.position.x +0.005f, transform.position.y, transform.position.z);
        myMaterial.mainTexture = texturesRight[Random.Range(0,8)];
    }
}
