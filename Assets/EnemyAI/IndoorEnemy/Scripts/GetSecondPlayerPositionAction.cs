using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Get SecondPlayerPosition", story: "Get [SecondPlayerPosition] from [EnemyFOV]", category: "Action", id: "48c7b621865e95eb147da02ba691161d")]
public partial class GetSecondPlayerPositionAction : Action
{
    [SerializeReference] public BlackboardVariable<Vector3> SecondPlayerPosition;
    [SerializeReference] public BlackboardVariable<EnemyFOV> EnemyFOV;

    protected override Status OnStart()
    {
        SecondPlayerPosition.Value = EnemyFOV.Value.secondPlayerPosition;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

