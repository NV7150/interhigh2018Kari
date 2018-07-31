using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.Player {
    /// <summary>
    /// プレイヤーの能力値ではない部分のステート管理します
    /// 現在HPとかもここ（現状）
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
        /// カメラが今反動の影響を受けているか否か
        /// </summary>
        private bool isRecoiling = false;

        public bool IsRecoiling {
            get { return isRecoiling; }
            set { isRecoiling = value; }
        }
        
        /// <summary>
        /// 現在エイムしているか
        /// </summary>
        private bool isAiming = false;

        public bool IsAiming {
            get { return isAiming; }
            set { isAiming = value; }
        }
        
    }
}

