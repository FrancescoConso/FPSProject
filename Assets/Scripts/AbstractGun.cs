using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AmmoType
{
    NONE,
    SBULLET,
    SHELL,
    LBULLET,
    ROCKET
};

public enum GunState
{
    NOTAVAILABLE,
    HOLSTERED,
    DEPLOYED,
    FIRING
};

public abstract class AbstractGun : MonoBehaviour
{
    //NOTA: dovremmo vincolare il maxammo all ammotype
    //ma visto che ogni arma usa un ammo diverso
    //possiamo tenere il max dentro l'arma
    public AmmoType ammoType;
    public int maxAmmo;
    public bool readyToFire;
    public GunState state;
    public int damage;
    public abstract void Deploy();
    public abstract void Fire();
    public abstract void Holster();

}
