using System.Collections;
using UnityEditor.IMGUI.Controls;
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
    public float projectileSpeed;
    public float timeBetweenAttacks;

    private Transform _player;
    private Vector2 _randomDirection;

    private float _nextAttackTime;
    private float _idleTimer;
    private float _walkTimer;
    private float _timeSinceLastAttack;
    private bool _isWalking;

    private EnemyHealth _enemyHealth;

    void Start()
    {
        _currentState = EnemyState.Idle;
        _player = GameObject.FindGameObjectWithTag("Player")
            .transform;

        _idleTimer = 0f;
        _walkTimer = 0f;
        _timeSinceLastAttack = 0f;

        _enemyHealth = GetComponent<EnemyHealth>();
    }

    void Update()
    {
        if (_enemyHealth.died) return;

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
                FlipCharacter(_randomDirection * new Vector2(10f, 10f));
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

        FlipCharacter(_player.position);

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
            int randomAttack = Random.Range(1, 11);

            if (randomAttack <= 2)
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
        // Debug.Log("Melee Attack!");

        StartCoroutine(Advance());
    }

    void RangedAttack()
    {
        // Debug.Log("Ranged Attack!");

        LaunchProjectile();
    }

    void LaunchProjectile()
    {
        Transform projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        Vector2 direction = (_player.position - transform.position).normalized;

        projectile.GetComponent<Rigidbody2D>().velocity = direction * projectileSpeed;

        Destroy(projectile.gameObject, 2f);
    }

    IEnumerator Advance()
    {
        gameObject.transform.position = _player.position;
        gameObject.tag = "EnemyMelee";
        gameObject.layer = 9;
        yield return new WaitForSeconds(0.5f);
        gameObject.layer = 7;
        gameObject.tag = "Enemy";
    }

    private Vector2 ReturnRandomDirection()
    {
        return Random.insideUnitCircle.normalized;
    }

    private void FlipCharacter(Vector3 targetPosition)
    {
        if (targetPosition.x > transform.position.x)
        {
            var localScale = transform.localScale;
            localScale = new Vector3(Mathf.Abs(localScale.x), localScale.y,
                localScale.z);
            transform.localScale = localScale;
        }
        else
        {
            var localScale = transform.localScale;
            localScale = new Vector3(-Mathf.Abs(localScale.x), localScale.y,
                localScale.z);
            transform.localScale = localScale;
        }
    }
}