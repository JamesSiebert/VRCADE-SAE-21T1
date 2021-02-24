using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(RoomManager))]
public class RoomManagerEditorScript : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.HelpBox("This script is responsible for creating and joining rooms", MessageType.Info);

        RoomManager roomManager = (RoomManager)target;
        
        if(GUILayout.Button("Join School Room"))
        {
            roomManager.OnEnterRoomButtonClicked_School();
        }

        if(GUILayout.Button("Join Outdoor Room"))
        {
            roomManager.OnEnterRoomButtonClicked_Outdoor(); 
        }
        
        if(GUILayout.Button("Join Lobby Room"))
        {
            roomManager.OnEnterRoomButtonClicked_Lobby();
        }
        
        if(GUILayout.Button("Join Vertigo Room"))
        {
            roomManager.OnEnterRoomButtonClicked_Vertigo();
        }
        
        if(GUILayout.Button("Join Air Hockey Room"))
        {
            roomManager.OnEnterRoomButtonClicked_AirHockey();
        }
        
        if(GUILayout.Button("Join Basketball"))
        {
            roomManager.OnEnterRoomButtonClicked_Basketball();
        }
        
        if(GUILayout.Button("Join Archery Room"))
        {
            roomManager.OnEnterRoomButtonClicked_Archery();
        }
        
        if(GUILayout.Button("Join Shooter Room"))
        {
            roomManager.OnEnterRoomButtonClicked_Shooter();
        }
    }
}
