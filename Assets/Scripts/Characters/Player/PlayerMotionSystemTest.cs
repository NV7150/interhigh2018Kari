using System.Collections;
using System.Collections.Generic;
using RootMotion.FinalIK;
using UnityEngine;
using UnityEngine.Networking;

namespace Characters.Player {
    public class PlayerMotionSystemTest : PlayerMotionSystem{
        
        /// <summary>
        /// プレイヤーのAnimator
        /// </summary>
        public Animator _animator;

        private void Start() {}

        // Update is called once per frame
        void FixedUpdate() {
            base.FixedUpdate();
           
        }

        protected override void moveForward() {
            //Runningフラグが真でない場合はRunningフラグをtrueに
            if (!_animator.GetBool("Running"))
                _animator.SetBool("Running", true);
            //前進（アニメーションに因らない)
            GetComponent<CharacterController>().SimpleMove(transform.forward * 10);
        }

        protected override void moveRight() {
//            throw new System.NotImplementedException();
        }

        protected override void moveLeft() {
//            throw new System.NotImplementedException();
        }

        protected override void moveBack() {
//            throw new System.NotImplementedException();
        }

        protected override void turn() {
            //マウスから回転
            var mouseAngle = Input.GetAxis("Mouse X") * Time.deltaTime * 300;
            transform.Rotate(0,mouseAngle,0);
        }
    }
}


