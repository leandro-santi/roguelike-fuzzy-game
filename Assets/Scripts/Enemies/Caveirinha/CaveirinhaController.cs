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

    [Header("Chase")] public float chaseSpeed = 5f;
    public float chaseRange;

    [Header("Attack")] public float attackRange = 2f;
    public float attackCooldown = 2f;

    private Transform _player;

    // private float _nextAttackTime = 0f;
    private Vector2 _randomDirection;
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

        if (_idleTimer >= 2.5f)
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
        
        // if (Vector2.Distance(transform.position, _player.position) < attackRange)
        // {
        //     _currentState = EnemyState.Chase;
        // }

        _walkTimer += Time.deltaTime;

        if (_walkTimer >= 2f)
        {
            _currentState = EnemyState.Idle;
            _isWalking = false;
            _walkTimer = 0f;
        }
    }

    void Chase()
    {
        // Debug.Log("Chase");
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

    private IEnumerator WalkingDelay()
    {
        yield return 3f;
        _isWalking = false;
    }
}