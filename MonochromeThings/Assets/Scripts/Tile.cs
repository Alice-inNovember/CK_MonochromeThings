using System.Collections;
using System.Collections.Generic;
using Data;
using Manager;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;

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