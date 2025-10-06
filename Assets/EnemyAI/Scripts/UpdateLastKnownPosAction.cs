using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Update LastKnownPos", story: "Update [LastKnownPlayerPos] using [EnemyFOV]", category: "Action", id: "5129d591f323bb44de160987047a4bda")]
public partial class UpdateLastKnownPosAction : Action
{
    [SerializeReference] public BlackboardVariable<Vector3> LastKnownPlayerPos;
    [SerializeReference] public BlackboardVariable<EnemyFOV> EnemyFOV;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        LastKnownPlayerPos.Value = EnemyFOV.Value.lastKnownPlayerPosition;
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

