using System;
using System.Collections.Generic;
using ClassTemp;
using Data;
using ScriptableObject;
using UnityEngine;

namespace Manager
{
	public class EntityDataManager : MonoBehaviourSingleton<EntityDataManager>
	{
		[SerializeField] private List<EntityPieceData> enemyDataList;
		private Dictionary<int, EntityPieceData> _enemyDataDictionary;

		private void Start()
		{
			_enemyDataDictionary = new Dictionary<int, EntityPieceData>();
			foreach (var data in enemyDataList)
			{
				_enemyDataDictionary.Add(data.Id, data);
			}
		}

		public EntityPieceData GetEntityData(int enemyID)
		{
			return _enemyDataDictionary[enemyID];
		}
	}
}