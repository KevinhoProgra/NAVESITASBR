using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewShipData", menuName = "Ships/ShipData")]
public class ShipData : ScriptableObject
{
    [Header("Stats")]
    public float maxHealth;
    public float maxShield;
    public float speed;
    public float rotationSpeed;

    [Header("Multiplicadores de Movimiento")]
    [Range(0, 1)] public float strafeMultiplier = 0.3f;   
    [Range(0, 1)] public float backwardMultiplier = 0.5f;

    [Header("Dash")]
    public float dashPower;
    public float dashTime;
    public float dashCooldown;

    [Header("Disparo")]
    public float bulletDamage;
    public float fireRate;
    public float bulletSpeed;

    [Header("Habilidades")]
    public float abilityDuration; 
    public float abilityCooldown; 

    [Header("Visual")]
    public Sprite shipSprite;
    public GameObject shipPrefab;

   
}
