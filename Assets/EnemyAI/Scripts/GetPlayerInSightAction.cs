using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.Assertions.Must;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "GetPlayerInSight", story: "Is the [Player] within LOS of the Enemy [Self] [IsInLOS] with [EnemyFOV]", category: "Action", id: "61ef2cc7aa1565583f67b7e6ce1a5413")]
public partial class GetPlayerInSightAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Player;
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<bool> IsInLOS;
    [SerializeReference] public BlackboardVariable<EnemyFOV> EnemyFOV;
    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        IsInLOS.Value = EnemyFOV.Value.isPlayerInSight;
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

