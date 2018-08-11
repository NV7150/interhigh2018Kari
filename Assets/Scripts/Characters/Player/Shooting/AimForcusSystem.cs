using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Player;
using UnityEngine;

namespace Characters.Player{
    public class AimForcusSystem : MonoBehaviour {
        /// <summary>
        /// 現在どのくらい命中円が狭まっているか
        /// 単位は割合
        /// </summary>
        private float currentForcusRate = 1.0f;
        
        /// <summary>
        /// ステートマネージャ
        /// </summary>
        private PlayerStateManager stateMan;
        
        /// <summary>
        /// プレイヤーの能力値
        /// </summary>
        private PlayerAbilities abilities;
        
        public float CurrentForcusRate {
            get { return currentForcusRate; }
        }
        
        private void Awake() {
            abilities = GetComponent<PlayerAbilities>();
            stateMan = GetComponent<PlayerStateManager>();
        }

        // Update is called once per frame
        void Update () {
            //条件判定
            if (judgeForcussing()) {
                //命中円の割合を狭める
                currentForcusRate -= abilities.ForcusSpeed * Time.deltaTime;
                currentForcusRate = (currentForcusRate > 0) ? currentForcusRate : 0f;
            } else {
                currentForcusRate = 1.0f;
            }
        }
        
        /// <summary>
        /// フォーカスするかどうか
        /// </summary>
        /// <returns>trueならフォーカスする</returns>
        bool judgeForcussing() {
            return isMouseDidntMove();
        }
        
        /// <summary>
        /// マウスが動いていないかを判定します
        /// </summary>
        /// <returns>この時点までのjudgeForcussingの結果</returns>
        bool isMouseDidntMove() {
            //マウスの動きを検出
            var xAxis = Input.GetAxis("Mouse X");
            bool isXNotMoved = Math.Abs(xAxis) < 0.1f;
		
            var yAxis = Input.GetAxis("Mouse Y");
            bool isYNotMoved = Math.Abs(yAxis) < 0.1f;

            if (!isXNotMoved || !isYNotMoved) {
                return false;
            }

            return isPlayerDidntMove();
        }
        
        /// <summary>
        /// プレイヤーが動いているかを判定します
        /// </summary>
        /// <returns>この時点までのjudgeForcussingの結果</returns>
        bool isPlayerDidntMove() {
            var h = Input.GetAxis("Horizontal");
            var v = Input.GetAxis("Vertical");
            if (Math.Abs(h) > 0.1f || Math.Abs(v)> 0.1f)
                return false;
            return isntRecoiling();
        }
        
        /// <summary>
        /// リコイル影響下かどうかを判定します
        /// </summary>
        /// <returns>この時点までのjudgeForcussingの結果</returns>
        bool isntRecoiling() {
            if (stateMan.IsRecoilEffecting)
                return false;
            return isAiming();
        }

        
        /// <summary>
        /// エイム動作中かどうかを判定します
        /// </summary>
        /// <returns>この時点までのjudgeForcussingの結果</returns>
        bool isAiming() {
            //注：フォーカスはエイム中のみ
            if (!stateMan.IsAiming)
                return false;
            return true;
        }
    }
}

