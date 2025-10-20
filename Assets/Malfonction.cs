using UnityEngine;
using UnityEngine.UI;

public class CameraSurveillanceControl : MonoBehaviour
{
    [Header("Références à assigner dans l'inspecteur")]
    [SerializeField] private MonoBehaviour salleScript; // Script contenant la variable d'état
    [SerializeField] private string variableName = "etatSalle"; // Nom exact de la variable dans le script
    [SerializeField] private Camera cameraSalle; // La caméra de la salle
    [SerializeField] private RenderTexture renderTexture; // RenderTexture utilisée quand tout va bien
    [SerializeField] private Texture2D imageHS; // Image affichée quand la salle est endommagée
    [SerializeField] private RawImage ecranImage; // L'image sur l'écran (UI)

    private Texture textureInitiale;
    private System.Reflection.FieldInfo variableInfo;

    void Start()
    {
        // Sauvegarde la texture initiale
        if (ecranImage != null)
            textureInitiale = ecranImage.texture;

        // On récupère la variable du script de salle par réflexion
        if (salleScript != null)
            variableInfo = salleScript.GetType().GetField(variableName);

        if (variableInfo == null)
            Debug.LogWarning($"Impossible de trouver la variable '{variableName}' dans le script '{salleScript}'.");
    }

    void Update()
    {
        if (variableInfo == null) return;

        // Récupère la valeur de la variable
        object value = variableInfo.GetValue(salleScript);


        if (value is float floatValue)
        {
            GérerAffichage(floatValue);
        }
        else if (value is int intValue)
        {
            GérerAffichage(intValue);
        }
    }

    private void GérerAffichage(float valeur)
    {
        // Quand la salle est endommagée
        if (valeur <= 0)
        {
            if (cameraSalle != null) cameraSalle.enabled = false;
            if (ecranImage != null && imageHS != null)
                ecranImage.texture = imageHS;
        }
        // Quand la salle est réparée
        else if (valeur >= 25)
        {
            if (cameraSalle != null) cameraSalle.enabled = true;
            if (ecranImage != null && renderTexture != null)
                ecranImage.texture = renderTexture;
        }
    }
}