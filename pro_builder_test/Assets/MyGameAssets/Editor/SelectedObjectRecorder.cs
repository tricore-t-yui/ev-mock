//  SelectedObjectRecorder.cs
//  http://kan-kikuchi.hatenablog.com/entry/SelectedObjectRecorder
//
//  Created by kan.kikuchi on 2019.07.03.

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 選択していたオブジェクトを記録するクラス
/// </summary>
public class SelectedObjectRecorder : EditorWindow
{

    //選択していたObjectのID群
    private static List<int> _selectedObjectIDs = new List<int>();

    //現在選択してるObjectが何番目に選択されたやつか
    private static int _currentObjectNo
    {
        get
        {
            //何も選択してない場合は-1を返す
            if (Selection.activeObject == null)
            {
                return -1;
            }
            return _selectedObjectIDs.IndexOf(Selection.activeObject.GetInstanceID());
        }
    }

    //一つ前に選択していたオブジェクトが何番目に選択されたやつか
    private static int _beforeObjectNo = -1;

    //初期化したか
    private static bool _isInitialized = false;

    //過去のオブジェクトを選択した
    private static bool _isSelectedPastObject = false;

    //スクロール位置
    private Vector2 _scrollPosition = Vector2.zero;

    //EditorUserSettingsに保存する時のKey
    private const string SELECTION_OBJECT_COUNT_SAVE_KEY = "SELECTION_OBJECT_COUNT_SAVE_KEY";
    private const string SELECTION_OBJECT_SAVE_KEY = "SELECTION_OBJECT_SAVE_KEY";

    //=================================================================================
    //初期化
    //=================================================================================

    //初期化していなければ初期化する
    private static void InitIfNeeded()
    {
        if (_isInitialized)
        {
            return;
        }
        _isInitialized = true;
        Init();
    }

    //初期化(InitializeOnLoadMethodはEditorUtility.InstanceIDToObjectが絶対nullになるので、使わない)
    private static void Init()
    {
        //選択しているオブジェクトが変わった時のイベントにメソッド登録
        Selection.selectionChanged += OnChangedSelection;

        //選択していたオブジェクトの数を取得
        string numText = EditorUserSettings.GetConfigValue(SELECTION_OBJECT_COUNT_SAVE_KEY);
        if (!string.IsNullOrEmpty(numText))
        {

            int num = int.Parse(numText);

            //選択していたオブジェクトのIDを取得
            for (int i = 0; i < num; i++)
            {
                int id = int.Parse(EditorUserSettings.GetConfigValue(SELECTION_OBJECT_SAVE_KEY + i));

                //IDからオブジェクトを検索し、存在しなければスルー
                if (EditorUtility.InstanceIDToObject(id) == null)
                {
                    continue;
                }
                _selectedObjectIDs.Add(id);
            }
        }

        _beforeObjectNo = _selectedObjectIDs.Count - 1;
        Save();
    }

    //メニューからウィンドウを表示
    [MenuItem("Tools/Selected Object Recorder/Open Window")]
    private static void Open()
    {
        GetWindow(typeof(SelectedObjectRecorder));
    }

    //=================================================================================
    //保存
    //=================================================================================

    //EditorUserSettingsに選択履歴を保存する
    private static void Save()
    {
        //履歴の数を保存
        EditorUserSettings.SetConfigValue(SELECTION_OBJECT_COUNT_SAVE_KEY, _selectedObjectIDs.Count.ToString());

        //選択していたオブジェクトのIDを保存
        for (int i = 0; i < _selectedObjectIDs.Count; i++)
        {
            EditorUserSettings.SetConfigValue(SELECTION_OBJECT_SAVE_KEY + i, _selectedObjectIDs[i].ToString());
        }
    }

    //=================================================================================
    //イベント
    //=================================================================================

