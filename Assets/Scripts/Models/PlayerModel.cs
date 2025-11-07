using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[Serializable]
public class PlayerModel
{
    public Dictionary<string, int> Resources { get; private set; } = new();

    // --- Movement & state ---
    public Vector3 TargetPosition { get; private set; }
    public bool IsMoving { get; private set; }
    public bool IsCollecting { get; private set; }

    private NavMeshAgent _agent;

    public void Initialize(NavMeshAgent agent, Vector3 startPos)
    {
        _agent = agent ?? throw new ArgumentNullException(nameof(agent));
        _agent.Warp(startPos);
        TargetPosition = startPos;
        IsMoving = false;
        IsCollecting = false;
    }

    public void MoveTo(Vector3 point)
    {
        if (_agent == null || !_agent.isOnNavMesh) return;

        TargetPosition = point;
        IsCollecting = false;
        IsMoving = true;

        _agent.isStopped = false;
        _agent.SetDestination(point);
    }

    public void StopAndCollect()
    {
        if (_agent == null || !_agent.isOnNavMesh) return;

        _agent.isStopped = true;
        IsMoving = false;
        IsCollecting = true;
    }

    public void StopMoving()
    {
        if (_agent == null || !_agent.isOnNavMesh) return;

        _agent.isStopped = true;
        IsMoving = false;
    }

    public bool HasReached(Vector3 target, float threshold = 0.5f)
    {
        if (_agent == null || !_agent.isOnNavMesh) return false;
        return !_agent.pathPending && _agent.remainingDistance <= threshold;
    }

    public void Update()
    {
        if (_agent == null) return;
        IsMoving = _agent.velocity.magnitude > 0.1f && !_agent.pathPending;
    }

    public bool TryAddResource(string name, int amount, int maxPerType)
    {
        if (!Resources.ContainsKey(name))
            Resources[name] = 0;

        if (Resources[name] >= maxPerType) return false;

        Resources[name] = Mathf.Min(Resources[name] + amount, maxPerType);
        return true;
    }

    public void SetPosition(Vector3 pos)
    {
        if (_agent != null && _agent.isOnNavMesh)
            _agent.Warp(pos);

        TargetPosition = pos;
        IsMoving = false;
        IsCollecting = false;
    }
}
