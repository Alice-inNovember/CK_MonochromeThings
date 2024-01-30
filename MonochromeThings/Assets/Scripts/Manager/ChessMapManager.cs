using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using Unity.Mathematics;
using Unity.VisualScripting;
using Random = UnityEngine.Random;

namespace Manager
{
	public enum PieceType
	{
		Player,
		Enemy,
		Empty
	}

	internal class TileData
	{
		public Tile[,] TileArray;
		public GameObject[,] ObjArray;
	}

	internal class PlayerData
	{
		public int X, Y;
		public GameObject Object;
		public PlayerPieceController Controller;
		public bool IsSelected;
	}

	public class ChessMapManager : ClassTemp.Singleton<ChessMapManager>
	{
		[SerializeField] private GameObject playerPrefab;
		[SerializeField] private GameObject enemyPrefab;
		[SerializeField] private GameObject tilePrefab;
		
		private const int MapSize = 7;
		private readonly PieceType[,] _chessMap = new PieceType[MapSize, MapSize];
		private PieceType _turn = PieceType.Empty;

		private readonly TileData _tile = new ();
		private readonly PlayerData _player = new ();
		private readonly List<EnemyPiece> _enemyPieces = new ();

		private int _turnToSpawn;

		private void Start()
		{
			InitTile();
			InitPlayer();
			InitEnemy();
			_turnToSpawn = 4;
			GameObject.Find("ChessMapUIManager").GetComponent<ChessMapUIManager>().SetTurnToSpawn(_turnToSpawn);
		}

		private void InitTile()
		{
			_tile.TileArray = new Tile[MapSize, MapSize];
			_tile.ObjArray = new GameObject[MapSize, MapSize];
			for (var x = 0; x < MapSize; x++)
			{
				for (var y = 0; y < MapSize; y++)
				{
					_chessMap[x, y] = PieceType.Empty;
					_tile.ObjArray[x, y] = Instantiate(tilePrefab, this.transform);
					_tile.ObjArray[x, y].transform.position = new Vector3(-(MapSize / 2) + x, MapSize / 2 - y, 0);
					_tile.ObjArray[x, y].name = "Tile " + "[" + x + "," + y + "]";
					_tile.TileArray[x, y] = _tile.ObjArray[x, y].GetComponent<Tile>();
					_tile.TileArray[x, y].Init(x, y, this);
				}
			}
		}

		private void InitPlayer()
		{
			_player.Object = Instantiate(playerPrefab, this.transform);
			_player.Controller = _player.Object.GetComponent<PlayerPieceController>();
			_player.X = MapSize / 2;
			_player.Y = MapSize / 2;
			_player.IsSelected = false;
			_chessMap[_player.X, _player.Y] = PieceType.Player;
			_turn = PieceType.Player;
		}

		private void InitEnemy()
		{
			CreateEnemy(0, 0);
			CreateEnemy(0, MapSize - 1);
			CreateEnemy(MapSize - 1, 0);
			CreateEnemy(MapSize - 1, MapSize - 1);
		}

		private void CreateEnemy()
		{
			var x = Random.Range(0, MapSize);
			var y = Random.Range(0, MapSize);
			while (_chessMap[x, y] != PieceType.Empty)
			{
				x = Random.Range(0, MapSize);
				y = Random.Range(0, MapSize);
			}
			CreateEnemy(x, y);
		}

		private void CreateEnemy(int x, int y)
		{
			if (_chessMap[x, y] != PieceType.Empty)
				return;
			var enemy = Instantiate(enemyPrefab).GetComponent<EnemyPiece>();
			enemy.X = x;
			enemy.Y = y;
			enemy.transform.position = new Vector3(100, 100, 100);
			EnemyMove(enemy, x, y);
			_chessMap[x, y] = PieceType.Enemy;
			_enemyPieces.Add(enemy);
		}

