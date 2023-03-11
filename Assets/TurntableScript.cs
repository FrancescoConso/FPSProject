using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurntableScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(transform.up, 0.1f);
    }
}
