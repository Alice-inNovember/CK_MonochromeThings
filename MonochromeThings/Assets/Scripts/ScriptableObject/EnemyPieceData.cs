using System.Collections.Generic;
using Data;
using UnityEngine;

namespace ScriptableObject
{
    [CreateAssetMenu(fileName = "EnemyPieceData", menuName = "Scriptable Object/Enemy Piece Data", order = int.MaxValue)]
    [System.Serializable]
    public class EnemyPieceData : UnityEngine.ScriptableObject
    {
        [SerializeField] private int id;
        [SerializeField] private string enemyName;
        [SerializeField] private int uniqueNbr;
        [SerializeField] private int minTurnToMove;
        [SerializeField] private int maxTurnToMove;
        [SerializeField] private List<Point> availablePoints;

        public int Id => id;
        public string Name => enemyName;
        public int UniqueNbr => uniqueNbr;
        public int MinTurnToMove => minTurnToMove;
        public int MaxTurnToMove => maxTurnToMove;
        public List<Point> AvailablePoints => availablePoints;
        
        //인카운터시 생성되는 적 개체 수
        
        //인카운터시 버프 및 디버프 종류
    }
}