using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    [Header("Attack")] public float attackForce;
    public float attackCooldown;
    public GameObject sword;

    [Header("Dash")] public float dashSpeed;
    public float dashLength = 0.5f;
    public float dashCooldown = 1f;
    private float _dashCounter;
    private float _dashCoolCounter;

    private float _nextAttackTime;
    private float _nextDashTime;
    private Rigidbody2D _rb;
    private PlayerMovement _playerMovement;
    private Vector3 _moveDirection;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerMovement = GetComponent<PlayerMovement>();
        // _nextDashTime = dashDuration;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time > _nextAttackTime)
        {
            Attack();
            _nextAttackTime = Time.time + attackCooldown;
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (_dashCoolCounter <= 0 && _dashCounter <= 0)
            {
                _playerMovement.EnableDashSpeed(dashSpeed);
                _dashCounter = dashLength;
            }
        }

        if (_dashCounter > 0)
        {
            _dashCounter -= Time.deltaTime;

            if (_dashCounter <= 0)
            {
                _playerMovement.DisableDashSpeed();
                _dashCoolCounter = dashCooldown;
            }
        }

        if (_dashCoolCounter > 0)
        {
            _dashCoolCounter -= Time.deltaTime;
        }
    }

    private void Attack()
    {
        sword.GetComponent<Animator>().Play("PlayerSwordAttack");
        sword.GetComponent<BoxCollider2D>().enabled = true;

        StartCoroutine(EndSwordAttack());
    }

    private IEnumerator EndSwordAttack()
    {
        yield return new WaitForSeconds(attackCooldown);
        sword.GetComponent<Animator>().Play("New State");
        sword.GetComponent<BoxCollider2D>().enabled = false;
    }
}