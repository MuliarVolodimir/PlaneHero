using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemIconInfo :MonoBehaviour
{
    [SerializeField] Image _rewardImage;
    [SerializeField] Image _priceImage;
    [SerializeField] TextMeshProUGUI _reward;
    [SerializeField] TextMeshProUGUI _price;

    public void SetInfo(Sprite rewardSprite, Sprite priceSprite, int reward, int price)
    {
        if (rewardSprite != null)
             _rewardImage.sprite = rewardSprite;
        if (priceSprite != null)
             _priceImage.sprite = priceSprite;

        if (price != 0)
             _price.text = $"{price}";
        if (reward != 0)
            _reward.text = $"{reward}";
    }
}
