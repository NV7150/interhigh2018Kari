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
        /// アタッチされているEnemyMotionSystem
        /// </summary>
        private EnemyMotionSystem enemyMotion;

        /// <summary>
        /// 視界の距離
        /// </summary>
        public float seeRange = 1000;
        
        /// <summary>
        /// 隠密判定を行う間隔
        /// 単位は秒
        /// </summary>
        public float judgeInterval = 2.0f;
        
        /// <summary>
        /// 隠密判定を続けるかのフラグ
        /// </summary>
        private bool isInJudge = false;

        private bool isCoroutineStarted = false;

        private Transform foundPlayer;
        
        // Use this for initialization
        void Start() {
            stateMan = GetComponent<EnemyStateManager>();
            enemyMotion = GetComponent<EnemyMotionSystem>();
        }

        private void Update() {
            if (isCoroutineStarted && isEnemySeeingPlayer(foundPlayer)) {
                notified(foundPlayer);
                isInJudge = false;
            }
        }

        /// <summary>
        /// プレイヤーが感知させる範囲に引っかかった時の処理
        /// </summary>
        /// <param name="other">感知したトリガー</param>
        private void OnTriggerEnter(Collider other) {
            //プレイヤーを感知したら
            if (other.CompareTag("Player")) {
                foundPlayer = other.transform;
                //プレイヤーを視認していれば無条件で発見
                if (isEnemySeeingPlayer(foundPlayer)) {
                    notified(foundPlayer);
                } else {
                    //それ以外なら隠密判定（コルーチン）を開始
                    StartCoroutine(judgeNotifyCoroutine(foundPlayer));
                }
            }
        }
        
        /// <summary>
        /// 感知させる範囲から抜け出した時の処理
        /// </summary>
        /// <param name="other">感知したトリガー</param>
        private void OnTriggerExit(Collider other) {
            if (other.CompareTag("Player")) {
                isInJudge = false;
            }
        }
        
        /// <summary>
        /// このエネミーがプレイヤーを視認しているかの判定
        /// </summary>
        /// <returns>視認しているならtrue</returns>
        private bool isEnemySeeingPlayer(Transform player) {
            //プレイヤーとの間に障害物がなければ
            RaycastHit hit;
            if(Physics.Linecast(transform.position + new Vector3(0,1,0), player.position + new Vector3(0,1,0),out hit)){
                if (hit.collider.CompareTag("Player")) {
                    //かつ、視認範囲内なら
                    if (Vector3.Distance(transform.position, player.position) <= seeRange) {
                        //かつ、プレイヤーがエネミーの100度以内の位置なら視認していると判定
                        if (Vector3.Angle(transform.forward, player.position - transform.position) <= 100) {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// エネミーがプレイヤーに気づいた時の処理
        /// </summary>
        private void notified(Transform player) {
            if (stateMan.Player == null) {
                stateMan.Player = player.transform;
            }
            //目的地を設定
            enemyMotion.setDistnation(player.position);
            stateMan.State = EnemyState.FOUND;
        }
        
        /// <summary>
        /// 隠密判定を行うコルーチン
        /// </summary>
        private IEnumerator judgeNotifyCoroutine(Transform player) {
            isCoroutineStarted = true;
            isInJudge = true;
            while (isInJudge) {
                //隠密判定に勝利したらコルーチン終了
                if (judgeNotify()) {
                    notified(player);
                    break;
                }
                yield return new WaitForSeconds(judgeInterval);
            }

            isCoroutineStarted = false;
        }
        
        /// <summary>
        /// 隠密判定の内容
        /// </summary>
        /// <returns>エネミーが勝利したらtrue</returns>
        private bool judgeNotify() {
            //仮（10%）
            int rand = UnityEngine.Random.Range(0,10);
            return rand == 0;
        }
    }
}

