using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterStateManager : MonoBehaviour {
    private bool isRecoilEffecting;

    public bool IsRecoilEffecting {
        get { return isRecoilEffecting; }
        set { isRecoilEffecting = value; }
    }
}