    //選択しているオブジェクトが変わった
    private static void OnChangedSelection()
    {
        InitIfNeeded();

        //nullになったもの(削除されたもの)は記録から消す
        _selectedObjectIDs.RemoveAll(no => EditorUtility.InstanceIDToObject(no) == null);

        //選択しているオブジェクトがない場合や、過去のオブジェクトを選択した場合は終了
        if (Selection.activeObject == null || _isSelectedPastObject)
        {
            _isSelectedPastObject = false;
            _beforeObjectNo = _currentObjectNo;
            Save();
            return;
        }

        //選択したオブジェクトのIDを取得
        int id = Selection.activeObject.GetInstanceID();

        //選択してたやつより後のやつを記録から削除(新たに選択したやつを最後尾にするため)
        while (_selectedObjectIDs.Count - 1 > _beforeObjectNo)
        {
            _selectedObjectIDs.RemoveAt(_selectedObjectIDs.Count - 1);
        }

        //既に選択済みのやつだったら一旦Listから消す(最後尾に追加するため)
        if (_selectedObjectIDs.Contains(id))
        {
            _selectedObjectIDs.Remove(id);
        }

        //Listに追加し、保存
        _selectedObjectIDs.Add(id);
        _beforeObjectNo = _currentObjectNo;
        Save();
    }

    //=================================================================================
    //表示するGUIの設定
    //=================================================================================

    private void OnGUI()
    {
        InitIfNeeded();

        //まだ何も選択していない場合は何も表示しない
        if (_selectedObjectIDs.Count == 0)
        {
            return;
        }

        //nullになったもの(削除されたもの)は記録から消す
        _selectedObjectIDs.RemoveAll(no => EditorUtility.InstanceIDToObject(no) == null);

        //描画範囲が足りなければスクロール出来るように
        _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

        //選択したオブジェクトごとにUI表示
        EditorGUILayout.BeginHorizontal(GUI.skin.box);

        var count = _selectedObjectIDs.Count;
        for (var i = count-1; i >= 0; --i)
        {
            ShowSelectedObjectGUI(i);
        }

        EditorGUILayout.EndHorizontal();

        //選択していたオブジェクトの履歴を消すボタン
        GUILayout.Space(10);
        if (GUILayout.Button("削除"))
        {
            Clear();
        }

        //スクロール箇所終了
        EditorGUILayout.EndScrollView();
    }

    //選択したオブジェクトのGUIを表示
    private void ShowSelectedObjectGUI(int no)
    {
        Object selectedObject = EditorUtility.InstanceIDToObject(_selectedObjectIDs[no]);

        //選択中のものはラベルで表示、それ以外はボタンで表示し、ボタンを押したらそのオブジェクトを選択するように
        if (Selection.activeObject == selectedObject)
        {
            GUILayout.Label(selectedObject.name);
        }
        else if (GUILayout.Button(selectedObject.name))
        {
            SelectPastObject(no);
        }
    }

    //指定した番号の過去のオブジェクトを選択する
    private static void SelectPastObject(int no)
    {
        InitIfNeeded();

        no = Mathf.Clamp(no, 0, _selectedObjectIDs.Count - 1);
        if (_currentObjectNo == no)
        {
            return;
        }

        _isSelectedPastObject = true;
        Selection.activeObject = EditorUtility.InstanceIDToObject(_selectedObjectIDs[no]);
    }

    //=================================================================================
    //メニュー
    //=================================================================================

    //一つ前に選択していたオブジェクトを選択
    [MenuItem("Tools/Selected Object Recorder/Back %&b")]
    private static void Back()
    {
        SelectPastObject(_currentObjectNo - 1);
    }

    //一つ後に選択したオブジェクトを選択
    [MenuItem("Tools/Selected Object Recorder/Advance %&a")]
    private static void Advance()
    {
        SelectPastObject(_currentObjectNo + 1);
    }

    //選択していたオブジェクトの履歴を消す
    [MenuItem("Tools/Selected Object Recorder/Clear")]
    private static void Clear()
    {
        _selectedObjectIDs.Clear();
        _beforeObjectNo = -1;
        Save();
    }

}