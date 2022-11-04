using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(SphereCollider))]
public class EnemyMovement : MonoBehaviour
{
    // radius of range when enemy would "see" player and start follow
    [SerializeField] private float visionRadius = 10.0f;
    [SerializeField] private float moveSpeed = 2.0f;
    [SerializeField] private float rotationSpeed = 100.0f;

    private NavMeshAgent _meshAgent;
    private Vector3 _startPosition;
    private GameObject _player;
    private SphereCollider _sphereCollider;

    private void Start()
    {
        _meshAgent = GetComponent<NavMeshAgent>();
        _meshAgent.speed = moveSpeed;
        _meshAgent.angularSpeed = rotationSpeed;
        _startPosition = transform.position;
        //_player = GameObject.Find("Player");
        _sphereCollider = GetComponent<SphereCollider>();
        _sphereCollider.radius = visionRadius;
        _sphereCollider.isTrigger = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out PlayerMotor playerMotor))
        {
            _player = other.gameObject;
            StartChasingPlayer();
        }
    }

    private void StartChasingPlayer()
    {
        _meshAgent.SetDestination(_player.transform.position);
    }
}
