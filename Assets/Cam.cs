using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class Cam : MonoBehaviour
{

    [SerializeField] GameObject Cams;
    [SerializeField] GameObject Playercam;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cams.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ActivateCam()
    {
        Cams.SetActive(true);
        Playercam.SetActive(false);
    }

    public void CloseCams()
    {
        Cams.SetActive(false);
        Playercam.SetActive(true);
    }
}
