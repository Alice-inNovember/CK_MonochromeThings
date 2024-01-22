
using System;
using ClassTemp;
using UnityEngine;
using DG.Tweening;

namespace Manager
{
    public class ChessMapManager : Singleton<ChessMapManager>
    {
        public GameObject tilePrefab;
        [SerializeField] private const int MapSize = 7;
        [SerializeField] private GameObject[,] _tileObjArray = new GameObject[MapSize, MapSize];
        [SerializeField] private Tile[,] _tileArray = new Tile[MapSize, MapSize];

        private void Start()
        {
            CreateBlankTile();
        }
        
        private void CreateBlankTile()
        {
            for (var x = 0; x < MapSize; x++)
            {
                for (var y = 0; y < MapSize; y++)
                {
                    _tileObjArray[x, y] = Instantiate(tilePrefab, this.transform);
                    _tileObjArray[x, y].transform.position = new Vector3(-(MapSize / 2) + x, MapSize / 2 - y, 0);
                    _tileObjArray[x, y].name = "Tile " + "[" + x + "," + y + "]";
                    _tileArray[x, y] = _tileObjArray[x, y].GetComponent<Tile>();
                    _tileArray[x, y].Init(x, y, this);
                }
            }
        }
        
        public void TileSelect(int x, int y)
        {
            for (var ix = 0; ix < MapSize; ix++)
            {
                for (var iy = 0; iy < MapSize; iy++)
                {
                    _tileObjArray[ix, iy].GetComponent<SpriteRenderer>().DOColor(new Color(1, 1, 1), 0.25f).SetEase(Ease.InSine);
                }
            }
            PlayerPieceController.Instance.MovePlayerTo(x, y);
        }

        private void ChangeTileColor(int x, int y, Color color)
        {
            if (x is < 0 or >= MapSize)
                return;
            if (y is < 0 or >= MapSize)
                return;
            _tileObjArray[x, y].GetComponent<SpriteRenderer>().DOColor(new Color(0.5f, 1, 0.5f), 0.25f).SetEase(Ease.InSine);
        }
        public void HighlightAvailableTile(int x, int y)
        {
            ChangeTileColor(x + 1, y, new Color(0.5f, 1, 0.5f));
            ChangeTileColor(x - 1, y, new Color(0.5f, 1, 0.5f));
            ChangeTileColor(x, y + 1, new Color(0.5f, 1, 0.5f));
            ChangeTileColor(x, y - 1, new Color(0.5f, 1, 0.5f));
        }
    }
}















