using UnityEngine;
using System.Collections;

public class Nave1AB : MonoBehaviour
{
    public ShipData shipData;
    public Animator animator;

    // Valores originales
    private float originalSpeed;
    private float originalFireRate;

    // Buff
    public float abilitySpeedMultiplier = 2f;   
    public float abilityFireRateMultiplier = 0.5f; 
    public float abilityDuration = 10f;          

    void Start()
    {
       
        originalSpeed = shipData.speed;
        originalFireRate = shipData.fireRate;
    }

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.O))
        {
            StartCoroutine(ActivateAbility());
        }
    }

    private IEnumerator ActivateAbility()
    {
        animator.SetBool("IsAbilityActive", true); 

        shipData.speed *= abilitySpeedMultiplier;
        shipData.fireRate *= abilityFireRateMultiplier;

        yield return new WaitForSeconds(abilityDuration);

        shipData.speed = originalSpeed;
        shipData.fireRate = originalFireRate;

        animator.SetBool("IsAbilityActive", false); 
    }
}
