using System;
using Data;
using Manager;
using ScriptableObject;
using UnityEngine;

namespace ChessPiece.AI
{
    public class EntityAi
    {
        public virtual Point CalculateActionsPoint(EntityData data, Point pos)
        {
            var distances = new float[data.AvailablePoints.Capacity];
            for (var i = 0; i < data.AvailablePoints.Capacity; i++)
                distances[i] = Point.Dist(ChessGameManager.Instance.GetPlayerPos(), pos + data.AvailablePoints[i]);
            var minIndex = 0;
            minIndex = Array.IndexOf(distances, Mathf.Min(distances));

            //이동하려는 위치가 가능하다면 리턴
            if (MapManager.Instance.IsMapAvailable(pos + data.AvailablePoints[minIndex]))
                return pos + data.AvailablePoints[minIndex];

            //이동 반경 내 가능한 아무 위치로 이동
            for (var i = 0; i < data.AvailablePoints.Capacity - 1; i++)
            {
                if (MapManager.Instance.IsMapAvailable(pos + data.AvailablePoints[i]))
                    return pos + data.AvailablePoints[minIndex];
            }
            return pos;
        }
    }
}
