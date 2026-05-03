using UnityEngine;

public class Bullet : MonoBehaviour
{

    //Stats
    public float speed = 10f;
    public float damage = 10f;
    public float lifeTime = 2f;

    private Vector3 startPosition;

    public GameObject owner;

    //Salida
    void Start()
    {
        startPosition = transform.position;
        Destroy(gameObject, lifeTime);
    }
    //Destruccion de la bala
    void Update()
    {
        float distance = Vector3.Distance(startPosition, transform.position);
        if (distance > 200f)
        {
            Destroy(gameObject);
        }
    }
    //Choque
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) 
            return;

        ShipShield shield = other.GetComponent<ShipShield>();
        if (shield != null)
        {
            if (shield.isActive)
            {
                
                shield.TakeDamage(damage);
                Destroy(gameObject);
                return;
            }
        }

        ShipHealth health = other.GetComponent<ShipHealth>();
        if (health != null)
        {
            health.TakeDamage(damage);
            Destroy(gameObject);
        }

        //No hacerse dano
        if (other.transform.root.gameObject == owner)
            return;
    }         
    
}