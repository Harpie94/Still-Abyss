using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "GetDoorState", story: "Update [doorBool] using [DoorScript]", category: "Action", id: "804654ae5a732e5cb916c71f687b524c")]
public partial class GetDoorStateAction : Action
{
    [SerializeReference] public BlackboardVariable<bool> DoorBool;
    [SerializeReference] public BlackboardVariable<DoorScript> DoorScript;

    protected override Status OnStart()
    {

        if (DoorScript.Value == null)
        {
            Debug.LogError("No DoorScript assigned to the action, please assign one in the inspector.");
            return Status.Failure;
        }
        DoorBool.Value = DoorScript.Value.isOpen;
        return Status.Success;
    }
}

