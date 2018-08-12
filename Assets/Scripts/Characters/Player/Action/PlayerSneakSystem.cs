using System.Collections;
using System.Collections.Generic;
using Characters.Player;
using UnityEngine;

namespace Characters.Player {
    public class PlayerSneakSystem : MonoBehaviour {
        /// <summary>
        /// プレイヤーのGameObject
        /// </summary>
        public GameObject player;
        
        /// <summary>
        /// 音が出ている範囲を示すSphereCollider
        /// </summary>
        private SphereCollider soundCol;
	
        /// <summary>
        /// 隠密状態時、範囲が何倍されるか((1 - sneakMag) * 100% 分縮小する)
        /// </summary>
        public float sneakMag = 0.5f;
        
        /// <summary>
        /// スニークマネージャ
        /// </summary>
        private PlayerStateManager stateMan;
        
        /// <summary>
        /// 能力値
        /// </summary>
        private PlayerAbilities abilities;

        private void Awake() {
            soundCol = GetComponent<SphereCollider>();
            abilities = player.GetComponent<PlayerAbilities>();
            stateMan = player.GetComponent<PlayerStateManager>();
        }

        // Use this for initialization
        void Start () {
            soundCol.radius = abilities.SneakRad;
        }
	
        // Update is called once per frame
        void Update () {
            float radius = abilities.SneakRad;
		
            if (Input.GetButton("Sneak")) {
                //隠密状態にする
                radius *= sneakMag;
                stateMan.IsSneaking = true;
            } else {
                stateMan.IsSneaking = false;
            }

            if (stateMan.IsMoving) {
                radius *= 1.5f;
            }

            soundCol.radius = radius;
        }
    }
}

