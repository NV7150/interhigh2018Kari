using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface WeaponSwitcher {
    void switchShoot(Transform shootAim,Transform shootFrom);
    void switchMelee();
}
