using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;
using Sirenix.OdinInspector;
using Cinemachine;

public class SystemTextManager : MonoBehaviour
{
    public static SystemTextManager instanceSystemText;

    [SerializeField]
    private Image textBox;
    [SerializeField]
    private Image MiniBox;
    [SerializeField]
    private Text systemText;
    [SerializeField]
    private float messageSpeed = 0.05f;
    [SerializeField]
    private AudioClip systemSE;

    AudioSource audioSource;
    private bool isDisplay;
    private string beforeText;

    public PlayerStatus playerStatus;
    public PlayerBodyController playerBodyController;

    public CinemachineVirtualCamera virtualCamera;
    private float originalXDamping;
    private float originalLookaheadTime;





    void Awake()
    {
        if (instanceSystemText == null)
        {
            instanceSystemText = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }

        
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        originalXDamping = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_XDamping;
        originalLookaheadTime = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_LookaheadTime;
    }

    private static readonly string[] INVALID_CHARS = {
        " ", "　", "!", "?", "！", "？", "\"", "\'", "\\",
        ".", ",", "、", "。", "…", "・"
    };

    public void PrepareSystemMessage(string systemMessage, EventBase eventBase)
    {
        StartCoroutine(SyestemMessage(systemMessage, eventBase));
        beforeText = systemMessage;
    }

    private IEnumerator SyestemMessage(string systemMessage, EventBase eventBase)
    {

        textBox.enabled = true;
        systemText.enabled = true;

        systemText.DOText(systemMessage, systemMessage.Length * messageSpeed).SetEase(Ease.Linear).OnUpdate(() =>
        {
            var currentText = systemText.text;
            if (beforeText == currentText)
            {
                return;
            }
            var lastIndex = currentText.Length - 1;
            if (lastIndex >= 0)
            {
                var newChar = currentText[lastIndex].ToString();
                if (!INVALID_CHARS.Contains(newChar))
                {
                    audioSource.PlayOneShot(systemSE);
                }
            }

            //次のチェック用にテキスト更新
            beforeText = currentText;

        }).OnComplete(() =>
        {
            isDisplay = true;
        });
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E) && isDisplay == true);
        ExitSystemText();
        eventBase.isEventPlay = false;
    }

    private void ExitSystemText()
    {
        textBox.enabled = false;
        systemText.text = string.Empty;
        isDisplay = false;
    }


    public void PrepareSystem_Heal(bool heal, EventBase eventBase)
    {
        playerStatus.HealHP();
    }
    public void PrepareSystem_Warp(GameObject tranPointGameObject, PosEnum afterPlayerPos, EventBase eventBase)
    {
        // Dampingを設定する
        virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_XDamping = 0;
        // LookaheadTimeを設定する
        virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_LookaheadTime = 0;
        Debug.Log(virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_XDamping);

        if (afterPlayerPos == PosEnum.左)
        {
            // プレイヤーのワープ
            playerBodyController.gameObject.transform.position = tranPointGameObject.transform.position + new Vector3(-1, -0.5f, 0);
        }
        if (afterPlayerPos == PosEnum.右)
        {
            // プレイヤーのワープ
            playerBodyController.gameObject.transform.position = tranPointGameObject.transform.position + new Vector3(1, -0.5f, 0);
        }

        // 1秒後にDampingとLookaheadTimeを元の値に戻す
        Invoke("ResetDampingAndLookaheadTime", 1f);


        eventBase.isEventPlay = false;
    }

    private void ResetDampingAndLookaheadTime()
    {
        // Dampingを元の値に戻す
        virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_XDamping = originalXDamping;
        // LookaheadTimeを元の値に戻す
        virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_LookaheadTime = originalLookaheadTime;
    }


    public void PrepareSystem_Warp_MonsterDebug(GameObject tranPointGameObject, PosEnum afterPlayerPos, EventBase eventBase)
    {
        // Dampingを設定する
        //virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_XDamping = 0;
        // LookaheadTimeを設定する
        //virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_LookaheadTime = 0;
        Debug.Log(virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_XDamping);

        if (afterPlayerPos == PosEnum.左)
        {
            // プレイヤーのワープ
            playerBodyController.gameObject.transform.position = tranPointGameObject.transform.position + new Vector3(-1, -0.5f, 0);
        }
        if (afterPlayerPos == PosEnum.右)
        {
            // プレイヤーのワープ
            playerBodyController.gameObject.transform.position = tranPointGameObject.transform.position + new Vector3(1, -0.5f, 0);
        }

        // 1秒後にDampingとLookaheadTimeを元の値に戻す
        //Invoke("ResetDampingAndLookaheadTime", 1f);


        eventBase.isEventPlay = false;
    }
}
