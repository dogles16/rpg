using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
// using System.Runtime.InteropServices;

public enum BattleStateOld { Start, PlayerSpell, PlayerItem, EnemyMove, Busy, PlayerWon, PlayerLost }
public class BattleSystemOld : MonoBehaviour
{
    [SerializeField] BattleDialogBox dialogBox;
    [SerializeField] SpellBox spellBox;
    [SerializeField] ItemBox itemBox;
    [SerializeField] BattleScene battleScene;
    [SerializeField] BattleStateOld state;
    [SerializeField] int selectedActionIndex = 0;
    [SerializeField] int selectedSpellIndex = 0;
    [SerializeField] int selectedItemIndex = 0;
    [SerializeField] GameObject endTransition;
    [SerializeField] CurrentEnemy currentEnemy;
    [SerializeField] AttackController attackController;
    [SerializeField] Stats playerStats;
    [SerializeField] Stats respawnStats;
    [SerializeField] VectorValue playerPosition;
    [SerializeField] VectorValue respawnPosition;
    [SerializeField] LevelManager levelManager;
    public UnityEvent onRunAway;
    public UnityEvent onPlayerWin;
    public UnityEvent onPlayerLose;
    // [DllImport("__Internal")] private static extern void GiveGold(int goldAmount);
    //float transitionWait = 1.0f;
    float textPause = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        if (onRunAway == null)
            onRunAway = new UnityEvent();
        if (onPlayerWin == null)
            onPlayerWin = new UnityEvent();
        if (onPlayerLose == null)
            onPlayerLose = new UnityEvent();

