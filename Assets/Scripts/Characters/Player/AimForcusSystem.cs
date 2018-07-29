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

        public float CurrentForcusRate {
            get { return currentForcusRate; }
        }

        /// <summary>
        /// どのくらいの速さで命中円を狭めるか
        /// 単位は割合/秒
        /// </summary>
        public float forcusSpeed = 0.5f;
        
        /// <summary>
        /// ステートマネージャ
        /// </summary>
        private PlayerStateManager stateMan;

        private void Start() {
            stateMan = GetComponent<PlayerStateManager>();
        }

        // Update is called once per frame
        void Update () {
            //マウスの動きを検出
            var xAxis = Input.GetAxis("Mouse X");
            bool isXNotMoved = Math.Abs(xAxis) < 0.1f;
		
            var yAxis = Input.GetAxis("Mouse Y");
            bool isYNotMoved = Math.Abs(yAxis) < 0.1f;
		
            //マウスが動いていない、かつ反動制御中でないなら
            if (isXNotMoved && isYNotMoved && !stateMan.IsRecoiling) {
                //命中円の割合を狭める
                currentForcusRate -= forcusSpeed * Time.deltaTime;
                currentForcusRate = (currentForcusRate > 0) ? currentForcusRate : 0f;
            } else {
                currentForcusRate = 1.0f;
            }
        }
    }
}

