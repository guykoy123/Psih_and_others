﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Debugger : MonoBehaviour {
    Text TextBox;
    string text = "";
    public Player_Controller Player;
	// Use this for initialization
	void Start ()
    {
        TextBox = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        text = Player.ToString();
        TextBox.text = text;
	}
}
