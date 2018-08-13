using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.Enemy {
    public class EnemyHelth : MonoBehaviour {
        /// <summary>
        /// 残りHP
        /// </summary>
        private float hp = 1.0f;
        
        /// <summary>
        /// 自然回復が発生するまでの時間を計測するタイマー
        /// </summary>
        private float recoveryTimer;

        
        private EnemyStateManager stateMan;
        private EnemyAbilities abilities;
        

        private void Awake() {
            stateMan = GetComponent<EnemyStateManager>();
            abilities = GetComponent<EnemyAbilities>();
        }
        
        // Update is called once per frame
        void Update () {
            if(stateMan.State != EnemyState.DEAD)
                autoHeal();
            Debug.Log("HP : " + hp);
        }
        
        /// <summary>
        /// 指定数値分のダメージを受けます
        /// </summary>
        /// <param name="damage">ダメージ</param>
        public void damage(float damage) {
            hp -= damage * abilities.Protect;
            if (hp <= 0) {
                dead();
                hp = 0;
            }
            
            //自然回復までの残り時間を設定
            recoveryTimer = abilities.RecoveryStart;
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
                hp += abilities.RecoveryRate * Time.deltaTime;
                hp = (hp < 1.0f) ? hp : 1.0f;
            } else {
                //自然回復までの残り時間を減らす
                recoveryTimer -= Time.deltaTime;
            }
        }
    }
}
