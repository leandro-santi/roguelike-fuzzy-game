using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Attack")] public float attackForce;
    public float attackCooldown;

    [Header("Dash")] public float dashDistance;
    public float dashDuration;
    public bool isDashing;

    private float _nextAttackTime;
    private float _nextDashTime;
    private CharacterController _controller;
    private PlayerMovement _playerMovement;
    private Vector3 _moveDirection;

    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _playerMovement = GetComponent<PlayerMovement>();
        _nextDashTime = dashDuration;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time > _nextAttackTime)
        {
            Attack();
            _nextAttackTime = Time.time + attackCooldown;
        }

        if (Input.GetMouseButtonDown(1) && !isDashing)
        {
            Dash();
        }

        if (isDashing)
        {
            dashDuration -= Time.deltaTime;

            if (dashDuration <= 0f)
            {
                isDashing = false;
                dashDuration = _nextDashTime;
            }
        }
    }

    private void Attack()
    {
        // Debug.Log("Atacando");
    }

    private void Dash()
    {
        // Debug.Log("Esquivando");

        isDashing = true;

        _moveDirection = _playerMovement.ReturnPlayerDirection();

        Vector3 dashPosition = transform.position + _moveDirection * dashDistance;
        _controller.Move(dashPosition - transform.position);
    }
}