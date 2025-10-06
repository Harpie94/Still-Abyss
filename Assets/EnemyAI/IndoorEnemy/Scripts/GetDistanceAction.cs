using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Get Distance", story: "Get the distance [distanceBetweenPlayerAndCCTV] between [Player] and [CCTV]", category: "Action", id: "72ec29f525a06a626122f801aa7c00cc")]
public partial class GetDistanceAction : Action
{
    [SerializeReference] public BlackboardVariable<float> DistanceBetweenPlayerAndCCTV;
    [SerializeReference] public BlackboardVariable<GameObject> Player;
    [SerializeReference] public BlackboardVariable<GameObject> CCTV;

    protected override Status OnStart()
    {
        if (Player.Value == null || CCTV.Value == null)
        {
            Debug.LogWarning("Player or CCTV is not assigned.");
            return Status.Failure;
        }
        DistanceBetweenPlayerAndCCTV.Value = Vector3.Distance(Player.Value.transform.position, CCTV.Value.transform.position);
        return Status.Success;
    }
}

