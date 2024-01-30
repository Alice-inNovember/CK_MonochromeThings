using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;

public class Tile : MonoBehaviour
{
	private ChessMapManager _chessMapManager;
	private int _x;
	private int _y;

	public void Init(int x, int y, ChessMapManager chessMapManager)
	{
		_chessMapManager = chessMapManager;
		_x = x;
		_y = y;
	}

	public void OnPointerClick()
	{
		_chessMapManager.TileSelect(_x, _y);
	}

	public void ChangeColor(Color color)
	{
		GetComponent<SpriteRenderer>().DOColor(color, 0.25f).SetEase(Ease.InSine);
	}
}