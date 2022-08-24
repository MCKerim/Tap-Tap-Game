using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantNumberOfEnemysStage : MonoBehaviour, IStage
{
    [SerializeField] private EnemyEntry[] enemys;
    private EnemySpawnFunctions enemySpawnFunctions;

    private bool isEndless;
    private int numberOfEnemysTillOver;
    [SerializeField] private int minNumberOfEnemysTillOver;
    [SerializeField] private int maxNumberOfEnemysTillOver;

    private int amountOfConstantEnemys;
    [SerializeField] private AmountChances[] amountOfConstantEnemysChances;
    private bool isOver;

    private void Start()
    {
        enemySpawnFunctions = GameObject.FindObjectOfType<EnemySpawnFunctions>();
    }

    public void SetIsEndless(bool isEndless)
    {
        this.isEndless = isEndless;
    }

    public void Reset()
    {
        if (!isEndless)
        {
            numberOfEnemysTillOver = Random.Range(minNumberOfEnemysTillOver, maxNumberOfEnemysTillOver + 1);
        }
        amountOfConstantEnemys = enemySpawnFunctions.SelectRandomAmount(amountOfConstantEnemysChances);
        isOver = false;
    }

    public bool IsOver()
    {
        return isOver;
    }

    public void UpdateStage()
    {
        if (IsOver())
        {
            return;
        }

        int currentEnemys = GameObject.FindObjectsOfType<Enemy>().Length;

        if (!isEndless)
        {
            if (numberOfEnemysTillOver <= 0 && currentEnemys == 0)
            {
                isOver = true;
                return;
            }
        }
        
        for(int i = currentEnemys; i < amountOfConstantEnemys && (numberOfEnemysTillOver > 0 || isEndless); i++)
        {
            enemySpawnFunctions.SpawnRandomEnemy(enemys);
            if (!isEndless)
            {
                numberOfEnemysTillOver--;
            }
        }
    }
}
