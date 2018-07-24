using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Characters.Enemy {
    public class EnemyMotionSystem : MonoBehaviour {
        /// <summary>
        /// ステートマネージャ
        /// </summary>
        private EnemyStateManager stateMan;
        
        private EnemyNotifySystem notifySys;
        private int foundCount = 0;
        private readonly int NOTIFIED_LIMIT = 10;

        private void Start() {
            stateMan = GetComponent<EnemyStateManager>();
            notifySys = GetComponent<EnemyNotifySystem>();
        }

        private void Update() {
            switch (stateMan.State){
                    case EnemyState.FINDING :
                        findingFunc();
                        break;
                    case EnemyState.FOUND : 
                        foundFunc();
                        break;
                    case EnemyState.ATTACKING :
                        attackingFunc();
                        break;
            }
            Debug.Log(stateMan.State);
        }

        private void findingFunc() {
            
        }

        private void foundFunc() {
            if (notifySys.updateDestination()) {
                stateMan.State = EnemyState.FINDING;
            }
        }

        private void attackingFunc() {
            
        }
    }

}