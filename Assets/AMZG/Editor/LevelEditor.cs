using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using NgoUyenNguyen.GridSystem; // ⚡ thêm để dùng Grid enums

[CustomEditor(typeof(LevelGridController))] // 👈 chỉ định editor cho LevelGridController
public class LevelEditor : Editor
{
    private LevelGridController gridController;
    private bool showResources = false;

    public override void OnInspectorGUI()
    {
        gridController = (LevelGridController)target;
        GUIStyle headerStyle = new GUIStyle();
        headerStyle.richText = true;
        headerStyle.fontStyle = FontStyle.Bold;

        // ---- SAVE ALL BUTTON ----
        GUILayout.BeginHorizontal("BOX");
        if (GUILayout.Button("SAVE ALL", GUILayout.Width(250), GUILayout.Height(40)))
        {
            gridController.EditorSaveAll();
        }
        GUILayout.EndHorizontal();

        // ---- SHOW/HIDE RESOURCES ----
        GUILayout.BeginHorizontal("BOX");
        if (GUILayout.Button((showResources ? "Hide" : "Show") + " Map Resources", GUILayout.Width(250), GUILayout.Height(40)))
        {
            showResources = !showResources;
        }
        GUILayout.EndHorizontal();

        if (showResources)
        {
            GUILayout.BeginVertical("BOX", GUILayout.Width(250), GUILayout.Height(30));
            EditorGUILayout.LabelField("<color=cyan>MAP RESOURCES</color>", headerStyle);
            GUILayout.EndVertical();
        }

        EditorGUILayout.Space();
        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal("BOX");
        if (GUILayout.Button("CREATE NEW LEVEL", GUILayout.Width(250), GUILayout.Height(40)))
        {
            gridController.EditorAddLevel();
        }
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();

        EditorGUILayout.Space();

        // ---- LEVEL MANAGEMENT ----
        GUILayout.BeginVertical("BOX", GUILayout.Width(250), GUILayout.Height(30));
        GUILayout.BeginVertical();
        EditorGUILayout.LabelField("<color=cyan>LEVEL</color>", headerStyle);
        GUILayout.EndVertical();
        GUILayout.BeginHorizontal();

        gridController.CurrentLevel = EditorGUILayout.IntField("Current Level", gridController.CurrentLevel);

        if (GUILayout.Button("LOAD", GUILayout.Width(80), GUILayout.Height(25)))
        {
            gridController.EditorLoadLevel(gridController.CurrentLevel);
        }
        if (GUILayout.Button("<<", GUILayout.Width(40), GUILayout.Height(25)))
        {
            gridController.EditorLoadPrevLevel();
        }
        if (GUILayout.Button(">>", GUILayout.Width(40), GUILayout.Height(25)))
        {
            gridController.EditorLoadNextLevel();
        }
        if (GUILayout.Button("CLONE", GUILayout.Width(80), GUILayout.Height(25)))
        {
            gridController.EditorCloneLevel(gridController.CurrentLevel);
        }

        GUILayout.EndHorizontal();
        GUILayout.EndVertical();

        EditorGUILayout.Space();

        // ---- SAVE LEVEL BUTTON ----
        GUILayout.BeginVertical("BOX");
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("SAVE LEVEL", GUILayout.Width(250), GUILayout.Height(40)))
        {
            gridController.EditorSaveLevel();
        }
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();

        // ---- GRID SETTINGS ----
        GUILayout.Space(10);
        GUILayout.BeginVertical("BOX");
        EditorGUILayout.LabelField("<color=cyan>GRID SETTINGS</color>", headerStyle);

        gridController.cellPrefab = (GameObject)EditorGUILayout.ObjectField("Cell Prefab", gridController.cellPrefab, typeof(GameObject), false);
        gridController.gridSize = EditorGUILayout.Vector2IntField("Grid Size", gridController.gridSize);
        gridController.cellSize = EditorGUILayout.FloatField("Cell Size", gridController.cellSize);
        gridController.gridAlignment = (GridAlignment)EditorGUILayout.EnumPopup("Alignment", gridController.gridAlignment);
        gridController.gridSpace = (GridSpace)EditorGUILayout.EnumPopup("Space", gridController.gridSpace);
        gridController.cellLayout = (CellLayout)EditorGUILayout.EnumPopup("Layout", gridController.cellLayout);

        GUILayout.Space(5);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("CREATE GRID", GUILayout.Height(30)))
        {
            gridController.EditorCreateGrid();
        }
        if (GUILayout.Button("CLEAR GRID", GUILayout.Height(30)))
        {
            gridController.EditorClearGrid();
        }
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
        // ---- END GRID SETTINGS ----

        EditorGUILayout.Space();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(gridController);
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        }
    }
}