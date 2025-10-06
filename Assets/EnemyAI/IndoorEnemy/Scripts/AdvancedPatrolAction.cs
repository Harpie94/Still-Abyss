using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Advanced Patrol", story: "Move [Agent] along random [Waypoints] every [x] to [y] seconds", category: "Action", id: "c8b28a16e6baa1d8ff8478738aa43167")]
public partial class AdvancedPatrolAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<List<GameObject>> Waypoints;
    [SerializeReference] public BlackboardVariable<float> X = new(1f);
    [SerializeReference] public BlackboardVariable<float> Y = new(3f);
    [SerializeReference] public BlackboardVariable<float> Speed = new(3f);
    [SerializeReference] public BlackboardVariable<float> DistanceThreshold = new(0.2f);
    [SerializeReference] public BlackboardVariable<string> AnimatorSpeedParam = new("SpeedMagnitude");
    [Tooltip("Should patrol restart from the latest point?")]
    [SerializeReference] public BlackboardVariable<bool> PreserveLatestPatrolPoint = new(false);

    private NavMeshAgent _navAgent;
    private int _lastWaypointIndex = -1;
    private int _currentWaypointIndex = -1;
    private float _waitTimer = 0f;
    private float _nextWaitTime = 0f;
    private bool _waiting = false;

    protected override Status OnStart()
    {
        if (Agent.Value == null || Waypoints.Value == null || Waypoints.Value.Count == 0)
            return Status.Failure;

        _navAgent = Agent.Value.GetComponent<NavMeshAgent>();
        if (_navAgent == null)
            return Status.Failure;

        _navAgent.speed = Speed.Value;
        _waiting = true;
        _waitTimer = 0f;
        _nextWaitTime = UnityEngine.Random.Range(X.Value, Y.Value);
        _currentWaypointIndex = GetNextWaypointIndex();
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (_navAgent == null || Waypoints.Value == null || Waypoints.Value.Count == 0)
            return Status.Failure;

        if (_waiting)
        {
            _waitTimer += Time.deltaTime;
            if (_waitTimer >= _nextWaitTime)
            {
                _waiting = false;
                _navAgent.SetDestination(Waypoints.Value[_currentWaypointIndex].transform.position);
            }
            return Status.Running;
        }

        if (!_navAgent.pathPending && _navAgent.remainingDistance <= DistanceThreshold.Value)
        {
            _lastWaypointIndex = _currentWaypointIndex;
            _currentWaypointIndex = GetNextWaypointIndex();
            _waiting = true;
            _waitTimer = 0f;
            _nextWaitTime = UnityEngine.Random.Range(X.Value, Y.Value);
        }

        // Optionally update animator speed parameter
        if (!string.IsNullOrEmpty(AnimatorSpeedParam.Value))
        {
            var animator = Agent.Value.GetComponent<Animator>();
            if (animator != null)
                animator.SetFloat(AnimatorSpeedParam.Value, _navAgent.velocity.magnitude);
        }

        return Status.Running;
    }

    protected override void OnEnd()
    {
        if (_navAgent != null)
            _navAgent.ResetPath();
    }

    private int GetNextWaypointIndex()
    {
        int count = Waypoints.Value.Count;
        if (count <= 1)
            return 0;

        if (count == 2)
            return (_lastWaypointIndex == 0) ? 1 : 0;

        int nextIndex;
        do
        {
            nextIndex = UnityEngine.Random.Range(0, count);
        } while (nextIndex == _lastWaypointIndex);

        return nextIndex;
    }
}

