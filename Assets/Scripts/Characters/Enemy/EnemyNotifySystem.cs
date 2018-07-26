using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace Characters.Enemy {
    public class EnemyNotifySystem : MonoBehaviour {
        /// <summary>
        /// ステートマネージャ
        /// </summary>
        private EnemyStateManager stateMan;
        
        /// <summary>
        /// プレイヤー
        /// notifyで初期化される
        /// </summary>
        private GameObject player;

        private EnemyMotionSystem enemyMotion;
        
        // Use this for initialization
        void Start() {
            stateMan = GetComponent<EnemyStateManager>();
            enemyMotion = GetComponent<EnemyMotionSystem>();
        }

        /// <summary>
        /// プレイヤーの感知範囲に引っかかった時の処理
        /// </summary>
        /// <param name="other">感知したトリガー</param>
        private void OnTriggerStay(Collider other) {
            if (other.CompareTag("Player")) {
                if (enemyMotion.Player == null) {
                    enemyMotion.Player = other.transform;
                }

                stateMan.State = EnemyState.FOUND;
            }
        }
        
    }
}

