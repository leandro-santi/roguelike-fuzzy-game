using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth;

    private Rigidbody2D _rb;
    private SpriteRenderer _sprite;

    public float knockForce;
    public float knockDuration;
    private bool _isKnockedBack;
    private float _knockTimer;

    void Start()
    {
        currentHealth = maxHealth;
        
        _isKnockedBack = false;

        _sprite = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (_isKnockedBack)
        {
            _knockTimer -= Time.deltaTime;

            if (_knockTimer <= 0f)
            {
                _isKnockedBack = false;
                _rb.velocity = Vector2.zero;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EnemyProjectile"))
        {
            TakeDamage(1);
            Destroy(other.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("EnemyMelee"))
        {
            TakeDamage(1);
            ApplyKnock(other.transform.position);
        }
    }

    void ApplyKnock(Vector3 contactPosition)
    {
        if (_rb != null && !_isKnockedBack)
        {
            Vector2 knockDirection = (transform.position - contactPosition).normalized;
            _rb.AddForce(knockDirection * knockForce, ForceMode2D.Impulse);
            _isKnockedBack = true;
            _knockTimer = knockDuration;
        }
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }

        StartCoroutine(DamageFeedback());
    }

    void Die()
    {
        Debug.Log("Player defeated!");

        _sprite.color = Color.red;
    }

    IEnumerator DamageFeedback()
    {
        _sprite.color = new Color(255f, 255f, 255f, 0f);
        yield return new WaitForSeconds(0.1f);
        _sprite.color = new Color(255f, 255f, 255f, 255f);
        yield return new WaitForSeconds(0.1f);
        _sprite.color = new Color(255f, 255f, 255f, 0f);
        yield return new WaitForSeconds(0.1f);
        _sprite.color = new Color(255f, 255f, 255f, 255f);
    }
}