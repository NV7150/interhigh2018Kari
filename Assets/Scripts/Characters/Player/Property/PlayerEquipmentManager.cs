using System.Collections;
using System.Collections.Generic;
using Characters.Player;
using UnityEngine;

public class PlayerEquipmentManager : EquipmentManager {
    private WeaponSwitcher switcher;

    protected override WeaponSwitcher Switcher {
        get { return switcher; }
    }

    protected override void Awake() {
        base.Awake();
        switcher = GetComponent<PlayerEquipmentSwitcher>();
    }
}
