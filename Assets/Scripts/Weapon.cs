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
    public WeaponType weaponType;
    public int bulletsInMagazine;
        public int magazineCapacity;
    public int totalReserveAmmo;

    public bool CanShoot()
    {
        return HaveEnoughBullets();
    }

    private bool HaveEnoughBullets()
    {
        if (bulletsInMagazine > 0)
        {
            bulletsInMagazine--;
            return true;
        }

        return false;
    }

    public bool CanReload()
    {
        if(bulletsInMagazine == magazineCapacity)
        {
            return false;
        }

        if (totalReserveAmmo > 0)
        {
            return true;
        }
        return false;
    }

    public void RefillBullets()
    {
        //totalReserveAmmo += bulletsInMagazine; //retorna as balas do magazine para o total de balas
        int bulletsToReload = magazineCapacity;

        if(bulletsToReload > totalReserveAmmo)
        {
            bulletsToReload = totalReserveAmmo;
        }

        totalReserveAmmo -= bulletsToReload;
        bulletsInMagazine = bulletsToReload;

        if(totalReserveAmmo < 0)
        {
            totalReserveAmmo = 0;
        }  
    }
}
