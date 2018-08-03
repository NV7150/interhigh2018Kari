using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.Player {
    /// <summary>
    /// プレイヤーの能力値ではない部分のステート管理します
    /// 現在HPとかもここ（現状）
    /// </summary>
    public class PlayerStateManager : CharacterStateManager {
        /// <summary>
        /// 今が射撃中か否か
        /// </summary>
        private bool isShooting = false;

        public bool IsShooting {
            get { return isShooting; }
            set { isShooting = value; }
        }

        private bool isAiming = false;

        public bool IsAiming {
            get { return isAiming; }
            set { isAiming = value; }
        }

        private bool isSneaking = false;

        public bool IsSneaking {
            get { return isSneaking; }
            set { isSneaking = value; }
        }

        private bool isMoving = false;

        public bool IsMoving {
            get { return isMoving; }
            set { isMoving = value; }
        }

        public bool isMouseActivated = true;

        public bool IsMouseActivated {
            get { return isMouseActivated; }
            set { isMouseActivated = value; }
        }
    }
}

