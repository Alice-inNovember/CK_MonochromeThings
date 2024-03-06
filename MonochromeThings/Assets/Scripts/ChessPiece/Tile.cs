using System;
using Data;
using DG.Tweening;
using Manager;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ChessPiece
{
	public class Tile : MonoBehaviour
	{
		[SerializeField] private GameObject warningSprite;
		private Point _pos;
		private Color _prevColor;
		private SpriteRenderer _spriteRenderer;
		private Renderer _renderer;

		private void Awake()
		{
			_spriteRenderer = GetComponent<SpriteRenderer>();
			_renderer = GetComponent<Renderer>();
			_prevColor = Color.white;
		}

		private void Start()
		{
			warningSprite.SetActive(false);
			var position = transform.position;
			transform.DOLocalMove(new Vector3(position.x, position.y, Random.Range(0.51f, 0.71f)), 1f);
		}
		public void Init(Point pos)
		{
			_pos = pos;
		}

		public void SetWarning(bool set)
		{
			warningSprite.SetActive(set);
		}

		public void OnPointerClick()
		{
			ChessGameManager.Instance.TileSelect(_pos);
		}
		
		// public void ChangeColor(Color color)
		// {
		// 	if (_prevColor == color)
		// 		return;
		// 	_prevColor = color;
		// 	_spriteRenderer.DOPause();
		// 	_spriteRenderer.DOColor(color, 0.25f).SetEase(Ease.InSine);
		// }
		
		public void ChangeColor(Color color)
		{
			if (_prevColor == color)
				return;
			_prevColor = color;
			_renderer.material.DOPause();
			_renderer.material.DOColor(color, 0.25f).SetEase(Ease.InSine);
		}
	}
}