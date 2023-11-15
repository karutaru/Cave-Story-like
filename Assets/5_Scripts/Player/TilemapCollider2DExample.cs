using UnityEngine;
using UniRx;
using UniRx.Triggers;

using UnityEngine.Tilemaps;

public class TilemapCollider2DExample : MonoBehaviour
{
    [SerializeField] private TilemapCollider2D tilemapCollider;
    
    void Start()
    {
        // TilemapCollider2Dを取得
        //if (!TryGetComponent(out tilemapCollider))
        //{
        //    Debug.Log("TilemapCollider2D not found on this GameObject.");
        //    return;
        //}
        //​
        // OnTriggerStay2DのイベントをUniRxで購読
        tilemapCollider
            .OnCollisionStay2DAsObservable()
            .Where(otherCollider => otherCollider.gameObject.TryGetComponent(out BulletController bullet))
            .Subscribe(otherCollider =>
            {
                // OnTriggerStay2Dイベントが発生した際の処理
                Debug.Log($"OnTriggerStay2D event triggered with collider: {otherCollider.gameObject.name}");
                
                // ここで弾を消す(otherCollider を利用する)
				if (otherCollider.gameObject.TryGetComponent(out BulletController bullet))
                {
                    Destroy(bullet.gameObject);
                }
            })
            .AddTo(this); // サブスクリプションを破棄するために、このコンポーネントが破棄された時に自動的に破棄するように登録
    }
}