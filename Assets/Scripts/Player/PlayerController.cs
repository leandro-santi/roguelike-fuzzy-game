using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Attack")] public float attackForce;
    public float attackCooldown;
    public GameObject sword;

    [Header("Dash")] public float dashDistance;
    public float dashDuration;
    public bool isDashing;

    private float _nextAttackTime;
    private float _nextDashTime;
    private Rigidbody2D _rb;
    private PlayerMovement _playerMovement;
    private Vector3 _moveDirection;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
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
        Debug.Log("Attacking");

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

    private void Dash()
    {
        isDashing = true;

        _moveDirection = _playerMovement.ReturnPlayerDirection();

        Vector3 dashPosition = transform.position + _moveDirection * dashDistance;
        _rb.MovePosition(dashPosition);
    }
}