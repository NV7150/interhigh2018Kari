﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Characters.Player;
using UnityEngine.UI;

public class abilityTest : MonoBehaviour {
	public PlayerAbilities Abilities;
	public Text text;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		text.text = "str:" + Abilities.Strength + " tech:" + Abilities.Technic + " agi:" + Abilities.Agility + " tough:" + Abilities.Toughness;
		if (Input.GetKeyDown(KeyCode.R)) {
			Abilities.plusStr();
		}

		if (Input.GetKeyDown(KeyCode.T)) {
			Abilities.plusTech();
		}

		if (Input.GetKeyDown(KeyCode.G)) {
			Abilities.plusAgi();
		}

		if (Input.GetKeyDown(KeyCode.O)) {
			Abilities.plusTough();
		}
	}
}
