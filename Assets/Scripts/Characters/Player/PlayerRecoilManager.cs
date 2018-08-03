using System.Collections;
using System.Collections.Generic;
using Characters.Player;
using UnityEngine;

public class PlayerRecoilManager : RecoilManager {
    public float recoilMouseSensability = 50f;
    public PlayerStateManager stateMan;
    public GameObject cam;

    void Start() {
    }

    protected override void FixedUpdate() {
        base.FixedUpdate();
        transform.rotation = cam.transform.rotation * Quaternion.Euler(-Recoiled, 0, 0);
    }

    public override void updateCurrentRecoilAngle() {
        base.updateCurrentRecoilAngle();
        if (IsRecoiling) {
            //マウス補正
            recoiled -= Input.GetAxis("Mouse Y") * Time.deltaTime * recoilMouseSensability;
            recoiled = (recoiled > 0) ? recoiled : 0;
        } else {
            stateMan.IsRecoilEffecting = false;
            stateMan.IsMouseActivated = true;
        }
    }

    public override void recoil(float recoilVelocity) {
        base.recoil(recoilVelocity);
        stateMan.IsRecoilEffecting = true;
        stateMan.IsMouseActivated = false;
    }

    public float getToCamAngle() {
        return Vector3.Angle(transform.forward, cam.transform.forward);
    }
    
}
