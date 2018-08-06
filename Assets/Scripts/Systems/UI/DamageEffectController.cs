using System.Collections;
using System.Collections.Generic;
using Characters.Player;
using UnityEngine;
using UnityEngine.UI;

public class DamageEffectController : MonoBehaviour {
	/// <summary>
	/// 画面周囲につく血のエフェクト
	/// </summary>
	public Image bloodImage;
	
	/// <summary>
	/// 画面全体を覆う赤のエフェクト
	/// </summary>
	public Image screenImage;
	
	public PlayerHealth health;
	
	/// <summary>
	/// bloodImage用のカラー
	/// 全10段階
	/// </summary>
	private List<Color> bloodImageColors = new List<Color>();
	
	/// <summary>
	/// screenImage用のカラー
	/// 全10段階
	/// </summary>
	private List<Color> screenImageColors = new List<Color>();
	
	// Use this for initialization
	void Start () {
		setupBloodColors();
		setupScreenColors();
	}

	void setupBloodColors() {
		var imageColor = bloodImage.color;
		for (int i = 0; i < 100; i++) {
			imageColor.a = i * 0.01f;
			bloodImageColors.Add(imageColor);
		}
	}

	void setupScreenColors() {
		var imageColor = screenImage.color;
		for (int i = 0; i < 100; i++) {
			imageColor.a = i * 0.01f / 4;
			screenImageColors.Add(imageColor);
		}
	}
	
	// Update is called once per frame
	void Update () {
		changeBloodAlpha();
		changeScreenAlpha();
	}
	
	/// <summary>
	/// bloodImageのアルファ値を更新します
	/// </summary>
	void changeBloodAlpha() {
		Debug.Log("index" + judgeIndex());
		bloodImage.color = bloodImageColors[judgeIndex()];
	}
	
	/// <summary>
	/// screenImageのアルファ値を更新します
	/// </summary>
	void changeScreenAlpha() {
		screenImage.color = screenImageColors[judgeIndex()];
	}

	int judgeIndex() {
		for (int i = 0; i < 100; i++) {
			float val = (100f - i) / 100f;
			if (health.Hp >= val) {
				return i;
			}
		}

		return 99;
	}
}
