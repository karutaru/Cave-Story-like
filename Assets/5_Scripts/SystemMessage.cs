using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public enum SomeEnum
{
    会話,回復, ワープ
}
public enum PosEnum
{
    左, 右
}
public enum WarpEnum
{
    なし,モンスターデバッグ
}
public enum WarpEnum_2
{
    なし, テキストを表示, テキストをポップアップ
}




public class SystemMessage : EventBase
{

    [Title("オブジェクトの機能"), EnumToggleButtons, HideLabel]
    public SomeEnum wideEnumField;

    [Title("会話ON"), HideLabel]
    [ShowIf("wideEnumField", SomeEnum.会話)]
    public bool talk = false;

    [Title("メッセージ(会話)"), HideLabel]
    [ShowIf("wideEnumField", SomeEnum.会話), SerializeField, Multiline(3)]
    protected string systemMessage_Talk;

    [Title("回復ON"), HideLabel]
    [ShowIf("wideEnumField", SomeEnum.回復)]
    public bool heal = false;

    [Title("メッセージ(回復)"), HideLabel]
    [ShowIf("wideEnumField", SomeEnum.回復), SerializeField, Multiline(3)]
    protected string systemMessage_Heal;

    [Title("ワープON"), HideLabel]
    [ShowIf("wideEnumField", SomeEnum.ワープ)]
    public bool warp = false;

    [Title("ワープ先オブジェクト"), HideLabel]
    [ShowIf("wideEnumField", SomeEnum.ワープ)]
    public GameObject tranPointGameObject;

    [Title("ワープ先のプレイヤーの位置"), HideLabel]
    [ShowIf("wideEnumField", SomeEnum.ワープ), EnumPaging]
    public PosEnum afterPlayerPos;

    [Title("ワープの追加機能"), EnumToggleButtons, HideLabel]
    [ShowIf("wideEnumField", SomeEnum.ワープ)]
    public WarpEnum warpEnumField;

    [Title("ワープの追加機能その２"), EnumToggleButtons, HideLabel]
    [ShowIf("wideEnumField", SomeEnum.ワープ)]
    public WarpEnum_2 warpEnumField_2;

    [Title("テキストを後から表示"), HideLabel]
    [ShowIf("warpEnumField_2", WarpEnum_2.テキストを表示)]
    public bool warp_AfterText;

    [Title("メッセージ(ワープテキスト)"), HideLabel]
    [ShowIf("warpEnumField_2", WarpEnum_2.テキストを表示)]
    [SerializeField, Multiline(3)]
    protected string systemMessage_Warp;

    [Title("テキストを後からポップアップ"), HideLabel]
    [ShowIf("warpEnumField_2", WarpEnum_2.テキストをポップアップ)]
    public bool warp_AfterTextPopUp;

    [Title("メッセージ(ワープテキスト)"), HideLabel]
    [ShowIf("warpEnumField_2", WarpEnum_2.テキストをポップアップ)]
    [SerializeField, Multiline(3)]
    protected string systemMessage_Warp_2;





    public override void ExecuteEvent()
    {
        if (warpEnumField_2 == WarpEnum_2.なし) // テキストを表示 or ポップアップ
        {

        }
        else
        {
            //ゲーム画面にメッセージウィンドを表示
            SystemTextManager.instanceSystemText.PrepareSystemMessage(systemMessage_Heal, GetComponent<EventBase>());
        }
    }

    public override void ExecuteSelect()
    {
        //機能を選択して実行

        if (heal) // 回復
        {
            SystemTextManager.instanceSystemText.PrepareSystem_Heal(heal, GetComponent<EventBase>());
        }
        if (warp) // ワープ
        {
            if (warpEnumField == WarpEnum.モンスターデバッグ)
            {
                SystemTextManager.instanceSystemText.PrepareSystem_Warp_MonsterDebug(tranPointGameObject, afterPlayerPos, GetComponent<EventBase>());

            } else // なし
            {
                SystemTextManager.instanceSystemText.PrepareSystem_Warp(tranPointGameObject, afterPlayerPos, GetComponent<EventBase>());
            }
        }
    }
}
