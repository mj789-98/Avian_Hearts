using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int maxHits = 4;
    public GameObject deathEffect;

    private int currentHits;

    private void Start()
    {
        currentHits = 0;
    }

    public void TakeHit()
    {
        currentHits++;

        if (currentHits >= maxHits)
        {
            Die();
        }
    }

    private void Die()
    {
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }

        // Destroy the enemy object
        Destroy(gameObject);
    }
}
