using System.Collections;
using System.Collections.Generic;
using RootMotion.FinalIK;
using UnityEngine;

public class EnemyEquipmentSwitcher : MonoBehaviour, WeaponSwitcher  {
    private Animator anim;

    private EnemyShootAttackSystem shootSys;
    private AimIK ik;

    private void Awake() {
        anim = GetComponent<Animator>();
        shootSys = GetComponent<EnemyShootAttackSystem>();
        ik = GetComponent<AimIK>();
    }

    public void switchShoot(Transform shootAim, Transform shootFrom) {
        //ikの設定
        ik.enabled = true;
        ik.solver.transform = shootAim;
        ik.solver.axis = new Vector3(0,0,1);
        ik.solver.target = null;

        //スクリプトの設定
        shootSys.enabled = true;
        shootSys.shootFrom = shootFrom;
        
        //アニメータのレイヤのweight設定
        anim.SetLayerWeight(2, 0);
        anim.SetLayerWeight(1, 1);
    }

    public void switchMelee() {
        throw new System.NotImplementedException();
    }
}
