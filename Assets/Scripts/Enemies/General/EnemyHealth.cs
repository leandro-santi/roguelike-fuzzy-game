using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth;
    public bool died;
    public int currentHealth;
    public bool shieldIsOn;
    private bool _canTakeDamage;
    private float _damageDelay;

    public float knockForce;
    public float knockDuration;
    private bool _isKnockedBack;
    private float _knockTimer;

    private Rigidbody2D _rb;
    private SpriteRenderer _sprite;

    void Start()
    {
        currentHealth = maxHealth;

        _isKnockedBack = false;
        died = false;
        shieldIsOn = false;

        _rb = GetComponent<Rigidbody2D>();
        _sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
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

        _damageDelay += Time.deltaTime;

        if (_damageDelay >= 1f)
        {
            _canTakeDamage = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (shieldIsOn) return;
        
        if (other.CompareTag("PlayerSword") && _canTakeDamage)
        {
            TakeDamage(1);
            _canTakeDamage = false;
            _damageDelay = 0f;
            ApplyKnock(other.transform.position);
        }
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
            FindObjectOfType<PlayerController>().CountKill();
            return;
        }

        StartCoroutine(DamageFeedback());
    }

    void ApplyKnock(Vector3 swordPosition)
    {
        if (_rb != null && !_isKnockedBack)
        {
            Vector2 knockDirection = (transform.position - swordPosition).normalized;
            _rb.AddForce(knockDirection * knockForce, ForceMode2D.Impulse);
            _isKnockedBack = true;
            _knockTimer = knockDuration;
        }
    }

    void Die()
    {
        Debug.Log("Enemy defeated!");

        _sprite.color = Color.red;
        died = true;
        Destroy(gameObject, 1f);
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