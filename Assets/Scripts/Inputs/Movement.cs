using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
    public ShipData shipData;
    public Animator animator;   

    private float horizontal;
    private float vertical;



    // Dash
    private bool canDash = true;
   private bool isDashing;


    private void Start()
    {
        rb.gravityScale = 0f;
        rb.linearDamping = 2f;
        rb.angularDamping = 5f;
    }
    void Update()
    {
        rb.angularVelocity = 0f;

        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        if (vertical != 0)
            horizontal = 0;

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    void FixedUpdate()
    {
        if (isDashing) return;

        Vector2 forwardMovement = transform.up * vertical * shipData.speed;
        Vector2 strafeMovement = transform.right * horizontal * shipData.speed;

        rb.AddForce((forwardMovement + strafeMovement), ForceMode2D.Force);

        // Freno suave
        if (horizontal == 0 && vertical == 0)
        {
            rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, Vector2.zero, 5f * Time.fixedDeltaTime);
        }

        // Limitar velocidad
        rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity, shipData.speed);

        // Rotación
        float rotationInput = 0f;
        if (Input.GetKey(KeyCode.Y)) rotationInput = 1f;
        if (Input.GetKey(KeyCode.U)) rotationInput = -1f;

        rb.MoveRotation(rb.rotation + rotationInput * shipData.rotationSpeed * Time.fixedDeltaTime);

        // Animación
        bool isMovingForward = vertical > 0 && rb.linearVelocity.magnitude > 0.1f;
        animator.SetBool(AnimatorParams.IsMoving, isMovingForward);
    }
    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;

        
        float originalDrag = rb.linearDamping;
        rb.linearDamping = 0f;

        rb.linearVelocity = transform.up * shipData.dashPower;

       
        yield return new WaitForSeconds(shipData.dashTime);        
        rb.linearDamping = originalDrag;
        isDashing = false;

        // Cooldown
        yield return new WaitForSeconds(shipData.dashCooldown);
        canDash = true;
    }
}
