using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ShipSelection : MonoBehaviour
{
    [Header("Base de Datos (Mismo Orden en Ambos Arrays)")]
    public ShipData[] listaStatsNaves;
    public GameObject[] prefabsNaves;

    private int indiceActual = 0;

    [Header("Componentes de UI de la Nave")]
    
    public Image imagenNaveActual;
    public TMP_Text textoNombre;

    [Header("Textos dentro de Panel (Estadísticas)")]
    public TMP_Text textoVida;
    public TMP_Text textoEscudo;
    public TMP_Text textoVelocidad;
    public TMP_Text textoDano;

    private void Start()
    {
        if (listaStatsNaves == null || prefabsNaves == null || listaStatsNaves.Length == 0 || prefabsNaves.Length == 0)
        {
            Debug.LogError("¡recuerda rellenar los arrays de Stats y Prefabs en el Inspector!");
            return;
        }

        ActualizarPantallaNave();
    }

    void ActualizarPantallaNave()
    {

        // 1. Sacamos las estadísticas puras del ScriptableObject
        ShipData datos = listaStatsNaves[indiceActual];

        // 2. Sacamos el Prefab que corresponde a este mismo índice
        GameObject prefabActual = prefabsNaves[indiceActual];

        // 3. Buscamos el SpriteRenderer que está pegado en tu Prefab (donde está tu arte de la nave)
        if (prefabActual != null)
        {
            SpriteRenderer spritePrefab = prefabActual.GetComponent<SpriteRenderer>();

            if (spritePrefab != null && imagenNaveActual != null)
            {
                // Le pasamos el sprite real del prefab a tu componente Image de la UI
                imagenNaveActual.sprite = spritePrefab.sprite;
            }
            else if (spritePrefab == null)
            {
                Debug.LogError($"El prefab {prefabActual.name} no tiene un componente SpriteRenderer en la raíz.");
            }
        }

        // Actualiza los textos de stats dentro de tu Panel de estadísticas
        if (textoNombre != null) textoNombre.text = datos.name.ToUpper();
        if (textoVida != null) textoVida.text = "HP: " + datos.maxHealth;
        if (textoEscudo != null) textoEscudo.text = "SHIELD: " + datos.maxShield;
        if (textoVelocidad != null) textoVelocidad.text = "SPEED: " + datos.speed;
        if (textoDano != null) textoDano.text = "DAMAGE: " + datos.bulletDamage;
    }

    public void SiguienteNave()
    {
        indiceActual++;
        if (indiceActual >= listaStatsNaves.Length) indiceActual = 0;
        ActualizarPantallaNave();
    }

    public void AnteriorNave()
    {
        indiceActual--;
        if (indiceActual < 0) indiceActual = listaStatsNaves.Length - 1;
        ActualizarPantallaNave();
    }

    public void ConfirmarSeleccion()
    {
        PlayerPrefs.SetInt("NaveElegidaID", indiceActual);
        PlayerPrefs.Save();

        // Recuerda cambiar esto por el nombre exacto de tu escena de partida
        SceneManager.LoadScene("SampleScene");
    }
}