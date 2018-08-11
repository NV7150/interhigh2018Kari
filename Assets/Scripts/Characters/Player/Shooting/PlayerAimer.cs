using System.Collections;
using System.Collections.Generic;
using RootMotion.FinalIK;
using UnityEngine;

namespace Characters.Player {
    public class PlayerAimer : MonoBehaviour {
        /// <summary>
        /// カメラからのRayの発信源
        /// </summary>
        public AimObject aimObj;
        
        /// <summary>
        /// カメラのTransform
        /// </summary>
        public Transform cam;
        
        /// <summary>
        /// 銃口
        /// </summary>
        public Transform shootFrom;
        
        
        private IKSolverAim ikSol;

        private PlayerEquipmentManager equipmentMan;
        
        
        /// <summary>
        /// ロックオンを適用する最短の距離
        /// 小さすぎると壁とかに近づいた時にikの位置が近すぎて変になる
        /// </summary>
        public float aimCapDistance;
        
        
        /// <summary>
        /// ロックしている部分がaimCapDistance内であるか
        /// </summary>
        private bool isAimCap;
        
        /// <summary>
        /// isAimCapがtrueの際にaimCap処理を抜いた物体までの距離が入る
        /// </summary>
        private float realAimDistToObj;
        
        /// <summary>
        /// 現在ロックオンしている物体までの距離
        /// </summary>
        private float rockDist;

        public bool IsAimCap {
            get { return isAimCap; }
        }

        public float RealAimDistToObj {
            //エイムキャップじゃない時は異常だけど変数の意味的にrockDist返しておけば問題はない
            get { return (IsAimCap) ? realAimDistToObj : rockDist; }
        }

        public float RockDist {
            get { return rockDist; }
        }

        private void Awake() {
            equipmentMan = GetComponent<PlayerEquipmentManager>();
            ikSol = GetComponent<AimIK>().solver;
        }


        // Update is called once per frame
        void Update () {
            searchAim();
        }
        
        /// <summary>
        /// 銃口の向き（AimIKの位置）を修正します。
        /// update毎に呼ばれます。
        /// </summary>
        void searchAim() {
            //エイムオブジェクトからRayを発信
            var ray = aimObj.getFrontRay();

            RaycastHit hit;
            //とりあえずPlayer以外なら命中判定
            if (Physics.Raycast(ray, out hit, equipmentMan.CurrentShootWeapon.Range * 10) && !hit.collider.CompareTag("Player")) {
                
                var aimpoint = hit.point;
                var dist = Vector3.Distance(shootFrom.transform.position, aimpoint);
                
                //当たったところが射程圏内ならそこをロック
                if (dist < equipmentMan.CurrentShootWeapon.Range) {
                    rockInRange(dist,aimpoint);
                }
                
            } else {
                normalRock(ray.direction);
            }
        }
        
        /// <summary>
        /// エイムキャップに達していた場合にはaimpointを修正します
        /// </summary>
        /// <param name="dist">ロック予定の物体までの距離</param>
        /// <param name="aimpoint">ロック予定の物体の具体的な位置</param>
        /// <returns>修正済みのVector3位置。修正の必要がなかったらaimpointをそのまま返します</returns>
        Vector3 ajustAimCap(float dist,Vector3 aimpoint) {
            //エイムキャップ圏内ならエイム修正なしでエイムキャップ分の距離をロックする
            if (dist <= aimCapDistance) {
                //実距離を保存
                realAimDistToObj = dist;
                //aimCapになるまでaimpointを調節
                aimpoint += cam.forward * (aimCapDistance - realAimDistToObj);
                isAimCap = true;
            } else {
                isAimCap = false;
            }

            return aimpoint;
        }
        
        /// <summary>
        /// 射程内に入っていた場合の処理
        /// </summary>
        /// <param name="dist">ロック予定の物体までの距離</param>
        /// <param name="aimpoint">ロック予定の物体の具体的な位置</param>
        void rockInRange(float dist,Vector3 aimpoint) {
            //ロック最低位置に入っていたら修正
            aimpoint = ajustAimCap(dist, aimpoint);
                    
            //距離を更新：カメラからの距離の方がaimcursor的に都合がいいのでそっち
            rockDist = Vector3.Distance(cam.position, aimpoint);

            //物体に当たった位置を向く
            ikSol.IKPosition = aimpoint;
        }
        
        /// <summary>
        /// 射程外、またはrayがどこにも当たらなかった時の処理
        /// 現状やり方が見つかってないのでカメラから射程の位置をロック（理想は銃口から射程の位置までロック）
        /// </summary>
        /// <param name="direction">カメラからのrayのベクトル</param>
        void normalRock(Vector3 direction) {
            ikSol.IKPosition = direction * equipmentMan.CurrentShootWeapon.Range + cam.position;
            //距離は最大距離
            rockDist = Vector3.Distance(cam.position, ikSol.IKPosition);
        }
    }
}

