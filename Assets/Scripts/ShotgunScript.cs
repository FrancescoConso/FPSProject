using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunScript : AbstractGun
{
    public Animator gunSpriteAnimator;
    //public bool readyToFire;

    public GameObject[] traceEmitters;

    public GameObject playerStatsObj;
    PlayerStats ps;

    // Start is called before the first frame update
    void Start()
    {
        ps = playerStatsObj.GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        readyToFire = !gunSpriteAnimator.GetBool("isFiring");

        if (Input.GetKeyDown(KeyCode.LeftControl) && readyToFire && ps.ammoShells > 0)
        {
            gunSpriteAnimator.SetBool("isFiring", true);          
        }

    }

    public override void Fire()
    {
        //rumore arma
        RaycastHit[] noiseAlerted;
        noiseAlerted = Physics.SphereCastAll(transform.position, 100.0f, transform.forward);
        foreach (RaycastHit r in noiseAlerted)
        {
            if (r.transform.gameObject.CompareTag("Enemy"))
            {
                //se è un nemico faccio un raycast, se il primo trovato è il nemico allora lo allertiamo
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
        //hitscanCast(this.gameObject, 20);
        ps.ammoShells--;
        foreach (GameObject g in traceEmitters)
        {
            hitscanCast(g, 50);
        }

        
    }

    //Queste saranno cambiate in un secondo momento
    public override void Holster() { state = GunState.HOLSTERED; this.gameObject.SetActive(false); }

    public override void Deploy() { state = GunState.DEPLOYED; this.gameObject.SetActive(true); }

    public void canFireAgain()
    {
        gunSpriteAnimator.SetBool("isFiring", false);
    }

    void hitscanCast(GameObject castStartingPoint, float range)
    {
        RaycastHit hitTarget;

        Debug.DrawRay(castStartingPoint.transform.position, castStartingPoint.transform.forward * range, Color.red, 0.1f);

        bool[] accuracyLevels = { false, false, false, false };

        GameObject targetToDestroy = null;


        
        accuracyLevels[0] = Physics.Raycast(castStartingPoint.transform.position, transform.forward * range, out hitTarget);
        if (hitTarget.collider != null && hitTarget.collider.gameObject.CompareTag("Enemy"))
            targetToDestroy = hitTarget.collider.gameObject;
        accuracyLevels[1] = Physics.SphereCast(castStartingPoint.transform.position, 0.25f, castStartingPoint.transform.forward, out hitTarget, range - 0.25f);
        if (hitTarget.collider != null && hitTarget.collider.gameObject.CompareTag("Enemy"))
            targetToDestroy = hitTarget.collider.gameObject;
        accuracyLevels[2] = Physics.SphereCast(castStartingPoint.transform.position, 0.5f, castStartingPoint.transform.forward, out hitTarget, range-0.5f);
        if (hitTarget.collider != null && hitTarget.collider.gameObject.CompareTag("Enemy"))
            targetToDestroy = hitTarget.collider.gameObject;
        accuracyLevels[3] = Physics.SphereCast(castStartingPoint.transform.position, 1f, castStartingPoint.transform.forward, out hitTarget, range-1f);
        if (hitTarget.collider != null && hitTarget.collider.gameObject.CompareTag("Enemy"))
            targetToDestroy = hitTarget.collider.gameObject;


        bool hit = accuracyLevels[0] | accuracyLevels[1] | accuracyLevels[2] | accuracyLevels[3];

        //if (hit) Debug.Log("HIT");

        if (hit && targetToDestroy != null)
        {
            targetToDestroy.GetComponent<AbstractEnemy>().TakeDamage(damage, transform);
        }
    }
}
