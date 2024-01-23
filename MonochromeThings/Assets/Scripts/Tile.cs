using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;

public class Tile : MonoBehaviour
{
	private int _x;
	private int _y;

	public void Init(int x, int y, Manager.ChessMapManager chessMapManager)
	{
		_x = x;
		_y = y;
	}

	public void OnPointerClick()
	{
		ChessMapManager.Instance.TileSelect(_x, _y);
	}

	public void ChangeColor(Color color)
	{
		GetComponent<SpriteRenderer>().DOColor(color, 0.25f).SetEase(Ease.InSine);
	}
}