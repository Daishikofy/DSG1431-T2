﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private List<string> itens;

    public void addItem(string item)
    {
        itens.Add(item);
    }

    public void removeItem(string item)
    {
        itens.Remove(item);
    }

    public bool containsItem(string name)
    {
        return itens.Contains(name);
    }
}