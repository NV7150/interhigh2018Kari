using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
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

        private NavMeshAgent nav;
        private Animator anim;

        private void Start() {
            stateMan = GetComponent<EnemyStateManager>();
            notifySys = GetComponent<EnemyNotifySystem>();
            nav = GetComponent<NavMeshAgent>();
            anim = GetComponent<Animator>();
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
            float speed = nav.desiredVelocity.magnitude;
            anim.SetFloat("speed",speed,0.1f,Time.deltaTime);

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