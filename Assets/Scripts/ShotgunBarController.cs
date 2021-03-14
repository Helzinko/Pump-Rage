using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShotgunBarController : MonoBehaviour
{
    public Image xpFillImage;
    
    public TMP_Text levelText;
    public TMP_Text bulletsText;

    public GameObject shotgun;
    
    public void ChangeBulletsText(int currentBulletsCount)
    {
        bulletsText.text = currentBulletsCount + "/" + shotgun.GetComponent<ShotgunController>()._maxBulletCount;
    }

    public void ChangeLevelBarValue(float currentXp)
    {
        xpFillImage.fillAmount = currentXp;
    }

    public void ChangeLevelValue(int currentLevel)
    {
        levelText.text = currentLevel.ToString();
    }
}
