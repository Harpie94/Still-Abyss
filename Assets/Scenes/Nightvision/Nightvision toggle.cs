using UnityEngine;
using UnityEngine.UI;

public class Nightvisiontoggle : MonoBehaviour
{


    public GameObject Nightvision;

    bool NightVisionON;
    private void Start()
    {
        NightVisionON = false;
    }

    public void ToggleNightVision()
    {
        if (NightVisionON == false)
        {
            Nightvision.SetActive(true);
            NightVisionON = true;
        }
        else if (NightVisionON == true)
        {
            Nightvision.SetActive(false);
            NightVisionON = false;
        }
    }
}
