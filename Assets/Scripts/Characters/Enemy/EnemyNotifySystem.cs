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
        private Transform player;
        
        /// <summary>
        /// アタッチされているEnemyMotionSystem
        /// </summary>
        private EnemyMotionSystem enemyMotion;

        /// <summary>
        /// 視界の距離
        /// </summary>
        public float seeRange = 100;
        
        /// <summary>
        /// 隠密判定を行う間隔
        /// 単位は秒
        /// </summary>
        public float judgeInterval = 2.0f;
        
        /// <summary>
        /// 隠密判定を続けるかのフラグ
        /// </summary>
        private bool isInJudge = false;
        
        // Use this for initialization
        void Start() {
            stateMan = GetComponent<EnemyStateManager>();
            enemyMotion = GetComponent<EnemyMotionSystem>();
        }

        /// <summary>
        /// プレイヤーが感知させる範囲に引っかかった時の処理
        /// </summary>
        /// <param name="other">感知したトリガー</param>
        private void OnTriggerEnter(Collider other) {
            //プレイヤーを感知したら
            if (other.CompareTag("Player")) {
                player = other.transform;
                
                //プレイヤーを視認していれば無条件で発見
                if (isEnemySeeingPlayer()) {
                    notified();
                } else {
                    //それ以外なら隠密判定（コルーチン）を開始
                    StartCoroutine(judgeNotifyCoroutine());
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
        private bool isEnemySeeingPlayer() {
            //プレイヤーとの間に障害物がなければ
            RaycastHit hit;
            if(Physics.Linecast(transform.position, player.position,out hit)){
                if (hit.collider.CompareTag("Player")) {
                    //かつ、視認範囲内なら
                    if (Vector3.Distance(transform.position, player.position) <= seeRange) {
                        //かつ、zプレイヤーがエネミーの100度以内の位置なら視認していると判定
                        if (Vector3.Angle(transform.forward, player.forward) >= 100) {
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
        private void notified() {
            if (enemyMotion.Player == null) {
                enemyMotion.Player = player.transform;
            }
            //目的地を設定
            enemyMotion.setDistnation(player.position);
            stateMan.State = EnemyState.FOUND;
        }
        
        /// <summary>
        /// 隠密判定を行うコルーチン
        /// </summary>
        private IEnumerator judgeNotifyCoroutine() {
            
            isInJudge = true;
            while (isInJudge) {
                //隠密判定に勝利したらコルーチン終了
                if (judgeNotify()) {
                    notified();
                    break;
                }
                yield return new WaitForSeconds(judgeInterval);
            }
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

