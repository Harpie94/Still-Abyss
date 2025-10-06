using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Update IsPlayerOnCams", story: "Update [IsPlayerOnCams] using [CheckPlayerOnCams]", category: "Action", id: "5c6662b71173b070426ba08ff3af4bf1")]
public partial class UpdateIsPlayerOnCamsAction : Action
{
    [SerializeReference] public BlackboardVariable<bool> IsPlayerOnCams;
    [SerializeReference] public BlackboardVariable<CCTVPlayer> CheckPlayerOnCams;

    protected override Status OnStart()
    {
        IsPlayerOnCams.Value = CheckPlayerOnCams.Value.isLookingAtCams;
        return Status.Success;
    }

}

