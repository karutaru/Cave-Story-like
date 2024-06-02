using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Cinemachine;

/// <summary>
/// ゲームの進行に関わるスクリプト
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager game; // GameManager.game.という書き方で記述する。
    // システム◆◆◆◆◆
    [Header("システム")]
    public InitScript init; // 初期化

    // オブジェクト◆◆◆
    [Header("オブジェクト")]
    public Transform playerObj; // プレイヤーのオブジェクト
    public GameObject object_UI; // UI画像
    public GameObject object_AIM; // プレイヤーのAIM画像
    public Transform tran_PlayerCamera; // プレイヤー用のカメラポイント
    public Transform tran_EventCamera; // イベント用のカメラポイント
    public CinemachineVirtualCamera system_mainCamera; // カメラ

    // エフェクト◆◆◆
    [Header("エフェクト")]
    public GameObject wallEffectPrefab;
    public List<GameObject> bloodEffectPrefabs;

    // SE◆◆◆
    [Header("SE")]
    public AudioClip shotHitSE;

    // スクリプト◆◆◆◆
    [Header("スクリプト")]
    public CameraFollowMouse cameraFollowMouse;         // マウスをカメラが追従する処理
    public PlayerBodyController playerBodyController;   // プレイヤーのスクリプト
    public PlayerBodyRotation playerBodyRotation;       // プレイヤーの移動の振りむきスクリプト
    public PlayerHandsScript playerHandsScript;         // プレイヤーの移動の腕振りスクリプト
    public GunShot gunShot;                             // gunShotスクリプト
    public Knockback_Player knockback_Player;

    // スイッチ◆◆◆◆◆
    [Header("スイッチ")]
    public bool changeCamera_If = true;

    // ボタン◆◆◆◆◆◆
    [Button]private void UIとAIMとカメラのマウス追従無効()
    {
        Title_Disable();
    }

    [Button] private void プレイヤーカメラとイベントカメラを切り替え()
    {
        ChangeCamera();
    }
    [Button]private void プレイヤーの動きを止める()
    {
        Player_StopMove();
    }
    [Button]private void プレイヤーの動きを自由にする()
    {
        Player_CanMove();
    }
    [Button]
    private void 銃の射撃禁止()
    {
        Shot_Disable();
    }

    // ------------------------------------------------------------------------------------- ◆↓スクリプト↓◆ -------------------------------------------------------------------------------------------

    void Awake()
    {
        if (game == null)
        {
            game = this;
        }
        else if (game != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // タイトル画面モードに移行
        //Title_Disable();
    }

    /// <summary>
    /// タイトル画面モード
    /// </summary>
    public void Title_Disable()
    {
        // 各オブジェクトの有効/無効状態を切り替えます。
        // UIを非表示
        object_UI.SetActive(false);
        // AIMマークを非表示
        object_AIM.SetActive(false);
        // カメラのマウスへの追従を禁止
        cameraFollowMouse.enabled = false;
        // プレイヤーの動きを停止
        Player_StopMove();
        // 射撃禁止
        Shot_Disable();
    }

    public void Title_Enable()
    {
        // 各オブジェクトの有効/無効状態を切り替えます。
        // UIを非表示
        object_UI.SetActive(true);
        // AIMマークを非表示
        object_AIM.SetActive(true);
        // カメラのマウスへの追従を禁止
        cameraFollowMouse.enabled = true;
        // プレイヤーの動きを停止
        Player_CanMove();
        // 射撃禁止
        Shot_Enable();
    }

    public void Player_StopMove()
    {
        playerBodyController.isMove = true;     // プレイヤーの動き停止
        playerBodyRotation.enabled = false;     // プレイヤーの振り向き停止
        playerHandsScript.enabled = false;      // プレイヤーの腕振り停止
        playerBodyController.isJump = false;    // プレイヤーのジャンプ禁止
    }
    public void Player_CanMove()
    {
        playerBodyController.isMove = false;    // プレイヤーの動き開始
        playerBodyRotation.enabled = true;      // プレイヤーの振り向き開始
        playerHandsScript.enabled = true;       // プレイヤーの腕振り開始
        playerBodyController.isJump = true;     // プレイヤーのジャンプ解禁
    }

    public void Shot_Enable() // 射撃を許可する
    {
        if (gunShot != null)
        {
            gunShot.isShot = true;
        }
    }

    // 銃を撃てない状態にする
    public void Shot_Disable() // 射撃を禁止する
    {
        if (gunShot != null)
        {
            gunShot.isShot = false;
        }
    }

    public void ChangeCamera()
    {
        if (changeCamera_If == true)
        {
            system_mainCamera.Follow = tran_EventCamera; // イベントカメラに切り替える
            changeCamera_If = false;
        }
        else if (changeCamera_If == false)
        {
            system_mainCamera.Follow = tran_PlayerCamera; // プレイヤーカメラに切り替える
            changeCamera_If = true;
        }
    }
}
