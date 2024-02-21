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
		[SerializeField] private List<EnemyData> enemyDataList;
		private Dictionary<int, EnemyData> _enemyDataDictionary;

		private void Start()
		{
			_enemyDataDictionary = new Dictionary<int, EnemyData>();
			foreach (var data in enemyDataList)
			{
				_enemyDataDictionary.Add(data.Id, data);
			}
		}

		public EnemyData GetEnemyData(int enemyID)
		{
			return _enemyDataDictionary[enemyID];
		}
	}
}