using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PesteController : MonoBehaviour
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
    public float explosionSpeed;

    [Header("Attack")] public float attackRange;
    public float attackCooldown;
    public Transform projectilePrefab;
    public float projectileSpeed;

    [Header("Defense")] public GameObject shield;
    public GameObject screamFeedback;

    private Transform _player;
    private Vector2 _randomDirection;

    private float _idleTimer;
    private float _walkTimer;
    private float _timeSinceLastAttack;
    private float _emotionTimer;

    private EnemyHealth _enemyHealth;
    private FuzzyController _fuzzy;

    // Fuzzy Functions List
    private readonly List<Action> _fearFunctionList = new List<Action>();
    private readonly List<Action> _braveFunctionList = new List<Action>();
    private readonly List<Action> _angryFunctionList = new List<Action>();

    void Start()
    {
        _currentState = EnemyState.Idle;
        _player = GameObject.FindGameObjectWithTag("Player").transform;

        _idleTimer = 0f;
        _walkTimer = 0f;
        _timeSinceLastAttack = 0f;
        _emotionTimer = 0f;

        _enemyHealth = GetComponent<EnemyHealth>();
        _fuzzy = GetComponent<FuzzyController>();

        // _fearFunctionList.Add(Defense);
        // _fearFunctionList.Add(Dodge);
        // _fearFunctionList.Add(Scream);
        //
        // _braveFunctionList.Add(Explosion);
        // _braveFunctionList.Add(Bomb);
        // _braveFunctionList.Add(ApplyStun);
        //
        // _angryFunctionList.Add(AngryMode);
    }

    void Update()
    {
        if (_enemyHealth.died) return;

        if (shield.activeSelf)
        {
            Chase();
            return;
        }

        _emotionTimer += Time.deltaTime;

        if (_emotionTimer >= 3f)
        {
            switch (_fuzzy.emotion)
            {
                case "Calm":
                    _emotionTimer = 0f;
                    break;
                case "Fear":
                    // _fearFunctionList[Random.Range(0, 3)].Invoke();
                    // StartCoroutine(_fuzzy.StopEmotion());
                    _emotionTimer = 0f;
                    break;
                case "Brave":
                    // _braveFunctionList[Random.Range(0, 3)].Invoke();
                    // StartCoroutine(_fuzzy.StopEmotion());
                    _emotionTimer = 0f;
                    break;
                case "Angry":
                    // _angryFunctionList[0].Invoke();
                    // StartCoroutine(_fuzzy.StopEmotion());
                    _emotionTimer = 0f;
                    break;
            }
        }

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
        Vector2 direction = (_player.position - transform.position).normalized;

        if (shield.activeSelf) direction *= -1;

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
        _timeSinceLastAttack += Time.deltaTime;

        if (_timeSinceLastAttack >= attackCooldown)
        {
            RangedAttack();
            _timeSinceLastAttack = 0f;
        }

        if (Vector2.Distance(transform.position, _player.position) >= attackRange)
        {
            _currentState = EnemyState.Chase;
        }
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

    private Vector2 ReturnRandomDirection()
    {
        return Random.insideUnitCircle.normalized;
    }

    private void FlipCharacter(Vector3 targetPosition)
    {
        if (targetPosition.x > transform.position.x)
        {
            var localScale = transform.localScale;
            localScale = new Vector3(Mathf.Abs(localScale.x), localScale.y, localScale.z);
            transform.localScale = localScale;
        }
        else
        {
            var localScale = transform.localScale;
            localScale = new Vector3(-Mathf.Abs(localScale.x), localScale.y, localScale.z);
            transform.localScale = localScale;
        }
    }

    #region FuzzyEvents

    // Fear

    #region Fear

    // alo

    #endregion

    // Courage

    #region Courage

    // alo

    #endregion

    // Angry

    #region Angry

    // alo

    #endregion

    #endregion
}