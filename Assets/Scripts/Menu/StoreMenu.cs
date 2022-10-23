using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Purchasing;

public class StoreMenu : MonoBehaviour
{
    public MainMenu _mainMenu;
    private string _100GreenBits = "com.maruuna.floo.100greenbits", _noAds = "com.maruuna.floo.noads";
    public GameObject _restoreButton; 
    public Button _backButton;

    private void Awake()
    {
        if(Application.platform != RuntimePlatform.IPhonePlayer)
        {
            _restoreButton.SetActive(false);
            _backButton.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -220, 0);
        }
    }

    public void OnPurchaseComplete(Product product)
    {
        if(product.definition.id == _noAds)
        {
            GameManager.Instance._hasPaidForNoAds = true;
            GameManager.Instance.SaveData();
        }
        else if(product.definition.id == _100GreenBits)
        {
            GameManager.Instance.SavedGreens += 100;
            _mainMenu.UpdateSavedGreens();
            GameManager.Instance.SaveData();
        }
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failReason)
    {
        Debug.Log(product.definition.id + " failed because " + failReason);
    }
}
