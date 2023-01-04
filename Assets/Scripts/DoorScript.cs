using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{

    //questo script gestisce il movimento della porta
    public float openingSpeed = 1;


    // Start is called before the first frame update
    /*void Start()
    {
        
    }*/

    // Update is called once per frame
    void Update()
    {
        //distance = Vector3.Distance(transform.position, transform.position - (transform.up * transform.localScale.y));
    }

    public void Open()
    {
        StartCoroutine(goDown());
        //isOpening = true;
        //transform.position = Vector3.MoveTowards(transform.position, transform.position - transform.up * transform.localScale.y, Time.deltaTime * openingSpeed);
    }

    IEnumerator goDown()
    {
        Vector3 destinationPos = transform.position - (2 * transform.localScale.y * transform.up);
        while (Vector3.Distance(transform.position, destinationPos) > 0.1)
        {
            transform.position -= transform.up * Time.deltaTime * openingSpeed;
            yield return null;
        }
        
    }
}
