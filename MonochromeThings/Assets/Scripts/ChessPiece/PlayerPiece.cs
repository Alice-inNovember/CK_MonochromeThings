using Data;
using DG.Tweening;
using Manager;
using UnityEngine;

namespace ChessPiece
{
	public class PlayerPiece : MonoBehaviour
	{
		public Point pos;
		private bool _isMoving;

		private void Start()
		{
			_isMoving = false;
		}

		public void Move(Point p)
		{
			_isMoving = true;
			transform.DOMove(new Vector3(p.x, p.y, 0), 0.5f).SetEase(Ease.InOutSine).OnComplete(() =>
			{
				_isMoving = false;
			});
		}

		public void OnPointerClick()
		{
			if (_isMoving == true)
				return;
			ChessGameManager.Instance.OnPlayerClick();
		}
		
		public void HighlightAvailableTile()
		{
			MapManager.Instance.HighlightPath(pos + new Point(0, 1));
			MapManager.Instance.HighlightPath(pos + new Point(0, -1));
			MapManager.Instance.HighlightPath(pos + new Point(1, 0));
			MapManager.Instance.HighlightPath(pos + new Point(-1, 0));
		}
	}
}