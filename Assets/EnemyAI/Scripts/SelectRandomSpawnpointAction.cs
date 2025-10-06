using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Select random spawnpoint", story: "Select a random spawnpoint from [ListSpawnPoint] and change [SelectedSpawnPoint]", category: "Action", id: "20802c693af3dacccf4a53fcc7da156c")]
public partial class SelectRandomSpawnpointAction : Action
{
    [SerializeReference] public BlackboardVariable<List<GameObject>> ListSpawnPoint;
    [SerializeReference] public BlackboardVariable<GameObject> SelectedSpawnPoint;

    protected override Status OnStart()
    {
        if (ListSpawnPoint.Value == null || ListSpawnPoint.Value.Count == 0)
        {
            Debug.LogWarning("ListSpawnPoint is null or empty.");
            return Status.Failure;
        }
        int randomIndex = UnityEngine.Random.Range(0, ListSpawnPoint.Value.Count);
        SelectedSpawnPoint.Value = ListSpawnPoint.Value[randomIndex];
        return Status.Success;
    }
}

