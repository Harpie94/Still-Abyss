using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class Cam : MonoBehaviour
{
    [SerializeField] RawImage output;
    [SerializeField] RenderTexture cam1;
    [SerializeField] RenderTexture cam2;
    [SerializeField] RenderTexture cam3;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ActivateCam1()
    {
        
         output.texture = cam1;
         
        
    }

    public void ActivateCam2()
    {
        output.texture = cam2;

    }

    public void ActivateCam3()
    {
        output.texture = cam3;

    }
}
