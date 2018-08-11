using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

/// <summary>
/// 射撃武器のプロパティ設定用クラスです
/// </summary>
public class ShootWeaponBuilder{
    //shootWeaponの各値
    private int id;
    private string name;
    private float recoil;
    private float range;
    private float damage;
    private int ammo;
    private float zoomRate;
    private int needStr;
    private float weight;
    private float fireRate;
    private float reloadSec;
    private bool isAutomatic;
    private RuntimeAnimatorController weaponAnim;

    /// <summary>
    /// 対象Weaponのプレファブ
    /// </summary>
    private GameObject weaponPrefab;

    //各値のアクセサー
    public int Id {
        get { return id; }
        set { id = value; }
    }

    public string Name {
        get { return name; }
        set { name = value; }
    }

    public float Recoil {
        get { return recoil; }
        set { recoil = value; }
    }

    public float Range {
        get { return range; }
        set { range = value; }
    }

    public float Damage {
        get { return damage; }
        set { damage = value; }
    }

    public int Ammo {
        get { return ammo; }
        set { ammo = value; }
    }

    public float ZoomRate {
        get { return zoomRate; }
        set { zoomRate = value; }
    }

    public int NeedStr {
        get { return needStr; }
        set { needStr = value; }
    }

    public float Weight {
        get { return weight; }
        set { weight = value; }
    }

    public float FireRate {
        get { return fireRate; }
        set { fireRate = value; }
    }

    public bool IsAutomatic {
        get { return isAutomatic; }
        set { isAutomatic = value; }
    }

    public float ReloadSec {
        get { return reloadSec; }
        set { reloadSec = value; }
    }

    public RuntimeAnimatorController WeaponAnim {
        get { return weaponAnim; }
        set { weaponAnim = value; }
    }

    /// <summary>
    /// 対象武器ののプレファブ
    /// </summary>
    public GameObject WeaponPrefab {
        get { return weaponPrefab; }
        set { weaponPrefab = value; }
    }

    public ShootWeapon creatWeapon() {
        return new ShootWeapon(this);
    }
}
