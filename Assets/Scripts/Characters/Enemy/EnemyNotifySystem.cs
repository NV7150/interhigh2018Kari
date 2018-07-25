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
        
        /// <summary>
        /// Enemyの視界
        /// </summary>
        public int searchRange = 100;
        
        /// <summary>
        /// NavMeshAgentのオブジェクト
        /// </summary>
        private NavMeshAgent nav;
        
        // Use this for initialization
        void Start() {
            stateMan = GetComponent<EnemyStateManager>();
            nav = GetComponent<NavMeshAgent>();
            Debug.Log(nav.ToString());
        }
        
        /// <summary>
        /// プレイヤーの感知範囲に引っかかった時の処理
        /// </summary>
        /// <param name="other">感知したトリガー</param>
        private void OnTriggerStay(Collider other) {
            Debug.Log("called");
            if (other.CompareTag("Player")) {
                if (player == null)
                    player = other.gameObject;
                nav.destination = other.transform.position;
                stateMan.State = EnemyState.FOUND;
            }
        }
        
        /// <summary>
        /// プレイヤーの現在地をnavitagionの目標に定めます
        /// </summary>
        /// <exception cref="Exception">プレイヤーを発見できない</exception>
        /// <returns>プレイヤーの最終発見位置に到達したか</returns>
        public bool updateDestination() {
            //プレイヤーが発見されていない時の例外
            if(player == null)
                throw new Exception("this enemy hasn't found player yet.");
            if (!nav.pathPending) {

                //プレイヤーが視界内にいるかどうか
                bool isPlayerInRange = false;

                //プレイヤー方向にレイを飛ばす
                var ray = new Ray(transform.position + new Vector3(0, 1, 0),player.transform.position - transform.position);
                RaycastHit hit;
                //プレイヤーを発見できた方向に向かう
                if (Physics.Raycast(ray, out hit, searchRange)) {
                    if (hit.collider.CompareTag("Player")) {
                        nav.destination = player.transform.position;
                        isPlayerInRange = true;
                        Debug.Log(transform.position + "" + nav.destination);
                    }
                }

                if (nav.remainingDistance <= nav.stoppingDistance) {
                    nav.isStopped = true;
                } else {
                    nav.isStopped = false;
                }

                return nav.isStopped && !isPlayerInRange;
            } else {
                return false;
            }
        }
    }
}

