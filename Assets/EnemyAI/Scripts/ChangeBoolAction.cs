using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Change Bool", story: "Change [bool1] to [bool2]", category: "Action", id: "ad8e74fa065a89b1bc158ad3faa7990b")]
public partial class ChangeBoolAction : Action
{
    [SerializeReference] public BlackboardVariable<bool> Bool1;
    [SerializeReference] public BlackboardVariable<bool> Bool2;
    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        Bool1.Value = Bool2.Value;
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

