using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberEnemyStage : MonoBehaviour, IStage
{
    [SerializeField] private GameObject numberEnemyPerfab;

    [SerializeField] private AmountChances[] amountChances;
    private bool isEndless;

    private EnemySpawnFunctions enemySpawnFunctions;

    private bool hasSpawnedEnemys;
    private bool isOver;

    private void Start()
    {
        enemySpawnFunctions = GameObject.FindObjectOfType<EnemySpawnFunctions>();
    }

    public void SetIsEndless(bool isEndless)
    {
        this.isEndless = isEndless;
    }

    public bool IsOver()
    {
        return isOver;
    }

    public void Reset()
    {
        hasSpawnedEnemys = false;
        isOver = false;
    }

    public void UpdateStage()
    {
        if (IsOver())
        {
            return;
        }

        if (!hasSpawnedEnemys)
        {
            SpawnEnemys();
        }
        else if(GameObject.FindObjectsOfType<Enemy>().Length == 0)
        {
            if (isEndless)
            {
                hasSpawnedEnemys = false;
            }
            else
            {
                isOver = true;
            }
        }
    }

    private void SpawnEnemys()
    {
        NumberEnemy lastSpawnedEnemy = null;
        for (int i = 1; i <= enemySpawnFunctions.SelectRandomAmount(amountChances); i++)
        {
            NumberEnemy newSpawnedEnemy = enemySpawnFunctions.SpawnEnemyAtRandomPos(numberEnemyPerfab).GetComponent<NumberEnemy>();
            newSpawnedEnemy.SetNumber(i);

            if (i == 1)
            {
                newSpawnedEnemy.CanBeKilledNow();
            }
            else
            {
                lastSpawnedEnemy.SetNextEnemy(newSpawnedEnemy);
            }

            lastSpawnedEnemy = newSpawnedEnemy;
        }
        hasSpawnedEnemys = true;
    }
}
