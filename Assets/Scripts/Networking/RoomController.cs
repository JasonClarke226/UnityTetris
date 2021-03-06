﻿using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;
using Photon.Realtime;
using System.Linq;
using System;
using TetrisEngine;
using static SpawnController;

public class RoomController : MonoBehaviourPunCallbacks {

// player instance prefab, must be located in the Resources folder    
[SerializeField] GameObject m_playerPrefab;

private bool playerSpawned = false;

void Start() {
    // in case we started this scene with the wrong scene being active, simply load the menu scene        
    if (PhotonNetwork.CurrentRoom == null) {
    Debug.Log("Is not in the room, returning back to Lobby");
    UnityEngine.SceneManagement.SceneManager.LoadScene("Lobby");
    return; }
}

private void Update() {
    if (!playerSpawned && m_playerPrefab != null) {
    
    if (PhotonNetwork.LocalPlayer.IsLocal && PhotonNetwork.CurrentRoom.MaxPlayers != 1) {
    if (PhotonNetwork.LocalPlayer.ActorNumber - 1 == 0) {

    gameObject.transform.Translate(Vector3.left * 3.25f);
    
    } else {
    gameObject.transform.Translate(Vector3.right * 12.75f);
    }
    }

    GameObject go = PhotonNetwork.Instantiate(m_playerPrefab.name, SpawnController.instance.SpawnPoints[PhotonNetwork.LocalPlayer.ActorNumber - 1].spawnpoint.position, Quaternion.identity, 0);
    go.transform.parent = transform;
    SpawnController spawnController = SpawnController.instance;
    //.Player = go;
    playerSpawned = true;
    }
}

void OnGUI() {
    if (PhotonNetwork.CurrentRoom == null) return;
    // leave this Room        
    if (GUI.Button(new Rect(5, 5, 125, 25), "Leave Room")) { PhotonNetwork.LeaveRoom(); }
    // show the Room name        
    GUI.Label(new Rect(5, 30, 200, 25), $"Server Name: {PhotonNetwork.CurrentRoom.Name}");
    // show the list of the players connected to this Room        
    for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++) {
    // show if this player is a Master Client. There can only be one Master Client per Room so use this to define the authoritative logic etc.)            
    string isMasterClient = (PhotonNetwork.PlayerList[i].IsMasterClient ? ": MasterClient" : "");
    GUI.Label(new Rect(5, 50 + 30 * i, 200, 25), PhotonNetwork.PlayerList[i].NickName + isMasterClient);
    }
}

public override void OnLeftRoom() {
    // left the Room, return back to the GameLobby
    UnityEngine.SceneManagement.SceneManager.LoadScene("Lobby");
}

public override void OnJoinedRoom() {
    base.OnJoinedRoom();
    Debug.Log("Joined");
}

public override void OnPlayerLeftRoom(Player otherPlayer) {
    Debug.Log("PlayerLeft");
    base.OnPlayerLeftRoom(otherPlayer);
}

public override void OnDisconnected(DisconnectCause cause) {
    Debug.Log(cause);
}

}