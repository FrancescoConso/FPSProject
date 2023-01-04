using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class CharacterControllerMovement : MonoBehaviour
{

    public CharacterController charController;

    float xInput;
    float zInput;

    public float xInputMul;
    public float zInputMul;

    public TextMeshProUGUI status;

    bool isStrafing;

    public Animator gunSpriteAnimator;

    GameObject activeWeapon;
    AbstractGun heldWeapon;
    int indexActiveWeapon;
    public GameObject[] weaponList;

    public GameObject playerStatsObj;
    PlayerStats ps;

    public TextMeshProUGUI actionText;

    Coroutine runningRoutine;

    public GameObject shotgun;
    public GameObject rifle;
    public GameObject weaponsHolder;

    public TextMeshProUGUI shotgunAmmoText;
    public TextMeshProUGUI rifleAmmoText;

    public GameObject shotgunIcon;
    public GameObject rifleIcon;

    public GameObject magentaKeyIcon;
    public GameObject greenKeyIcon;
    public GameObject cyanKeyIcon;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        ps = playerStatsObj.GetComponent<PlayerStats>();
        activeWeapon = weaponList[1];
        indexActiveWeapon = 1;
        heldWeapon = activeWeapon.GetComponent<AbstractGun>();
        heldWeapon.Deploy();
        runningRoutine = null;
        
    }

    // Update is called once per frame
    void Update()
    {

        if (ps.healthPoints < 1) Die();

        isStrafing = false;

        xInput = Input.GetAxis("Horizontal");
        zInput = Input.GetAxis("Vertical") * zInputMul;


        if(charController.enabled)
        {
            if (Input.GetKey(KeyCode.LeftAlt)) isStrafing = true;
            if (isStrafing)
            {
                charController.SimpleMove(transform.right * xInput * zInputMul * Time.deltaTime + zInput * transform.forward * Time.deltaTime);
            }
            else
            {
                this.transform.Rotate(0, xInput * xInputMul * Time.deltaTime, 0);
                charController.SimpleMove(zInput * transform.forward * Time.deltaTime);
            }
        }
        
        status.text = "\nxInput " + xInput + "\nzInput " + zInput + "\nStrafing " + isStrafing.ToString() + "\nBullets: " + ps.ammoSBullets + "\nShells: " + ps.ammoShells + "\nCartridges: " + ps.ammoBBullets;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (heldWeapon.readyToFire && indexActiveWeapon != 0)
            {
                heldWeapon.Holster();
                activeWeapon = weaponList[0];
                indexActiveWeapon = 0;
                heldWeapon = activeWeapon.GetComponent<AbstractGun>();
                heldWeapon.Deploy();
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (heldWeapon.readyToFire && indexActiveWeapon != 1)
            {
                heldWeapon.Holster();
                activeWeapon = weaponList[1];
                indexActiveWeapon = 1;
                heldWeapon = activeWeapon.GetComponent<AbstractGun>();
                heldWeapon.Deploy();
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (heldWeapon.readyToFire && indexActiveWeapon != 2 && shotgun.GetComponent<ShotgunScript>().state!=GunState.NOTAVAILABLE)
            {
                heldWeapon.Holster();
                activeWeapon = weaponList[2];
                indexActiveWeapon = 2;
                heldWeapon = activeWeapon.GetComponent<AbstractGun>();
                heldWeapon.Deploy();
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (heldWeapon.readyToFire && indexActiveWeapon != 3 && rifle.GetComponent<RifleScript>().state != GunState.NOTAVAILABLE)
            {
                heldWeapon.Holster();
                activeWeapon = weaponList[3];
                indexActiveWeapon = 3;
                heldWeapon = activeWeapon.GetComponent<AbstractGun>();
                heldWeapon.Deploy();
            }
        }

    }

    IEnumerator showAndFade(TextMeshProUGUI UItext, string updateText, float waitTime)
    {
        actionText.color = new Color(1, 1, 1, 1);
        UItext.text = updateText;
        yield return new WaitForSeconds(waitTime);
        for (float i = 1; i >= 0; i -= Time.deltaTime)
        {
            // set color with i as alpha
            UItext.color = new Color(1, 1, 1, i);
            yield return null;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        
        //Debug.Log("Collision");
        if (other.gameObject.CompareTag("Health") && ps.healthPoints < ps.maxHealth)
        {
            DisplayFadingMessage("Picked up some health");

            ps.healthPoints = Mathf.Clamp(ps.healthPoints + 10, 0, ps.maxHealth);
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("Shield") && ps.luckPoints < ps.maxHealth)
        {
            DisplayFadingMessage("Picked up a shield");

            ps.luckPoints = Mathf.Clamp(ps.luckPoints + 10, 0, ps.maxHealth);
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("Score"))
        {
            DisplayFadingMessage("Found a score item");

            ps.scorePoints += 25;
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("PistolAmmo") && ps.ammoSBullets < ps.maxmAmoSBullets)
        {
            DisplayFadingMessage("Picked up 12 pistol bullets");

            ps.ammoSBullets = Mathf.Clamp(ps.ammoSBullets + 12, 0, ps.maxmAmoSBullets);
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("ShotgunAmmo") && ps.ammoShells < ps.maxAmmoShells)
        {
            DisplayFadingMessage("Picked up 5 shells");

            ps.ammoShells = Mathf.Clamp(ps.ammoShells + 5, 0, ps.maxAmmoShells);
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("RifleAmmo") && ps.ammoBBullets < ps.maxAmmoBBullets)
        {
            DisplayFadingMessage("Picked up 9 rifle cartridges");

            ps.ammoBBullets = Mathf.Clamp(ps.ammoBBullets + 9, 0, ps.maxAmmoBBullets);
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("ShotgunPickup"))
        {
            DisplayFadingMessage("You found the Shotgun");

            shotgunIcon.SetActive(true);
            shotgunAmmoText.gameObject.SetActive(true);
            shotgun.GetComponent<ShotgunScript>().state = GunState.HOLSTERED;
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("RiflePickup"))
        {
            DisplayFadingMessage("You found the Rifle");

            rifle.GetComponent<RifleScript>().state = GunState.HOLSTERED;
            rifleAmmoText.gameObject.SetActive(true);
            rifleIcon.SetActive(true);
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("MagentaKey"))
        {
            DisplayFadingMessage("You found the Magenta Key");

            magentaKeyIcon.SetActive(true);
            ps.hasItem[0] = true;
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("GreenKey"))
        {
            DisplayFadingMessage("You found the Green Key");

            greenKeyIcon.SetActive(true);
            ps.hasItem[1] = true;
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("CyanKey"))
        {
            DisplayFadingMessage("You found the Cyan Key");

            cyanKeyIcon.SetActive(true);
            ps.hasItem[2] = true;
            Destroy(other.gameObject);
        }

    }

    public void DisplayFadingMessage(string fadingText)
    {
        if (runningRoutine != null)
        {
            //Debug.Log("STOP COROUTINE");
            StopCoroutine(runningRoutine);
        }

        runningRoutine = StartCoroutine(showAndFade(actionText, fadingText, 5));
    }

    private void Die()
    {
        //in caso di morte disabilito il character controller
        //e imposto il rigidbody a non cinematico
        //todo reset impostazioni

        charController.enabled = false;
        Rigidbody deadBody = GetComponent<Rigidbody>();
        deadBody.isKinematic = false;
        deadBody.AddForce(transform.right);
        Destroy(weaponsHolder);
    }

}
