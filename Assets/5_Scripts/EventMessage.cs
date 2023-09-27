using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EventMessage : EventBase
{
    public enum FACE
    {
        なし,
        普通,
        笑顔,
        怒り顔,
        悲しい顔,
        楽しい顔
    }

    public TalksList[] talksList;

    [System.Serializable]
    public struct TalksList
    {
        [Multiline(3)]
        public string talks;
        public FACE playerFaceName; // 表示するプレイヤーの顔
        public FACE doctorFaceName; // 表示するドクターの顔
    }

    [SerializeField]
    protected int talksNumber = 0;

    protected EventBase eventBase;

    //-------------------------------------------------ここまで------------------------------------------------------------------


    /// <summary>
    /// イベントを実行
    /// </summary>
    public override void ExecuteEvent()
    {
        //base.ExecuteEvent();

        // 配列の範囲内にあるかどうかをチェック
        if (talksNumber < talksList.Length && DisplayManager.instance != null)
        {
            // ゲーム画面にメッセージウィンドを表示
            if (talksList[talksNumber].talks != null)
            {
                DisplayManager.instance.PrepareDisplayMessage(talksList[talksNumber].talks, GetComponent<EventBase>(), GetComponent<EventMessage>(), talksList[talksNumber].playerFaceName, talksList[talksNumber].doctorFaceName);
            }
        }
        else
        {
            // 会話を初期化
            talksNumber = 0;
        }
    }


    /// <summary>
    /// 会話を一巡した時に起動
    /// </summary>
    public override void FinishEvent()
    {
        // 会話を次の段階へ
        talksNumber++;

        // 配列の範囲内にあるかどうかをチェック
        if (talksNumber < talksList.Length)
        {
            ExecuteEvent();
        } else
        {
            eventBase = GetComponent<EventBase>();
            eventBase.isEventPlay = false;

            // 会話を初期化
            talksNumber = 0;
        }
    }
}
