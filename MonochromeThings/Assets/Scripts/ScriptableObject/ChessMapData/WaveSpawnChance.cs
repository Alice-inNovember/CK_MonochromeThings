using System.Collections.Generic;
using ChessPiece;
using Data;
using UnityEngine;

namespace ScriptableObject.ChessMapData
{
	[CreateAssetMenu(fileName = "Sn Wn ChanceInfo", menuName = "Scriptable Object/Spawn Info/Wave Spawn Chance Info", order = int.MaxValue)]
	[System.Serializable]
	public class WaveSpawnChanceInfo : UnityEngine.ScriptableObject
	{
		[SerializeField] private List<EntitySpawnInfo> waveSpawnInfoList;
	}
}

namespace Data
{
	[System.Serializable]
	class EntitySpawnInfo
	{
		public EntityType type;
		public float chance;
		public int maxSpawnCnt;
	}
}