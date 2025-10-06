using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Teleport", story: "Teleport [Self] to random [PatrolWaypoint]", category: "Action", id: "c742f214d5283dd4028d6f03ac086b5f")]
public partial class TeleportAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<List<GameObject>> PatrolWaypoint;

    private bool _teleported = false;

    protected override Status OnStart()
    {
        _teleported = false;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (_teleported)
            return Status.Success;

        var self = Self?.Value;
        var waypoints = PatrolWaypoint?.Value;

        if (self == null || waypoints == null || waypoints.Count == 0)
            return Status.Failure;

        int randomIndex = UnityEngine.Random.Range(0, waypoints.Count);
        var targetWaypoint = waypoints[randomIndex];

        if (targetWaypoint == null)
            return Status.Failure;

        self.transform.position = targetWaypoint.transform.position;
        _teleported = true;

        return Status.Success;
    }

    protected override void OnEnd()
    {
        _teleported = false;
    }
}

