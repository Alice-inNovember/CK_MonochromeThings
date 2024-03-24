using System.Collections.Generic;
using Data;
using UnityEngine;

namespace ScriptableObject.ChessMapData
{
	[CreateAssetMenu(fileName = "StageWaveInfo", menuName = "Scriptable Object/Spawn Info/Stage Wave Info", order = int.MaxValue)]
	[System.Serializable]
	public class StageWaveInfo : UnityEngine.ScriptableObject
	{
		[SerializeField] private int stageNbr;
		[SerializeField] private List<int> waveSpawnCnt;
		[SerializeField] private int incrementValueAfterMax;

		public int StageNbr => stageNbr;
		public List<int> WaveSpawnCnt => waveSpawnCnt;
		public int IncrementValueAfterMax => incrementValueAfterMax;
	}
}