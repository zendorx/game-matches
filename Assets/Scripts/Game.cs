using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Game {

    int total = 0;
    int left = 0;
    int currentPlayer = 0;
    int lastNumber = 0;

    public void Init(int total, int currentPlayer)
    {
        this.total = total;
        this.currentPlayer = currentPlayer;
        left = total;
    }

    public int GetLastNumber()
    {
        return lastNumber;
    }

    public int GetLeft()
    {
        return left;
    }

    public int GetCurrentPlayer()
    {
        return currentPlayer;
    }

    public int GetMin()
    {
        return Math.Max(lastNumber - 1, 1);
    }
    
    public int GetMax()
    {
        return lastNumber > 0 ? lastNumber + 1 : 3;
    }

    public bool IsValidMove(int number)
    {
        return number <= GetMax() && number >= GetMin() || number == left;
    }

    public bool IsGameOver()
    {
        return left <= 0;
    }

    public void MakeTurn(int number)
    {
        if (!IsValidMove(number))
        {
            Debug.LogError("Wrong move: " + number + " max is " + GetMax() + ", min is " + GetMin());
        }

        left -= number;
        lastNumber = number;

        if (IsGameOver())
        {
            return;
        }

        currentPlayer = currentPlayer == 1 ? 0 : 1;
        return;
    }
}
