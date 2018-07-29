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
        /// 反動が一定以上あるなどで射撃が封じられている場合はtrue
        /// </summary>
        private bool isRockedShooting = false;

        public bool IsRockedShooting {
            get { return isRockedShooting; }
            set { isRockedShooting = value; }
        }
        
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
    }
}

