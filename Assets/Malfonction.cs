using UnityEngine;
using System.Collections.Generic;

public class Malfunction : MonoBehaviour
{
    // Dictionnaire qui associe chaque caméra à sa valeur de "panne"
    private Dictionary<Camera, int> cameraMalfunctions = new Dictionary<Camera, int>();

    void Start()
    {
        // Trouver toutes les caméras avec le tag "Camera"
        GameObject[] camObjects = GameObject.FindGameObjectsWithTag("Camera");

        foreach (GameObject camObj in camObjects)
        {
            Camera cam = camObj.GetComponent<Camera>();
            if (cam != null && !cameraMalfunctions.ContainsKey(cam))
            {
                // Par défaut chaque caméra fonctionne (1)
                cameraMalfunctions.Add(cam, 1);
            }
        }
    }

    void Update()
    {
        foreach (var kvp in cameraMalfunctions)
        {
            Camera cam = kvp.Key;
            int malfunctionValue = kvp.Value;

            if (malfunctionValue == 0)
            {
                cam.enabled = false;   // Caméra coupée
                // ou cam.targetDisplay = 7;
            }
            else
            {
                cam.enabled = true;    // Caméra active
                cam.targetDisplay = 0;
            }
        }
    }

    // Permet de modifier la valeur de "panne" pour une caméra spécifique
    public void SetMalfunction(Camera cam, int value)
    {
        if (cameraMalfunctions.ContainsKey(cam))
        {
            cameraMalfunctions[cam] = Mathf.Clamp(value, 0, 1);
        }
    }

    // Récupérer l’état actuel d’une caméra
    public int GetMalfunction(Camera cam)
    {
        if (cameraMalfunctions.ContainsKey(cam))
            return cameraMalfunctions[cam];
        return -1; // pas trouvé
    }
}