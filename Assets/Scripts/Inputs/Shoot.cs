using UnityEngine;
using UnityEngine.InputSystem; // <- Librería obligatoria para el nuevo sistema

public class Shoot : MonoBehaviour
{
    public GameObject BulletPrefab;
    public Transform FirePoint;
    public ShipData shipData;
    public Animator animator;

    private float nextFire;

    // Controles generados por Unity
    private Controls inputActions;

    // Booleano para saber si el jugador mantiene presionada la tecla de disparo
    private bool isShooting;

    private void Awake()
    {
        // Inicializamos el mapa de acciones de fábrica
        inputActions = new Controls();
    }

    private void OnEnable()
    {
        inputActions.Enable();

        // CORRECCIÓN: Usamos .performed en lugar de .started para evitar el doble disparo por clic
        inputActions.Player.Shoot.performed += OnShootStarted;

        // canceled: Se ejecuta en el instante en que SUELTAN la tecla
        inputActions.Player.Shoot.canceled += OnShootCanceled;
    }

    private void OnDisable()
    {
        inputActions.Disable();
        inputActions.Player.Shoot.performed -= OnShootStarted;
        inputActions.Player.Shoot.canceled -= OnShootCanceled;
    }

    private void OnShootStarted(InputAction.CallbackContext context)
    {
        isShooting = true;
    }

    private void OnShootCanceled(InputAction.CallbackContext context)
    {
        isShooting = false;
    }

    void Update()
    {
        // Si el jugador mantiene el botón Y ya pasó el tiempo de cooldown
        if (isShooting && Time.time >= nextFire)
        {
            ShootBullet();

            // Controla el cooldown del disparo
            nextFire = Time.time + shipData.fireRate;
        }
    }

    private void ShootBullet()
    {
        // SOLO reproduce la animación
        // La bala se crea desde un Animation Event
        animator.SetTrigger(AnimatorParams.Shoot);
    }

    // Esta función sigue siendo llamada desde tu Animation Event intacta
    public void FireProjectile()
    {
        GameObject bullet = Instantiate(
            BulletPrefab,
            FirePoint.position,
            FirePoint.rotation
        );

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            // Movimiento del proyectil (Usando linearVelocity de Unity 6)
            rb.linearVelocity = FirePoint.up * shipData.bulletSpeed;
        }

        Bullet bulletScript = bullet.GetComponent<Bullet>();

        if (bulletScript != null)
        {
            // Daño del proyectil
            bulletScript.damage = shipData.bulletDamage;

            // CORRECCIÓN ANTERIOR: Asigna la nave completa como dueña para evitar el auto-daño
            bulletScript.owner = transform.root.gameObject;
        }
    }
}