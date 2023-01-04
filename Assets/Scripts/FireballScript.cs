using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballScript : MonoBehaviour
{

    PlayerStats ps;
    public int speed;
    public int fireballDamage;
    GameObject playerObj;
    // Start is called before the first frame update
    void Start()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player");
        ps = GameObject.Find("PlayerStatsObj").GetComponent<PlayerStats>();
        //transform.forward = new Vector3(Camera.main.transform.forward.x, transform.forward.y, Camera.main.transform.forward.z);
        transform.forward = playerObj.transform.position - transform.position;
        //StartCoroutine(selfDestruct());
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
        //transform.forward = new Vector3(Camera.main.transform.forward.x, transform.forward.y, Camera.main.transform.forward.z);
    }

    /*private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision");
        ps.DamagePlayer(fireballDamage);
        Destroy(this.gameObject);
    }*/

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Fireball Collision");
        if(collision.gameObject.CompareTag("Player"))
        {
            ps.DamagePlayer(fireballDamage);          
        }
        if (!collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(this.gameObject);
        }
        

    }


    /*IEnumerator selfDestruct()
    {
        yield return new WaitForSeconds(10f);
        Destroy(this.gameObject);
    }*/

}
