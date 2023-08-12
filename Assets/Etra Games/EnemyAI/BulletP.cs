using UnityEngine;

public class BulletP : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Get the EnemyController component of the collided object
            EnemyController enemyController = other.GetComponent<EnemyController>();

            // If the collided object has an EnemyController, apply a hit
            if (enemyController != null)
            {
                enemyController.TakeHit();
            }

            // Destroy the bullet
            Destroy(gameObject);
        }
    }
}
