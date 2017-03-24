using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class CheaterEnemy : RandomEnemy {

    Solutions solutions;

    public CheaterEnemy(string solutionsData)
    {
        var serializer = new XmlSerializer(typeof(Solutions));
        solutions = serializer.Deserialize(new StringReader(solutionsData)) as Solutions;
       // solutions.PrintAll();
    }

    public override int DoTurn(int lastNumber, int left, int min, int max)
    {
        if (lastNumber == 0)
            lastNumber = 2;

        string id = left.ToString() + "-" + lastNumber.ToString();
        
        Solution s = solutions.Find(id);
        if (s != null)
        {
            Debug.Log("Found solution with " + s.wins + " wins: number " + s.number);
            return s.number;
        }
        else
        {
            Debug.Log("Solutions not found, do random: '" + id + "'");
            return base.DoTurn(lastNumber, left, min, max);
        }
    }

}
