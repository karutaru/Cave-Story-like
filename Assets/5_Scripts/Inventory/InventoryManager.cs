using System;
using System.Collections.Generic;
using InputAsRx.Triggers;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryManager : MonoBehaviour
{
#pragma warning disable 0649  // [SerializeField] を利用している際に Console に表示される CS0649 の警告を無視する(表示させない)
    [SerializeField] private GameObject inventory_UI;           // インベントリー
    [SerializeField] private GameObject[] inventoryMenus;       // 個別に用意せず、配列や List で管理する。増減したときにソースコードを変更しなくて済むため
    [SerializeField] private GameObject itemIconPrefab;         // アイテムのゲームオブジェクトのプレハブ
    
    [SerializeField] private List<Transform> gridList = new();  // インベントリ内に配置する Grid の位置
    [SerializeField] private GameObject cursor;                 // カーソル
    [SerializeField] private bool isMemoryCursorPos;            // カーソルの位置を記録するかどうか。true なら記録する
#pragma warning restore 0649  // ここまでの変数が対象
    
    private Transform[,] inventryGrid;   // 多次元配列を利用してインベントリ内の Grid を座標として管理する
    private int numRows = 4;             // インベントリの Grid 行数(X 方向)
    private int numColumns = 3;          // インベントリの Grid 列数(Y 方向)
    
    private int cursor_horizontal = 0;   // カーソルの X 軸の位置
    private int cursor_vertical = 0;     // カーソルの Y 軸の位置
    private float inputDelay = 0.25f;    // キー入力時の待機時間。連続入力を防ぎ、一気に移動しないように制御する

    private int inventoryMenuNum;        // 現在のインベントリーメニューの番号

    
    void Start()
    {
        // 初期設定
        // SetUpInventoryManager() メソッドを利用する場合には、処理内部をコメントアウトする
        InitializeInventory();
    }
    
    /// <summary>
    /// 初期設定
    /// </summary>
    private void InitializeInventory()
    {
        inventory_UI.SetActive(false);
        
		// インベントリ内の Grid の設定
        SetGrids();
		
        // インベントリ内にアイテムアイコン作成
        //GenerateItemIcons();
        
        // インベントリ内のカーソル位置の初期設定
        SetDefaultCursorPos(0);

        // インベントリ内のカーソル移動の入力設定
        HandleCursorInputs();
    }    

    /// <summary>
    /// インベントリ内の Grid の設定
    /// </summary>
    private void SetGrids() {
        
        // TODO この中の処理は、インベントリのメニューの Grid の形で変わる
        
        // 今回は四角形ではない場合なので、座標を作り直す
        inventryGrid = new Transform[numRows, numColumns];

        int count = 0;
        for (int x = 0; x < numRows; x++) {
            for (int y = 0; y < numColumns; y++) {
                if ((x == 2 || x == 3) && y == 0) {
                    continue;
                }

                inventryGrid[x, y] = gridList[count];
                gridList[count].name = $"{x} : {y}";
                count++;
            }
        }

        // 多次元配列時のメニュー１内の座標 [x-y]
        // [0-0, 1-0]
        // [0-1, 1-1, 2-1, 3-1]
        // [0-2, 1-2, 2-2, 3-2]
    }

    /// <summary>
    /// 所持しているアイテムの情報を元にしてアイテムアイコンを生成して配置する
    /// </summary>
    private void GenerateItemIcons() {

        if (itemIconPrefab == null) {
            return;
        }

        // TODO 仮処理。アイテムアイコンの生成
        for (int i = 0; i < gridList.Count; i++) {
            Instantiate(itemIconPrefab, gridList[i], false);
            
            // TODO ここでアイテムのデータを設定する
            
        }
    }
    
    /// <summary>
    /// カーソル関連の初期設定
    /// および、カーソル位置を記憶していない場合のカーソル設定
    /// </summary>
    private void SetDefaultCursorPos(int menuNum) {
        // カーソル情報の初期化
        cursor_vertical = 0;
        cursor_horizontal = 0;

        inventoryMenuNum = menuNum;
            
        // カーソルの位置を記憶しない場合、初期位置にフォーカスしてカーソルも初期位置に置く
        // TODO InventoryGrid は、メニューごとに設定するので、List 化する
        EventSystem.current.SetSelectedGameObject(inventryGrid[0, 0].gameObject);
        
        cursor.transform.SetParent(inventryGrid[0, 0]);
        cursor.transform.localPosition = Vector2.zero;
    }
    
    /// <summary>
    /// キー入力制御
    /// </summary>
    private void HandleCursorInputs() {

        // namespace InputAsRx.Triggers に用意されている UniRx の拡張メソッドを利用する
        this.OnAxisRawAsObservable("Horizontal")      // GetAxisRaw メソッドと同じ
            .Where(horizontal => horizontal != 0)     // キー入力があるか確認
            .ThrottleFirst(TimeSpan.FromSeconds(inputDelay))     // 連続入力を制御。inputDelay 時間だけ連続入力を受け付けない
            .Subscribe(horizontal => MoveCursor((int)horizontal, 0));   // キー入力の値を MoveCursor に渡して移動先を決める
        
        this.OnAxisRawAsObservable("Vertical")
            .Where(vertical => vertical != 0)
            .ThrottleFirst(TimeSpan.FromSeconds(inputDelay))
            .Subscribe(vertical => MoveCursor(0, (int)vertical));
    }
    
    /// <summary>
    /// カーソル移動
    /// </summary>
    /// <param name="horizontal"></param>
    /// <param name="vertical"></param>
    private void MoveCursor(int horizontal, int vertical) {
        //Debug.Log($"入力値 {horizontal} : {vertical}");
        
        // 新しいカーソル位置が移動できない地点である可能性があるので、一旦、現在の位置を保持しておく
        Vector2Int tempPos = new(cursor_horizontal, cursor_vertical);
        
        // インベントリー内の Grid からはみ出ないように補正(配列外にならないように)した上でカーソル位置を更新
        cursor_horizontal = Mathf.Clamp(cursor_horizontal + horizontal, 0, numRows -1);
        cursor_vertical = Mathf.Clamp(cursor_vertical - vertical, 0,  numColumns -1);

        //Debug.Log($"新しい位置 {cursor_horizontal} : {cursor_vertical}");

        // 新しい位置が移動できない位置(配列外)の場合には、現在の位置をもう一度指定する
        if (cursor_vertical == 0 && (cursor_horizontal == 2 || cursor_horizontal == 3)) {
            cursor_horizontal = tempPos.x;
            cursor_vertical = tempPos.y;
        }

        // カーソル移動
        cursor.transform.SetParent(inventryGrid[cursor_horizontal, cursor_vertical]);
        cursor.transform.localPosition = Vector2.zero;

        // TODO 親子関係にしない場合の移動。こちらも問題なし
        //cursor.transform.position = inventryGrid[cursor_horizontal, cursor_vertical].position;
    }
    
    void Update()
    {
        // TODO インベントリーを開けない状態がある場合には、ここで制御する
        
        // 各ボタン制御
        ProcessInventoryButtons();
    }
    
    /// <summary>
    /// 各ボタンの制御
    /// </summary>
    private void ProcessInventoryButtons()
    {
        // インベントリーの表示/非表示の制御
        if (Input.GetKeyDown(KeyCode.E))
        {
            // インベントリーの状態に応じて表示/非表示を切り替える
            if (inventory_UI.activeSelf) {
                HideInventory();
                //Time.timeScale = 1;
            } else {
                ShowInventry();
                //Time.timeScale = 0;
            }
        }

        // インベントリー内のメニュー切り替え
        if (Input.GetKeyDown(KeyCode.R)) {

            // メニュー番号を増やす。インベントリメニューの数を超えたら 0 に戻す
            inventoryMenuNum = ++inventoryMenuNum % inventoryMenus.Length;

            // インベントリーメニュー切り替え
            ChangeInventoryMenus();
        }
    }

    /// <summary>
    /// インベントリー非表示
    /// </summary>
    private void HideInventory() {
        inventory_UI.SetActive(false);
        
        // TODO 他にもインベントリーを閉じる際に必要な処理があれば追加する
        
        
    }
    
    /// <summary>
    /// インベントリの表示
    /// </summary>
    private void ShowInventry() {
        inventory_UI.SetActive(true);

        // カーソルをフォーカスする位置の設定
        if (!isMemoryCursorPos) {
            // カーソル位置を記憶させない場合、初期メニューを初期カーソル位置で開く
            SetDefaultCursorPos(0);
        }
        else {
            // カーソルの位置を記憶させる場合、カーソルの位置にフォーカスし、記憶したメニューで開く
            SetMemoryCursorPos();
        }

        // 記録されているメニューを開く。カーソル位置を記憶していない場合には初期メニューが開く
        ChangeInventoryMenus();
    }
    
    /// <summary>
    /// 表示するメニューを更新
    /// </summary>
    private void ChangeInventoryMenus() {
        
        for (int i = 0; i < inventoryMenus.Length;i++) {
            // if (i == inventryMenuNum) {
            //     inventoryMenus[i].SetActive(true);
            // }
            // else {
            //     inventoryMenus[i].SetActive(false);
            // }
            
            // 上記の if / else を1行で書く場合(評価結果が bool 値になるので、それを利用する)
            inventoryMenus[i].SetActive(i == inventoryMenuNum);
        }
        
        // TODO メニューごとにカーソル座標を保持しておき、その位置に戻す
        
    }
    
    /// <summary>
    /// カーソル位置を記憶している場合のカーソル設定
    /// </summary>
    private void SetMemoryCursorPos() {
        // カーソルの位置を記憶させる場合、カーソルの位置にフォーカスする
        EventSystem.current.SetSelectedGameObject(cursor.transform.gameObject);
    }
    
    /// <summary>
    /// 外部クラスから実行する
    /// このメソッドは Start メソッドを利用しない場合に使う
    /// </summary>
    public void SetUpInventoryManager() {   // 必要に応じて引数を設定する
        
        // TODO 引数を利用する場合には、ここで利用する
        
        // そのため、Start メソッド内の内容をここに記載する
        InitializeInventory();
    }
}