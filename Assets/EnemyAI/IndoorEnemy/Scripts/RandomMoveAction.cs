using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Random Move", story: "Move [Self] towards random point in [List]", category: "Action", id: "31037e153482629be6fd52ac35fd65cc")]
public partial class RandomMoveAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<List<GameObject>> List;
    [SerializeReference] public BlackboardVariable<float> Speed;
    [SerializeReference] public BlackboardVariable<float> DistanceThreshold;

    private NavMeshAgent _agent;
    private GameObject _target;
    private bool _destinationSet;

    protected override Status OnStart()
    {
        _destinationSet = false;

        var selfObj = Self?.Value;
        var list = List?.Value;
        var speed = Speed != null ? Speed.Value : 3.5f; // Valeur par défaut
        var threshold = DistanceThreshold != null ? DistanceThreshold.Value : 0.5f; // Valeur par défaut

        if (selfObj == null || list == null || list.Count == 0)
            return Status.Failure;

        _agent = selfObj.GetComponent<NavMeshAgent>();
        if (_agent == null)
            return Status.Failure;

        _agent.speed = speed;
        _agent.stoppingDistance = threshold;

        // Choisir un objet aléatoire dans la liste
        int idx = UnityEngine.Random.Range(0, list.Count);
        _target = list[idx];

        if (_target == null)
            return Status.Failure;

        _agent.SetDestination(_target.transform.position);
        _destinationSet = true;

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (!_destinationSet || _agent == null || _target == null)
            return Status.Failure;

        // Vérifier si l'agent est proche de la destination
        if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
        {
            if (!_agent.hasPath || _agent.velocity.sqrMagnitude == 0f)
                return Status.Success;
        }

        return Status.Running;
    }

    protected override void OnEnd()
    {
        if (_agent != null)
            _agent.ResetPath();
    }
}


