using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewShipData", menuName = "Ships/ShipData")]
public class ShipData : ScriptableObject
{
    [Header("Stats básicos")]
    public float maxHealth;
    public float maxShield;
    public float speed;
    public float rotationSpeed;

    [Header("Dash")]
    public float dashPower;
    public float dashTime;
    public float dashCooldown;

    [Header("Disparo")]
    public float bulletDamage;
    public float fireRate;
    public float bulletSpeed;

    [Header("Visual")]
    public Sprite shipSprite;
    public GameObject shipPrefab;

   
}
