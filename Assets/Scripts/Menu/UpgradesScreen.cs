using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class UpgradesScreen : MenuBase
{
    public MainMenu _mainMenu;
    public Image _msgButton, _lidButton, _nfcButton, _mgButton, _ldButton, _tjButton;
    public TMP_Text _msgText, _lidText, _nfcText, _mgText, _ldText, _tjText;
    public Sprite _upgradeActiveSprite, _upgradeInactiveSprite;

    private void Awake()
    {
        _flashTimer = gameObject.AddComponent<RepeatingTimer>();
        _flashTimer.OnTimerCompleted += FlashGreenRed;
    }

    private void OnDestroy()
    {
        _flashTimer.OnTimerCompleted -= FlashGreenRed;
    }

    private void OnEnable()
    {
        if (GameManager.Instance._moreStartGreenPaid == true)
        {
            _msgText.text = "-";
            if (GameManager.Instance.MoreStartGreen)
            {
                _msgButton.sprite = _upgradeActiveSprite;
            }
            else
            {
                _msgButton.sprite = _upgradeInactiveSprite;
            }
        }
        if (GameManager.Instance._lessImpactDamagePaid == true)
        {
            _lidText.text = "-";
            if (GameManager.Instance.LessImpactDamage)
            {
                _lidButton.sprite = _upgradeActiveSprite;
            }
            else
            {
                _lidButton.sprite = _upgradeInactiveSprite;
            }
        }
        if (GameManager.Instance._noFlyConsumePaid == true)
        {
            _nfcText.text = "-";
            if (GameManager.Instance.NoFlyConsume)
            {
                _nfcButton.sprite = _upgradeActiveSprite;
            }
            else
            {
                _nfcButton.sprite = _upgradeInactiveSprite;
            }
        }
        if (GameManager.Instance._moreGreenPaid == true)
        {
            _mgText.text = "-";
            if (GameManager.Instance.MoreGreen)
            {
                _mgButton.sprite = _upgradeActiveSprite;
            }
            else
            {
                _mgButton.sprite = _upgradeInactiveSprite;
            }
        }
        if (GameManager.Instance._lessDamagePaid == true)
        {
            _ldText.text = "-";
            if (GameManager.Instance.LessDamage)
            {
                _ldButton.sprite = _upgradeActiveSprite;
            }
            else
            {
                _ldButton.sprite = _upgradeInactiveSprite;
            }
        }
        if (GameManager.Instance._tripleJumpPaid == true)
        {
            _tjText.text = "-";
            if (GameManager.Instance.TripleJump)
            {
                _tjButton.sprite = _upgradeActiveSprite;
            }
            else
            {
                _tjButton.sprite = _upgradeInactiveSprite;
            }
        }
    }

    public void MoreStartingGreen()
    {
        if (GameManager.Instance.SavedGreens >= 200 && GameManager.Instance._moreStartGreenPaid == false) 
        {
            GameManager.Instance.SavedGreens -= 200;
            _mainMenu.UpdateSavedGreens();
            GameManager.Instance.MoreStartGreen = true;
            GameManager.Instance._moreStartGreenPaid = true;
            _msgButton.sprite = _upgradeActiveSprite;
            _msgText.text = "-";
        }else if (GameManager.Instance._moreStartGreenPaid)
        {
            if (GameManager.Instance.MoreStartGreen == false)
            {
                GameManager.Instance.MoreStartGreen = true;
                _msgButton.sprite = _upgradeActiveSprite;
            }else
            {
                GameManager.Instance.MoreStartGreen = false;
                _msgButton.sprite = _upgradeInactiveSprite;
            }
        }
        else if (GameManager.Instance._moreStartGreenPaid == false && GameManager.Instance.SavedGreens < 200)
        {
            StartFlash();
        }
    }

    public void BuyLessImpactDamage()
    {
        if (GameManager.Instance.SavedGreens >= 300 && GameManager.Instance._lessImpactDamagePaid == false)
        {
            GameManager.Instance.SavedGreens -= 300;
            _mainMenu.UpdateSavedGreens();
            GameManager.Instance.LessImpactDamage = true;
            _lidButton.sprite = _upgradeActiveSprite;
            _lidText.text = "-";
            GameManager.Instance._lessImpactDamagePaid = true;
        }
        else if (GameManager.Instance._lessImpactDamagePaid)
        {
            if (GameManager.Instance.LessImpactDamage == false)
            {
                GameManager.Instance.LessImpactDamage = true;
                _lidButton.sprite = _upgradeActiveSprite;
            }
            else
            {
                GameManager.Instance.LessImpactDamage = false;
                _lidButton.sprite = _upgradeInactiveSprite;
            }
        }
        else if (GameManager.Instance._lessImpactDamagePaid == false && GameManager.Instance.SavedGreens < 300)
        {
            StartFlash();
        }
    }

    public void BuyNoFlyConsume()
    {
        if (GameManager.Instance.SavedGreens >= 200 && GameManager.Instance._noFlyConsumePaid == false)
        {
            GameManager.Instance.SavedGreens -= 200;
            _mainMenu.UpdateSavedGreens();
            GameManager.Instance.NoFlyConsume = true;
            _nfcButton.sprite = _upgradeActiveSprite;
            _nfcText.text = "-";
            GameManager.Instance._noFlyConsumePaid = true;
        }
        else if (GameManager.Instance._noFlyConsumePaid)
        {
            if (GameManager.Instance.NoFlyConsume == false)
            {
                GameManager.Instance.NoFlyConsume = true;
                _nfcButton.sprite = _upgradeActiveSprite;
            }
            else
            {
                GameManager.Instance.NoFlyConsume = false;
                _nfcButton.sprite = _upgradeInactiveSprite;
            }
        }
        else if (GameManager.Instance._noFlyConsumePaid == false && GameManager.Instance.SavedGreens < 200)
        {
            StartFlash();
        }
    }

    public void BuyMoreGreen()
    {
        if (GameManager.Instance.SavedGreens >= 500 && GameManager.Instance._moreGreenPaid == false)
        {
            GameManager.Instance.SavedGreens -= 500;
            _mainMenu.UpdateSavedGreens();
            GameManager.Instance.MoreGreen = true;
            _mgButton.sprite = _upgradeActiveSprite;
            _mgText.text = "-";
            GameManager.Instance._moreGreenPaid = true;
        }
        else if (GameManager.Instance._moreGreenPaid)
        {
            if (GameManager.Instance.MoreGreen == false)
            {
                GameManager.Instance.MoreGreen = true;
                _mgButton.sprite = _upgradeActiveSprite;
            }
            else
            {
                GameManager.Instance.MoreGreen = false;
                _mgButton.sprite = _upgradeInactiveSprite;
            }
        }
        else if (GameManager.Instance._moreGreenPaid == false && GameManager.Instance.SavedGreens < 500)
        {
            StartFlash();
        }
    }

    public void BuyLessDamage()
    {
        if (GameManager.Instance.SavedGreens >= 200 && GameManager.Instance._lessDamagePaid == false)
        {
            GameManager.Instance.SavedGreens -= 200;
            _mainMenu.UpdateSavedGreens();
            GameManager.Instance.LessDamage = true;
            _ldButton.sprite = _upgradeActiveSprite;
            _ldText.text = "-";
            GameManager.Instance._lessDamagePaid = true;
        }
        else if (GameManager.Instance._lessDamagePaid)
        {
            if (GameManager.Instance.LessDamage == false)
            {
                GameManager.Instance.LessDamage = true;
                _ldButton.sprite = _upgradeActiveSprite;
            }
            else
            {
                GameManager.Instance.LessDamage = false;
                _ldButton.sprite = _upgradeInactiveSprite;
            }
        }
        else if (GameManager.Instance._lessDamagePaid == false && GameManager.Instance.SavedGreens < 200)
        {
            StartFlash();
        }
    }

    public void BuyTripleJump()
    {
        if (GameManager.Instance.SavedGreens >= 400 && GameManager.Instance._tripleJumpPaid == false)
        {
            GameManager.Instance.SavedGreens -= 400;
            _mainMenu.UpdateSavedGreens();
            GameManager.Instance.TripleJump = true;
            _tjButton.sprite = _upgradeActiveSprite;
            _tjText.text = "-";
            GameManager.Instance._tripleJumpPaid = true;
        }
        else if (GameManager.Instance._tripleJumpPaid)
        {
            if (GameManager.Instance.TripleJump == false)
            {
                GameManager.Instance.TripleJump = true;
                _tjButton.sprite = _upgradeActiveSprite;
            }
            else
            {
                GameManager.Instance.TripleJump = false;
                _tjButton.sprite = _upgradeInactiveSprite;
            }
        }
        else if (GameManager.Instance._tripleJumpPaid == false && GameManager.Instance.SavedGreens < 400)
        {
            StartFlash();
        }
    }
}
