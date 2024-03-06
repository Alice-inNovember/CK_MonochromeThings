using System;
using System.Collections.Generic;
using ClassTemp;
using Data;
using ScriptableObject;
using UnityEngine;

namespace Manager
{
	public class EnemyDataManager : MonoBehaviourSingleton<EnemyDataManager>
	{
		[SerializeField] private List<EnemyPieceData> enemyDataList;
		private Dictionary<int, EnemyPieceData> _enemyDataDictionary;

		private void Start()
		{
			_enemyDataDictionary = new Dictionary<int, EnemyPieceData>();
			foreach (var data in enemyDataList)
			{
				_enemyDataDictionary.Add(data.Id, data);
			}
		}

		public EnemyPieceData GetEnemyData(int enemyID)
		{
			return _enemyDataDictionary[enemyID];
		}
	}
}