		IEnumerator EnemyTurn()
		{
			_turn = PieceType.Enemy;
			yield return new WaitForSeconds(0.5f);
			foreach (var enemy in _enemyPieces)
			{
				var dist = new List<float>();
				dist.Add(Vector2.Distance(new Vector2(_player.X, _player.Y), new Vector2(enemy.X + 1, enemy.Y)));
				dist.Add(Vector2.Distance(new Vector2(_player.X, _player.Y), new Vector2(enemy.X - 1, enemy.Y)));
				dist.Add(Vector2.Distance(new Vector2(_player.X, _player.Y), new Vector2(enemy.X, enemy.Y + 1)));
				dist.Add(Vector2.Distance(new Vector2(_player.X, _player.Y), new Vector2(enemy.X, enemy.Y - 1)));
				var minIndex = dist.IndexOf(dist.Min());
				switch (minIndex)
				{
					case 0:
						EnemyMove(enemy, enemy.X + 1, enemy.Y);
						break;
					case 1:
						EnemyMove(enemy, enemy.X - 1, enemy.Y);
						break;
					case 2:
						EnemyMove(enemy, enemy.X, enemy.Y + 1);
						break;
					case 3:
						EnemyMove(enemy, enemy.X, enemy.Y - 1);
						break;
				}
			}
			yield return new WaitForSeconds(0.5f);
			EnemyPiece collidedEnemy = null;
			foreach (var enemy in _enemyPieces.Where(enemy => _player.X == enemy.X && _player.Y == enemy.Y))
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
			_turnToSpawn--;
			if (_turnToSpawn <= 0)
			{
				CreateEnemy();
				_turnToSpawn = 4;
			}
			GameObject.Find("ChessMapUIManager").GetComponent<ChessMapUIManager>().SetTurnToSpawn(_turnToSpawn);
			_turn = PieceType.Player;
		}

		private void EnemyMove(EnemyPiece enemyPiece, int x, int y)
		{
			if (_chessMap[x, y] == PieceType.Enemy)
				return;
			enemyPiece.Move(-(MapSize / 2) + x, MapSize / 2 - y);
			_chessMap[enemyPiece.X, enemyPiece.Y] = PieceType.Empty;
			enemyPiece.X = x;
			enemyPiece.Y = y;
			_chessMap[enemyPiece.X, enemyPiece.Y] = PieceType.Enemy;
		}

		public void TileSelect(int x, int y)
		{
			UpdateTileColor();
			if (_player.IsSelected)
				PlayerTurn(x, y);
			_player.IsSelected = false;
		}

		private void PlayerMove(int x, int y)
		{
			_player.Controller.Move(-(MapSize / 2) + x, MapSize / 2 - y);
			_chessMap[_player.X, _player.Y] = PieceType.Empty;
			_player.X = x;
			_player.Y = y;
			_chessMap[_player.X, _player.Y] = PieceType.Player;
			EnemyPiece collidedEnemy = null;
			foreach (var enemy in _enemyPieces.Where(enemy => _player.X == enemy.X && _player.Y == enemy.Y))
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

		private void PlayerTurn(int x, int y)
		{
			if (_player.IsSelected == false)
				return;
			if (x == _player.X)
			{
				if (math.abs(_player.Y - y) > 1)
					return;
				PlayerMove(x, y);
				StartCoroutine(EnemyTurn());
			}
			else if (y == _player.Y)
			{
				if (math.abs(_player.X - x) > 1)
					return;
				PlayerMove(x, y);
				StartCoroutine(EnemyTurn());
			}
		}

		public void OnPlayerClick()
		{
			if (_turn != PieceType.Player)
				return;
			_player.IsSelected = true;
			HighlightAvailableTile();
		}

		private void UpdateTileColor()
		{
			ResetTileColor();
		}

		private void HighlightAvailableTile()
		{
			ChangeTileColor(_player.X + 1, _player.Y, new Color(0.5f, 1, 0.5f));
			ChangeTileColor(_player.X - 1, _player.Y, new Color(0.5f, 1, 0.5f));
			ChangeTileColor(_player.X, _player.Y + 1, new Color(0.5f, 1, 0.5f));
			ChangeTileColor(_player.X, _player.Y - 1, new Color(0.5f, 1, 0.5f));
		}

		private void ResetTileColor()
		{
			for (var y = 0; y < MapSize; y++)
				for (var x = 0; x < MapSize; x++)
					ChangeTileColor(x, y, Color.white);
		}
		
		private void ChangeTileColor(int x, int y, Color color)
		{
			if (x is < 0 or >= MapSize)
				return;
			if (y is < 0 or >= MapSize)
				return;
			_tile.TileArray[x, y].ChangeColor(color);
		}
	}
}