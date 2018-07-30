using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TomeMenuManager : MonoBehaviour {
    public GameObject tomePresetPointer;
    public int selectedTome;
    private TomeManager playerTM;
	// Use this for initialization
	void Start () {
        playerTM = GameObject.FindGameObjectWithTag("Player").GetComponent<TomeManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void selectTomePreset(int n)
    {
        selectedTome=n;
    }
}
