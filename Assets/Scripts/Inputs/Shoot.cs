using UnityEngine;

public class Shoot : MonoBehaviour
{
    public GameObject BulletPrefab;
    
    public Transform FirePoint;
    public ShipData shipData;   

    private float nextFire;

    public Animator animator;

    void Update()
    {
        if (Input.GetKey(KeyCode.I) && Time.time >= nextFire)
        {
            ShootBullet();
            nextFire = Time.time + shipData.fireRate;  
        }
    }

    private void ShootBullet()
    {
        GameObject bullet = Instantiate(BulletPrefab, FirePoint.position, FirePoint.rotation);

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = FirePoint.up * shipData.bulletSpeed;
        }

        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.damage = shipData.bulletDamage;

            bulletScript.owner = gameObject;
        }

        
        animator.SetTrigger(AnimatorParams.Shoot);
    }
}
