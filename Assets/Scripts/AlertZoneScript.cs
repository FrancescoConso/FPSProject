using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertZoneScript : MonoBehaviour
{
    // Start is called before the first frame update
    /*void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }*/

    public float alertradius;

    //lo script gestisce le allerte dei nemici
    //quando qualcosa entra nell'alert zone viene effettuata una proiezione
    //se il primo oggetto che incontra è un giocatore viene sollevata l'allerta

    private void OnTriggerStay(Collider other)
    {

        Debug.DrawRay(transform.position, other.transform.position - transform.position);

        RaycastHit[] hitTargets;

        hitTargets = Physics.RaycastAll(transform.position, other.transform.position - transform.position, alertradius);
        
        foreach(RaycastHit r in hitTargets)
        {
            if (!r.collider.gameObject.CompareTag("Player") && !r.collider.gameObject.CompareTag("Enemy")) break;
            if (r.collider.gameObject.CompareTag("Player"))
                transform.parent.gameObject.GetComponent<AbstractEnemy>().Alert(other.gameObject);
        }
    }
}
