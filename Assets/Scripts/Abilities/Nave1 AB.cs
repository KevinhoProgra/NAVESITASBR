using UnityEngine;
using System.Collections;

public class Nave1AB : MonoBehaviour
{
 
    public ShipData shipData;
    public Animator animator;
    
    public float abilitySpeedMultiplier = 2f;
    public float abilityFireRateMultiplier = 0.5f;

    
    private bool isAbilityRunning = false;
    private bool estaEnCooldown = false;

    public float CurrentSpeed { get; private set; }
    public float CurrentFireRate { get; private set; }

    void Start()
    {
        // Inicializamos con los valores base del ShipData
        CurrentSpeed = shipData.speed;
        CurrentFireRate = shipData.fireRate;

        if (shipData == null) Debug.LogError("Asigna el ShipData en el Inspector de " + gameObject.name);
    }

    void Update()
    {
        // Activación con 'O' usando tiempos universales de ShipData
        if (Input.GetKeyDown(KeyCode.O) && !isAbilityRunning && !estaEnCooldown)
        {
            StartCoroutine(ActivateAbility());
        }
    }

    private IEnumerator ActivateAbility()
    {
        isAbilityRunning = true;
        Debug.Log("Habilidad Potenciadora Activada");

        animator.SetBool("IsAbilityActive", true);

        // Aplicamos el Buff a nuestras variables locales, NO al ShipData
        CurrentSpeed = shipData.speed * abilitySpeedMultiplier;
        CurrentFireRate = shipData.fireRate * abilityFireRateMultiplier;

        // Esperamos la duración universal definida en ShipData
        yield return new WaitForSeconds(shipData.abilityDuration);

        // Reset de valores al estado original
        CurrentSpeed = shipData.speed;
        CurrentFireRate = shipData.fireRate;

        animator.SetBool("IsAbilityActive", false);
        isAbilityRunning = false;

        // Iniciamos el enfriamiento universal
        StartCoroutine(IniciarCooldown());
    }

    private IEnumerator IniciarCooldown()
    {
        estaEnCooldown = true;
        Debug.Log("Habilidad Nave1 en cooldown...");

        yield return new WaitForSeconds(shipData.abilityCooldown);

        estaEnCooldown = false;
        Debug.Log("Habilidad Nave1 lista");
    }
}