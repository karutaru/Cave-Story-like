using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;
using UniRx.Triggers;

public class PropertyTest : MonoBehaviour
{
    public ReactiveProperty<int> Score = new();
    [SerializeField]
    private Text textScore;
    [SerializeField]
    private Button buttonSub;

    void Start()
    {
        Score.Subscribe(score  => {
            UpdateDisplayScore(score);
            Debug.Log(score); });

        this.UpdateAsObservable()
            .Where(_ => Input.GetMouseButtonDown(0))
            .ThrottleFirst(System.TimeSpan.FromSeconds(2.0f))
            .Subscribe(_ =>
            {
                    Score.Value += 1;
            });

        buttonSub.OnClickAsObservable()
            .ThrottleFirst(System.TimeSpan.FromSeconds(2.0f))
            .Subscribe(_ =>
            {
                Score.Value -= 1;
            });
        this.OnCollisionEnter2DAsObservable()
            .Where(col => col.gameObject.tag == "Enemy")
            .Subscribe(col => Debug.Log(col.gameObject.name));
    }

    //private void Update()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        Score.Value += 1;
    //    }
    //}

    private void UpdateDisplayScore(int score)
    {
        textScore.text = score.ToString();
    }
}
