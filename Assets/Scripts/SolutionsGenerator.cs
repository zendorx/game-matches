using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using System.Xml.Serialization;

class Node
{
    public int left = 0;
    public int number = 0;
    public double wins = 0;
    public Node parent = null;
    public bool myTurn = true;

    public Node(Node parent, int number, int left, bool myTurn)
    {
        this.parent = parent;
        this.left = left;
        this.number = number;
        this.wins = 0;
        this.myTurn = myTurn;

        if (myTurn)
        {
            if (parent != null)
            {
                Debug.Log("Node[MY]: " + left + "-" + number + "  parent: " + parent.left + "-" + parent.number);
            }
            else
            {
                Debug.Log("Node[MY]: " + left + "-" + number);
            }
                    
        }
        else
        {
            if (parent != null)
            {
                Debug.Log("Node: " + left + "-" + number + "  parent: " + parent.left + "-" + parent.number);
            }
            else
            {
                Debug.Log("Node: " + left + "-" + number);
            }

         }

        SolutionsGenerator.nodesCount++;
    }
    public List<Node> turns = new List<Node>();
}

public class SolutionsGenerator : MonoBehaviour {

    private static string filename = "Assets/solutions.xml";

    void Start () {
		
	}
	
	void Update () {
		
	}
    
    bool IsValidNumber(int number, int prevNumber)
    {
        return number >= (prevNumber - 1) && number <= (prevNumber + 1) && number > 0;
    }

    double GenerateTurns(Node parent, Dictionary<string, Node> dict)
    {
        double sum = 0;
        for (int i = -1; i <= 1; i++)
        {
            int number = parent.number + i;

            if (IsValidNumber(number, parent.number))
            {
                int left = parent.left - number;
                string id = left + "-" + number;
                if (!dict.ContainsKey(id))
                {
                    Node node = new Node(parent, number, left, !parent.myTurn);
                    parent.turns.Add(node);
                    if (left <= 0)
                    {
                        if (parent.myTurn)
                        {
                            sum++;
                        }

                    }
                    else
                    {
                        node.wins = GenerateTurns(node, dict); ;
                        sum += node.wins;
                    }

                    if (parent.myTurn)
                        dict.Add(id, node);
                }
                else
                {
                    Node node = dict[id];
                    sum += node.wins;
                }
            }
        }


        return sum;
    }

    Solution MakeSolution(Node node)
    {
        Solution s = new Solution();
        s.id = node.left.ToString() + "-" + node.number;
        s.number = node.number;
        s.wins = node.wins;
        return s;
    }

    void MakeSolutions(Node node, Solutions solutions)
    {
        foreach(Node turn in node.turns)
        {
            MakeSolutions(turn, solutions);
        }

        Solution sparent = MakeSolution(node);
        solutions.AddIfBetter(sparent);
    }

    public static int nodesCount = 0;

    Solutions GenerateSolutions()
    {
        nodesCount = 0;
        List<Node> nodes = new List<Node>();
        Dictionary<string, Node> dict = new Dictionary<string, Node>();

        for (int left = 20; left <= 45; left++)
            for (int i = 1; i < 4; i++)
            {
                if (IsValidNumber(i, 2))
                {
                    Node node = new Node(null, i, left, true);
                    node.wins = GenerateTurns(node, dict);
                    nodes.Add(node);
                }
            }

        Debug.Log("Nodes genereated: " + nodesCount);

        Solutions solutions = new Solutions();
        foreach(Node node in nodes)
        {
            MakeSolutions(node, solutions);
        }

        return solutions;
    }

    public void Generate()
    {
        Solutions solutions = GenerateSolutions();

        var serializer = new XmlSerializer(typeof(Solutions));
        using (var stream = new FileStream(filename, FileMode.Create))
        {
            serializer.Serialize(stream, solutions);
            Debug.Log("Solutions saved: " + filename);
        }
    }
}
