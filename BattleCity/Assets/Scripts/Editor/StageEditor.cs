/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 30/09/2016 23:55
*/

using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Stage))]
public class StageEditor : Editor
{
    //public static readonly Uid64 UNIQ = "AE15B574D8FEC802";
    private static string TilePath = "Assets/Resources/Prefabs/MapObjects";
    private static List<MapObject> Tiles;
    private static Dictionary<MapObject, Texture2D> Previews;
    private static Vector2 ScrollPosition;
    private const float ButtonWidth = 70;
    private const float ButtonHeight = 70;
    private const int TileRowLenght = 3;
    private static GUIStyle ButtonGUIStyle
    {
        get {
            GUIStyle guiStyle = new GUIStyle(GUI.skin.button);
            guiStyle.alignment = TextAnchor.LowerCenter;
            guiStyle.imagePosition = ImagePosition.ImageAbove;
            guiStyle.fixedWidth = ButtonWidth;
            guiStyle.fixedHeight = ButtonHeight;
            guiStyle.fontSize = 8;
            return guiStyle;
        }
    }

    private Stage _target;
    private string _mapName;
    private Vector2 _mapSize;
    private bool _showNewMap;

    private static MapObject _activeTile;
    private static Vector2 _cursorPos;

    private void OnEnable()
    {
        _target = (Stage)target;
        Tools.current = Tool.View;
    }

    private void OnDisable()
    {
        ClearActiveTile();
        Tools.current = Tool.Move;
    }

    public override void OnInspectorGUI()
    {
        if (_target == null) { _target = (Stage)target; }
        DrawStageMapInfo();
        EditorGUILayout.Space();
        DrawTiles();
    }

    void OnSceneGUI()
    {
        if (_target == null) { return; }
        if (_activeTile != null) {
            Tools.current = Tool.View;
        }
        EditMap();
        SceneView.RepaintAll();
    }

    private void DrawStageMapInfo()
    {
        if (_target.Map == null || _target.Map.SizeX == 0 || _target.Map.SizeY == 0) {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.HelpBox("Map is not created", MessageType.Info);
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField(_target.Map.MapName + " " + (_target.Map.SizeX) + ", " + (_target.Map.SizeY) + " cells");

        _mapName = EditorGUILayout.TextField("Map name: ", _mapName);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Map size (x2 sections)", GUILayout.Width(130));
        _mapSize = EditorGUILayout.Vector2Field("",new Vector2(
           Mathf.FloorToInt(Mathf.Clamp(_mapSize.x, 8, 20)),
            Mathf.FloorToInt(Mathf.Clamp(_mapSize.y, 8, 20))), GUILayout.MaxWidth(100)
            );
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Create new")) {
            if (_target.Map != null) {
                _target.ClearMap();
            }
            StageMap map = new StageMap((int)_mapSize.x * 2, (int)_mapSize.y * 2);
            map.MapName = _mapName;
            _target.Map = map;
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
    }

    private void DrawTiles()
    {
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Refresh")) {
            InitializeTileList(true);
        }
        else {
            InitializeTileList(false);
        }
        if (GUILayout.Button("Clear map")) {
            _target.ClearMap();
            ClearActiveTile();
        }
        EditorGUILayout.EndHorizontal();

        DrawScrollList();
    }

    private void DrawScrollList()
    {
        if (Tiles == null) { return; }
        EditorGUILayout.HelpBox("[Q] if tile active - [add tile], else [remove tile]", MessageType.Info);
        ScrollPosition = EditorGUILayout.BeginScrollView(ScrollPosition);
        int tileIndex = -1;
        tileIndex = GUILayout.SelectionGrid(tileIndex, GetGUIContentsFromTile(), TileRowLenght, ButtonGUIStyle);
        if (tileIndex != -1) {
            ClearActiveTile();
            MapObject tile = Instantiate(Tiles[tileIndex]);
            _activeTile = tile;
        }
        EditorGUILayout.EndScrollView();
    }

    private GUIContent[] GetGUIContentsFromTile()
    {
        List<GUIContent> guiContents = new List<GUIContent>();
        int tilesCount = Tiles.Count;
        for (int i = 0; i < tilesCount; i++) {
            Texture2D tex = null;  Previews.TryGetValue(Tiles[i], out tex);
            if (tex != null) {
                string name = Tiles[i].name;
                GUIContent cont = new GUIContent(name, tex);
                guiContents.Add(cont);
            }
        }

        return guiContents.ToArray();
    }

    private void InitializeTileList(bool forceRefresh)
    {
        if (Tiles == null || forceRefresh) {
            Tiles = GetAssetsWithScript<MapObject>(TilePath);
            if (Tiles.Count == 0) { Tiles = null; }
        }
        if ((Previews == null || forceRefresh) && Tiles != null) {
            Previews = new Dictionary<MapObject, Texture2D>();

            foreach (MapObject tile in Tiles) {
                if (!Previews.ContainsKey(tile)) {
                    Texture2D preview = AssetPreview.GetAssetPreview(tile.gameObject);
                    if (preview != null) {
                        Previews.Add(tile, preview);
                    }
                }
            }
        }
    }

