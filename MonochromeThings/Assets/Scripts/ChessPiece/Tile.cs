using Data;
using DG.Tweening;
using Manager;
using UnityEngine;

namespace ChessPiece
{
	public class Tile : MonoBehaviour
	{
		private Point _pos;

		public void Init(Point pos)
		{
			_pos = pos;
		}

		public void OnPointerClick()
		{
			ChessGameManager.Instance.TileSelect(_pos);
		}

		public void ChangeColor(Color color)
		{
			GetComponent<SpriteRenderer>().DOColor(color, 0.25f).SetEase(Ease.InSine);
		}
	}
}