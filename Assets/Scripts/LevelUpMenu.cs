using Opsive.UltimateInventorySystem.UI.CompoundElements;
using Opsive.UltimateInventorySystem.UI.Panels;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpMenu : MonoBehaviour
{

    public CharStats playerStats;
    public int pointsFromLevelUp = 0;

    public Button levelUpButton;
    public TextMeshProUGUI levelUpButtonText;

    public DisplayPanel saveStatsPanel;
    public DisplayPanel levelUpPanel;


    public TextMeshProUGUI currentLevelText;
    public TextMeshProUGUI nextLevelText;
    public TextMeshProUGUI currentXPText;
    public TextMeshProUGUI nextXPText;
    public TextMeshProUGUI xpNeededText;
    public TextMeshProUGUI pointsRemainingText;
    public TextMeshProUGUI currentHPText;
    public TextMeshProUGUI nextHPText;
    public TextMeshProUGUI currentMPText;
    public TextMeshProUGUI nextMPText;
    public TextMeshProUGUI currentAttackText;
    public TextMeshProUGUI nextAttackText;
    public TextMeshProUGUI currentDefenseText;
    public TextMeshProUGUI nextDefenseText;
    public TextMeshProUGUI currentSpeedText;
    public TextMeshProUGUI nextSpeedText;
    public TextMeshProUGUI currentMagicPowerText;
    public TextMeshProUGUI nextMagicPowerText;

    public QuantityPicker hpQP;
    public QuantityPicker mpQP;
    public QuantityPicker attackQP;
    public QuantityPicker defenseQP;
    public QuantityPicker speedQP;
    public QuantityPicker magicPowerQP;

    [HideInInspector] public bool isLevelUp = false;

    private Color darkGreen = new Color(0, 0.5f, 0);

    private void Awake()
    {
        playerStats = FindObjectOfType<CharStats>();
    }

    // Start is called before the first frame update
    private void OnEnable()
    {
        if (playerStats == null)
        {
            playerStats = FindObjectOfType<CharStats>();
        }
       
        ResetMenu();

    }

    public void ClickLevelUpButton()
    {
        if(levelUpButtonText.text == "Level Up")
        {
            saveStatsPanel.SmartOpen();
        }
        else
        {
            levelUpPanel.SmartClose();
        }
    }

    public void ResetMenu()
    {
        //Determine if it's a levelup or just using saved points
        if (playerStats.xp < playerStats.xpToLevelUp)
            isLevelUp = false;
        else
            isLevelUp = true;

        //If leveling up, determine how many points you get
        pointsFromLevelUp = playerStats.pointsRemaining;
        if (isLevelUp)
            pointsFromLevelUp += (playerStats.level + 3);

        //Set each number on the screen to the correct value
        currentLevelText.text = playerStats.level.ToString();

        if (isLevelUp)
            nextLevelText.text = (playerStats.level + 1).ToString();
        else
            nextLevelText.text = playerStats.level.ToString();

        currentXPText.text = playerStats.xp.ToString();

        if (isLevelUp)
            nextXPText.text = (playerStats.xp - playerStats.xpToLevelUp).ToString();
        else
            nextXPText.text = playerStats.xp.ToString();

        xpNeededText.text = playerStats.xpToLevelUp.ToString();

        pointsRemainingText.text = pointsFromLevelUp.ToString();

        currentHPText.text = playerStats.healthBase.ToString();
        nextHPText.text = playerStats.healthBase.ToString();

        currentMPText.text = playerStats.magicBase.ToString();
        nextMPText.text = playerStats.magicBase.ToString();

        currentAttackText.text = playerStats.attackBase.ToString();
        nextAttackText.text = playerStats.attackBase.ToString();

        currentDefenseText.text = playerStats.defenseBase.ToString();
        nextDefenseText.text = playerStats.defenseBase.ToString();

        currentSpeedText.text = playerStats.speedBase.ToString();
        nextSpeedText.text = playerStats.speedBase.ToString();

        currentMagicPowerText.text = playerStats.magicPowerBase.ToString();
        nextMagicPowerText.text = playerStats.magicPowerBase.ToString();


        //Set max of each quantity picker
        hpQP.MaxQuantity = 0;
        hpQP.SetQuantity(0);
        mpQP.MaxQuantity = 0;
        mpQP.SetQuantity(0);
        defenseQP.MaxQuantity = 0;
        defenseQP.SetQuantity(0);
        attackQP.MaxQuantity = 0;
        attackQP.SetQuantity(0);
        speedQP.MaxQuantity = 0;
        speedQP.SetQuantity(0);
        magicPowerQP.MaxQuantity = 0;
        magicPowerQP.SetQuantity(0);
        SetPickerQuantity();

        //Set colors of permanent texts
        if (isLevelUp)
        {
            nextXPText.color = Color.red;
            nextLevelText.color = darkGreen;
        }
    }

    private void OnDisable()
    {
        Debug.Log("Level up menu disabled");
    }

    public void SaveChanges()
    {
        if(levelUpButtonText.text == "Level Up")
        {
            //Change text
            if (isLevelUp)
            {
                int nextXpNeeded;
                if (playerStats.level == 1)
                {
                    nextXpNeeded = 10;
                }
                else if (playerStats.level == 2)
                {
                    nextXpNeeded = 30;
                }
                else if (playerStats.level == 3)
                {
                    nextXpNeeded = 75;
                }
                else if (playerStats.level == 4)
                {
                    nextXpNeeded = 150;
                }
                else if (playerStats.level == 5)
                {
                    nextXpNeeded = 250;
                }
                else if (playerStats.level == 6)
                {
                    nextXpNeeded = 350;
                }
                else if (playerStats.level == 7)
                {
                    nextXpNeeded = 500;
                }
                else if (playerStats.level == 8)
                {
                    nextXpNeeded = 700;
                }
                else if (playerStats.level == 9)
                {
                    nextXpNeeded = 1000;
                }
                else 
                {
                    nextXpNeeded = int.MaxValue;
                    Debug.Log("Something went wrong with level up xp calculation");
                }

                xpNeededText.text = nextXpNeeded.ToString();

                currentLevelText.text = nextLevelText.text;
                nextLevelText.color = Color.black;

                currentXPText.text = nextXPText.text;
                nextXPText.color = Color.black;

                currentHPText.text = nextHPText.text;
                nextHPText.color = Color.black;

                currentMPText.text = nextMPText.text;
                nextMPText.color = Color.black;

                currentAttackText.text = nextAttackText.text;
                nextAttackText.color = Color.black;

                currentDefenseText.text = nextDefenseText.text;
                nextDefenseText.color = Color.black;

                currentSpeedText.text = nextSpeedText.text;
                nextSpeedText.color = Color.black;

                currentMagicPowerText.text = nextMagicPowerText.text;
                nextMagicPowerText.color = Color.black;
            }


            //Change player stats
            playerStats.pointsRemaining = Int32.Parse(pointsRemainingText.text);
            playerStats.xpToLevelUp = Int32.Parse(xpNeededText.text);
            playerStats.level = Int32.Parse(currentLevelText.text);
            playerStats.xp = Int32.Parse(currentXPText.text);
            playerStats.healthBase = Int32.Parse(currentHPText.text);
            playerStats.magicBase = Int32.Parse(currentMPText.text);
            playerStats.attackBase = Int32.Parse(currentAttackText.text);
            playerStats.defenseBase = Int32.Parse(currentDefenseText.text);
            playerStats.speedBase = Int32.Parse(currentSpeedText.text);
            playerStats.magicPowerBase = Int32.Parse(currentMagicPowerText.text);


            //Update player stats and if it's a level up, set health and magic to max

            if (isLevelUp)
            {
                playerStats.healthTotal = playerStats.healthMax;
                playerStats.magicTotal = playerStats.magicMax;
                playerStats.UpdateStats();
            }

            playerStats.UpdateStats();
            playerStats.UpdateUI();



            //Change other menu options
            pointsFromLevelUp = Int32.Parse(pointsRemainingText.text);

            ResetMenu();
        }
    }

    public void SetPickerQuantity()
    {
        int pointsUsed = hpQP.Quantity + mpQP.Quantity + defenseQP.Quantity + attackQP.Quantity + speedQP.Quantity + magicPowerQP.Quantity;

        if (pointsUsed > 0)
            levelUpButtonText.text = "Level Up";
        else
            levelUpButtonText.text = "Exit";

        if (pointsUsed >= pointsFromLevelUp)
            pointsRemainingText.text = "0";
        else
            pointsRemainingText.text = (pointsFromLevelUp - pointsUsed).ToString();

        hpQP.MaxQuantity =  Int32.Parse(pointsRemainingText.text) + hpQP.Quantity;


        mpQP.MaxQuantity =  Int32.Parse(pointsRemainingText.text) + mpQP.Quantity;


        defenseQP.MaxQuantity =  Int32.Parse(pointsRemainingText.text) + defenseQP.Quantity;


        attackQP.MaxQuantity =  Int32.Parse(pointsRemainingText.text) + attackQP.Quantity;


        speedQP.MaxQuantity =  Int32.Parse(pointsRemainingText.text) + speedQP.Quantity;


        magicPowerQP.MaxQuantity =  Int32.Parse(pointsRemainingText.text) + magicPowerQP.Quantity;
    }

    public void SetNewLevels()
    {
        nextHPText.text = (Int32.Parse(currentHPText.text) + (hpQP.Quantity * 3)).ToString();
        nextMPText.text = (Int32.Parse(currentMPText.text) + (mpQP.Quantity * 3)).ToString();
        nextAttackText.text = (Int32.Parse(currentAttackText.text) + (attackQP.Quantity * 2)).ToString();
        nextDefenseText.text = (Int32.Parse(currentDefenseText.text) + (defenseQP.Quantity * 2)).ToString();
        nextSpeedText.text = (Int32.Parse(currentSpeedText.text) + (speedQP.Quantity * 2)).ToString();
        nextMagicPowerText.text = (Int32.Parse(currentMagicPowerText.text) + (magicPowerQP.Quantity * 2)).ToString();

        //Set colors of stats texts based on their differences
        SetColor(nextHPText, currentHPText);
        SetColor(nextMPText, currentMPText);
        SetColor(nextAttackText, currentAttackText);
        SetColor(nextDefenseText, currentDefenseText);
        SetColor(nextSpeedText, currentSpeedText);
        SetColor(nextMagicPowerText, currentMagicPowerText);
    }


    public void SetColor(TextMeshProUGUI textMesh, TextMeshProUGUI compareText)
    {
        if (Int32.Parse(textMesh.text) > Int32.Parse(compareText.text))
            textMesh.color = darkGreen;
        else if (Int32.Parse(textMesh.text) < Int32.Parse(compareText.text))
            textMesh.color = Color.red;
        else
            textMesh.color = Color.black;
    }
}