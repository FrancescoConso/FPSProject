using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTriggerScript : MonoBehaviour
{
    public int requiredItemIndex=-1;
    PlayerStats ps;

    // Start is called before the first frame update
    void Start()
    {
        ps = GameObject.Find("PlayerStatsObj").GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    /*void Update()
    {
        
    }*/

    private void OnTriggerEnter(Collider other)
    {
        if (requiredItemIndex == -1 || ps.hasItem[requiredItemIndex] == true)
        GetComponentInParent<DoorScript>().Open();
        else
        {
            if(other.CompareTag("Player"))
            {
                if(requiredItemIndex == 0)
                    other.gameObject.GetComponent<CharacterControllerMovement>().DisplayFadingMessage("You need the Magenta Key");
                if (requiredItemIndex == 1)
                    other.gameObject.GetComponent<CharacterControllerMovement>().DisplayFadingMessage("You need the Green Key");
                if (requiredItemIndex == 2)
                    other.gameObject.GetComponent<CharacterControllerMovement>().DisplayFadingMessage("You need the Cyan Key");
            }
        }
    }
}
