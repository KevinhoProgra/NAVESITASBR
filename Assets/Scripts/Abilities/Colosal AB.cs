using UnityEngine;
using System.Collections;

public class ColosalAB : MonoBehaviour
{
    [Header("Referencias")]
    public ShipData shipData;
    public ParticleSystem teleportParticles;
    public LayerMask obstaclesLayer;

    [Header("Ajustes Visuales")]
    public float fadeSpeed = 5f;
    public float teleportDistance = 5f;

    private SpriteRenderer[] allRenderers; // Array para guardar la nave y sus hijos
    private bool isTeleporting = false;
    private float lastAbilityTime;

    void Awake()
    {
        // Obtenemos todos los SpriteRenderers de este objeto y sus hijos
        allRenderers = GetComponentsInChildren<SpriteRenderer>();

        if (shipData != null)
            lastAbilityTime = -shipData.abilityCooldown;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O) && !isTeleporting && Time.time >= lastAbilityTime + shipData.abilityCooldown)
        {
            StartCoroutine(TeleportSequence());
        }
    }

    IEnumerator TeleportSequence()
    {
        isTeleporting = true;
        lastAbilityTime = Time.time;

        // 1. Dirección
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        Vector2 moveDirection = new Vector2(moveX, moveY).normalized;
        if (moveDirection == Vector2.zero) moveDirection = transform.up;

        // 2. Efecto Salida (Fade Out)
        if (teleportParticles != null)
            Instantiate(teleportParticles, transform.position, Quaternion.identity);

        // Desvanecemos todos los renderizadores (Hijos incluidos)
        float currentAlpha = 1f;
        while (currentAlpha > 0.05f)
        {
            currentAlpha = Mathf.MoveTowards(currentAlpha, 0f, fadeSpeed * Time.deltaTime);
            SetAllAlphas(currentAlpha);
            yield return null;
        }

        // 3. El Salto
        Vector3 targetPosition = transform.position + (Vector3)moveDirection * teleportDistance;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, moveDirection, teleportDistance, obstaclesLayer);

        if (hit.collider != null)
            transform.position = hit.point - (moveDirection * 0.5f);
        else
            transform.position = targetPosition;

        // 4. Efecto Entrada (Fade In)
        if (teleportParticles != null)
            Instantiate(teleportParticles, transform.position, Quaternion.identity);

        // Aparecemos todos los renderizadores
        while (currentAlpha < 1f)
        {
            currentAlpha = Mathf.MoveTowards(currentAlpha, 1f, fadeSpeed * Time.deltaTime);
            SetAllAlphas(currentAlpha);
            yield return null;
        }

        isTeleporting = false;
    }

    // Método auxiliar para cambiar el alpha de todos los hijos a la vez
    void SetAllAlphas(float alpha)
    {
        foreach (SpriteRenderer sr in allRenderers)
        {
            if (sr == null) continue; // Por si algún hijo fue destruido
            Color c = sr.color;
            c.a = alpha;
            sr.color = c;
        }
    }
}