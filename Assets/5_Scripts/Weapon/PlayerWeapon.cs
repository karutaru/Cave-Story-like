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
    private WeaponLevelDataSO weaponLevelDataSO;

    private int currentExp; // 現在の経験値
    private int currentWeaponLevel; // 現在の武器レベル

    private WeaponLevelData currentWeaponLevelData; // 現在の武器レベルデータ（弾の速度、ダメージ、プレハブ）
    public WeaponLevelData CurrentWeaponLevelData => currentWeaponLevelData; // プロパティ

    [SerializeField]
    private Text playerGunLevel;
}
