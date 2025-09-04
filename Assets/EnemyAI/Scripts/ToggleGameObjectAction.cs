using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Toggle GameObject", story: "Set [TargetGameObject] to [bool]", category: "Action", id: "1ff1d1e8656dbf56f2329f046f49962c")]
public partial class ToggleGameObjectAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> TargetGameObject;
    [SerializeReference] public BlackboardVariable<bool> Bool;
    protected override Status OnStart()
    {

        if (TargetGameObject == null || TargetGameObject.Value == null)
        {
            Debug.LogWarning("TargetGameObject is not set.");
            return Status.Failure;
        }

        TargetGameObject.Value.SetActive(Bool.Value);
        return Status.Success;
    }
}

