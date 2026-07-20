using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
    public ShipData shipData;
    public Animator animator;

    // Controles generados por Unity
    private Controls inputActions;

    // Valores guardados de los ejes analógicos
    private Vector2 moveInput;
    private float rotationInput;

    // Dash
    private bool canDash = true;
    private bool isDashing;

    private void Awake()
    {
        inputActions = new Controls();
    }

    private void OnEnable()
    {
        inputActions.Enable();
        inputActions.Player.Dash.started += Context => IntentarDash();
    }

    private void OnDisable()
    {
        inputActions.Disable();
        inputActions.Player.Dash.started -= Context => IntentarDash();
    }

    private void Start()
    {
        rb.gravityScale = 0f;
        rb.linearDamping = 2f;
        rb.angularDamping = 5f;
    }

    void Update()
    {
        
        moveInput = inputActions.Player.Move.ReadValue<Vector2>();
        rotationInput = inputActions.Player.Rotate.ReadValue<float>();

        if (moveInput.y != 0)
        {
            moveInput.x = 0; // Bloqueo de strafe lateral si vas adelante/atrás
        }
    }

    void FixedUpdate()
    {
        if (isDashing)
        {
            
            rb.MoveRotation(rb.rotation + rotationInput * shipData.rotationSpeed * Time.fixedDeltaTime);
            return;
        }

        // --- MOVIMIENTO LINEAL ---
        float vSpeed = (moveInput.y < 0) ? shipData.speed * shipData.backwardMultiplier : shipData.speed;
        float hSpeed = shipData.speed * shipData.strafeMultiplier;
        Vector2 forwardMovement = transform.up * moveInput.y * vSpeed;
        Vector2 strafeMovement = transform.right * moveInput.x * hSpeed;

        rb.AddForce((forwardMovement + strafeMovement), ForceMode2D.Force);

        // Frenado inercial
        if (moveInput.x == 0 && moveInput.y == 0)
        {
            if (rb.linearVelocity.magnitude <= shipData.speed)
            {
                rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, Vector2.zero, 5f * Time.fixedDeltaTime);
            }
        }

        if (rb.linearVelocity.magnitude <= shipData.speed)
        {
            rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity, shipData.speed);
        }

     
        rb.angularVelocity = rotationInput * shipData.rotationSpeed;


        bool isMovingForward = moveInput.y > 0 && rb.linearVelocity.magnitude > 0.1f;
        animator.SetBool(AnimatorParams.IsMoving, isMovingForward);
    }

    private void IntentarDash()
    {
        if (canDash && !isDashing)
        {
            StartCoroutine(EjecutarDashImpulse());
        }
    }

    private IEnumerator EjecutarDashImpulse()
    {
        canDash = false;
        isDashing = true;

        // 1. Limpiamos velocidad vieja para que el impulso salga limpio en cualquier dirección
        rb.linearVelocity = Vector2.zero;

        // 2. Un único golpe seco de energía física (Impulse)
        rb.AddForce(transform.up * shipData.dashPower, ForceMode2D.Impulse);

        // 3. Duración del impulso (Ajusta esto en el ShipData, ej: 0.1s es ideal)
        yield return new WaitForSeconds(shipData.dashTime);

        // 4. Apagamos el estado de dash. El FixedUpdate retoma el control, 
        // pero gracias al nuevo filtro de velocidad, la nave se deslizará con inercia.
        isDashing = false;

        // 5. Cooldown antes de poder usarlo otra vez
        yield return new WaitForSeconds(shipData.dashCooldown);
        canDash = true;
    }
}