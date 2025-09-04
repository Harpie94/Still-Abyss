using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class Malfunction : MonoBehaviour
{
    //public Texture malfuctiontexture;

    public RenderTexture CamRenderTexture;
    public RenderTexture MalfuctionTexture;

    [SerializeField]
    public Camera cam1;
    public Camera cam2;
    public Camera cam3;

    [Range(0, 100)]
    public int malfunctionValue = 100;

    void Start()
    {

    }

    void Update()
    {
            if (malfunctionValue == 0)
            {
                cam1.targetTexture = MalfuctionTexture;
                //cam1.enabled = false;   // Caméra coupée
            }
            else
            {
                //cam1.enabled = true;    // Caméra active
                cam1.targetTexture = CamRenderTexture;
            }
    }

}