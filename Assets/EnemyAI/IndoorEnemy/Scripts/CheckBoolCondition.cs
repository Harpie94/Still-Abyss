using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "CheckBool", story: "[Bool] State", category: "Conditions", id: "8be95b862b8ac1007e64409c0e5989b5")]
public partial class CheckBoolCondition : Condition
{
    [SerializeReference] public BlackboardVariable<bool> Bool;

    public override bool IsTrue()
    {

        return Bool;
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
