using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "PlayerInLOS", story: "[IsInLOS] true?", category: "Conditions", id: "81a0295c2e494dbc5ec372b3ba19c2a0")]
public partial class PlayerInLosCondition : Condition
{
    [SerializeReference] public BlackboardVariable<bool> IsBoolTrue;

    public override bool IsTrue()
    {
        return IsBoolTrue? true : false;
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
