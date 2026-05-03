using UnityEngine;

public class ShipHealth : MonoBehaviour
{

    public ShipData shipData;
    public float health;


    void Start()
    {
        health = shipData.maxHealth;
    }
    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}