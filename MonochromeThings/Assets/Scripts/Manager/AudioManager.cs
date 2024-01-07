using System;
using ClassTemp;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Manager
{
	//유저가 조작할 수 있는 믹서그룹 볼륨변수들의 종류
	public enum VolType
	{
		Master,
		Bgm,
		Sfx,
		Voice
	}

	public class AudioManager : Singleton<AudioManager>
	{
		[SerializeField] private AudioMixer mainMixer;
		[SerializeField] private Slider masterSlider;
		[SerializeField] private Slider bgmSlider;
		[SerializeField] private Slider sfxSlider;
		[SerializeField] private Slider voiceSlider;

		//설정에 음량 슬라이더를 현재의 볼륨값으로 설정하는 함수.
		public void InitSliderData()
		{
			if (masterSlider)
				masterSlider.value = DataManager.Instance.AudioVolume.Master;
			if (bgmSlider)
				bgmSlider.value = DataManager.Instance.AudioVolume.Bgm;
			if (sfxSlider)
				sfxSlider.value = DataManager.Instance.AudioVolume.Sfx;
			if (voiceSlider)
				voiceSlider.value = DataManager.Instance.AudioVolume.Voice;
		}

		//유저가 조작할 수 있는 믹서그룹 볼륨변수들의 이름을 반환하는 함수
		private static string AudioTypeName(VolType type)
		{
			return type switch
			{
				VolType.Master => "MasterVol",
				VolType.Bgm => "BgmVol",
				VolType.Sfx => "SfxVol",
				VolType.Voice => "VoiceVol",
				_ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
			};
		}

		//마스터 볼륨 조작
		public void MasterControl()
		{
			DataManager.Instance.AudioVolume.Master = masterSlider.value;

			if (DataManager.Instance.AudioVolume.Master <= -40f)
				mainMixer.SetFloat(AudioTypeName(VolType.Master), -80);
			else
				mainMixer.SetFloat(AudioTypeName(VolType.Master), DataManager.Instance.AudioVolume.Master);
		}

		//BGM 볼륨 조작
		public void BgmControl()
		{
			DataManager.Instance.AudioVolume.Bgm = bgmSlider.value;

			if (DataManager.Instance.AudioVolume.Bgm <= -40f)
				mainMixer.SetFloat(AudioTypeName(VolType.Bgm), -80);
			else
				mainMixer.SetFloat(AudioTypeName(VolType.Bgm), DataManager.Instance.AudioVolume.Bgm);
		}

		//SFX 볼륨 조작
		public void SfxControl()
		{
			DataManager.Instance.AudioVolume.Sfx = sfxSlider.value;

			if (DataManager.Instance.AudioVolume.Sfx <= -40f)
				mainMixer.SetFloat(AudioTypeName(VolType.Sfx), -80);
			else
				mainMixer.SetFloat(AudioTypeName(VolType.Sfx), DataManager.Instance.AudioVolume.Sfx);
		}

		//음성 볼륨 조작
		public void VoiceControl()
		{
			DataManager.Instance.AudioVolume.Voice = voiceSlider.value;

			if (DataManager.Instance.AudioVolume.Voice <= -40f)
				mainMixer.SetFloat(AudioTypeName(VolType.Voice), -80);
			else
				mainMixer.SetFloat(AudioTypeName(VolType.Voice), DataManager.Instance.AudioVolume.Voice);
		}
	}
}