using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data;
using ClassTemp;
using UnityEngine;
using DG.Tweening;
using Unity.Mathematics;
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

	public class MapManager : MonoBehaviourSingleton<MapManager>
	{
		[SerializeField] private GameObject tilePrefab;

		public const int MapSize = 9;
		private readonly TileData _tile = new ();
		private readonly PieceType[,] _chessMap = new PieceType[MapSize, MapSize];

		private void Start()
		{
			ResetTileState();
			InitTile();
		}
		private void InitTile()
		{
			_tile.TileArray = new Tile[MapSize, MapSize];
			_tile.ObjArray = new GameObject[MapSize, MapSize];
			for (var x = 0; x < MapSize; x++)
			{
				for (var y = 0; y < MapSize; y++)
				{
					_tile.ObjArray[x, y] = Instantiate(tilePrefab, this.transform);
					_tile.ObjArray[x, y].transform.position = ChessGameManager.CalWorldPos(new Point(x, y)).ToVector2();
					_tile.ObjArray[x, y].name = "Tile " + "[" + x + "," + y + "]";

					_tile.TileArray[x, y] = _tile.ObjArray[x, y].GetComponent<Tile>();
					_tile.TileArray[x, y].Init(new Point(x, y));
				}
			}
		}
		
		public void ResetTileState()
		{
			for (var y = 0; y < MapSize; y++)
				for (var x = 0; x < MapSize; x++)
					UpdateTileState(new Point(x, y), PieceType.Empty);
		}
		public void UpdateTileState(Point p, PieceType type)
		{
			_chessMap[p.x, p.y] = type;
		}

		public bool IsMapEmpty(Point p)
		{
			return _chessMap[p.x, p.y] == PieceType.Empty;
		}

		public PieceType GetPieceType(Point p)
		{
			return _chessMap[p.x, p.y];
		}

		public void HighlightAvailableTile(Point p)
		{
			ChangeTileColor(new Point(p.x + 1, p.y), new Color(0.5f, 1, 0.5f));
			ChangeTileColor(new Point(p.x - 1, p.y), new Color(0.5f, 1, 0.5f));
			ChangeTileColor(new Point(p.x, p.y + 1), new Color(0.5f, 1, 0.5f));
			ChangeTileColor(new Point(p.x, p.y - 1), new Color(0.5f, 1, 0.5f));
		}

		public void ResetTileColor()
		{
			for (var y = 0; y < MapSize; y++)
				for (var x = 0; x < MapSize; x++)
					ChangeTileColor(new Point(x, y), Color.white);
		}

		private void ChangeTileColor(Point p, Color color)
		{
			if (p.x is < 0 or >= MapSize)
				return;
			if (p.y is < 0 or >= MapSize)
				return;
			_tile.TileArray[(int)p.x, (int)p.y].ChangeColor(color);
		}
	}
}