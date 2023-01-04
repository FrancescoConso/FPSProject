using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MadMoaiEnemyScript : AbstractEnemy
{

    //GameObject playerStats;
    PlayerStats ps;
    public Animator MoaiAnimController;
    GameObject AlertZone;
    public EnemyState charState;
    GameObject chaseTargetObj;
    NavMeshAgent agent;
    Coroutine attackCoroutine;
    public GameObject FireballAttack;
    // Start is called before the first frame update
    void Start()
    {
        ps = GameObject.Find("PlayerStatsObj").GetComponent<PlayerStats>();
        charState = EnemyState.IDLE;
        agent = GetComponent<NavMeshAgent>();
        attackCoroutine = null;
        AlertZone = transform.Find("AlertZone").gameObject;
    }

    // Update is called once per frame
    void Update()
    {

        if (health < 1 && charState != EnemyState.DEAD) Die();
        if (charState == EnemyState.CHASING)
        {
            if(chaseTargetObj!=null) { 
                agent.destination = chaseTargetObj.transform.position + chaseTargetObj.transform.right * Random.Range(0, 5f) + chaseTargetObj.transform.forward * Random.Range(0, 5f);
                RaycastHit hitTarget;
                Physics.Linecast(transform.position, chaseTargetObj.transform.position, out hitTarget);
                if (agent.remainingDistance < 50f && hitTarget.transform != null && hitTarget.transform.gameObject.CompareTag("Player"))
                {
                    if (attackCoroutine == null)
                    {
                        attackCoroutine = StartCoroutine(instantiateFireballs());
                    }
                    //alla distanza di 20 unità estrae la pistola
                    //e entra in attacco
                    if (agent.remainingDistance < 20f)
                        agent.isStopped = true;
                }
            }
            else
            {
                //se è distante o viene spezzato il campo visivo torna ad inseguire
                agent.isStopped = false;
                if (attackCoroutine != null) { StopCoroutine(attackCoroutine); attackCoroutine = null; }
                //CharAnimController.SetBool("playerInSight", false);
            }
        }
        if (charState == EnemyState.DEAD)
        {
            
        }
    }

    public override void Alert(GameObject alertedBy)
    {
        charState = EnemyState.ALERT;
        Chase(alertedBy);
    }

    public void Chase(GameObject chaseTarget)
    {
        charState = EnemyState.CHASING;
        chaseTargetObj = chaseTarget;
    }

    public override void Attack()
    {
        //Debug.Log("Mad Moai Attack!");
        Vector3 vectorToTarget = chaseTargetObj.transform.position - transform.position;
        Debug.DrawRay(transform.position, vectorToTarget, Color.cyan, 1f);
        Instantiate(FireballAttack, transform.position, Quaternion.Euler(vectorToTarget));
        //Instantiate(FireballAttack, transform.position, Quaternion.Euler(vectorToTarget));
    }

    public override void TakeDamage(int dmg, Transform damageSource)
    {
        health -= dmg;
    }

    public override void Die()
    {
        MoaiAnimController.SetBool("isDead", true);
        charState = EnemyState.DEAD;
        Collider coll = GetComponent<CapsuleCollider>();
        if (attackCoroutine != null) StopCoroutine(attackCoroutine);
        Destroy(coll);
        Destroy(AlertZone);
        Destroy(agent);
        Destroy(this);
    }


    IEnumerator instantiateFireballs()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            //Debug.Log("Ready to attack");
            Attack();                            
        }
    }

}
