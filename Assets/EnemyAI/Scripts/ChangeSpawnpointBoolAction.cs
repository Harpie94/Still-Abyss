using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Change Spawnpoint Bool", story: "Change [SelectedSpawnPoint] bool using current [SelectedPoint] and [CheckPointAvailability]", category: "Action", id: "f726a6f5a46db6c0915bcce0c8e5d4bd")]
public partial class ChangeSpawnpointBoolAction : Action
{
    [SerializeReference] public BlackboardVariable<bool> SelectedSpawnPoint;
    [SerializeReference] public BlackboardVariable<GameObject> SelectedPoint;
    [SerializeReference] public BlackboardVariable<CheckPointAvailability> CheckPointAvailability;
    protected override Status OnStart()
    {
        var availability = SelectedPoint.Value.GetComponent<CheckPointAvailability>();
        if (availability == null)
            return Status.Failure;

        // Met à jour SelectedSpawnPoint selon la disponibilité
        SelectedSpawnPoint.Value = availability.isAvailable;

        return Status.Success;
    }

}

