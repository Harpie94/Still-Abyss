using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Change IsInLOS", story: "Change [IsInLOS] to [bool]", category: "Action", id: "ad8e74fa065a89b1bc158ad3faa7990b")]
public partial class ChangeIsInLosAction : Action
{
    [SerializeReference] public BlackboardVariable<bool> IsInLOS;
    [SerializeReference] public BlackboardVariable<bool> Bool;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        IsInLOS.Value = Bool.Value;
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