    private void EditMap()
    {
        Ray mapRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        int tpX = Mathf.FloorToInt(mapRay.origin.x); int tpY = Mathf.FloorToInt(mapRay.origin.y);
        if (_activeTile != null) {
            tpX = Mathf.FloorToInt(mapRay.origin.x / (int)_activeTile.MapSize) * (int)_activeTile.MapSize;
            tpY = Mathf.FloorToInt(mapRay.origin.y / (int)_activeTile.MapSize) * (int)_activeTile.MapSize;
        }
        _cursorPos = new Vector2(tpX, tpY);

        Event curr = Event.current;
        if (_activeTile != null) {
            float offset = (int)_activeTile.MapSize % 2 == 0 ? 1f : 0;
            _activeTile.transform.position = new Vector3(tpX + offset, tpY + offset, 0);

            int size = (int)_activeTile.MapSize;
            bool inMapBound = true;
            for (int x = 0; x < size; x++) {
                for (int y = 0; y < size; y++) {
                    Vector3 tPos = new Vector3(tpX + x, tpY + y, 0);
                    if (!_target.IsMapBounded(tPos)) {
                        inMapBound = false;
                        continue;
                    }
                }
            }

            if (!inMapBound) {
                _activeTile.gameObject.SetActive(false);
            }
            else {
                _activeTile.gameObject.SetActive(true);
            }

            if (curr.type == EventType.KeyDown && curr.keyCode == KeyCode.Q && inMapBound) {
                bool reg = _target.CanPlaceMapObject(_activeTile);
                if (reg) {
                    _activeTile.transform.SetParent(_target.transform);
                    MapObject newTile = Instantiate(_activeTile, _activeTile.transform.position, Quaternion.identity) as MapObject;
                    newTile.gameObject.name = _activeTile.name;
                    _activeTile = newTile;
                }
            }
            else if (curr.type == EventType.KeyDown && curr.keyCode == KeyCode.R) {
                _activeTile.Direction++;
            }
        }
        else {
            if (curr.type == EventType.KeyDown && curr.keyCode == KeyCode.Q) {
                if (_target.IsMapBounded(tpX, tpY)) {
                    MapObject mo = _target.Map[tpX, tpY].RegisterObject;
                    if (mo!= null) {
                        _target.UnregisterMapObject(mo);
                    }
                }
            }
        }

        if (curr.type == EventType.MouseDown && curr.button == 1) {
            ClearActiveTile();
        }
    }

    private void ClearActiveTile()
    {
        if (_activeTile != null) {
            DestroyImmediate(_activeTile.gameObject);
            _activeTile = null;
        }
    }

    [DrawGizmo(GizmoType.InSelectionHierarchy | GizmoType.Active)]
    private static void MyCustomOnDrawGizmosSelected(Stage targetObj, GizmoType gizmoType)
    {
        if (targetObj == null || targetObj.Map == null || targetObj.Map.SizeX == 0 || targetObj.Map.SizeY == 0) { return; }
        Gizmos.matrix = targetObj.transform.localToWorldMatrix;

        
        if (_activeTile == null || _activeTile.MapSize == MapSize.x1) {
            DrawMapFrame(targetObj.Map.SizeX, targetObj.Map.SizeY, 1f);
            DrawMapGrid(targetObj.Map.SizeX, targetObj.Map.SizeY, 1f);
            DrawGizmoBrush(1);
        }
        else {
            DrawMapFrame(targetObj.Map.SizeX / 2, targetObj.Map.SizeY / 2, 2f);
            DrawMapGrid(targetObj.Map.SizeX / 2, targetObj.Map.SizeY / 2, 2f);
            DrawGizmoBrush(2);
        }
    }

    private static void DrawMapFrame(int x, int y, float size)
    {
        Gizmos.DrawLine(new Vector3(0, 0, 0), new Vector3(0, y * size, 0));
        Gizmos.DrawLine(new Vector3(0, 0, 0), new Vector3(x * size, 0, 0));
        Gizmos.DrawLine(new Vector3(x * size, 0, 0), new Vector3(x * size, y * size, 0));
        Gizmos.DrawLine(new Vector3(0, y * size, 0), new Vector3(x * size, y * size, 0));
    }

    private static void DrawMapGrid(int x, int y, float size)
    {
        Gizmos.color = Color.grey;
        for (int i = 1; i < x; i++) {
            Gizmos.DrawLine(new Vector3(i * size, 0, 0), new Vector3(i * size, y * size, 0));
        }
        for (int j = 1; j < y; j++) {
            Gizmos.DrawLine(new Vector3(0, j * size, 0), new
            Vector3(x * size, j * size, 0));
        }
    }

    private static void DrawGizmoBrush(int snapSize)
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(new Vector3(_cursorPos.x + (0.5f * snapSize), _cursorPos.y + (0.5f * snapSize), 0.5f), 
            new Vector3(0.2f, 0.2f, 0.2f));
    }

    public static List<T> GetAssetsWithScript<T>(string path) where T : MonoBehaviour
    {
        T tmp;
        string assetPath;
        GameObject asset;
        List<T> assetList = new List<T>();
        string[] guids = AssetDatabase.FindAssets("t:Prefab", new
        string[] { path });
        for (int i = 0; i < guids.Length; i++) {
            assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
            asset = AssetDatabase.LoadAssetAtPath(assetPath,
            typeof(GameObject)) as GameObject;
            tmp = asset.GetComponent<T>();
            if (tmp != null) {
                assetList.Add(tmp);
            }
        }
        return assetList;
    }
}
