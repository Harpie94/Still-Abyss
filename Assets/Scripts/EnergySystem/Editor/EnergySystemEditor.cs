using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnergySystem))]
public class EnergySystemEditor : Editor
{
    private float energyToAdd = 10f;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        SerializedProperty prop = serializedObject.GetIterator();
        bool enterChildren = true;

        while (prop.NextVisible(enterChildren))
        {
            enterChildren = false;
            EditorGUILayout.PropertyField(prop, true);
        }

        EnergySystem energySystem = (EnergySystem)target;

        //la parti permetant de changer la current energy in game
        EditorGUILayout.Space();
        float newEnergy = EditorGUILayout.FloatField("Current Energy", energySystem.CurrentEnergy);
        newEnergy = Mathf.Clamp(newEnergy, 0f, energySystem.GetMaxEnergy());
        if (newEnergy != energySystem.CurrentEnergy)
        {
            Undo.RecordObject(energySystem, "Change Current Energy");
            energySystem.SetEnergy(newEnergy);
        }

        //Pour afficher la barre d'energy dans l'inspector (purement visuel)
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Energy status", EditorStyles.boldLabel);
        float energyPercent = energySystem.GetEnergyPercentage();
        Rect rect = GUILayoutUtility.GetRect(18, 18, "TextField");
        EditorGUI.ProgressBar(rect, energyPercent, $"Energy : {(energyPercent * 100):F1}%");

        //permet de faire du debug meme si on a deja le moyen de changer la valeur de l'energy ca pourra servir on c jamais
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Debug", EditorStyles.boldLabel);
        energyToAdd = EditorGUILayout.Slider("Add energy", energyToAdd, 1f, energySystem.GetMaxEnergy());
        if (GUILayout.Button("Add"))
        {
            energySystem.AddEnergy(energyToAdd);
        }

        serializedObject.ApplyModifiedProperties();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(energySystem);
        }

        Repaint();
    }
}
