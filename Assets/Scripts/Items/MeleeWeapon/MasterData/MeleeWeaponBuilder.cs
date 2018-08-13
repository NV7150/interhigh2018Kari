using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponBuilder {
    private int id;
    private string name;
    private float damage;
    private RuntimeAnimatorController weaponAnim;
    private GameObject weponPrefab;

    public int Id {
        get { return id; }
        set { id = value; }
    }

    public string Name {
        get { return name; }
        set { name = value; }
    }

    public float Damage {
        get { return damage; }
        set { damage = value; }
    }

    public RuntimeAnimatorController WeaponAnim {
        get { return weaponAnim; }
        set { weaponAnim = value; }
    }

    public GameObject WeponPrefab {
        get { return weponPrefab; }
        set { weponPrefab = value; }
    }
    
    /// <summary>
    /// 近接武器を作って返します
    /// </summary>
    /// <returns>近接武器</returns>
    public MeleeWeapon getWeapon() {
        return new MeleeWeapon(this);
    }
}
