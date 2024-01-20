using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class InventoryData
{
    private const int Empty = 0;
    private const int InUse = -1;
    private int[,] _inventory;
    private List<int>[][] _i2;
    
    private void Awake()
    {
        _inventory = new int[25, 25];

        for (var yi = 0; yi < 25; yi++)
        {
            for (var xi = 0; xi < 25; xi++)
            {
                _inventory[xi, yi] = Empty;
            }
        }
    }

    public void AddItem(int x, int y, int id)
    {
        var item = ItemInfoManager.Instance.GetItemInfo(id);
        for (var yi = 0; yi < y + item.Height; yi++)
        {
            for (var xi = 0; xi < x + item.Width; xi++)
            {
                _inventory[xi, yi] = InUse;
            }
        }

        _inventory[x, y] = id;
    }

    public void RemoveItem(int x, int y)
    {
        var id = _inventory[x, y];
        var item = ItemInfoManager.Instance.GetItemInfo(id);

        for (var yi = 0; yi < y + item.Height; yi++)
        {
            for (var xi = 0; xi < x + item.Width; xi++)
            {
                _inventory[xi, yi] = Empty;
            }
        }
    }

    public int GetItemID(int x, int y)
    {
        return _inventory[x, y];
    }
}

