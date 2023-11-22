using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveirinhaController : MonoBehaviour
{
    private enum EnemyState
    {
        Idle,
        Walk,
        Chase,
        Attack
    }

    private EnemyState _currentState;

    [Header("Walk")] public float walkSpeed;

    [Header("Chase")] public float chaseSpeed;
    public float chaseRange;

    [Header("Attack")] public float attackRange;
    public float attackCooldown;

    private Transform _player;
    private Vector2 _randomDirection;

    // private float _nextAttackTime = 0f;
    private float _idleTimer = 0f;
    private float _walkTimer = 0f;

    private bool _isWalking;

    void Start()
    {
        _currentState = EnemyState.Idle;
        _player = GameObject.FindGameObjectWithTag("Player")
            .transform;
    }

    void Update()
    {
        switch (_currentState)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Walk:
                Walk();
                break;
            case EnemyState.Chase:
                Chase();
                break;
            case EnemyState.Attack:
                Attack();
                break;
        }
    }

    void Idle()
    {
        // Debug.Log("Idle");

        _idleTimer += Time.deltaTime;

        if (_idleTimer >= 1f)
        {
            int randomMove = Random.Range(0, 3);

            if (randomMove >= 1)
            {
                _randomDirection = ReturnRandomDirection();
                _currentState = EnemyState.Walk;
            }

            _idleTimer = 0f;
        }
    }

    void Walk()
    {
        Debug.Log("Walk");

        transform.Translate(_randomDirection * (walkSpeed * Time.deltaTime));

        if (Vector2.Distance(transform.position, _player.position) < chaseRange)
        {
            _currentState = EnemyState.Chase;
        }

        _walkTimer += Time.deltaTime;

        if (_walkTimer >= 2f)
        {
            _currentState = EnemyState.Idle;
            _walkTimer = 0f;
        }
    }

    void Chase()
    {
        Debug.Log("Chase");

        Vector2 direction = (_player.position - transform.position).normalized;
        // transform.right = direction;

        transform.Translate(direction * (Time.deltaTime * chaseSpeed));

        if (Vector2.Distance(transform.position, _player.position) > chaseRange)
        {
            _currentState = EnemyState.Walk;
        }
    }

    void Attack()
    {
        // Debug.Log("Attack");

        // int randomAttack = Random.Range(0, 2);
        //
        // if (randomAttack == 0)
        // {
        //     MeleeAttack();
        // }
        //
        // else
        // {
        //     RangedAttack();
        // }
    }

    void MeleeAttack()
    {
        Debug.Log("Melee Attack!");
    }

    void RangedAttack()
    {
        Debug.Log("Ranged Attack!");
    }

    private Vector2 ReturnRandomDirection()
    {
        return Random.insideUnitCircle.normalized;
    }
}