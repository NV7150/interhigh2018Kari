using System.Collections;
using System.Collections.Generic;
using Characters.Player;
using UnityEngine;

/// <summary>
/// エイムオブジェクトを操作し、リコイル動作を再現します
/// </summary>
public class AimObjectRecoiler : RecoilManager {
    /// <summary>
    /// リコイル時、この感度の分だけマウス動作による制御が行われる
    /// </summary>
    public float recoilMouseSensability = 50f;
    
    /// <summary>
    /// プレイヤーのステートマネージャ
    /// </summary>
    public PlayerStateManager stateMan;
    
    /// <summary>
    /// カメラ（親オブジェクト）
    /// </summary>
    public GameObject cam;
    
    /// <summary>
    /// 能力値
    /// </summary>
    public PlayerAbilities abilities;

    protected override float RecoilControll {
        get { return abilities.RecoilControll; }
    }

    protected override void FixedUpdate() {
        base.FixedUpdate();
        //親クラスの計算結果分自身を回転
        transform.rotation = cam.transform.rotation * Quaternion.Euler(-Recoiled, 0, 0);
    }
    
    public override void updateCurrentRecoilAngle() {
        base.updateCurrentRecoilAngle();
        if (IsRecoiling) {
            //マウス補正
            recoiled -= Input.GetAxis("Mouse Y") * Time.deltaTime * recoilMouseSensability;
            recoiled = (recoiled > 0) ? recoiled : 0;
        } else {
            //ステートマネージャのフラグ管理
            stateMan.IsRecoilEffecting = false;
            stateMan.IsMouseActivated = true;
        }
    }

    public override void recoil(float recoilVelocity) {
        base.recoil(recoilVelocity);
        //ステートマネージャのステート管理
        stateMan.IsRecoilEffecting = true;
        stateMan.IsMouseActivated = false;
    }
}
