using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeScript : AbstractGun
{
    public Animator gunSpriteAnimator;
    //public bool readyToFire;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        readyToFire = !gunSpriteAnimator.GetBool("isFiring");

        if (Input.GetKeyDown(KeyCode.LeftControl) && readyToFire)
        {
            gunSpriteAnimator.SetBool("isFiring", true);
            
        }

    }

    public override void Fire()
    {
        
        /*GameObject spawnBulletPrefab;
        spawnBulletPrefab = Instantiate(bulletPrefab, transform.position, transform.rotation);*/
        RaycastHit hitTarget;

        Debug.DrawRay(transform.position, transform.forward * 1, Color.red, 0.1f);

        bool[] accuracyLevels = { false, false, false, false };

        GameObject targetToDestroy = null;


        accuracyLevels[0] = Physics.SphereCast(transform.position-transform.forward, 1f, transform.forward, out hitTarget, 2f);
        if (hitTarget.collider != null && hitTarget.collider.gameObject.CompareTag("Enemy"))
            targetToDestroy = hitTarget.collider.gameObject;
        accuracyLevels[1] = Physics.SphereCast(transform.position-transform.forward, 1f, transform.forward, out hitTarget, 1f);
        if (hitTarget.collider != null && hitTarget.collider.gameObject.CompareTag("Target"))
            targetToDestroy = hitTarget.collider.gameObject;
        /*accuracyLevels[1] = Physics.Raycast(transform.position, transform.forward * 5f, out hitTarget);
        if (hitTarget.collider != null && hitTarget.collider.gameObject.CompareTag("Target"))
            targetToDestroy = hitTarget.collider.gameObject;*/

        bool hit = accuracyLevels[0] || accuracyLevels[1];

        //if (hit) Debug.Log("HIT");

        if (hit && targetToDestroy != null)
        {
                targetToDestroy.GetComponent<AbstractEnemy>().TakeDamage(damage,transform);
        }
    }

    //Queste saranno cambiate in un secondo momento
    public override void Holster() { state = GunState.HOLSTERED; this.gameObject.SetActive(false); }

    public override void Deploy() { state = GunState.DEPLOYED; this.gameObject.SetActive(true); }

    public void canFireAgain()
    {
        gunSpriteAnimator.SetBool("isFiring", false);
    }
}
