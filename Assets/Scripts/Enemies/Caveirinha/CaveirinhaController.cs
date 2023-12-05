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

    [Header("RangedAttack")] public Transform projectilePrefab;
    public float projectileSpeed = 5f;
    public float timeBetweenAttacks = 2f;

    private Transform _player;
    private Vector2 _randomDirection;
    private SpriteRenderer _sprite;

    // private float _nextAttackTime = 0f;
    private float _idleTimer;
    private float _walkTimer;
    private float _timeSinceLastAttack;

    private bool _isWalking;

    void Start()
    {
        _currentState = EnemyState.Idle;
        _player = GameObject.FindGameObjectWithTag("Player")
            .transform;
        _sprite = GetComponent<SpriteRenderer>();

        _idleTimer = 0f;
        _walkTimer = 0f;
        _timeSinceLastAttack = 0f;
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
        // Debug.Log("Walk");

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
        // Debug.Log("Chase");

        Vector2 direction = (_player.position - transform.position).normalized;

        transform.Translate(direction * (Time.deltaTime * chaseSpeed));

        if (_player.position.x > transform.position.x)
        {
            var localScale = transform.localScale;
            localScale = new Vector3(Mathf.Abs(localScale.x), localScale.y,
                localScale.z);
            transform.localScale = localScale;

            // _sprite.flipX = false;
        }
        else
        {
            var localScale = transform.localScale;
            localScale = new Vector3(-Mathf.Abs(localScale.x), localScale.y,
                localScale.z);
            transform.localScale = localScale;

            // _sprite.flipX = true;
        }

        if (Vector2.Distance(transform.position, _player.position) <= attackRange)
        {
            _currentState = EnemyState.Attack;
        }

        else if (Vector2.Distance(transform.position, _player.position) >= chaseRange)
        {
            _currentState = EnemyState.Walk;
        }
    }

    void Attack()
    {
        // Debug.Log("Attack");

        _timeSinceLastAttack += Time.deltaTime;

        if (_timeSinceLastAttack >= timeBetweenAttacks)
        {
            int randomAttack = Random.Range(0, 2);

            if (randomAttack == 0)
            {
                MeleeAttack();
            }

            else
            {
                RangedAttack();
            }

            _timeSinceLastAttack = 0f;
        }

        if (Vector2.Distance(transform.position, _player.position) >= attackRange)
        {
            _currentState = EnemyState.Chase;
        }
    }

    void MeleeAttack()
    {
        Debug.Log("Melee Attack!");
    }

    void RangedAttack()
    {
        Debug.Log("Ranged Attack!");

        LaunchProjectile();
    }

    void LaunchProjectile()
    {
        Transform projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        Vector2 direction = (_player.position - transform.position).normalized;

        projectile.GetComponent<Rigidbody2D>().velocity = direction * projectileSpeed;

        Destroy(projectile.gameObject, 2f);
    }

    private Vector2 ReturnRandomDirection()
    {
        return Random.insideUnitCircle.normalized;
    }
}