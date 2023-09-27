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

    private int maxLevel = 0;
    [SerializeField]
    private Text playerGunLevel;


    /// <summary>
    /// 武器情報の初期化
    /// </summary>
    public void Init()
    {
        // 初期化
        currentExp = 0;
        currentWeaponLevel = -1;

        // 武器のデータベースの最大レベルをマックスに設定
        maxLevel = weaponLevelDataSO.weaponLevelDataList[weaponLevelDataSO.weaponLevelDataList.Count - 1].level;

        // 武器レベルの更新
        UpdateWeaponLevel();
    }

    /// <summary>
    /// 経験値加算と武器レベルの更新
    /// </summary>
    /// <param name="amount"></param>
    public void AddExp(int amount)
    {
        // 経験値加算
        currentExp += amount;

        // 武器レベルの更新
        UpdateWeaponLevel();
    }

    /// <summary>
    /// 経験値減算と武器レベルの更新
    /// この処理によって、1度に1つ以上の武器レベルは下げない
    /// 2つ以上下がる場合には、1つだけ下がるようにし、下がった後の最低経験値を適用する
    /// </summary>
    /// <param name="amount"></param>
    public void RemoveExp(int amount)
    {
        // 減少後の経験値の試算。試算結果が 0 以下なら 0 にする
        int targetExperience = Mathf.Max(0, currentExp - amount);
        Debug.Log("試算結果 : " + targetExperience);

        // 現在の武器レベルから1つ下の武器レベルの算出
        int minExperienceForCurrentLevel = currentWeaponLevel == 0 ? 0 : weaponLevelDataSO.weaponLevelDataList[currentWeaponLevel - 1].exp;
        Debug.Log("最低経験値 : " + minExperienceForCurrentLevel);

        // 経験値の制限（上記の2つの値を使って、経験値の下限値を制限する）
        currentExp = Mathf.Max(targetExperience, minExperienceForCurrentLevel);

        Debug.Log(currentExp);

        // 武器レベルの更新
        UpdateWeaponLevel();
    }

    /// <summary>
    /// 武器レベルの更新
    /// </summary>
    private void UpdateWeaponLevel()
    {
        int prevLevel = currentWeaponLevel;
        currentWeaponLevel = -1;

        // スクリプタブルオブジェクトに登録されている武器レベルデータ数だけ処理する
        for (int i = weaponLevelDataSO.weaponLevelDataList.Count - 1; i >= 0; i--)
        {
            // 現在の経験値が、weaponLevelDataSO.weaponLevelDataList[i] 内の経験値よりも同じか大きい場合
            if (currentExp >= weaponLevelDataSO.weaponLevelDataList[i].exp)
            {
                // 最大レベルの判定で、現在が最大レベルなら
                if (i == weaponLevelDataSO.weaponLevelDataList.Count - 1 && prevLevel >= maxLevel)
                {
                    currentWeaponLevel = prevLevel;
                    break;
                }

                // 現在の武器レベルが、weaponLevelDataSO.weaponLevelDataList[i] 内の武器レベルと異なる場合
                if (currentWeaponLevel != weaponLevelDataSO.weaponLevelDataList[i].level)
                {
                    // 武器レベルと武器レベルデータを更新
                    currentWeaponLevel = weaponLevelDataSO.weaponLevelDataList[i].level;
                    currentWeaponLevelData = weaponLevelDataSO.weaponLevelDataList[i];

                    // UIに現在の武器レベルを表示
                    playerGunLevel.text = currentWeaponLevel.ToString();

                    // for 文処理を終了する
                    break;
                }
            }
        }
    }
}
