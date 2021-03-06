﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class character_select : MonoBehaviour {
  public string mac = "";

  public int player_num;
  public Vector3 start_position;
  public List<GameObject> player_prefabs;

  Vector3 resize;

  public GameObject manager;
  
  public GameObject join_set;
  public GameObject select_set;
  // public GameObject return_set;
  public GameObject podium;

  public GameObject join_button;
  public GameObject confirm_button;
  public GameObject cancel_button;
  public GameObject controls_layout;
  public GameObject ready_text;

  public Sprite red_line;
  public Sprite white_line;

  public int borders_touched = 0;

  public GameObject[] borders;

  public int prefab_number = 0;
  public GameObject player_object;

  public GameObject camera;

  int last_cycle = 1;

  Color character_color = new Color(1, 1, 1, 1);

  bool joined;
  bool selected;
  bool axis_in_use;

  public bool tutorial;

	void Start(){
    // Mac Check
    if(Application.platform == RuntimePlatform.OSXEditor
      || Application.platform == RuntimePlatform.OSXPlayer
      || Application.platform == RuntimePlatform.OSXPlayer){
      mac = "Mac ";
    }
    else{
      mac = "";
    }

    joined = false;
    selected = false;
    axis_in_use = false;

    if(PlayerPrefs.GetFloat("tutorial") == 0){
      tutorial = false;
    }
    else {
      tutorial = true;
    }

    resize = new Vector3(1f, 1f, 1f);

    if(player_num > Input.GetJoystickNames().Length){
    //if(player_num > 4){
      // deactivate player if controller not connected
      this.gameObject.SetActive(false);
      join_button.SetActive(false);
    }
    else if(PlayerPrefs.GetInt("P" + player_num + "Num") != -2){
      prefab_number = PlayerPrefs.GetInt("P" + player_num + "Num");
      Join(prefab_number);
      Confirm();
      if(tutorial){
        borders_touched = 4;
        GameObject.Find("p" + player_num + "_bottom").GetComponent<SpriteRenderer>().sprite = GameObject.Find("p" + player_num + "_bottom").GetComponent<character_select_border>().red_line;
        GameObject.Find("p" + player_num + "_top").GetComponent<SpriteRenderer>().sprite = GameObject.Find("p" + player_num + "_top").GetComponent<character_select_border>().red_line;
        GameObject.Find("p" + player_num + "_left").GetComponent<SpriteRenderer>().sprite = GameObject.Find("p" + player_num + "_left").GetComponent<character_select_border>().red_line;
        GameObject.Find("p" + player_num + "_right").GetComponent<SpriteRenderer>().sprite = GameObject.Find("p" + player_num + "_right").GetComponent<character_select_border>().red_line;
      }
    }

    prefab_number = player_num + 1;

	}
	
	void Update(){

    if(System.Math.Abs(Input.GetAxis(mac + "Controller " + player_num + " Left Stick X Axis")) < 0.9f){
      axis_in_use = false;
    }

    // set color to black if already selected
    if(player_object != null){
      if(manager.GetComponent<character_select_manager>().character_is_selected("P" + player_num, prefab_number)){
        player_object.GetComponent<SpriteRenderer>().color = Color.black;
        Cycle(last_cycle);
        axis_in_use = false;
      }
      else {
        player_object.GetComponent<SpriteRenderer>().color = character_color;
      }
    }

    // join
    if(!joined){
      // join game
      if(Input.GetButtonDown(mac + "Controller " + player_num + " X Button") ||
         Input.GetKeyDown(KeyCode.X)){
        Join();
      }

      // exit game
      if(Input.GetButtonDown(mac + "Controller " + player_num + " B Button") ||
         Input.GetButtonDown(mac + "Controller " + player_num + " Back Button") ||
         Input.GetKeyDown(KeyCode.B)){
        PlayerPrefs.SetFloat("audio_time", camera.GetComponent<AudioSource>().time);
        SceneManager.LoadScene("_title");
      }

    }

    // in character select
    else if(joined && !selected){

      // change color right
      if(Input.GetButtonDown(mac + "Controller " + player_num + " Right Bumper") ||
         (!axis_in_use && Input.GetAxis(mac + "Controller " + player_num + " Left Stick X Axis") >= 0.95f) ||
         Input.GetKeyDown(KeyCode.RightArrow)){
        Cycle(1);
      }

      // change color left
      if(Input.GetButtonDown(mac + "Controller " + player_num + " Left Bumper") ||
         (!axis_in_use && Input.GetAxis(mac + "Controller " + player_num + " Left Stick X Axis") <= -0.95f) ||
         Input.GetKeyDown(KeyCode.LeftArrow)){
        Cycle(-1);
      }

      // confirm color
      if(Input.GetButtonDown(mac + "Controller " + player_num + " A Button") ||
         Input.GetKeyDown(KeyCode.A)){
        Confirm();
      }

      // unjoin game
      if(Input.GetButtonDown(mac + "Controller " + player_num + " Back Button") ||
         Input.GetButtonDown(mac + "Controller " + player_num + " B Button") ||
         Input.GetKeyDown(KeyCode.B)){
        Unjoin();
      }

    }

    else {
      if(tutorial && borders_touched >= 4){
        WallsTouched();
      }
      // return to character select
      if(Input.GetButtonDown(mac + "Controller " + player_num + " Back Button") ||
         Input.GetKeyDown(KeyCode.B)){
        Unconfirm();
      }
    }
	}

  void Join(){
    joined = true;
    join_set.SetActive(false);
    join_button.SetActive(false);
    select_set.SetActive(true);
    confirm_button.SetActive(true);
    podium.SetActive(true);
    Show(true);
    manager.GetComponent<character_select_manager>().selected_character["P" + player_num] = -1;
    manager.GetComponent<character_select_manager>().ready_character["P" + player_num] = -1;
  }

  void Join(int value){
    joined = true;
    join_set.SetActive(false);
    join_button.SetActive(false);
    select_set.SetActive(true);
    confirm_button.SetActive(true);
    podium.SetActive(true);
    Show(true);
    manager.GetComponent<character_select_manager>().selected_character["P" + player_num] = value;
    manager.GetComponent<character_select_manager>().ready_character["P" + player_num] = value;
  }

  void Unjoin(){
    Show(false);
    joined = false;
    join_set.SetActive(true);
    join_button.SetActive(true);
    select_set.SetActive(false);
    confirm_button.SetActive(false);
    podium.SetActive(false);
    manager.GetComponent<character_select_manager>().selected_character["P" + player_num] = -2;
    manager.GetComponent<character_select_manager>().ready_character["P" + player_num] = -2;
  }

  void Confirm(){
    if(!manager.GetComponent<character_select_manager>().character_is_selected("P" + player_num, prefab_number)){
      selected = true;
      select_set.SetActive(false);
      confirm_button.SetActive(false);
      podium.SetActive(false);
      cancel_button.SetActive(true);
      player_object.GetComponent<player>().player_number = player_num;
      manager.GetComponent<character_select_manager>().selected_character["P" + player_num] = prefab_number;
      if(!tutorial){
        ready_text.SetActive(true);
        manager.GetComponent<character_select_manager>().ready_character["P" + player_num] = prefab_number;
      }
      else{
        controls_layout.SetActive(true);
      }
    }
  }

  void Unconfirm(){
    Show(false);
    selected = false;
    Join();
    cancel_button.SetActive(false);
    controls_layout.SetActive(false);
    ready_text.SetActive(false);
    manager.GetComponent<character_select_manager>().selected_character["P" + player_num] = -1;
    manager.GetComponent<character_select_manager>().ready_character["P" + player_num] = -1;
    borders_touched = 0;

    foreach(GameObject border in borders){
      border.GetComponent<character_select_border>().touched = !tutorial;
      border.GetComponent<SpriteRenderer>().sprite = white_line;
    }

  }

  void WallsTouched(){
    manager.GetComponent<character_select_manager>().ready_character["P" + player_num] = prefab_number;
    ready_text.SetActive(true);
    controls_layout.SetActive(false);
  }

  void Show(bool value){
    if(value){
      player_object = Instantiate(player_prefabs[prefab_number]) as GameObject;
      player_object.transform.position = start_position;
      player_object.transform.localScale = resize;
      player_object.GetComponent<player>().player_number = 0;
      if(PlayerPrefs.GetString("screen") == "TABLETOP"){
        if(player_num == 1){
          player_object.SendMessage("SetGravityFromCharacterSelect", "down");
        }
        else if(player_num == 2){
          player_object.SendMessage("SetGravityFromCharacterSelect", "right");
        }
        else if(player_num == 3){
          player_object.SendMessage("SetGravityFromCharacterSelect", "up");
        }
        else if(player_num == 4){
          player_object.SendMessage("SetGravityFromCharacterSelect", "left");
        }
      }
    }
    else{
      Destroy(player_object);
    }
  }

  // cycles player prefab
  void Cycle(int value){

    last_cycle = value;
    axis_in_use = true;

    Destroy(player_object);
    prefab_number += value;

    if(prefab_number >= player_prefabs.Capacity){
      prefab_number -= player_prefabs.Capacity;
    }
    else if(prefab_number < 0){
      prefab_number += player_prefabs.Capacity;
    }

    Show(true);
  }

}
