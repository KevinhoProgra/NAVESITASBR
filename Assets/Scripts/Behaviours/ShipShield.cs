using System.Collections;
using UnityEngine;

public class ShipShield : MonoBehaviour
{
    public GameObject shieldVisual;
    public ShipData shipData;

    [Header("Settings")]
    public float shieldHP;
    public float regenDelay = 5f;
    public float regenRate = 10f;
    public float hideDelay = 3f;
    public float fadeDuration = 0.5f;

    public bool isActive = true; // Indica si el escudo está "arriba" y puede absorber daño

    private SpriteRenderer shieldRenderer;
    private Color originalColor;
    private Coroutine regenRoutine;
    private Coroutine visibilityRoutine;

    private void Start()
    {
        shieldHP = shipData.maxShield;
        shieldRenderer = shieldVisual.GetComponent<SpriteRenderer>();
        originalColor = shieldRenderer.color;

        // Empezar con el alpha correcto
        shieldRenderer.enabled = false;
    }

    public void TakeDamage(float damage)
    {
        // Si el escudo está roto (isActive = false), el daño pasa directo a la nave
        if (!isActive) return;

        shieldHP -= damage;
        shieldHP = Mathf.Max(shieldHP, 0);

        // Mostrar el escudo al impactar
        StopVisibility();
        shieldRenderer.enabled = true;
        shieldRenderer.color = originalColor;

        if (shieldHP <= 0)
        {
            isActive = false; // ESCUDO ROTO
            visibilityRoutine = StartCoroutine(FadeShield(Color.red));
        }
        else
        {
            // Timer para ocultar si no se rompió
            visibilityRoutine = StartCoroutine(WaitAndFade());
        }

        // REGENERACIÓN
        if (regenRoutine != null) StopCoroutine(regenRoutine);
        regenRoutine = StartCoroutine(RegenerateShieldLogic());
    }

    private IEnumerator RegenerateShieldLogic()
    {
        // Esperar el tiempo de calma
        yield return new WaitForSeconds(regenDelay);

        while (shieldHP < shipData.maxShield)
        {
            shieldHP += regenRate * Time.deltaTime;
            shieldHP = Mathf.Min(shieldHP, shipData.maxShield);

           
            if (shieldHP > 0 && !isActive)
            {
                isActive = true;
            }

            yield return null;
        }
    }

    //Desaparicion de Escudo
    private IEnumerator WaitAndFade()
    {
        yield return new WaitForSeconds(hideDelay);
        yield return StartCoroutine(FadeShield(originalColor));
    }

    private IEnumerator FadeShield(Color baseColor)
    {
        float elapsedTime = 0f;
        shieldRenderer.color = baseColor;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(originalColor.a, 0f, elapsedTime / fadeDuration);
            shieldRenderer.color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);
            yield return null;
        }

        shieldRenderer.enabled = false;
    }

    private void StopVisibility()
    {
        if (visibilityRoutine != null) StopCoroutine(visibilityRoutine);
    }
}