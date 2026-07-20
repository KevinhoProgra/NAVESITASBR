using UnityEngine;
using Unity.Cinemachine;

public class SpawnPoint : MonoBehaviour
{
    [Header("SHIPS")]
    
    public GameObject[] playerPrefabs;

    public Transform spawnPoint;
    public CinemachineCamera virtualCamera;

    private void Start()
    {
        SpawnPlayer();
    }

    void SpawnPlayer()
    {
        // 1. Leer qué número de nave se guardó en el menú (0, 1 o 2)
        // Si por alguna razón no encuentra nada, usa la 0 por defecto
        int idNaveElegida = PlayerPrefs.GetInt("NaveElegidaID", 0);

        // 2. Seguridad: Verificar que el número sea válido y que existan naves en la lista
        if (playerPrefabs != null && idNaveElegida < playerPrefabs.Length && playerPrefabs[idNaveElegida] != null)
        {
            // 3. Clonar la nave elegida en la posición del SpawnPoint
            GameObject player = Instantiate(playerPrefabs[idNaveElegida], spawnPoint.position, Quaternion.identity);

            // 4. ¡Asignar la nueva nave clonada a Cinemachine para que la siga de inmediato!
            if (virtualCamera != null)
            {
                virtualCamera.Follow = player.transform;
            }
        }
        else
        {
            Debug.LogError("Error: No se pudo spawnear la nave. Revisa que el array tenga las 3 naves asignadas en el Inspector.");
        }
    }
}