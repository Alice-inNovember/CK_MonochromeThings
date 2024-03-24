using System.Collections.Generic;
using ChessPiece;
using Data;
using UnityEngine;

namespace ScriptableObject
{
	[CreateAssetMenu(fileName = "EnemyPieceData", menuName = "Scriptable Object/Enemy Piece Data",
		order = int.MaxValue)]
	[System.Serializable]
	public class EntityData : UnityEngine.ScriptableObject
	{
		[SerializeField] private EntityType type;
		[SerializeField] private string enemyName;
		[SerializeField] private int uniqueNbr;
		[SerializeField] private int minTurnToMove;
		[SerializeField] private int maxTurnToMove;
		[SerializeField] private List<Point> availablePoints;
		[SerializeField] private List<int> spawnCount;

		public EntityType Type => type;
		public string Name => enemyName;
		public int UniqueNbr => uniqueNbr;
		public int MinTurnToMove => minTurnToMove;
		public int MaxTurnToMove => maxTurnToMove;
		public List<Point> AvailablePoints => availablePoints;
		public List<int> SpawnCount => spawnCount;
	}
}