using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Get PlayerLookingAtEnemy", story: "Update [PlayerLookingAtEntity] using [PlayerFOV]", category: "Action", id: "d2122891f79c0251e9fec3075c13d4af")]
public partial class GetPlayerLookingAtEnemyAction : Action
{
    [SerializeReference] public BlackboardVariable<bool> PlayerLookingAtEntity;
    [SerializeReference] public BlackboardVariable<PlayerFOV> PlayerFOV;
    protected override Status OnStart()
    {
        if (PlayerFOV.Value == null)
        {
            Debug.LogError("PlayerFOV is not assigned");
            return Status.Failure;
        }
        PlayerLookingAtEntity.Value = PlayerFOV.Value.isEnemyInSight;
        return Status.Success;
    }


}

