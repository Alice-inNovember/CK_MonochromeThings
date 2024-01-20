using System;
using System.Collections.Generic;
using ClassTemp;
using UnityEngine;

namespace Manager
{
    public class ItemInfo
    {
        public readonly string Name;
        public readonly int Width;
        public readonly int Height;

        public ItemInfo(string name, int width, int height)
        {
            Name = name;
            Width = width;
            Height = height;
        }
    }
    public class ItemInfoManager : Singleton<ItemInfoManager>
    {
        private Dictionary<int, ItemInfo> _itemInfos;

        private void Start()
        {
            _itemInfos.Add(001, new ItemInfo("test11", 1, 1));
            _itemInfos.Add(002, new ItemInfo("test12", 1, 2));
            _itemInfos.Add(003, new ItemInfo("test22", 2, 2));
        }

        public ItemInfo GetItemInfo(int id)
        {
            return _itemInfos[id];
        }
    }
}