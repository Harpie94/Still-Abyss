using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "AddToInt", story: "Add [X] to [Int]", category: "Action", id: "f4d41a118ecc2f6977745e3d18fbb03b")]
public partial class AddToIntAction : Action
{
    [SerializeReference] public BlackboardVariable<int> X;
    [SerializeReference] public BlackboardVariable<int> Int;

    protected override Status OnStart()
    {
        Int.Value = X + Int;
        return Status.Success;
    }
}

