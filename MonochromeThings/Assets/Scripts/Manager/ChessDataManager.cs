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
		[SerializeField] private List<StageWaveInfo> stageWaveInfoList;

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
	}
}