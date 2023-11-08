using BehaviorDesigner.Runtime.Tasks.Unity.UnityVector2;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class GunRotation : MonoBehaviour
{
    // スクリプタブルオブジェクトの登録（アセットをアサイン）
    [SerializeField, Header("武器レベルデータのスクリプタブルオブジェクト")]
    private WeaponLevelDataSO weaponLevelDataSO;

    private SpriteRenderer spriteRenderer;

    private int currentWeaponID = 0;
    private float shotAccuracy = 0;

    // 弾に提供する回転情報
    private Vector3 bulletEulerAngles;
    public Vector3 BulletEulerAngles => bulletEulerAngles;

    // 弾に提供する方向情報
    private Vector3 bulletDirection;
    public Vector3 BulletDirection => bulletDirection;


    private void Start()
    {
        // また、TryGetComponent の場合、取得有無の確認もできるので、if 文と組み合わせて安全に利用する手法も選べます
        if (!TryGetComponent(out spriteRenderer))
        {
            // 取得できないときにはログを出す
            Debug.Log($"Sprite Renderer が取得出来ません : {spriteRenderer}");
        }
    }

    void Update()
    {
        if (Time.timeScale == 0)
        {
            return;
        }

        // マウスのスクリーン座標をワールド座標に変換
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new(Input.mousePosition.x, Input.mousePosition.y, transform.position.z));   // mainCamera.nearClipPlane

        // 銃の回転
        RotateGun(mousePosition);

        // 弾の方向と回転を計算
        CalculateBulletDirectionAndRotation(mousePosition);

        // マウスの位置により銃の画像を反転させる
        // mousePosition.x だと更新されない
        //FlipGunSprite();
        FlipGunSprite(Input.mousePosition.x);
    }

    /// <summary>
    /// 銃の回転
    /// </summary>
    /// <param name="mousePosition"></param>
    private void RotateGun(Vector3 mousePosition)
    {
        // マウスの方向を計算
        Vector2 direction = -(mousePosition - transform.position).normalized;

        // その方向に武器を回転させる
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, 0, angle);
    }

    /// <summary>
    /// 弾の方向と回転を計算
    /// </summary>
    /// <param name="mousePosition"></param>
    private void CalculateBulletDirectionAndRotation(Vector3 mousePosition)
    {
        // 弾の終着点のランダム性
        shotAccuracy = Random.Range(-weaponLevelDataSO.weaponLevelDataList[currentWeaponID].shotAccuracy, weaponLevelDataSO.weaponLevelDataList[currentWeaponID].shotAccuracy);

        // 弾の方向と回転方向用に計算
        //Vector2 direction = (mousePosition - transform.position).normalized;
        Vector2 direction = (new Vector3(mousePosition.x, mousePosition.y + shotAccuracy, mousePosition.z) - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 弾の方向と回転を更新
        bulletEulerAngles = new Vector3(0, 0, angle);
        bulletDirection = direction.normalized;

        //Debug.Log(bulletDirection);
    }

    /// <summary>
    /// マウスの位置により銃の画像を反転させる
    /// </summary>
    private void FlipGunSprite()
    {

        // マウスのスクリーン座標がスクリーンの中央よりも右側にあるかを判断
        if (Input.mousePosition.x > Screen.width / 2)
        {
            // 右側にある場合、スプライトのY軸を反転
            spriteRenderer.flipY = true;
        }
        else
        {
            // 左側にある場合、反転を解除
            spriteRenderer.flipY = false;
        }
    }

    /// <summary>
    /// マウスの位置により銃の画像を反転させる
    /// 引数つきのパターン
    /// </summary>
    /// <param name="mousePosX"></param>
    private void FlipGunSprite(float mousePosX)
    {

        //Debug.Log(mousePosX);

        // マウスのスクリーン座標がスクリーンの中央よりも右側にあるかを判断
        if (mousePosX > Screen.width / 2)
        {
            // 右側にある場合、スプライトのY軸を反転
            spriteRenderer.flipY = true;
        }
        else
        {
            // 左側にある場合、反転を解除
            spriteRenderer.flipY = false;
        }
    }

    public void ChangeWeapon(int amount)
    {
        currentWeaponID = amount;

        spriteRenderer.sprite = weaponLevelDataSO.weaponLevelDataList[currentWeaponID].gunImage;

        // 弾の終着点のランダム性
        shotAccuracy = Random.Range(-weaponLevelDataSO.weaponLevelDataList[currentWeaponID].shotAccuracy, weaponLevelDataSO.weaponLevelDataList[currentWeaponID].shotAccuracy);
    }
}
