using System;
using System.Collections.Generic;
using System.Linq;
using ChessPiece;
using ChessPiece.AI;
using ClassTemp;
using ScriptableObject;
using ScriptableObject.ChessMapData;
using UnityEngine;
using UnityEngine.Serialization;

namespace Manager
{
	public class ChessDataManager : MonoBehaviourSingleton<ChessDataManager>
	{
		[SerializeField] private List<EntityData> entityDataList;
		[SerializeField] private List<StageInfo> stageInfoList;

		public EntityData GetEntityData(EntityType type)
		{
			return entityDataList.FirstOrDefault(data => data.Type == type);
		}

		public EntityAi GetEntityAi(EntityType type)
		{
			return type switch
			{
				EntityType.Meme => new Meme(),
				EntityType.Denial => new Denial(),
				EntityType.Anger => new Anger(),
				EntityType.Bargain => new Bargain(),
				EntityType.Depress => new Depress(),
				EntityType.Accept => new Accept(),
				EntityType.Store => new Store(),
				EntityType.ShelterSafe => new ShelterSafe(),
				EntityType.ShelterCaptured => new ShelterCaptured(),
				_ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
			};
		}

		public StageInfo GetStageInfo(int stageNbr)
		{
			if (stageNbr >= 0 && stageNbr < stageInfoList.Count)
				return stageInfoList[stageNbr];
			return null;
		}

		public WaveInfo GetWaveInfo(int stageNbr, int waveNbr)
		{
			var stageInfo = GetStageInfo(stageNbr);
			if (stageInfo == null || waveNbr < 0)
				return null;

			var waveInfoList = stageInfo.WaveInfoList;
			if (waveNbr < waveInfoList.Count)
				return waveInfoList[waveNbr];

			var waveInfo = UnityEngine.ScriptableObject.CreateInstance<WaveInfo>();
			waveInfo.WaveSpawnInfoList = waveInfoList.Last().WaveSpawnInfoList.ToList();
			waveInfo.TotalSpawnCnt = ((waveNbr - waveInfoList.Count) * stageInfo.SpawnCntIncrement) + waveInfoList.Last().TotalSpawnCnt;
			return waveInfo;
		}
	}
}