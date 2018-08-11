using System.Collections;
using System.Collections.Generic;
using RootMotion.FinalIK;
using UnityEngine;

namespace Characters.Player{
    public class PlayerMeleeSystem : MonoBehaviour {
        /// <summary>
        /// アニメーターコンポーネント
        /// </summary>
        public Animator animator;

        public Camera cam;
        
        private AimIK ik;

        private PlayerAbilities abilities;

        public Animation attack;
        
        void Awake() {
            ik = GetComponent<AimIK>();
            abilities = GetComponent<PlayerAbilities>();
        }
	
        // Update is called once per frame
        void Update () {
            bool atk = Input.GetButton("Fire1");
            if(atk)
                Attack();
            searchAim();
        }
        
        /// <summary>
        ///　攻撃を行います
        /// </summary>
        void Attack() {
            attack["Attack"].speed = abilities.MeleeAttackSpeedRate;
            animator.SetBool("Attack",true);
            
        }
        
        /// <summary>
        /// 銃口の向き（AimIKの位置）を修正します。
        /// update毎に呼ばれます。
        /// </summary>
        void searchAim() {
            //カメラからRayを発信
            var ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;
            //とりあえずPlayer以外なら命中判定
            if (Physics.Raycast(ray, out hit,100) && !hit.collider.gameObject.CompareTag("Player")) {
                //物体に当たればそっちを向く
                ik.solver.IKPosition = hit.point;
                //距離を更新
            } else {
                //Rayの終端を計算//
                //元となるベクトル（射程）
                var vector = new Vector3(0,0,100);
                //射程をカメラの回転度分回転
                vector = cam.transform.rotation * vector;
                //カメラのpositionを加算
                vector = cam.transform.position + vector;
			
                //Rayの終端を向く
                ik.solver.IKPosition = vector;
                //距離は最大距離
            }
        }
    }
}

