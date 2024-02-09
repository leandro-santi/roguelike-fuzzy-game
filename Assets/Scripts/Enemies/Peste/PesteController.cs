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

    private Transform _player;
    private Vector2 _randomDirection;

    private float _idleTimer;
    private float _walkTimer;
    private float _timeSinceLastAttack;
    private float _emotionTimer;
    private bool _retreat;

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

        _retreat = false;

        _enemyHealth = GetComponent<EnemyHealth>();
        _fuzzy = GetComponent<FuzzyController>();

        _fearFunctionList.Add(Retreat);
        
        _braveFunctionList.Add(CadenceMin);
        
        _angryFunctionList.Add(CadenceMax);
        _angryFunctionList.Add(Dodge);
    }

    void Update()
    {
        if (_enemyHealth.died) return;

        if (_retreat)
        {
            Chase();
            return;
        }

        _emotionTimer += Time.deltaTime;

        if (_emotionTimer >= 3f && (_currentState == EnemyState.Chase || _currentState == EnemyState.Attack))
        {
            switch (_fuzzy.emotion)
            {
                case "Calm":
                    StartCoroutine(_fuzzy.StopEmotion());
                    _emotionTimer = 0f;
                    break;
                case "Fear":
                    _fearFunctionList[0].Invoke();
                    StartCoroutine(_fuzzy.StopEmotion());
                    _emotionTimer = 0f;
                    break;
                case "Brave":
                    _braveFunctionList[0].Invoke();
                    StartCoroutine(_fuzzy.StopEmotion());
                    _emotionTimer = 0f;
                    break;
                case "Angry":
                    _angryFunctionList[Random.Range(0, 2)].Invoke();
                    StartCoroutine(_fuzzy.StopEmotion());
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

        if (_retreat)
        {
            direction *= -1;
        }

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
        StartCoroutine(Aim(0.5f));
    }

    IEnumerator Aim(float time)
    {
        Transform projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        yield return new WaitForSeconds(time);

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

    private void Retreat()
    {
        StartCoroutine(EnableRetreat());
    }

    IEnumerator EnableRetreat()
    {
        _retreat = true;
        chaseSpeed = 5f;
        yield return new WaitForSeconds(3f);
        chaseSpeed = 2f;
        _retreat = false;
    }

    #endregion

    // Courage

    #region Courage

    private void CadenceMin()
    {
        StartCoroutine(MinCadence());
    }

    IEnumerator MinCadence()
    {
        attackCooldown = 0.5f;
        yield return new WaitForSeconds(3f);
        attackCooldown = 1f;
    }

    #endregion

    // Angry

    #region Angry
    
    void Dodge()
    {
        Vector2 dodgePosition = (Vector2)transform.position + Random.insideUnitCircle.normalized * 1f;

        gameObject.transform.position = dodgePosition;
    }

    private void CadenceMax()
    {
        StartCoroutine(MaxCadence());
    }

    IEnumerator MaxCadence()
    {
        attackCooldown = 0.25f;
        yield return new WaitForSeconds(3f);
        attackCooldown = 1f;
    }

    #endregion

    #endregion
}