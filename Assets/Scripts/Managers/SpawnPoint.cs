using UnityEngine;
using Unity.Cinemachine; 

public class SpawnPoint : MonoBehaviour
{
    public GameObject playerPrefab;
    public Transform spawnPoint;
    public CinemachineCamera virtualCamera; 

    private void Start()
    {
        SpawnPlayer();
    }

    void SpawnPlayer()
    {
        
        GameObject player = Instantiate(playerPrefab, spawnPoint.position, Quaternion.identity);

        
        virtualCamera.Follow = player.transform;
    }
}
