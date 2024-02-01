using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ClassTemp;
using Data;
using UnityEngine;
using DG.Tweening;
using Unity.Mathematics;
using Random = UnityEngine.Random;

namespace Manager
{
	internal class PlayerData
	{
		public Point Pos;
		public GameObject Object;
		public PlayerPiece Controller;
		public bool IsSelected;
	}

	public class ChessGameManager : MonoBehaviourSingleton<ChessGameManager>
	{
		[SerializeField] private GameObject playerPrefab;
		[SerializeField] private GameObject enemyPrefab;

		private readonly PlayerData _player = new ();
		private readonly List<EnemyPiece> _enemyPieces = new ();

		private int _turnToSpawn;

		private void Start()
		{
			InitPlayer();
			InitEnemy();
			_turnToSpawn = 4;
			GameObject.Find("MapUIManager").GetComponent<MapUIManager>().SetTurnToSpawn(_turnToSpawn);
		}

		private void InitPlayer()
		{
			_player.Object = Instantiate(playerPrefab, this.transform);
			_player.Controller = _player.Object.GetComponent<PlayerPiece>();
			_player.Pos = new Point(MapManager.MapSize / 2, MapManager.MapSize / 2);
			_player.IsSelected = false;
		}
		
		private void PlayerTurn(Point p)
		{
			if (p.x == _player.Pos.x)
			{
				if (math.abs(_player.Pos.y - p.y) > 1)
					return;
				PlayerMove(p);
				StartCoroutine(EnemyTurn());
			}
			else if (p.y == _player.Pos.y)
			{
				if (math.abs(_player.Pos.x - p.x) > 1)
					return;
				PlayerMove(p);
				StartCoroutine(EnemyTurn());
			}
		}
		
		private void PlayerMove(Point p)
		{
			_player.Controller.Move(CalWorldPos(p));
			MapManager.Instance.UpdateTileState(_player.Pos, PieceType.Empty);
			_player.Pos = p;
			MapManager.Instance.UpdateTileState(_player.Pos, PieceType.Player);
			EnemyPiece collidedEnemy = null;
			foreach (var enemy in _enemyPieces.Where(enemy => _player.Pos == enemy.pos))
			{
				collidedEnemy = enemy;
			}
			if (collidedEnemy != null)
			{
				_enemyPieces.Remove(collidedEnemy);
				var enemyTransform = collidedEnemy.transform;
				enemyTransform.DOPause();
				Destroy(enemyTransform.gameObject);
			}
		}

		private void InitEnemy()
		{
			MapManager.Instance.ResetTileState();
			CreateEnemy(new Point(0, 0));
			CreateEnemy(new Point(0, MapManager.MapSize - 1));
			CreateEnemy(new Point(MapManager.MapSize - 1, 0));
			CreateEnemy(new Point(MapManager.MapSize - 1, MapManager.MapSize - 1));
		}

		private void CreateEnemy()
		{
			var p = new Point(Random.Range(0, MapManager.MapSize), Random.Range(0, MapManager.MapSize));
			while (MapManager.Instance.IsMapEmpty(p) == false)
			{
				p.x= Random.Range(0, MapManager.MapSize);
				p.y = Random.Range(0, MapManager.MapSize);
			}
			CreateEnemy(p);
		}

		private void CreateEnemy(Point p)
		{
			if (MapManager.Instance.IsMapEmpty(p) == false)
				return;
			var enemy = Instantiate(enemyPrefab).GetComponent<EnemyPiece>();
			enemy.pos = p;
			enemy.transform.position = new Vector3(100, 100, 100);
			EnemyMove(enemy, p);
			_enemyPieces.Add(enemy);
		}

		private Point FindNearestActionsPoint(EnemyPiece enemy)
		{
			var availablePoints = EnemyDataManager.Instance.GetEnemyData(enemy.typeID).AvailablePoints;
			var distances = new float[availablePoints.Capacity];
			for (var i = 0; i < availablePoints.Capacity; i++)
				distances[i] = Point.Dist(_player.Pos, enemy.pos + availablePoints[i]);
			var minIndex = Array.IndexOf(distances, Mathf.Min(distances));

			return enemy.pos + availablePoints[minIndex];
		}
		
		IEnumerator EnemyTurn()
		{
			yield return new WaitForSeconds(0.5f);
			foreach (var enemy in _enemyPieces)
			{
				EnemyMove(enemy, FindNearestActionsPoint(enemy));
				yield return new WaitForSeconds(0.5f);
			}

			_turnToSpawn--;
			if (_turnToSpawn <= 0)
			{
				CreateEnemy();
				_turnToSpawn = 4;
			}
			GameObject.Find("MapUIManager").GetComponent<MapUIManager>().SetTurnToSpawn(_turnToSpawn);

			EnemyPiece collidedEnemy = null;
			foreach (var enemy in _enemyPieces.Where(enemy => enemy.pos == _player.Pos))
			{
				collidedEnemy = enemy;
			}
			if (collidedEnemy != null)
			{
				_enemyPieces.Remove(collidedEnemy);
				var enemyTransform = collidedEnemy.transform;
				enemyTransform.DOPause();
				Destroy(enemyTransform.gameObject);
			}
		}

		private void EnemyMove(EnemyPiece enemyPiece, Point p)
		{
			if (MapManager.Instance.GetPieceType(p) == PieceType.Enemy)
				return;
			enemyPiece.Move(CalWorldPos(p));
			MapManager.Instance.UpdateTileState(enemyPiece.pos, PieceType.Empty);
			enemyPiece.pos = p;
			MapManager.Instance.UpdateTileState(enemyPiece.pos, PieceType.Enemy);
		}

		public void TileSelect(Point p)
		{
			MapManager.Instance.ResetTileColor();
			if (_player.IsSelected)
			{
				_player.IsSelected = false;
				PlayerTurn(p);
			}
		}

		public void OnPlayerClick()
		{
			_player.IsSelected = true;
			MapManager.Instance.HighlightAvailableTile(_player.Pos);
		}

		public static Point CalWorldPos(Point p)
		{
			return (new Point(-(MapManager.MapSize / 2) + p.x, MapManager.MapSize / 2 - p.y));
		}
	}
}