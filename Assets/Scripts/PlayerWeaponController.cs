using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    private Player player;
    private const float REFERENCE_BULLET_SPEED = 20;

    [SerializeField] private Weapon currentWeapon;


    [Header("Bullet details")]

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private Transform gunPoint;



    [SerializeField] private Transform weaponHolder;
    //[SerializeField] private Transform aim;


    private void Start()
    {
        player = GetComponent<Player>();
        player.controls.Character.Fire.performed += context => Shoot();

        currentWeapon.ammo = currentWeapon.maxAmmo;
    }

    private void Shoot()
    {
        if(currentWeapon.ammo <= 0)
        {
            return;
        }
        currentWeapon.ammo--;
        
        GameObject newBullet = Instantiate(bulletPrefab, gunPoint.position, Quaternion.LookRotation(gunPoint.forward));

        Rigidbody rbNewBullet = newBullet.GetComponent<Rigidbody>();
        
        rbNewBullet.mass = REFERENCE_BULLET_SPEED / bulletSpeed;
        rbNewBullet.velocity = BulletDirection() * bulletSpeed;

        Destroy(newBullet, 6);
        GetComponentInChildren<Animator>().SetTrigger("Fire");
    }

    private Vector3 BulletDirection()
    {
        Transform aim = player.aim.Aim();
        Vector3 direction = (aim.position - gunPoint.position).normalized;

        if (player.aim.CanAimPrecisly() == false && player.aim.Target() == null)
            direction.y = 0;
 
 

        //weaponHolder.LookAt(aim);
        //gunPoint.LookAt(aim);

        return direction;
    }

//    private void OnDrawGizmos()
//{

//    Gizmos.DrawLine(weaponHolder.position, weaponHolder.position + weaponHolder.forward * 25);

//    Gizmos.color = Color.yellow;
//    Gizmos.DrawLine(gunPoint.position, gunPoint.position + BulletDirection() * 25);
//}
}
