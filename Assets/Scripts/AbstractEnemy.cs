using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractEnemy : MonoBehaviour
{

    public enum EnemyState
    {
        IDLE,
        ALERT,
        CHASING,
        ATTACKING,
        HIT,
        DEAD
    }

    //la classe rappresenta un nemico astratto
    public int health;
    public int damageMin;
    public int damageMax;
    public bool isDeaf;

    public GameObject instantiatedGameObject = null;

    public abstract void Attack();

    public abstract void TakeDamage(int dmg, Transform damageSource);

    public abstract void Die();

    public abstract void Alert(GameObject alertedBy);

    // Start is called before the first frame update
    /*void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }*/
}
