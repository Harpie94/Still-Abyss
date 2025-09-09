using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UI;

public class Malfunction : MonoBehaviour
{
    public RawImage CamOutput;
    public Texture malfuctiontexture;

    public RenderTexture CamRenderTexture;
    public RenderTexture MalfuctionTexture;

    [SerializeField]
    public Camera cam1;
    public Camera cam2;
    public Camera cam3;

    [Range(0, 100)]
    public int malfunctionValue1 = 100;

    [Range(0, 100)]
    public int malfunctionValue2 = 100;

    [Range(0, 100)]
    public int malfunctionValue3 = 100;

    void Start()
    {

    }

    void Update()
    {

    }


    public void buttoncheckmalfunction()
    {
        if (malfunctionValue1 == 0)
        {
            //cam1.targetTexture = MalfuctionTexture;
            //cam1.enabled = false;   // Caméra coupée
            CamOutput.texture = malfuctiontexture;
        }
        else
        {
            //cam1.enabled = true;    // Caméra active
            cam1.targetTexture = CamRenderTexture;
            CamOutput.texture = CamRenderTexture;
        }
    }

}