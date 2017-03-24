using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEnemy : Enemy {

    public override int DoTurn(int lastNumber, int left, int min, int max)
    {
        return Random.Range(min, max + 1);
    }

}
