using System.Collections.Generic;
using Data;
using UnityEngine;

namespace ScriptableObject.ChessMapData
{
	[CreateAssetMenu(fileName = "Sn Info", menuName = "Scriptable Object/Spawn Info/Stage Info", order = int.MaxValue)]
	[System.Serializable]
	public class StageInfo : UnityEngine.ScriptableObject
	{
		[SerializeField] private List<WaveInfo> waveInfoList;
		[SerializeField] private int spawnCntIncrement;
		public int SpawnCntIncrement => spawnCntIncrement;
		public List<WaveInfo> WaveInfoList => waveInfoList;
	}
}