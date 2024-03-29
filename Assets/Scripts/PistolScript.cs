using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolScript : AbstractGun
{
    public Animator gunSpriteAnimator;

    public GameObject playerStatsObj;
    PlayerStats ps;
    //public bool readyToFire;

    // Start is called before the first frame update
    void Start()
    {
        ps = playerStatsObj.GetComponent<PlayerStats>();

    }

    // Update is called once per frame
    void Update()
    {
        readyToFire = !gunSpriteAnimator.GetBool("isFiring");

        if (Input.GetKeyDown(KeyCode.LeftControl) && readyToFire && ps.ammoSBullets > 0)
        {
                gunSpriteAnimator.SetBool("isFiring", true);
        }

    }

    public override void Fire()
    {
        ps.ammoSBullets--;
        /*GameObject spawnBulletPrefab;
        spawnBulletPrefab = Instantiate(bulletPrefab, transform.position, transform.rotation);*/

        //rumore arma
        RaycastHit[] noiseAlerted;
        noiseAlerted = Physics.SphereCastAll(transform.position, 100.0f, transform.forward);
        foreach(RaycastHit r in noiseAlerted)
        {
            if (r.transform.gameObject.CompareTag("Enemy"))
            {
                //se � un nemico faccio un raycast, se il primo trovato � il nemico allora lo allertiamo
                RaycastHit[] hitTargets;
                hitTargets = Physics.RaycastAll(transform.position, r.transform.position - transform.position);

                foreach (RaycastHit r1 in hitTargets)
                {
                    if (!r1.collider.gameObject.CompareTag("Player") && !r1.collider.gameObject.CompareTag("Enemy")) break;
                    if (r1.collider.gameObject.CompareTag("Enemy") && !r1.transform.gameObject.GetComponent<AbstractEnemy>().isDeaf)
                        r1.transform.gameObject.GetComponent<AbstractEnemy>().Alert(transform.gameObject);
                }
            }
        }
        
        
        //danno raycast
        RaycastHit hitTarget;

        Debug.DrawRay(transform.position, transform.forward * 75, Color.red, 0.1f);

        bool[] accuracyLevels = { false, false, false, false };

        GameObject targetToDestroy = null;



        accuracyLevels[0] = Physics.Raycast(transform.position, transform.forward * 75, out hitTarget);
        if (hitTarget.collider != null && hitTarget.collider.gameObject.CompareTag("Enemy"))
            targetToDestroy = hitTarget.collider.gameObject;
        accuracyLevels[1] = Physics.SphereCast(transform.position, 0.25f, transform.forward, out hitTarget, 74.75f);
        if (hitTarget.collider != null && hitTarget.collider.gameObject.CompareTag("Enemy"))
            targetToDestroy = hitTarget.collider.gameObject;
        accuracyLevels[2] = Physics.SphereCast(transform.position, 0.5f, transform.forward, out hitTarget, 74.5f);
        if (hitTarget.collider != null && hitTarget.collider.gameObject.CompareTag("Enemy"))
            targetToDestroy = hitTarget.collider.gameObject;
        accuracyLevels[3] = Physics.SphereCast(transform.position, 1f, transform.forward, out hitTarget, 74f);
        if (hitTarget.collider != null && hitTarget.collider.gameObject.CompareTag("Enemy"))
            targetToDestroy = hitTarget.collider.gameObject;


        bool hit = accuracyLevels[0] | accuracyLevels[1] | accuracyLevels[2] | accuracyLevels[3];


        if (hit && targetToDestroy != null)
        {
            if (targetToDestroy.CompareTag("Enemy")) 
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
