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

    [Header("Walk")] public float walkSpeed = 2f;
    public float chaseSpeed = 5f;

    [Header("Attack")] public float attackRange = 2f;
    public float attackCooldown = 2f;

    private Transform _player;
    private float _nextAttackTime = 0f;

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

        if (Time.time > _nextAttackTime)
        {
            _currentState = EnemyState.Walk;
        }
    }

    void Walk()
    {
        // Debug.Log("Walk");

        transform.Translate(Vector2.right * (walkSpeed * Time.deltaTime));

        if (Vector2.Distance(transform.position, _player.position) < attackRange)
        {
            _currentState = EnemyState.Chase;
        }
    }

    void Chase()
    {
        Debug.Log("Chase");

        transform.right = _player.position - transform.position;

        // transform.Translate(Vector2.right * (chaseSpeed * Time.deltaTime));

        if (Vector2.Distance(transform.position, _player.position) < attackRange)
        {
            _currentState = EnemyState.Attack;
        }

        else if (Vector2.Distance(transform.position, _player.position) > attackRange * 2)
        {
            _currentState = EnemyState.Walk;
        }
    }

    void Attack()
    {
        Debug.Log("Attack");

        int randomAttack = Random.Range(0, 2);

        if (randomAttack == 0)
        {
            MeleeAttack();
        }

        else
        {
            RangedAttack();
        }

        _currentState = EnemyState.Idle;
        _nextAttackTime = Time.time + attackCooldown;
    }

    void MeleeAttack()
    {
        Debug.Log("Ataque corpo a corpo!");
    }

    void RangedAttack()
    {
        Debug.Log("Ataque à distância!");
    }
}