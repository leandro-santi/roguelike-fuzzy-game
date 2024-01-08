using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int _currentHealth;

    public float knockbackForce = 2f;
    public float knockbackDuration = 0.5f;
    private bool _isKnockedBack = false;
    private float _knockbackTimer;

    private Rigidbody2D _rigidbody;

    void Start()
    {
        _currentHealth = maxHealth;

        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (_isKnockedBack)
        {
            _knockbackTimer -= Time.deltaTime;

            if (_knockbackTimer <= 0f)
            {
                _isKnockedBack = false;
                _rigidbody.velocity = Vector2.zero;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerSword"))
        {
            TakeDamage(1);
            ApplyKnockback(other.transform.position);
        }
    }

    void TakeDamage(int damage)
    {
        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    void ApplyKnockback(Vector3 swordPosition)
    {
        if (_rigidbody != null && !_isKnockedBack)
        {
            Vector2 knockbackDirection = (transform.position - swordPosition).normalized;
            _rigidbody.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
            _isKnockedBack = true;
            _knockbackTimer = knockbackDuration;
        }
    }

    void Die()
    {
        Debug.Log("Enemy defeated!");
        Destroy(gameObject);
    }
}