using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoaiEnemyScript : AbstractEnemy
{

    //GameObject playerStats;
    PlayerStats ps;
    public Animator MoaiAnimController;
    GameObject AlertZone;
    EnemyState charState;
    GameObject chaseTargetObj;
    NavMeshAgent agent;
    Coroutine attackCoroutine;
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
            if (chaseTargetObj != null)
            {
                agent.destination = chaseTargetObj.transform.position + chaseTargetObj.transform.right * Random.Range(0, 5f) + chaseTargetObj.transform.forward * Random.Range(0, 5f);
                if (agent.remainingDistance < 5f)
                {
                    agent.destination = chaseTargetObj.transform.position;
                    if (attackCoroutine != null) StopCoroutine(attackCoroutine);
                }                  
                if (agent.remainingDistance < 3f)
                {
                    RaycastHit hitTarget;

                    Physics.Linecast(transform.position, agent.destination, out hitTarget);
                    if (hitTarget.collider != null && hitTarget.collider.gameObject.CompareTag("Player"))
                    {
                        //agent.isStopped = true;
                        if (attackCoroutine == null)
                            attackCoroutine = StartCoroutine(damageOverTime());
                    }

                }
                else
                {
                    if (attackCoroutine != null) { StopCoroutine(attackCoroutine); attackCoroutine = null; }
                    //agent.isStopped = false;
                }
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
        //Debug.Log("ATTACK");
        ps.DamagePlayer((int)Random.Range(damageMin, damageMax + 1));
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


    IEnumerator damageOverTime()
    {
        while (true)
        {
            Attack();
            yield return new WaitForSeconds(1f);                   
        }
    }

}
