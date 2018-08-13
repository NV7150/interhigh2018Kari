using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.Enemy {
    public class EnemyStateManager : MonoBehaviour {
        /// <summary>
        /// 現在のエネミーのステート
        /// </summary>
        private EnemyState state = EnemyState.FINDING;
        
        /// <summary>
        /// 現在発見したプレイヤー
        /// </summary>
        private Transform player;

        public EnemyState State {
            get { return state; }
            set { state = value; }
        }
        
        public Transform Player {
            get { return player; }
            set { player = value; }
        }
    }
}