        state = BattleStateOld.Busy;
        StartCoroutine(SetupBattle());
    }

    // Update is called once per frame
    void Update()
    {
        if (state == BattleStateOld.Start)
        {
            HandleActionSelection();
        }
        else if (state == BattleStateOld.PlayerSpell)
        {
            HandleSpellSelection();
        }
        else if (state == BattleStateOld.PlayerItem)
        {
            HandleItemSelection();
        }
    }

    public IEnumerator SetupBattle()
    {
        battleScene.EnableBattleScene();
        yield return StartCoroutine(dialogBox.TypeDialog($"A dangerous {currentEnemy.enemy.enemyName} approaches!"));
        yield return PlayerTurn();
    }

    IEnumerator PlayerTurn()
    {
        state = BattleStateOld.Start;
        yield return StartCoroutine(dialogBox.TypeDialog("Choose an action..."));
        dialogBox.EnableActionSelector();
        selectedActionIndex = 0;
        dialogBox.UpdateActionSelection(selectedActionIndex);
    }
    IEnumerator EnemyTurn()
    {
        state = BattleStateOld.EnemyMove;
        yield return new WaitForSecondsRealtime(textPause);
        yield return dialogBox.TypeDialog($"{currentEnemy.enemy.enemyName} attacks!");
        yield return new WaitForSecondsRealtime(textPause);
        float damageDone = attackController.EnemyAttack();
        yield return dialogBox.TypeDialog($"{currentEnemy.enemy.enemyName} does {damageDone} damage.");
        yield return new WaitForSecondsRealtime(textPause);
        yield return HandleBattleEnd();
        yield return PlayerTurn();
    }

    IEnumerator Attack()
    {
        state = BattleStateOld.Busy;
        dialogBox.DisableActionSelector();
        yield return new WaitForSecondsRealtime(textPause);
        yield return dialogBox.TypeDialog($"You attack!");
        yield return new WaitForSecondsRealtime(textPause);
        bool critical = (UnityEngine.Random.value < 0.05);
        float damageDone = attackController.Attack(critical);
        if (critical)
        {
            yield return dialogBox.TypeDialog($"Critical hit!");
            yield return new WaitForSecondsRealtime(textPause);
        }
        yield return dialogBox.TypeDialog($"You deal {damageDone} damage.");
        yield return new WaitForSecondsRealtime(textPause);
        yield return HandleBattleEnd();
        yield return EnemyTurn();
    }
    IEnumerator UseSpell()
    {
        state = BattleStateOld.Busy;
        float damage = spellBox.CastSpell(selectedSpellIndex);
        if (damage < 0.0) yield break;
        spellBox.DisableSpellSelector();
        yield return dialogBox.TypeDialog($"{damage} damage done.");
        yield return new WaitForSecondsRealtime(textPause);
        yield return HandleBattleEnd();
        yield return EnemyTurn();
    }
    IEnumerator ChooseSpell()
    {
        state = BattleStateOld.PlayerSpell;
        yield return StartCoroutine(dialogBox.TypeDialog("Cast a spell..."));
        spellBox.EnableSpellSelector();
        selectedSpellIndex = 0;
        spellBox.UpdateSpellSelection(selectedSpellIndex);
    }
    IEnumerator ChooseItem()
    {
        state = BattleStateOld.PlayerItem;
        yield return StartCoroutine(dialogBox.TypeDialog("Use an item..."));
        itemBox.EnableItemSelector();
        selectedItemIndex = 0;
        itemBox.UpdateItemSelection(selectedItemIndex);
    }

    // Hard coded for 4 actions: 0=Fight, 1=Item, 2=Spell, 3=Run
    void HandleActionSelection()
    {
        if (!dialogBox.selectorEnabled) return;
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (selectedActionIndex == 2) return;
            selectedActionIndex = Math.Min(selectedActionIndex + 2, 3);
            dialogBox.UpdateActionSelection(selectedActionIndex);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (selectedActionIndex == 1) return;
            selectedActionIndex = Math.Min(selectedActionIndex + 1, 3);
            dialogBox.UpdateActionSelection(selectedActionIndex);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (selectedActionIndex == 2) return;
            selectedActionIndex = Math.Max(selectedActionIndex - 1, 0);
            dialogBox.UpdateActionSelection(selectedActionIndex);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (selectedActionIndex == 1) return;
            selectedActionIndex = Math.Max(selectedActionIndex - 2, 0);
            dialogBox.UpdateActionSelection(selectedActionIndex);
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            if (selectedActionIndex == 0)
            {
                // FIGHT
                StartCoroutine(Attack());

            }
            else if (selectedActionIndex == 1)
            {
                // ITEM
                StartCoroutine(ChooseItem());
            }
            else if (selectedActionIndex == 2)
            {
                // SPELL
                StartCoroutine(ChooseSpell());
            }
            else
            {
                // RUN
                onRunAway.Invoke();
                ////StartCoroutine(EndBattleTransitionCo());
            }
        }
    }
    void HandleSpellSelection()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            selectedSpellIndex++;
            spellBox.UpdateSpellSelection(selectedSpellIndex);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            selectedSpellIndex--;
            spellBox.UpdateSpellSelection(selectedSpellIndex);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            spellBox.DisableSpellSelector();
            StartCoroutine(PlayerTurn());
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(UseSpell());
        }
    }
    void HandleItemSelection()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            selectedItemIndex++;
            itemBox.UpdateItemSelection(selectedItemIndex);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            selectedItemIndex--;
            itemBox.UpdateItemSelection(selectedItemIndex);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            itemBox.DisableItemSelector();
            StartCoroutine(PlayerTurn());
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            itemBox.UseItem(selectedItemIndex);
            itemBox.DisableItemSelector();
            StartCoroutine(PlayerTurn());
        }
    }
    IEnumerator HandleBattleEnd()
    {
        BattleStateOld newState = attackController.CheckBattleEnd();
        if (newState == BattleStateOld.PlayerWon)
        {
            yield return dialogBox.TypeDialog("You win!");
            yield return new WaitForSecondsRealtime(textPause);
            yield return dialogBox.TypeDialog($"You gained {currentEnemy.enemy.xp} xp and {currentEnemy.enemy.gold} gold.");
            yield return new WaitForSecondsRealtime(textPause);
            playerStats.gold.value += currentEnemy.enemy.gold;

            // #if UNITY_WEBGL == true && UNITY_EDITOR == false
            // GiveGold(currentEnemy.enemy.gold);
            // #endif
            playerStats.xp.value += currentEnemy.enemy.xp;
            BattleResult result = levelManager.getLevel(playerStats.playerType, playerStats.level.value, playerStats.xp.value);
            if (result.dLevel > 0)
            {
                //set new level and increase stats
                playerStats.level.value += result.dLevel;
                playerStats.agility.value += result.dAgility;
                playerStats.strength.value += result.dStrength;
                playerStats.magicPower.value += result.dMagicPower;
                playerStats.maxHealth.value += result.dMaxHealth;
                playerStats.maxMagic.value += result.dMaxMagic;

                yield return dialogBox.TypeDialog($"You leveled up to level {playerStats.level.value}!");
                yield return new WaitForSecondsRealtime(textPause);
                yield return dialogBox.TypeDialog($"You gained {result.dAgility} agility.");
                yield return new WaitForSecondsRealtime(textPause);
                yield return dialogBox.TypeDialog($"You gained {result.dStrength} strength.");
                yield return new WaitForSecondsRealtime(textPause);
                yield return dialogBox.TypeDialog($"You gained {result.dMagicPower} magic power.");
                yield return new WaitForSecondsRealtime(textPause);
                yield return dialogBox.TypeDialog($"You gained {result.dMaxHealth} health points.");
                yield return new WaitForSecondsRealtime(textPause);
                yield return dialogBox.TypeDialog($"You gained {result.dMaxMagic} magic points.");
                yield return new WaitForSecondsRealtime(textPause);

                if (result.newSpell)
                {
                    spellBox.AddSpell(result.newSpell);
                    yield return dialogBox.TypeDialog($"You learned the spell {result.newSpell.spellName}.");
                    yield return new WaitForSecondsRealtime(textPause);
                }
            }
            onPlayerWin.Invoke();
            ////yield return EndBattleTransitionCo();
        }
        else if (newState == BattleStateOld.PlayerLost)
        {
            yield return dialogBox.TypeDialog("You lost...");
            yield return new WaitForSecondsRealtime(textPause);
            playerStats = respawnStats;
            playerPosition = respawnPosition;
            onPlayerLose.Invoke();
            ////yield return EndBattleTransitionCo();
        }
    }

    /*Note from Isaac: I created a scriptable object called "SceneManager" that can be called from events to load scenes, including load previous scenes. Doing 
         * so from events is better practice than naming scenes through strings within code, so I commented out the places where scene transitions have been taking place
         * and changed them to be handled in the respective UnityEvents. The scenes still load asynchronously so the transitions still work for now*/
    /*IEnumerator EndBattleTransitionCo()
    {
        Time.timeScale = 0;
        if (endTransition)
        {
            Instantiate(endTransition, Vector3.zero, Quaternion.identity);
        }
        yield return new WaitForSecondsRealtime(transitionWait);
        Time.timeScale = 1;
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("SampleScene");
        while (!asyncOperation.isDone) yield return null;
    }*/
}
