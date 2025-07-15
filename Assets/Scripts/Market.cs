using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class Market : MonoBehaviour
{
    public bool Sword = false;
    public int Armor = 0;
    public int Hourse = 0;
    public bool Koshey = false;

    [SerializeField] private Sprite ChkbDisabled;
    [SerializeField] private Sprite ChkbEnabled;

    [SerializeField] private GameObject StartBattleButton;

    [SerializeField] private Image SwordChkb;
    [SerializeField] private GameObject SwordGO1;
    [SerializeField] private GameObject SwordGO2;

    [SerializeField] private GameObject Armor1Panel;
    [SerializeField] private Image Armor1Chkb;
    [SerializeField] private Image Armor2Chkb;
    [SerializeField] private GameObject ArmorGO0;
    [SerializeField] private GameObject ArmorGO1;
    [SerializeField] private GameObject ArmorGO2;

    [SerializeField] private GameObject Hourse1Panel;
    [SerializeField] private Image Hourse1Chkb;
    [SerializeField] private Image Hourse2Chkb;

    [SerializeField] private Image KosheyChkb;

    [SerializeField] private HeroControllerBase heroController;

    [Inject] private GameController _gameController;

    private void Start()
    {
        UpdateDisplay();
        UpdateHeroStars();
    }

    public void UpdateGameDataOnline(InitGameData initGameData)
    {
        Sword = initGameData.Sword;
        Armor = initGameData.Armor;
        Hourse = initGameData.Hourse;
        Koshey = initGameData.Koshey;

        UpdateDisplay();
        UpdateHeroStars();
    }

    public void SetGameMode(GameController.GameMode mode)
    {
        if(mode == GameController.GameMode.Offline)
            StartBattleButton.SetActive(true);
        else 
            StartBattleButton.SetActive(false);
    }

    public void SetSword(int buy=0)
    {
        Sword = (buy == 1 || buy == 0 && !Sword);
        UpdateDisplay();
        UpdateHeroStars();
    }

    public void SetArmor1(int buy = 0)
    {
        Armor = buy == 1 || buy == 0 && Armor != 1 ? 1 : 0;
        UpdateDisplay();
        UpdateHeroStars();
    }

    public void SetArmor2(int buy = 0)
    {
        Armor = buy == 1 || buy == 0 && Armor != 2 ? 2 : 0;
        UpdateDisplay();
        UpdateHeroStars();
    }

    public void SetHourse1(int buy = 0)
    {
        Hourse = buy == 1 || buy == 0 && Hourse!= 1 ? 1 : 0;
        UpdateDisplay();
        UpdateHeroStars();
    }
    public void SetHourse2(int buy = 0)
    {
        Hourse = buy == 1 || buy == 0 && Hourse != 2 ? 2 : 0;
        UpdateDisplay();
        UpdateHeroStars();
    }

    public void SetKoshey(int buy = 0)
    {
        Koshey = (buy == 1 || buy == 0 && !Koshey);
        UpdateDisplay();
        UpdateHeroStars();
    }

    private void UpdateDisplay()
    {
        if (Sword)
        {
            SwordChkb.sprite = ChkbEnabled;
            SwordGO1.SetActive(false);
            SwordGO2.SetActive(true);
        }
        else
        {
            SwordChkb.sprite = ChkbDisabled;
            SwordGO1.SetActive(true);
            SwordGO2.SetActive(false);
        }

        if (Armor==0)
        {
            Armor1Panel.SetActive(true);
            Armor1Chkb.sprite = ChkbDisabled;
            Armor2Chkb.sprite = ChkbDisabled;
            ArmorGO0.SetActive(true);
            ArmorGO1.SetActive(false);
            ArmorGO2.SetActive(false);
        }
        else if (Armor == 1)
        {
            Armor1Panel.SetActive(true);
            Armor1Chkb.sprite = ChkbEnabled;
            Armor2Chkb.sprite = ChkbDisabled;
            ArmorGO0.SetActive(false);
            ArmorGO1.SetActive(true);
            ArmorGO2.SetActive(false);
        }
        else if (Armor == 2)
        {
            Armor1Panel.SetActive(false);
            Armor1Chkb.sprite = ChkbDisabled;
            Armor2Chkb.sprite = ChkbEnabled;
            ArmorGO0.SetActive(false);
            ArmorGO1.SetActive(false);
            ArmorGO2.SetActive(true);
        }

        if (Hourse == 0)
        {
            Hourse1Panel.SetActive(true);
            Hourse1Chkb.sprite = ChkbDisabled;
            Hourse2Chkb.sprite = ChkbDisabled;
        } 
        else if (Hourse == 1)
        {
            Hourse1Panel.SetActive(true);
            Hourse1Chkb.sprite = ChkbEnabled;
            Hourse2Chkb.sprite = ChkbDisabled;
        }
        else if (Hourse == 2)
        {
            Hourse1Panel.SetActive(false);
            Hourse1Chkb.sprite = ChkbDisabled;
            Hourse2Chkb.sprite = ChkbEnabled;
        }

        if (Koshey)
        {
            KosheyChkb.sprite = ChkbEnabled;
        }
        else
        {
            KosheyChkb.sprite = ChkbDisabled;
        }
    }

    private void UpdateHeroStars()
    {
        if (_gameController.Mode == GameController.GameMode.Online) 
            return;
        int attackStars = 1;
        int defStars = 1;
        int speedStars = 3;
        if (Sword) { attackStars += 2; }
        if (Armor > 1) { defStars ++; }
        if (Armor > 0) { defStars++; }
        if (Hourse > 1) { attackStars ++; defStars++; speedStars++; }
        if (Hourse > 0) { speedStars++; }
        if (Koshey) { attackStars ++; defStars++; }
        heroController.SetStarsCnt(attackStars, defStars, speedStars);
    }
}
