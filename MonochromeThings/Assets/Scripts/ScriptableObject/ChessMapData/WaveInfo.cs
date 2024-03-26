using System.Collections.Generic;
using System.Linq;
using ChessPiece;
using Data;
using UnityEngine;

namespace ScriptableObject.ChessMapData
{
	[CreateAssetMenu(fileName = "Sn Wn Info", menuName = "Scriptable Object/Spawn Info/Wave Info", order = int.MaxValue)]
	[System.Serializable]
	public class WaveInfo : UnityEngine.ScriptableObject
	{
		[SerializeField] private int totalSpawnCnt;
		[SerializeField] private List<EntitySpawnInfo> waveSpawnInfoList;

		public int TotalSpawnCnt
		{
			get => totalSpawnCnt;
			set => totalSpawnCnt = value;
		}

		public List<EntitySpawnInfo> WaveSpawnInfoList
		{
			get => waveSpawnInfoList;
			set => waveSpawnInfoList = value;
		}

		public int GetEntityMaxCount(EntityType type)
		{
			return (from spawnInfo in waveSpawnInfoList where spawnInfo.type == type select spawnInfo.maxSpawnCnt).FirstOrDefault();
		}
	}
}

namespace Data
{
	[System.Serializable]
	public class EntitySpawnInfo
	{
		public EntityType type;
		public int chance;
		public int maxSpawnCnt;
	}
}