using UnityEngine;

// One reusable definition for any melee or ranged weapon we add later.
[System.Serializable]
public class WeaponData
{
    public string weaponName;
    public bool isRanged;
    public float damage;
    public float range;
    public float cooldown;
    public Color visualColor;
}

public static class WeaponLibrary
{
    public static readonly WeaponData Fists = new WeaponData
    {
        weaponName = "Fists", isRanged = false, damage = 25f, range = 2f, cooldown = 0.4f, visualColor = new Color(0.9f, 0.8f, 0.4f)
    };

    public static readonly WeaponData Sword = new WeaponData
    {
        weaponName = "Rift Sword", isRanged = false, damage = 40f, range = 2.5f, cooldown = 0.6f, visualColor = new Color(0.8f, 0.8f, 0.9f)
    };

    public static readonly WeaponData EnergyBolt = new WeaponData
    {
        weaponName = "Energy Bolt", isRanged = true, damage = 15f, range = 20f, cooldown = 0.5f, visualColor = new Color(1f, 0.9f, 0.3f)
    };
}