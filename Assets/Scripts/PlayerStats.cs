using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{

    public int ammoSBullets;
    public int ammoShells;
    public int ammoBBullets;
    public int maxmAmoSBullets;
    public int maxAmmoShells;
    public int maxAmmoBBullets;
    public int healthPoints;
    public int maxHealth;
    public int luckPoints;
    public int scorePoints;

    public TextMeshProUGUI pistolAmmoUI;
    public TextMeshProUGUI shotgunAmmoUI;
    public TextMeshProUGUI rifleAmmoUI;

    public TextMeshProUGUI healthPointsUI;
    public TextMeshProUGUI luckPointsUI;
    public TextMeshProUGUI scorePointsUI;

    //item array per le chiavi
    //0 = magenta
    //1 = verde
    //2 = ciano
    public bool[] hasItem;


    // Start is called before the first frame update
    void Start()
    {
        ammoSBullets = 30;
        ammoShells = 5;
        ammoBBullets = 15;
        maxmAmoSBullets = 210;
        maxAmmoShells = 50;
        maxAmmoBBullets = 90;
        healthPoints = 100;
        maxHealth = 100;
        luckPoints = 0;
        scorePoints = 0;

        hasItem = new bool[3];
        for(int i = 0; i<hasItem.Length; i++)
        {
            hasItem[i] = false;
        }

    }
    

    // Update is called once per frame
    void Update()
    {
        pistolAmmoUI.text = ammoSBullets + " / " + maxmAmoSBullets;
        shotgunAmmoUI.text = ammoShells + " / " + maxAmmoShells;
        rifleAmmoUI.text = ammoBBullets + " / " + maxAmmoBBullets;
        healthPointsUI.text = healthPoints+"";
        luckPointsUI.text = luckPoints + "";
        scorePointsUI.text = scorePoints + "";
    }

    public void DamagePlayer(int dmgPoints)
    {
        if (luckPoints > dmgPoints)
        {
            luckPoints -= dmgPoints;
        }
        else if (luckPoints == 0)
        {
            healthPoints = Mathf.Clamp(healthPoints - dmgPoints, 0, maxHealth);
        }
        else
        {
            int leftoverPoints = dmgPoints - luckPoints;
            luckPoints = 0;
            healthPoints = Mathf.Clamp(healthPoints - dmgPoints, 0, maxHealth);
        }
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
}
