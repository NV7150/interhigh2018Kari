using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace Characters.Enemy {
    public class EnemyMotionSystem : MonoBehaviour {
        /// <summary>
        /// 未発見最終接近距離
        /// プレイヤーを見失った場合、最後にプレイヤーを見失った地点にこの値の分だけ近づこうとします
        /// </summary>
        public float searchNear = 0.15f;
        
        /// <summary>
        /// 視覚によって追う最大範囲
        /// この長さのRayがプレイヤーに命中すれば追います
        /// </summary>
        public float searchRange = 100;
        
        
        /// <summary>
        /// ステートマネージャ
        /// </summary>
        private EnemyStateManager stateMan;
        
        /// <summary>
        /// navMeshAgentコンポーネント
        /// </summary>
        private NavMeshAgent nav;
        
        /// <summary>
        /// アニメーター
        /// </summary>
        private Animator anim;

        private EnemyAbilities abilities;

        private EnemyEquipmentManager equipMan;

        
        private void Awake() {
            stateMan = GetComponent<EnemyStateManager>();
            nav = GetComponent<NavMeshAgent>();
            anim = GetComponent<Animator>();
            equipMan = GetComponent<EnemyEquipmentManager>();
            abilities = GetComponent<EnemyAbilities>();
            
            //ナビゲーションだと動かない限りローテーションが更新されないのでナビゲーションのローテーションは切る
            nav.updateRotation = false;
        }

        private void Start() {
            nav.speed = abilities.WalkSpeed;
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

            animate();
            lookPlayer();
            
        }

        private void findingFunc() {
            
        }

        /// <summary>
        /// プレイヤーを発見した時の処理
        /// プレイヤーを可能な限り追います
        /// </summary>
        /// <exception cref="Exception">プレイヤーが設定されていない時</exception>
        private void foundFunc() {
            //プレイヤーが発見されていない時の例外
            if (stateMan.Player == null)
                throw new Exception("this enemy hasn't found player yet.");
            
            if (!nav.pathPending) {
                //プレイヤー方向にレイを飛ばす
                var ray = new Ray(transform.position + new Vector3(0, 1, 0),
                    stateMan.Player.transform.position - transform.position);
                RaycastHit hit;
                
                //プレイヤーを見つけたかどうか
                bool foundPlayer = false;
                
                //プレイヤーを発見できた方向に向かう
                if (Physics.Raycast(ray, out hit, searchRange)) {
                    if (hit.collider.CompareTag("Player")) {
                        //プレイヤーが視界内にいるなら武器射程までで止まる
                        nav.stoppingDistance = equipMan.getWeaponRange(searchNear);
                        //目標地点を決定
                        nav.destination = stateMan.Player.transform.position;
                        foundPlayer = true;
                    }
                    
                    //攻撃中でないなら
                    if (stateMan.State != EnemyState.ATTACKING) {
                        //射程内に入った時点で攻撃中にステート変更
                        if (nav.remainingDistance <= equipMan.getWeaponRange(searchNear)) {
                            stateMan.State = EnemyState.ATTACKING;
                        }
                    } else {
                        //攻撃中なら、射程外に行くと攻撃中ステートを解除
                        if (nav.remainingDistance >= equipMan.getWeaponRange(searchNear)) {
                            stateMan.State = EnemyState.FOUND;
                        }
                    }
                }

                //プレイヤーが見つからなかったら
                if (!foundPlayer) {
                    if (nav.remainingDistance >= 0.15f) {
                        if (nav.stoppingDistance >= equipMan.getWeaponRange(searchNear))
                            //まだ最後に見かけた地点まで到達しておらず、stopDistanceが再設定されていなかったら0.15fまで
                            //射程を踏み越えて見かけた地点まで行く
                            nav.stoppingDistance = searchNear;
                    } else {
                        //万策尽きたら見失う
                        stateMan.State = EnemyState.FINDING;
                    }
                }

                
            }
        }
        
        /// <summary>
        /// 敵をアニメーションさせます
        /// </summary>
        void animate() {
            //スピード値をアニメータに設定
            float speed = nav.desiredVelocity.magnitude;
            anim.SetFloat("speed",speed,0.1f,Time.deltaTime);
        }
        
        /// <summary>
        /// プレイヤーの方を向きます
        /// </summary>
        void lookPlayer() {
            //プレイヤーの方を向く
            if(stateMan.State == EnemyState.FOUND || stateMan.State == EnemyState.ATTACKING) {
                var relativeVector = stateMan.Player.position - transform.position;
                var plAngle = Quaternion.LookRotation(relativeVector);
                transform.rotation = Quaternion.RotateTowards(transform.rotation,plAngle,abilities.RotateSpeed);
            }
        }
        
        /// <summary>
        /// ナビゲーションの目的地を設定します
        /// </summary>
        /// <param name="pos"></param>
        public void setDistnation(Vector3 pos) {
            nav.SetDestination(pos);
        }
    }
}