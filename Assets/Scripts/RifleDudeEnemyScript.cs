using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RifleDudeEnemyScript : AbstractEnemy
{

    //GameObject playerStats;
    PlayerStats ps;
    public Animator CharAnimController;
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
            if(chaseTargetObj != null)
            {
                agent.destination = chaseTargetObj.transform.position + chaseTargetObj.transform.right * Random.Range(0, 9f) + chaseTargetObj.transform.forward * Random.Range(0, 9f);
                RaycastHit hitTarget;
                Physics.Linecast(transform.position, chaseTargetObj.transform.position, out hitTarget);
                if (agent.remainingDistance < 40f && hitTarget.transform != null && hitTarget.transform.gameObject.CompareTag("Player"))
                {
                    //alla distanza di 40 unità estrae la pistola
                    //e entra in attacco
                    agent.isStopped = true;
                    CharAnimController.SetBool("playerInSight", true);
                }
                else
                {
                    //se è distante o viene spezzato il campo visivo torna ad inseguire
                    agent.isStopped = false;
                    CharAnimController.SetBool("playerInSight", false);
                }
            }
            
        }

    }

    public override void Alert(GameObject alertedBy)
    {
        charState = EnemyState.ALERT;
        CharAnimController.SetBool("isAlerted", true);
        Chase(alertedBy);
    }

    public void Chase(GameObject chaseTarget)
    {
        CharAnimController.SetBool("isAlerted", true);
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
        //charState = EnemyState.HIT;
        health -= dmg;
        if (health > 0)
        {
            CharAnimController.SetTrigger("damageTrigger");
            agent.isStopped = true;
            StartCoroutine(Stunned());
            agent.isStopped = false;
        }
        if(charState == EnemyState.IDLE)
        {
            Chase(damageSource.transform.gameObject);
        }

    }

    public override void Die()
    {
        if (instantiatedGameObject != null) Instantiate(instantiatedGameObject, transform.position + transform.right - transform.up*0.75f, Quaternion.identity);
        CharAnimController.SetBool("isDead", true);
        charState = EnemyState.DEAD;
        Collider coll = GetComponent<CapsuleCollider>();
        if (attackCoroutine != null) StopCoroutine(attackCoroutine);
        
        Destroy(coll);
        Destroy(AlertZone);
        Destroy(agent);
        Destroy(this);
    }


    void HitscanShoot()
    {
        if(chaseTargetObj != null)
        {
            //l'attacco con hitscan crea un raytrace diretto verso il giocatore con una certa deviazione random
            RaycastHit[] hitTargets;
            //RaycastHit[] hitTargets;
            //Debug.DrawRay(transform.position, chaseTargetObj.transform.position - transform.position + chaseTargetObj.transform.right*Random.Range(-2f,2f), Color.cyan, 1f);
            Vector3 vectorToTarget = chaseTargetObj.transform.position - transform.position;
            float distanceToTarget = vectorToTarget.magnitude;
            hitTargets = Physics.RaycastAll(transform.position, vectorToTarget + chaseTargetObj.transform.right * Random.Range(-distanceToTarget / 2, distanceToTarget / 2));
            //Physics.Linecast(transform.position, chaseTargetObj.transform.forward, out hitTarget);
            foreach (RaycastHit r in hitTargets)
            {
                if (r.transform != null && r.transform.gameObject.CompareTag("Player")) Attack();
            }
        }
        
        
    }

    IEnumerator Stunned()
    {
        
        yield return new WaitForSeconds(0.2f);
        
    }
}
