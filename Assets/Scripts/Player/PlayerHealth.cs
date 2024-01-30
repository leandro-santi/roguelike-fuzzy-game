using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth;

    private SpriteRenderer _sprite;

    void Start()
    {
        currentHealth = maxHealth;

        _sprite = GetComponent<SpriteRenderer>();
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