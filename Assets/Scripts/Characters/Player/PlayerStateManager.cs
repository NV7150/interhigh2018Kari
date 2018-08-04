using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.Player {
    /// <summary>
    /// プレイヤーの能力値ではない部分のステートを一括管理します
    /// </summary>
    public class PlayerStateManager : MonoBehaviour {
        /// <summary>
        /// 今が射撃中か否か
        /// </summary>
        private bool isShooting = false;

        public bool IsShooting {
            get { return isShooting; }
            set { isShooting = value; }
        }
        
        /// <summary>
        /// 今がエイム中か否か
        /// </summary>
        private bool isAiming = false;

        public bool IsAiming {
            get { return isAiming; }
            set { isAiming = value; }
        }
        
        /// <summary>
        /// 今がスニーク中か否か
        /// </summary>
        private bool isSneaking = false;

        public bool IsSneaking {
            get { return isSneaking; }
            set { isSneaking = value; }
        }
        
        /// <summary>
        /// 今が移動中か否か
        /// </summary>
        private bool isMoving = false;

        public bool IsMoving {
            get { return isMoving; }
            set { isMoving = value; }
        }
        
        /// <summary>
        /// マウス動作を行うか否か
        /// </summary>
        public bool isMouseActivated = true;

        public bool IsMouseActivated {
            get { return isMouseActivated; }
            set { isMouseActivated = value; }
        }
        
        /// <summary>
        /// 今反動影響下にあるか否か
        /// </summary>
        private bool isRecoilEffecting = false;

        public bool IsRecoilEffecting {
            get { return isRecoilEffecting; }
            set { isRecoilEffecting = value; }
        }
    }
}

