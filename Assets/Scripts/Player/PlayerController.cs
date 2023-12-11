using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Attack")] public float attackForce;
    public float attackCooldown;
    public bool isAttacking;
    public Transform projectilePrefab;

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

        var enemy = GameObject.FindGameObjectWithTag("Enemy")
            .transform;

        Transform projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        Vector2 direction = (enemy.position - transform.position).normalized;

        projectile.GetComponent<Rigidbody2D>().velocity = direction * 10f;

        Destroy(projectile.gameObject, 2f);
    }

    private void Dash()
    {
        isDashing = true;

        _moveDirection = _playerMovement.ReturnPlayerDirection();

        Vector3 dashPosition = transform.position + _moveDirection * dashDistance;
        _rb.MovePosition(dashPosition);
    }
}