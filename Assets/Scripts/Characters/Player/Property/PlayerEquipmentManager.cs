using System.Collections;
using System.Collections.Generic;
using Characters.Player;
using UnityEngine;

public class PlayerEquipmentManager : EquipmentManager {
    private WeaponSwitcher switcher;
    private MeleeSystem meleeSys;

    protected override WeaponSwitcher Switcher {
        get { return switcher; }
    }

    protected override MeleeSystem MeleeSys {
        get { return meleeSys; }
    }

    protected override string EnemyTag {
        get { return "Enemy"; }
    }

    protected override void Awake() {
        base.Awake();
        switcher = GetComponent<PlayerEquipmentSwitcher>();
        meleeSys = GetComponent<PlayerMeleeSystem>();
    }
}
