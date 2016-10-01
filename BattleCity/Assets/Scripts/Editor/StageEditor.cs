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
            return guiStyle;
        }
    }

    private Stage _target;
    private string _mapName;
    private Vector2 _mapSize;
    private bool _showNewMap;

    private MapObject _activeTile;

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

        EditMap();

        //EditorUtility.SetDirty(_target);
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
        EditorGUILayout.LabelField("Map size", GUILayout.Width(115));
        _mapSize = EditorGUILayout.Vector2Field("",new Vector2(
           Mathf.FloorToInt(Mathf.Clamp(_mapSize.x, 1, 50)),
            Mathf.FloorToInt(Mathf.Clamp(_mapSize.y, 1, 50))), GUILayout.MaxWidth(100)
            );
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Create new")) {
            if (_target.Map != null) {
                _target.ClearMap();
            }
            StageMap map = new StageMap((int)_mapSize.x, (int)_mapSize.y);
            map.MapName = _mapName;
            _target.Map = map;
        }
        EditorGUILayout.EndFadeGroup();
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
        int tilesCount = Tiles.Count;
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
            Texture2D tex = Previews[Tiles[i]];
            string name = Tiles[i].name;
            GUIContent cont = new GUIContent(name, tex);
            guiContents.Add(cont);
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
        int tpX = Mathf.FloorToInt(mapRay.origin.x);
        int tpY = Mathf.FloorToInt(mapRay.origin.y);
        Event curr = Event.current;


        if (_activeTile != null) {
            float offsetX = _activeTile.MapSize.x % 2 == 0 ? 1f : 0;
            float offsetY = _activeTile.MapSize.y % 2 == 0 ? 1f : 0;
            _activeTile.transform.position = new Vector3(tpX + offsetX, tpY + offsetY, 0);

            Vector2 size = _activeTile.MapSize;
            bool inMapBound = true;
            for (int x = 0; x < size.x; x++) {
                for (int y = 0; y < size.y; y++) {
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

        DrawMapFrame(targetObj.Map.SizeX, targetObj.Map.SizeY, 1f);
        DrawMapGrid(targetObj.Map.SizeX, targetObj.Map.SizeY, 1f);
        DrawGizmoBrush();
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

    private static void DrawGizmoBrush()
    {
        Ray mapRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        int tpX = Mathf.FloorToInt(mapRay.origin.x);
        int tpY = Mathf.FloorToInt(mapRay.origin.y);

        Gizmos.color = Color.green;
        Gizmos.DrawCube(new Vector3(tpX + 0.5f, tpY + 0.5f, 0.5f), new Vector3(0.2f, 0.2f, 0.2f));
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
