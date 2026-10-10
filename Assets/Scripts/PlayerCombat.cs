using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    WeaponData currentWeapon = WeaponLibrary.Fists;
    float lastAttackTime = -999f;

    public void EquipWeapon(WeaponData weapon) => currentWeapon = weapon;

    public void RequestAttack()
    {
        if (Time.time - lastAttackTime < currentWeapon.cooldown) return;
        lastAttackTime = Time.time;

        if (currentWeapon.isRanged)
            FireProjectile(currentWeapon);
        else
            MeleeSwing(currentWeapon);
    }

    public void RequestRangedAttack()
    {
        if (Time.time - lastAttackTime < WeaponLibrary.EnergyBolt.cooldown) return;
        lastAttackTime = Time.time;
        FireProjectile(WeaponLibrary.EnergyBolt);
    }

    void MeleeSwing(WeaponData weapon)
    {
        Vector3 center = transform.position + Vector3.up * 0.5f + transform.forward * (weapon.range * 0.5f);
        Collider[] hits = Physics.OverlapSphere(center, weapon.range * 0.5f);

        foreach (var hit in hits)
        {
            if (hit.gameObject == gameObject) continue;
            var health = hit.GetComponent<Health>();
            if (health != null)
                health.TakeDamage(weapon.damage);
        }

        StartCoroutine(SwingVisual(weapon));
    }

    System.Collections.IEnumerator SwingVisual(WeaponData weapon)
    {
        var swing = GameObject.CreatePrimitive(PrimitiveType.Cube);
        swing.name = "SwingVisual";
        Destroy(swing.GetComponent<Collider>());
        swing.transform.localScale = new Vector3(1.2f, 0.1f, 0.4f);
        swing.transform.position = transform.position + Vector3.up * 1f + transform.forward * 1f;
        swing.transform.rotation = transform.rotation;
        swing.GetComponent<Renderer>().material.color = weapon.visualColor;
        yield return new WaitForSeconds(0.15f);
        Destroy(swing);
    }

    void FireProjectile(WeaponData weapon)
    {
        var proj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        proj.name = "Projectile";
        proj.transform.localScale = Vector3.one * 0.35f;
        proj.transform.position = transform.position + Vector3.up * 1f + transform.forward * 0.6f;
        proj.GetComponent<Renderer>().material.color = weapon.visualColor;

        var p = proj.AddComponent<Projectile>();
        p.damage = weapon.damage;
        p.Launch(transform.forward);
    }
}