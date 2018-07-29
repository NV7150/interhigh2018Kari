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

        private bool isShooting = false;

        public bool IsShooting {
            get { return isShooting; }
            set { isShooting = value; }
        }
    }
}

