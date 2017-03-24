using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Solution
{
    [SerializeField]
    public string id;
    [SerializeField]
    public int number;
    [SerializeField]
    public double wins;

}

[Serializable]
public class Solutions{

    [SerializeField]
    public List<Solution> items = new List<Solution>();


    public void AddIfBetter(Solution solution)
    {
        Debug.Log("Trying to add: " + solution.id + " wins: " + solution.wins);
        Solution old = Find(solution.id);
        if ((old == null || old.wins < solution.wins) && solution.wins > 0)
        {
            items.Add(solution);
        }
    }

    public Solution Find(string id)
    {
        foreach (var item in items)
        {
            if (item.id == id)
                return item;
        }

        return null;
    }

    public bool Has(string id, ref int number)
    {
        Solution s = Find(id);
        if (s != null)
        {
            number = s.number;
            return true;
        }

        return false;
    }

    public void PrintAll()
    {
        Debug.Log("total items: " + items.Count);
        foreach (var item in items)
        {
            Debug.Log("item: " + item.id + " wins: " + item.wins + " number: " + item.number);
        }
    }
}
