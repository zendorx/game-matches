using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchView : MonoBehaviour {

    MeshRenderer[] renderers = null;

    public void SetColor(Color color)
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            MeshRenderer r = renderers[i];
            r.material.color = color;
        }
    }

	// Use this for initialization
	void Awake () {
        renderers = GetComponents<MeshRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
