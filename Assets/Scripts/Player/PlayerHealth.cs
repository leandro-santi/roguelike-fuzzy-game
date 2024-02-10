using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;
    private float _lifeRechargerTimer;
    public float lifeRechargerInterval;
    public GameObject[] hearts;
    public TextMeshProUGUI stunFeedback;

    private Rigidbody2D _rb;
    private SpriteRenderer _sprite;
    private PlayerMovement _playerMovement;

    public float knockForce;
    public float knockDuration;
    private bool _isKnockedBack;
    private float _knockTimer;
    private bool _isDead;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHearts();

        _isKnockedBack = false;
        _isDead = false;

        _sprite = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
        _playerMovement = GetComponent<PlayerMovement>();
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

        LifeRecharge();
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

        if (other.gameObject.CompareTag("EnemyStun"))
        {
            TakeDamage(1);
            StartCoroutine(ApplyStun());
        }
    }

    void ApplyKnock(Vector3 contactPosition)
    {
        if (_isDead) return;

        if (_rb != null && !_isKnockedBack)
        {
            Vector2 knockDirection = (transform.position - contactPosition).normalized;
            _rb.AddForce(knockDirection * knockForce, ForceMode2D.Impulse);
            _isKnockedBack = true;
            _knockTimer = knockDuration;
        }
    }

    IEnumerator ApplyStun()
    {
        _playerMovement.ApplyStun(true);
        stunFeedback.enabled = true;
        yield return new WaitForSeconds(2f);
        stunFeedback.enabled = false;
        _playerMovement.ApplyStun(false);
    }

    void UpdateHearts()
    {
        foreach (var heart in hearts)
        {
            heart.SetActive(false);
        }

        for (var heart = 0; heart < currentHealth; heart++)
        {
            hearts[heart].SetActive(true);
        }
    }

    void LifeRecharge()
    {
        if ((currentHealth == maxHealth) || _isDead) return;

        _lifeRechargerTimer += Time.deltaTime;

        if (_lifeRechargerTimer >= lifeRechargerInterval)
        {
            _lifeRechargerTimer = 0f;
            currentHealth += 1;
            UpdateHearts();
        }
    }

    void TakeDamage(int damage)
    {
        if (_isDead) return;

        currentHealth -= damage;

        UpdateHearts();

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
        _isDead = true;
        GameController.Instance.FinishGame();
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