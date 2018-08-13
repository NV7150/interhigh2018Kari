using System.Collections;
using System.Collections.Generic;
using Characters.Enemy;
using RootMotion.FinalIK;
using UnityEngine;

namespace Characters.Player{
    public class PlayerMeleeSystem : MeleeSystem {

        private PlayerAbilities abilities;
        private PlayerEquipmentManager equipMan;

        protected override float attackSpeedRate {
            get { return abilities.MeleeAttackSpeedRate; }
        }

        void Awake() {
            base.Awake();
            abilities = GetComponent<PlayerAbilities>();
            equipMan = GetComponent<PlayerEquipmentManager>();
        }
	
        // Update is called once per frame
        void Update () {
            bool atk = Input.GetButtonDown("Fire1");
            if(atk)
                attack();
        }

        public override void hit(GameObject hitObj) {
            var enemyHelth = hitObj.GetComponent<EnemyHelth>();
            var damage = equipMan.CurrentMeleeWeapon.Damage * abilities.MeleeAttack;
            Debug.Log("<color=red>"+ damage+"equip " + equipMan.CurrentMeleeWeapon.Damage + "ability " + abilities.MeleeAttack + "</color>");
            //攻撃力分のダメージを与える
            enemyHelth.damage(damage);
        }
    }
}

