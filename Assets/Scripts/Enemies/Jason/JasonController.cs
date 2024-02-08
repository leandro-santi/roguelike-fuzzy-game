using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class JasonController : MonoBehaviour
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
    public Transform bombPrefab;
    public float bombSpeed;

    [Header("Defense")] public GameObject shield;

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
        _emotionTimer = 5f;

        _enemyHealth = GetComponent<EnemyHealth>();
        _fuzzy = GetComponent<FuzzyController>();
        
        _fearFunctionList.Add(Defense);
        _fearFunctionList.Add(Dodge);
        _fearFunctionList.Add(Scream);
        
        _braveFunctionList.Add(Explosion);
        _braveFunctionList.Add(Bomb);
        _braveFunctionList.Add(ApplyStun);
        
        _angryFunctionList.Add(AngryMode);
    }

    void Update()
    {
        // Debug.Log(_fuzzy.emotion);

        if (_enemyHealth.died) return;

        if (shield.activeSelf)
        {
            Chase();
            return;
        }

        _emotionTimer += Time.deltaTime;

        if (_emotionTimer >= 10f)
        {
            switch (_fuzzy.emotion)
            {
                case "Calm":
                    break;
                case "Fear":
                    _fearFunctionList[Random.Range(0, 3)].Invoke();
                    StartCoroutine(_fuzzy.StopEmotion());
                    _emotionTimer = 0f;
                    break;
                case "Brave":
                    _braveFunctionList[Random.Range(0, 3)].Invoke();
                    StartCoroutine(_fuzzy.StopEmotion());
                    _emotionTimer = 0f;
                    break;
                case "Angry":
                    _angryFunctionList[0].Invoke();
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
            MeleeAttack();
            _timeSinceLastAttack = 0f;
        }

        if (Vector2.Distance(transform.position, _player.position) >= attackRange)
        {
            _currentState = EnemyState.Chase;
        }
    }

    void MeleeAttack()
    {
        StartCoroutine(Advance());
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

    void Defense()
    {
        StartCoroutine(Shield());
    }

    void Dodge()
    {
        Vector2 dodgePosition = (Vector2)transform.position + Random.insideUnitCircle.normalized * 1f;

        gameObject.transform.position = dodgePosition;
    }

    void Scream()
    {
        FindObjectOfType<EnemySpawner>().ScreamSpawnEnemies(2);
    }

    IEnumerator Shield()
    {
        _enemyHealth.shieldIsOn = true;
        shield.SetActive(true);
        yield return new WaitForSeconds(5f);
        shield.SetActive(false);
        _enemyHealth.shieldIsOn = false;
    }

    #endregion

    // Courage

    #region Courage

    void Explosion()
    {
        StartCoroutine(IncreaseSpeed());
    }

    void Bomb()
    {
        Transform projectile = Instantiate(bombPrefab, transform.position, Quaternion.identity);

        Vector2 direction = (_player.position - transform.position).normalized;

        projectile.GetComponent<Rigidbody2D>().velocity = direction * bombSpeed;

        Destroy(projectile.gameObject, 2f);
    }

    void ApplyStun()
    {
        StartCoroutine(Stun());
    }

    IEnumerator Stun()
    {
        var o = gameObject;
        o.transform.position = _player.position;
        o.tag = "EnemyStun";
        o.layer = 9;
        yield return new WaitForSeconds(0.5f);
        o.layer = 7;
        o.tag = "Enemy";
    }

    IEnumerator IncreaseSpeed()
    {
        chaseSpeed = explosionSpeed;
        yield return new WaitForSeconds(5f);
        chaseSpeed = 2f;
    }

    #endregion

    // Angry

    #region Angry

    void AngryMode()
    {
        StartCoroutine(EnableAngryMode());
    }

    IEnumerator EnableAngryMode()
    {
        attackCooldown = 0.5f;
        attackRange = 4f;
        yield return new WaitForSeconds(5f);
        attackCooldown = 1f;
        attackRange = 2f;
    }

    #endregion

    #endregion
}