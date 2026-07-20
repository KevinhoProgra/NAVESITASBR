using UnityEngine;
using System.Collections;

public enum EstadoInvisibilidad { Solido, Desvaneciendo, Invisible }

public class X12AB : MonoBehaviour
{
    [Header("Referencias")]
    public ShipData shipData; 

    [Header("Efecto Visual Específico (Solo esta nave)")]
    public float velocidadDesvanecido = 0.75f;
    [Range(0f, 1f)] public float minAlpha = 0.2f;

    private bool estaEnCooldown = false;
    public EstadoInvisibilidad estadoActual = EstadoInvisibilidad.Solido;

    private Renderer rend;
    private Color colorActual;

    void Start()
    {
        rend = GetComponent<Renderer>();
        if (rend != null)
        {
            // Guardamos el color inicial 
            colorActual = rend.material.color;
        }

        if (shipData == null) Debug.LogError("Por favor, asigna el ShipData en el Inspector.");
    }

    void Update()
    {
        // Usamos la configuración de tiempo universal del ShipData
        if (Input.GetKeyDown(KeyCode.O) && estadoActual == EstadoInvisibilidad.Solido && !estaEnCooldown)
        {
            StartCoroutine(SecuenciaCamuflaje());
        }
    }

    IEnumerator SecuenciaCamuflaje()
    {
        estaEnCooldown = true;

        // 1. FADE OUT: De sólido a transparente
        estadoActual = EstadoInvisibilidad.Desvaneciendo;
        yield return StartCoroutine(GraduarInvisibilidad(1.0f, minAlpha, velocidadDesvanecido));

        estadoActual = EstadoInvisibilidad.Invisible;
        Debug.Log("Camuflaje activado");

        // 2. DURACIÓN: Usamos el tiempo universal definido en ShipData
        // Restamos el tiempo de los desvanecidos para que la suma total sea la duración pedida
        float tiempoEsperaNeto = Mathf.Max(0f, shipData.abilityDuration - (2 * velocidadDesvanecido));
        yield return new WaitForSeconds(tiempoEsperaNeto);

        // 3. FADE IN: De transparente a sólido
        Debug.Log("Camuflaje agotado, reapareciendo...");
        yield return StartCoroutine(GraduarInvisibilidad(minAlpha, 1.0f, velocidadDesvanecido));

        estadoActual = EstadoInvisibilidad.Solido;

        // 4. COOLDOWN: Usamos el enfriamiento universal del ShipData
        StartCoroutine(IniciarCooldown());
    }

    IEnumerator GraduarInvisibilidad(float alphaInicial, float alphaFinal, float tiempoTotal)
    {
        float tiempoTranscurrido = 0f;

        while (tiempoTranscurrido < tiempoTotal)
        {
            tiempoTranscurrido += Time.deltaTime;
            float porcentaje = tiempoTranscurrido / tiempoTotal;

            float nuevoAlpha = Mathf.Lerp(alphaInicial, alphaFinal, porcentaje);
            AplicarCambioColor(nuevoAlpha);

            yield return null;
        }

        AplicarCambioColor(alphaFinal);
    }

    IEnumerator IniciarCooldown()
    {
        Debug.Log("Habilidad en enfriamiento...");
        yield return new WaitForSeconds(shipData.abilityCooldown);
        estaEnCooldown = false;
        Debug.Log("Habilidad lista para usar");
    }

    void AplicarCambioColor(float nuevoAlpha)
    {
        if (rend != null)
        {
            colorActual.a = nuevoAlpha;
            rend.material.color = colorActual;
        }
    }
}