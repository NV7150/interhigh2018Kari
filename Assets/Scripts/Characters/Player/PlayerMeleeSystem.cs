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
	
        // Update is called once per frame
        void Update () {
            float atk = Input.GetAxis("Fire1");
            if(atk == 1f)
                Attack();
        }
        
        /// <summary>
        ///　攻撃を行います
        /// </summary>
        void Attack(){
           animator.SetBool("Attack",true);
        }
    }
}

