using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimObject : MonoBehaviour {
    /// <summary>
    /// 直線上のレイを取得
    /// </summary>
    /// <returns></returns>
    public Ray getFrontRay() {
        Ray ray = new Ray(transform.position,transform.forward);
        return ray;
    }
}
