using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ChessPiece;
using ClassTemp;
using Data;
using UnityEngine;
using ScriptableObject;
using ScriptableObject.ChessMapData;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Manager
{
	public class ChessGameManager : MonoBehaviourSingleton<ChessGameManager>
	{
		[SerializeField] private GameObject playerPrefab;
		[SerializeField] private GameObject enemyPrefab;
		[FormerlySerializedAs("enemySpawnPoint")] [SerializeField] private EnemyPieceSpawnPoint enemyPieceSpawnPoint;

		private int _stageNbr;
		private int _waveNbr;
		private PieceType _turn;
		private bool _isPlayerSelected;
		private PlayerPiece _player;
		private readonly List<EntityPiece> _entityPieces = new ();

		private Point _nextEnemySpawnPoint;

		private void Start()
		{
			Debug.Log("Start");
			InitGame();
		}

		private void InitGame()
		{
			_stageNbr = 0;
			_waveNbr = 0;
			_turn = PieceType.Player;
			_isPlayerSelected = false;
			InitPlayer();
			InitEnemy();
		}

		private void InitPlayer()
		{
			_turn = PieceType.Player;
			_player = Instantiate(playerPrefab, this.transform).GetComponent<PlayerPiece>();
			_player.pos = new Point(MapManager.MapSize / 2, MapManager.MapSize / 2);
			_isPlayerSelected = false;
		}

		private void InitEnemy()
		{
			MapManager.Instance.ResetTileAvailability();
			EnemySpawn();
		}

		private void CreateEnemy(EntityType type)
		{
			var p = enemyPieceSpawnPoint.SpawnPoints[Random.Range(0, enemyPieceSpawnPoint.SpawnPoints.Capacity - 1)];
			while (MapManager.Instance.IsMapAvailable(p) == false)
				p = enemyPieceSpawnPoint.SpawnPoints[Random.Range(0, enemyPieceSpawnPoint.SpawnPoints.Capacity - 1)];
			CreateEnemy(type, p);
		}

		private void CreateEnemy(EntityType type, Point p)
		{
			if (MapManager.Instance.IsMapAvailable(p) == false)
				return;
			var enemy = Instantiate(enemyPrefab).GetComponent<EntityPiece>();
			enemy.Init(type, p);
			_entityPieces.Add(enemy);
		}

		private void CreateEnemy(EntityType type, int uniqueNbr, EntityPiece subject)
		{
			var enemy = Instantiate(enemyPrefab).GetComponent<EntityPiece>();
			enemy.Init(type, uniqueNbr, subject);
			_entityPieces.Add(enemy);
		}

		public void TileSelect(Point p)
		{
			if (_turn == PieceType.Enemy)
				return;
			if (_isPlayerSelected)
				PlayerAction(p);
			MapManager.Instance.ResetTileColor();
		}

		public void OnPlayerClick()
		{
			if (_turn == PieceType.Enemy)
				return;
			_isPlayerSelected = true;
			MapManager.Instance.ResetTileColor();
			_player.HighlightAvailableTile();
		}

		public void OnEntityClick(EntityPiece piece)
		{
			if (_turn == PieceType.Enemy)
				return;
			if (_isPlayerSelected)
				TileSelect(piece.Pos);
			else
			{
				MapManager.Instance.ResetTileColor();
				piece.HighlightAvailableTile();
			}
		}

		public Point GetPlayerPos()
		{
			return _player.pos;
		}

		public EntityPiece GetEntity(Point pos)
		{
			return _entityPieces.FirstOrDefault(entity => entity.Pos == pos);
		}

		private void PlayerAction(Point p)
		{
			_isPlayerSelected = false;
			if (Point.Dist(_player.pos, p) > 1)
				return;
			PlayerMove(p);
			var encounter = CheckEnemyEncounter();
			if (encounter != null)
			{
				_entityPieces.Remove(encounter);
				encounter.Destroy();
			}
			StartCoroutine(EnemyTurn());
		}

		private EntityPiece CheckEnemyEncounter()
		{
			EntityPiece encounter = null;
			foreach (var enemy in _entityPieces.Where(enemy => enemy.Pos == _player.pos))
				encounter = enemy;
			return encounter;
		}

		private void EntityPromotion(EntityPiece reference, EntityPiece subject)
		{
			if (subject.Type is EntityType.Accept or EntityType.Depress)
			{
				_entityPieces.Remove(reference);
				reference.Destroy();
				return;
			}
			if (subject.UniqueNbr + reference.UniqueNbr >= 6)
				CreateEnemy(EntityType.Depress, subject.Pos);
			else if (subject.UniqueNbr > reference.UniqueNbr)
				CreateEnemy(subject.Type, subject.UniqueNbr + reference.UniqueNbr, subject);
			else if (subject.UniqueNbr < reference.UniqueNbr)
				CreateEnemy(reference.Type, subject.UniqueNbr + reference.UniqueNbr, subject);
			else
			{
				if (reference.Type == subject.Type)
					CreateEnemy(reference.Type, subject.UniqueNbr + reference.UniqueNbr, subject);
				else
				{
					//구현 해야함
					//임시
					CreateEnemy(reference.Type, subject.UniqueNbr + reference.UniqueNbr, subject);
				}
			}
			_entityPieces.Remove(reference);
			_entityPieces.Remove(subject);
			reference.Destroy();
			subject.Destroy();
		}

		private void EntityMove(EntityType type)
		{
			for (int i = 0; i < _entityPieces.Count; i++)
			{
				if (_entityPieces[i].Type != type || _entityPieces[i].HasMoved == true)
					continue;
				//이동 및 플레이어와의 충돌처리, HasMoved = true로 변경, 자신이 침범한 엔티티 반환
				var entityEncounter = _entityPieces[i].Action();
				if (entityEncounter == null)
					continue;
				EntityPromotion(_entityPieces[i], entityEncounter);
				i = 0;
			}
		}

		IEnumerator EnemyTurn()
		{
			yield return new WaitForSeconds(0.5f);
			MapManager.Instance.ResetTileColor();

			EntityMove(EntityType.Meme);
			EntityMove(EntityType.Anger);
			EntityMove(EntityType.Accept);
			EntityMove(EntityType.Denial);
			EntityMove(EntityType.Depress);
			EntityMove(EntityType.Bargain);

			//HasMoved 초기화
			foreach (var entity in _entityPieces)
				entity.HasMoved = false;

			yield return new WaitForSeconds(0.5f);

			//전투

			//스폰
			EnemySpawn();
		}
		private void PlayerMove(Point p)
		{
			_player.Move(CalWorldPos(p));
			_player.pos = p;
		}

		private List<EntityType> CreateRandomTable(WaveInfo waveInfo)
		{
			var randomTable = new List<EntityType>();

			
			for (var i = 0; i < waveInfo.WaveSpawnInfoList.Capacity; i++)
			{
				var spawnInfo = waveInfo.WaveSpawnInfoList[i];
				//100확률로 생성되는 오브제에 대한 처리 필요
				for (var j = 0; j < spawnInfo.chance; j++)
						randomTable.Add(spawnInfo.type);
			}
			return randomTable;
		}
		private void EnemySpawn()
		{
			Debug.Log("EnemySpawn");

			var waveInfo = ChessDataManager.Instance.GetWaveInfo(_stageNbr, _waveNbr);
			if (waveInfo == null)
				return;
			var randomTable = CreateRandomTable(waveInfo);
			var spawnList = new List<EntityType>();
			
			Debug.Log(waveInfo.TotalSpawnCnt);
			while (spawnList.Count < waveInfo.TotalSpawnCnt)
			{
				var entityType = randomTable[Random.Range(0, randomTable.Count)];
				Debug.Log(entityType);
				var entityCount = spawnList.Count(spawnEntity => spawnEntity == entityType);
				Debug.Log(entityCount);
				var entityMaxCount = waveInfo.GetEntityMaxCount(entityType);
				Debug.Log(entityMaxCount);
				if (entityMaxCount - entityCount < 1)
					continue;
				spawnList.Add(entityType);
			}

			foreach (var entity in spawnList)
			{
				CreateEnemy(entity);
			}
		}

		public static Point CalWorldPos(Point p)
		{
			return (new Point(-(MapManager.MapSize / 2) + p.x, MapManager.MapSize / 2 - p.y));
		}
	}
}