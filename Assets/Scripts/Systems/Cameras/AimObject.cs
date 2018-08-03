using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimObject : MonoBehaviour {
    public Ray getFrontRay() {
        Ray ray = new Ray(transform.position,transform.forward);
        return ray;
    }
}
