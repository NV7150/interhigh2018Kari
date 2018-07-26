using System;
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

        private Transform player;

        public Transform Player {
            get { return player; }
            set { player = value; }
        }

        /// <summary>
        /// 武器の射程
        /// 距離がこの値になるまでプレイヤーに近づこうとします
        /// </summary>
        public float weaponRange;

        /// <summary>
        /// 未発見最終接近距離
        /// プレイヤーを見失った場合、最後にプレイヤーを見失った地点にこの値の分だけ近づこうとします
        /// </summary>
        public float searchNear = 0.15f;

        public float searchRange = 100;
        
        private void Start() {
            stateMan = GetComponent<EnemyStateManager>();
            notifySys = GetComponent<EnemyNotifySystem>();
            nav = GetComponent<NavMeshAgent>();
            anim = GetComponent<Animator>();
            
            //ナビゲーションだと動かない限りローテーションが更新されないのでナビゲーションのローテーションは切る
            nav.updateRotation = false;
        }

        private void Update() {
            switch (stateMan.State){
                    case EnemyState.FINDING :
                        findingFunc();
                        break;
                    
                    //このスクリプトでは攻撃中は考慮しない
                    case EnemyState.ATTACKING :
                    case EnemyState.FOUND : 
                        foundFunc();
                        break;
            }
             
            float speed = nav.desiredVelocity.magnitude;
            anim.SetFloat("speed",speed,0.1f,Time.deltaTime);
            
            
            //プレイヤーの方を向く
            if(stateMan.State == EnemyState.FOUND)
                transform.LookAt(player.transform.position);
        }

        private void findingFunc() {
            
        }

        /// <summary>
        /// プレイヤーを発見した時の処理
        /// プレイヤーを可能な限り追います
        /// </summary>
        /// <exception cref="Exception"></exception>
        private void foundFunc() {
            //プレイヤーが発見されていない時の例外
            if (player == null)
                throw new Exception("this enemy hasn't found player yet.");
            
            if (!nav.pathPending) {
                //プレイヤー方向にレイを飛ばす
                var ray = new Ray(transform.position + new Vector3(0, 1, 0),
                    player.transform.position - transform.position);
                RaycastHit hit;
                //プレイヤーを見つけたかどうか
                bool foundPlayer = false;
                //プレイヤーを発見できた方向に向かう
                if (Physics.Raycast(ray, out hit, searchRange)) {
                    if (hit.collider.CompareTag("Player")) {
                        //プレイヤーが視界内にいるなら武器射程までで止まる
                        nav.stoppingDistance = weaponRange;
                        //目標地点を決定
                        nav.destination = player.transform.position;
                        foundPlayer = true;
                    }
                }

                //プレイヤーが見つからなかったら
                if (!foundPlayer) {
                    if (nav.remainingDistance >= 0.15f) {
                        if (nav.stoppingDistance >= weaponRange)
                            //まだ最後に見かけた地点まで到達しておらず、stopDistanceが再設定されていなかったら0.15fまで
                            //射程を踏み越えて見かけた地点まで行く
                            nav.stoppingDistance = searchNear;
                    } else {
                        stateMan.State = EnemyState.FINDING;
                    }
                }
            }
        }
    }
}