using System.Collections;
using System.Collections.Generic;
using ClassTemp;
using DG.Tweening;
using UnityEngine;
using Manager;
using Unity.Mathematics;


public class PlayerPieceController : Singleton<PlayerPieceController>
{
    [SerializeField] private const int MapSize = 7;
    private bool _isHighlighted;
    private bool _isMoving;
    private int _x;
    private int _y;
    
    private void Start()
    {
        _isHighlighted = false;
        _isMoving = false;
        _x = MapSize / 2;
        _y = MapSize / 2;
    }

    private void Move(int x, int y)
    {
        _isMoving = true;
        transform.DOMove(new Vector3(-(MapSize / 2) + x, MapSize / 2 - y, 0), 0.5f).SetEase(Ease.InOutSine).OnComplete(()=>
        {
            _isMoving = false;
        });
    }
    public void MovePlayerTo(int x, int y)
    {
        if (_isHighlighted == false)
            return;
        if (x == _x)
        {
            if (math.abs(y - _y) > 1)
                return;
            _y = y;
            _isHighlighted = false;
            Move(x, y);
        }
        else if (y == _y)
        {
            if (math.abs(x - _x) > 1)
                return;
            _x = x;
            _isHighlighted = false;
            Move(x, y);
        }
    }

    public void OnPointerClick()
    {
        if (_isMoving == true)
            return;
        ChessMapManager.Instance.HighlightAvailableTile(_x, _y);
        _isHighlighted = true;
    }
}
