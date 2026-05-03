using System.Collections;
using UnityEngine;

public class ShipShield : MonoBehaviour
{
    public GameObject shieldVisual;
    public ShipData shipData;

    public float shieldHP;
    public float regenDelay = 5f;
    public float regenRate = 10f;
    public bool isActive = true;
    private Coroutine regenRoutine;
    private SpriteRenderer shieldRenderer;

    private void Start()
    {
        shieldHP = shipData.maxShield;
        shieldRenderer = shieldVisual.GetComponent<SpriteRenderer>();


        shieldRenderer.enabled = false;
    }

    public void TakeDamage(float damage)
    {
        // Solo recibe daño si está activo
        if (!isActive) return;

        shieldHP -= damage;
        shieldHP = Mathf.Max(shieldHP, 0);

        shieldRenderer.enabled = true;

        // Si se rompe se desactiva
        if (shieldHP <= 0)
        {
            isActive = false;
            shieldRenderer.enabled = false;
        }

        if (regenRoutine != null)
            StopCoroutine(regenRoutine);

        regenRoutine = StartCoroutine(RegenerateShield());
    }

    //Regeneracion
    private IEnumerator RegenerateShield()
    {
        yield return new WaitForSeconds(regenDelay);

        while (shieldHP < shipData.maxShield)
        {
            shieldHP += regenRate * Time.deltaTime;
            shieldHP = Mathf.Min(shieldHP, shipData.maxShield);

            yield return null;
        }

        // vuelve a activarse al llegar a 100 de vida
        if (shieldHP >= shipData.maxShield)
        {
            isActive = true;
            shieldRenderer.enabled = false; // oculto hasta recibir daño
        }
    }

}
