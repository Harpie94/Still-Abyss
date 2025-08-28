using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "PlayerDetector", story: "Update [PlayerDetected] and assign [Target]", category: "Action", id: "d92fff620c25f0960b7fc3191174d8cf")]
public partial class PlayerDetectorAction : Action
{
    [SerializeReference] public BlackboardVariable<EnemyFOV> PlayerDetected;
    [SerializeReference] public BlackboardVariable<GameObject> Target;

    protected override Status OnUpdate()
    {
        Target.Value = PlayerDetected.Value.isPlayerInSight ? PlayerDetected.Value.Player : null;
        return PlayerDetected.Value.isPlayerInSight ? Status.Success : Status.Failure;
    }
}