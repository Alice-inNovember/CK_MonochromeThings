using System.IO;
using ClassTemp;
using Data;
using UnityEngine;

namespace Manager
{
	public class DataManager : MonoBehaviourSingleton<DataManager>
	{
		[SerializeField] private string audioDataFileName = "AudioData.json";
		public AudioVolumeData AudioVolume = new();

		private void Start()
		{
			AudioManager.Instance.InitSliderData();
		}

		//음량 설정 저장하기
		public void SaveAudioData()
		{
			var toJsonData = JsonUtility.ToJson(AudioVolume, true);
			var filePath = Application.persistentDataPath + "/" + audioDataFileName;

			File.WriteAllText(filePath, toJsonData);
			Debug.Log("Saved AudioData at : < " + filePath + " >");
		}

		//음량 설정 불러오기
		public void LoadAudioData()
		{
			var filePath = Application.persistentDataPath + "/" + audioDataFileName;

			//저장된 게임이 있다면
			if (!File.Exists(filePath))
				return;

			var fromJsonData = File.ReadAllText(filePath);
			AudioVolume = JsonUtility.FromJson<AudioVolumeData>(fromJsonData);
			Debug.Log("Loaded AudioData");

			if (AudioManager.Instance != null)
				AudioManager.Instance.InitSliderData();
		}
	}
}