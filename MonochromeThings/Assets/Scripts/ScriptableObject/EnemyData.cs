using System.Collections.Generic;
using Data;
using UnityEngine;

namespace ScriptableObject
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Object/Zombie Data", order = int.MaxValue)]
    [System.Serializable]
    public class EnemyData : UnityEngine.ScriptableObject
    {
        [SerializeField] private int id;
        [SerializeField] private string enemyName;
        [SerializeField] private List<Point> availablePoints;

        public int Id => id;
        public string Name => enemyName;
        public List<Point> AvailablePoints => availablePoints;
    }
}