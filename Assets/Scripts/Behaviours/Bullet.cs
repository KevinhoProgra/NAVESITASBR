using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Stats
    public float speed = 20f; // Asegúrate de asignar velocidad si la mueves por script
    public float damage = 10f;
    public float lifeTime = 2f;

    private Vector3 startPosition;

    [HideInInspector] public GameObject owner; // Quién disparó la bala

    void Start()
    {
        startPosition = transform.position;
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // Control de distancia máxima
        float distance = Vector3.Distance(startPosition, transform.position);
        if (distance > 200f)
        {
            Destroy(gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        // 1. FILTRO DE SEGURIDAD PRINCIPAL: ¿Es el dueño directo o la raíz de la jerarquía?
        if (owner != null)
        {
            if (other.gameObject == owner || other.transform.root.gameObject == owner)
            {
                return; // Ignorar por completo si es nuestra propia nave
            }
        }

        // 2. FILTRO DE SEGURIDAD AVANZADO: Comprobar componentes para evitar fuego amigo / daño propio

        // Manejo de Escudo
        ShipShield shield = other.GetComponent<ShipShield>();
        if (shield != null)
        {
            // Si el escudo encontrado pertenece a la misma nave que disparó, lo ignoramos
            if (shield.transform.root.gameObject == owner)
            {
                return;
            }

            if (shield.isActive)
            {
                shield.TakeDamage(damage);
                Destroy(gameObject);
                return;
            }
        }

        // Manejo de Vida
        ShipHealth health = other.GetComponent<ShipHealth>();
        if (health != null)
        {
            // Si la vida encontrada pertenece a la misma nave que disparó, lo ignoramos
            if (health.transform.root.gameObject == owner)
            {
                return;
            }

            health.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        // 3. Impacto con obstáculos (Paredes, asteroides, etc.) que no sean jugadores
        if (!other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}