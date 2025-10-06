using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SubtractFromInt", story: "Remove [X] to [Int]", category: "Action", id: "39f8276e0de6686d8162485e0275903a")]
public partial class SubtractFromIntAction : Action
{
    [SerializeReference] public BlackboardVariable<int> X;
    [SerializeReference] public BlackboardVariable<int> Int;

    protected override Status OnStart()
    {
        Int.Value = Int - X;
        return Status.Success;
    }
}

