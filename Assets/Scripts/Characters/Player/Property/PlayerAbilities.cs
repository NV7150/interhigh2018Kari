using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.Player {
    public class PlayerAbilities : Ability {
		/// <summary>
		/// 筋力を１増加させます
		/// </summary>
		public void plusStr() {
			Strength += 1;
			
			defStrParam();
		}
		
		/// <summary>
		/// 技巧を１増加させます
		/// </summary>
		public void plusTech() {
			Technic += 1;
			
			defTechParam();
		}
	    
		/// <summary>
		/// 敏捷性を１増加させます
		/// </summary>
		public void plusAgi() {
			Agility += 1;
			
			defAgiParam();
		}
	    
	    /// <summary>
	    /// 体力を１増加させます
	    /// </summary>
	    public void plusTough() {
		    Toughness += 1;
		    
		    defToughParam();
	    }
	}
}
