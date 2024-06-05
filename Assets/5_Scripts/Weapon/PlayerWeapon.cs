using UnityEngine;
using System.Linq;
using UnityEngine.UI;

/// <summary>
/// プレイヤーの武器情報の管理クラス
/// </summary>
public class PlayerWeapon : MonoBehaviour
{
    // スクリプタブルオブジェクトの登録（アセットをアサイン）
    [SerializeField, Header("武器レベルデータのスクリプタブルオブジェクト")]
    private WeaponDataSO weaponDataSO;


    private WeaponData currentWeaponData; // 現在の武器レベルデータ（弾の速度、ダメージ、プレハブ）
    public WeaponData CurrentWeaponLevelData => currentWeaponData; // プロパティ

    [SerializeField]
    private Text playerGunLevel;
}
