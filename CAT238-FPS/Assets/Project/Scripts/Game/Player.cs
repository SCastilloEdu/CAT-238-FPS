using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Visual")]
    public Camera playerCamera;
    [Header("Gameplay")]
    public int initialAmmo = 12;
    private int ammo;
    public int Ammo { get { return ammo; } }

    public int initialHealth = 100;
    private int health;
    public int Health { get { return health; } }

    public float knockbackForce = 100f;
    private bool isHurt;
    public float hurtDuration = 0.5f;
	
	public int weaponType = 1;
	public float weaponTwoSpread = 10.0f;

    private bool killed;
    public bool Killed { get { return killed; } }
    // Start is called before the first frame update
    void Start()
    {
        health = initialHealth;
        ammo = initialAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        // Mouse Click
        if (Input.GetMouseButtonDown(0))
        {
            if (ammo > 0)
            {
                // Decrease the ammo
                ammo--;
                // Object Pooling Scripts + Creating a bullet
                GameObject bulletObject = ObjectPoolingManager.Instance.GetBullet(true);
                bulletObject.transform.position = playerCamera.transform.position + transform.forward;
                bulletObject.transform.forward = playerCamera.transform.forward;
				// Checking if weaponType == 2 and making the shot tripleshot
				if (weaponType == 2)
				{
					GameObject bulletObjectL = ObjectPoolingManager.Instance.GetBullet(true);
					bulletObjectL.transform.position = playerCamera.transform.position + transform.forward;
					bulletObjectL.transform.forward = playerCamera.transform.forward;
					bulletObjectL.transform.Rotate(0.0f, -weaponTwoSpread, 0.0f, Space.Self);
					GameObject bulletObjectR = ObjectPoolingManager.Instance.GetBullet(true);
					bulletObjectR.transform.position = playerCamera.transform.position + transform.forward;
					bulletObjectR.transform.forward = playerCamera.transform.forward;
					bulletObjectR.transform.Rotate(0.0f, weaponTwoSpread, 0.0f, Space.Self);
					print("SHOTGUN!");
				}
            }

        }
    }
    void OnTriggerEnter(Collider otherCollider)
    {
        if (otherCollider.GetComponent<AmmoCrate>() != null)
        {
            // Get Ammo and Store it here and ready for it use
            AmmoCrate ammoCrate = otherCollider.GetComponent<AmmoCrate>();
            // Add Ammo
            ammo += ammoCrate.ammo;
            // Destroy the AmmoCrate Object
            Destroy(ammoCrate.gameObject);
        }
        if (isHurt == false)
        {
            GameObject hazard = null;

            if (otherCollider.GetComponent<Enemy>() != null)
            {
                // Get Enemy and ready for it use
                Enemy enemy = otherCollider.GetComponent<Enemy>();
                // Taking a damage
                health -= enemy.damage;
                // is Hurt is triggering
                hazard = enemy.gameObject;
            }
            else if (otherCollider.GetComponent<Bullet>() != null)
            {
                Bullet bullet = otherCollider.GetComponent<Bullet>();
                if (bullet.ShotByPlayer == false)
                {
                    hazard = bullet.gameObject;
                    health -= bullet.damage;
                    bullet.gameObject.SetActive(false);
                }
            }
            if (hazard != null)
            {
                isHurt = true;

                // Perform the knockback effect
                Vector3 hurtDirection = (transform.position - hazard.transform.position).normalized;
                Vector3 knockbackDirection = (hurtDirection + Vector3.up).normalized;
                GetComponent<ForceReceiver>().AddForce(knockbackDirection, knockbackForce);

                StartCoroutine(HurtRoutine());
            }
            if (health <= 0)
            {
                if (killed == false)
                {
                    killed = true;
                    OnKill();
                }
            }
        }
    }

    IEnumerator HurtRoutine()
    {
        yield return new WaitForSeconds(hurtDuration);
        isHurt = false;
    }

    private void OnKill()
    {

    }

}