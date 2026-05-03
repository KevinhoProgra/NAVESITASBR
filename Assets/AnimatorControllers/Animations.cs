using UnityEngine;

public class Animations : MonoBehaviour
{
    public Animator animator;   // referencia al Animator
    public Rigidbody2D rb;      // referencia al Rigidbody de la nave

    void Update()
    {
        // Movimiento
        bool isMoving = rb.linearVelocity.magnitude > 0.1f;
        animator.SetBool(AnimatorParams.IsMoving, isMoving);

        // Disparo
        if (Input.GetKeyDown(KeyCode.I))
        {
            animator.SetTrigger(AnimatorParams.Shoot);
        }

        // Dash
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            animator.SetTrigger(AnimatorParams.Dash);
        }

        
    }
}
