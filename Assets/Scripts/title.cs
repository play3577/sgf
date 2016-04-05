﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class title : MonoBehaviour {

  int i;

  public List<Text> menu_objects;

  Color red = new Color();
  Color white = new Color();

  bool axis_held;

	// Use this for initialization
	void Start(){

    i = 0;
    axis_held = false;

    ColorUtility.TryParseHtmlString("#ec393d", out red);
    ColorUtility.TryParseHtmlString("#ebebeb", out white);
	
	}
	
	// Update is called once per frame
	void Update(){

    if(Input.GetKeyDown(KeyCode.UpArrow) ||
       (!axis_held && Input.GetAxis("Controller 1 Left Stick Y Axis") <= -0.9f) ||
       (!axis_held && Input.GetAxis("Controller 2 Left Stick Y Axis") <= -0.9f) ||
       (!axis_held && Input.GetAxis("Controller 3 Left Stick Y Axis") <= -0.9f) ||
       (!axis_held && Input.GetAxis("Controller 4 Left Stick Y Axis") <= -0.9f) ){
      --i;
      if(i < 0){
        i = menu_objects.Count - 1;
      }
      axis_held = true;
    }

    else if(Input.GetKeyDown(KeyCode.DownArrow) ||
            (!axis_held && Input.GetAxis("Controller 1 Left Stick Y Axis") >= 0.9f) ||
            (!axis_held && Input.GetAxis("Controller 2 Left Stick Y Axis") >= 0.9f) ||
            (!axis_held && Input.GetAxis("Controller 3 Left Stick Y Axis") >= 0.9f) ||
            (!axis_held && Input.GetAxis("Controller 4 Left Stick Y Axis") >= 0.9f) ){
      ++i;
      if(i >= menu_objects.Count){
        i = 0;
      }
      axis_held = true;
    }

    if(Input.GetButtonDown("Controller 1 A Button") ||
       Input.GetButtonDown("Controller 2 A Button") ||
       Input.GetButtonDown("Controller 3 A Button") ||
       Input.GetButtonDown("Controller 4 A Button") ||
       Input.GetKeyDown(KeyCode.Return)){

      if(i == 0){
        SceneManager.LoadScene("_character_select");
      }

    }

    // if(Input.GetAxisRaw("Controller 1 Left Stick Y Axis") == 0){
    //   axis_held = false;
    // }

    foreach(Text menu_GO in menu_objects){
      menu_GO.color = white;
    }
    menu_objects[i].color = red;
	
	}
}
