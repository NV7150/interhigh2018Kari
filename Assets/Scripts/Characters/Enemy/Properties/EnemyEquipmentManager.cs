using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEquipmentManager : EquipmentManager {
    private bool isBuilderSet;
    
    private WeaponSwitcher switcher;
    private MeleeSystem meleeSys;

    protected override WeaponSwitcher Switcher {
        get { return switcher; }
    }

    protected override MeleeSystem MeleeSys {
        get { return meleeSys; }
    }

    protected override string EnemyTag {
        get { return "Player"; }
    }

    protected override void Awake() {
        base.Awake();
        switcher = GetComponent<EnemyEquipmentSwitcher>();
        meleeSys = GetComponent<EnemyMeleeSystem>();
    }
    
    /// <summary>
    /// 武器の射程を返します
    /// 射撃武器でない場合、引数に渡された値を返します
    /// </summary>
    /// <param name="meleeRange">近接時に接近する距離</param>
    /// <returns>武器の射程</returns>
    public float getWeaponRange(float meleeRange) {
        return (CurrentWeaponType == WeaponType.SHOOT) ? CurrentShootWeapon.Range : meleeRange;
    }

    public void setBuilder(EnemyBuilder builder) {
        if (!isBuilderSet) {
            var weapon = ShootWeaponMasterManager.INSTANCE.creatWeapon(builder.WeaponId);
            equip(weapon);
            isBuilderSet = true;
        }
    }
}
