public enum WeaponType 
{
    Pistol,
    Revolver,
    AutoRifle,
    Shotgun,
    Rifle
}

[System.Serializable] // make the class visible in the inspector
public class Weapon 
{
    public WeaponType weaponTye;
    public int ammo;
    public int maxAmmo;
}
