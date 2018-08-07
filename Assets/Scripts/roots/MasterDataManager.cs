using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MasterDataManager{
	/// <summary>
	/// string配列での生のデータ
	/// </summary>
	List<Dictionary<string,string>> rawData = new List<Dictionary<string, string>>();

	private int maxId;
	
	public int MaxId {
		get { return maxId; }
	}

	protected void loadData(string path) {
		//元データをロードしてstirng二次元配列に
		var csvAsset = (TextAsset) Resources.Load(path);
		var csvArray = CSVReader.SplitCsvGrid(csvAsset.text);
		
		//1段目をキーの名前として読み込む
		List<string> keys = new List<string>();
		//0段目を取得
		string[] firstLine = relineArray(0, csvArray);
		for(int index = 0;index < firstLine.Length;index++) {
			keys.Add(firstLine[index]);
		}
		
		int i = 1;
		//2段目以降を順次追加
		for (; i < csvArray.GetLength(1) - 1; i++) {
			//i番目を取得
			string[] iLine = relineArray(i, csvArray);
			//i番目のDictionaryを追加
			rawData.Add(new Dictionary<string, string>());
			for (int j = 0; j < iLine.Length; j++) {
				//Dictionaryにデータを追加(先頭抜かしなのでi-1)
				var key = keys[j];
				var data = iLine[j];				
				rawData[i - 1].Add(key,data);
			}
		}
		
		//IDの最大値を設定
		maxId = i - 1;
	}
	
	/// <summary>
	/// id番目のデータのparamNameというパラメータを取得します
	/// </summary>
	/// <param name="id">取得したいデータのID</param>
	/// <param name="paramName">取得したいパラメータの名前</param>
	/// <returns>string型のパラメータ</returns>
	protected string getRawParam(int id,string paramName) {
		return rawData[id][paramName];
	}

	string[] relineArray(int row,string[,] original) {
		string[] relinedArray = new string[original.GetLength(0) - 2];
		for (int i = 0; i < relinedArray.Length; i++) {
			relinedArray[i] = original[i,row];
		}

		return relinedArray;
	}
}
