using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.Enemy {
    public class EnemyHelth : MonoBehaviour {
        //仮設定
        public float recoveryRate;
        public float recoverySpeed;

        /// <summary>
        /// 残りHP
        /// </summary>
        private float hp;
        
        /// <summary>
        /// 自然回復が発生するまでの時間を計測するタイマー
        /// </summary>
        private float recoveryTimer;

        private EnemyStateManager stateMan;

        private void Awake() {
            stateMan = GetComponent<EnemyStateManager>();
        }

        private void Start() {
            hp = 1.0f;
        }

        // Update is called once per frame
        void Update () {
            if(stateMan.State != EnemyState.DEAD)
                autoHeal();
        }
        
        /// <summary>
        /// 指定数値分のダメージを受けます
        /// </summary>
        /// <param name="damage">ダメージ</param>
        public void damage(float damage) {
            hp -= damage;
            if (hp <= 0) {
                dead();
                hp = 0;
            }
            
            //自然回復までの残り時間を設定
            recoveryTimer = recoverySpeed;
        }
        
        /// <summary>
        /// 死にます
        /// </summary>
        void dead() {
            stateMan.State = EnemyState.DEAD;
            Debug.Log("dead");
            //仮
            Destroy(gameObject);
        }

        /// <summary>
        /// 自然回復処理を行います
        /// </summary>
        void autoHeal() {
            if (recoveryTimer < 0) {
                //自然回復
                hp += recoveryRate * Time.deltaTime;
                hp = (hp < 1.0f) ? hp : 1.0f;
            } else {
                //自然回復までの残り時間を減らす
                recoveryTimer -= Time.deltaTime;
            }
        }
    }
}
