using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateManager : MonoBehaviour {
    private EnemyState state = EnemyState.FINDING;

    public EnemyState State {
        get { return state; }
        set { state = value; }
    }
}
