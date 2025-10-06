using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CameraButtonGenerator : MonoBehaviour
{
    public GameObject buttonPrefab;         // Le prefab du bouton
    public Transform layoutGroupParent;     // Le parent avec un LayoutGroup (Vertical/Grid/etc.)

    void Start()
    {
        // Trouver toutes les caméras avec le tag "Camera"
        GameObject[] cameras = GameObject.FindGameObjectsWithTag("Camera");

        // Créer un bouton pour chaque caméra
        foreach (GameObject cam in cameras)
        {
            GameObject newButton = Instantiate(buttonPrefab, layoutGroupParent);

            // Récupérer le TMP_Text dans le bouton et mettre le nom de la caméra
            TMP_Text tmpText = newButton.GetComponentInChildren<TMP_Text>();
            if (tmpText != null)
                tmpText.text = cam.name;

            // Ajouter l'action au clic du bouton
            newButton.GetComponent<Button>().onClick.AddListener(() => OnCameraButtonClicked(cam));
        }
    }

   public void OnCameraButtonClicked(GameObject cam)
    {
        Debug.Log("Caméra sélectionnée : " + cam.name);

        // Désactiver toutes les caméras
        foreach (GameObject otherCam in GameObject.FindGameObjectsWithTag("Camera"))
        {
            if (otherCam.transform.parent != null)
                otherCam.transform.parent.gameObject.SetActive(false); // Désactive le parent
            else
                otherCam.SetActive(false);
        }

        // Activer celle cliquée
        if (cam.transform.parent != null)
            cam.transform.parent.gameObject.SetActive(true); // Active le parent
        else
            cam.SetActive(true);
    }
}