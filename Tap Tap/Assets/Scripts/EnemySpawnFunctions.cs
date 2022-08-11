using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnFunctions : MonoBehaviour
{
    private float borderLeft;
    private float borderRight;
    private float borderTop;
    private float borderBottom;
    [SerializeField] private Canvas spawnField;
    [SerializeField] private float marginLeft;
    [SerializeField] private float marginRight;
    [SerializeField] private float marginTop;
    [SerializeField] private float marginBottom;

    private float zAxis = 10;

    private TutorialManager tutorialManager;

    private void Start()
    {
        UpdateSpawnField();
        tutorialManager = GameObject.FindObjectOfType<TutorialManager>();
    }

    private void UpdateSpawnField()
    {
        Vector3 topLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelHeight, 0));
        Vector3 bottomRight = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, 0, 0));

        borderLeft = topLeft.x + marginLeft;
        borderRight = bottomRight.x - marginRight;
        borderTop = topLeft.y - marginTop;
        borderBottom = bottomRight.y + marginBottom;
    }

    private Vector3 point;
    private Vector3 scale;
    private LinkedList<Vector3> debugSpawnPosTrys = new LinkedList<Vector3>();

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (Vector3 p in debugSpawnPosTrys)
        {
            Gizmos.DrawWireCube(p, scale);
        }

        UpdateSpawnField();
        Gizmos.color = Color.green;
        Vector3 upperLeft = new Vector3(borderLeft, borderTop, zAxis);
        Vector3 upperRight = new Vector3(borderRight, borderTop, zAxis);
        Vector3 bottomLeft = new Vector3(borderLeft, borderBottom, zAxis);
        Vector3 bottomRight = new Vector3(borderRight, borderBottom, zAxis);

        Gizmos.DrawLine(upperLeft, upperRight);
        Gizmos.DrawLine(bottomLeft, bottomRight);
        Gizmos.DrawLine(upperLeft, bottomLeft);
        Gizmos.DrawLine(upperRight, bottomRight);
    }
#endif

    private int maxTrysToFindPos = 10000;

    public Vector3 GetRandomSpawnPos(GameObject type)
    {
        //Debug
        debugSpawnPosTrys = new LinkedList<Vector3>();
        scale = type.transform.localScale * 1.2f;

        Vector3 spawnPos;

        int currentTrys = 0;
        do
        {
            float x = UnityEngine.Random.Range(borderLeft + type.transform.localScale.x * 1.2f / 2, borderRight - type.transform.localScale.x * 1.2f / 2);
            float y = UnityEngine.Random.Range(borderBottom + type.transform.localScale.y * 1.2f / 2, borderTop - type.transform.localScale.y * 1.2f / 2);
            spawnPos = new Vector3(x, y, zAxis);
            debugSpawnPosTrys.AddLast(spawnPos);
            currentTrys++;
        } while (currentTrys <= maxTrysToFindPos && Physics2D.OverlapBox(spawnPos, type.transform.localScale * 1.2f, 0) != null);
        return spawnPos;
    }

    public void SpawnRandomEnemy(EnemyEntry[] enemys, AmountOfEnemysChance[] amountChances)
    {
        SpawnRandomEnemy(enemys, SelectRandomAmount(amountChances));
    }

    public int SelectRandomAmount(AmountOfEnemysChance[] amountChances)
    {
        float randomAmountChance = Random.Range(0, GetChanceSum(amountChances));

        float currentAmountChance = 0;
        foreach (AmountOfEnemysChance a in amountChances)
        {
            currentAmountChance += a.chance;
            if (currentAmountChance >= randomAmountChance)
            {
                return a.amount;
            }
        }
        return 0;
    }

    public void SpawnRandomEnemy(EnemyEntry[] enemys, int amount)
    {
        for(int i=0; i < amount; i++)
        {
            //If Boss Enemy than dont spawn two
            if (SpawnRandomEnemy(enemys))
            {
                return;
            }
        }
    }

    public bool SpawnRandomEnemy(EnemyEntry[] enemys)
    {
        GameObject type = GetRandomEnemy(enemys);
        GameObject spawnedEnemy = SpawnEnemyAtRandomPos(type);

        return spawnedEnemy.tag == "Boss";
    }

    public GameObject SpawnEnemyAtRandomPos(GameObject type)
    {
        Vector3 spawnPos = GetRandomSpawnPos(type);
        return SpawnEnemyAtPos(type, spawnPos);
    }

    public GameObject SpawnEnemyAtPos(GameObject type, Vector3 spawnPos)
    {
        GameObject spawnedEnemy = GameObject.Instantiate(type, spawnPos, Quaternion.identity);

        if (tutorialManager.CheckIfEnemyNeedsTutorial(spawnedEnemy))
        {
            tutorialManager.PlayTutorial(spawnedEnemy);
        }

        return spawnedEnemy;
    }

    public GameObject GetRandomEnemy(EnemyEntry[] enemys)
    {
        float randomChance = Random.Range(0, GetChanceSum(enemys));

        float currentChance = 0;
        foreach (EnemyEntry e in enemys)
        {
            currentChance += e.chanceToSpawn;
            if (currentChance >= randomChance)
            {
                return e.enemy;
            }
        }
        return null;
    }

    public GameObject GetRandomStage(StageEntry[] stages)
    {
        float randomChance = Random.Range(0, GetChanceSum(stages));

        float currentChance = 0;
        foreach (StageEntry s in stages)
        {
            currentChance += s.chance;
            if (currentChance >= randomChance)
            {
                return s.stage;
            }
        }
        return null;
    }

    private float GetChanceSum(EnemyEntry[] enemys)
    {
        float chanceSum = 0;
        foreach (EnemyEntry e in enemys)
        {
            chanceSum += e.chanceToSpawn;
        }
        return chanceSum;
    }

    private float GetChanceSum(StageEntry[] stages)
    {
        float chanceSum = 0;
        foreach (StageEntry s in stages)
        {
            chanceSum += s.chance;
        }
        return chanceSum;
    }

    private float GetChanceSum(AmountOfEnemysChance[] amountChances)
    {
        float chanceSum = 0;
        foreach (AmountOfEnemysChance e in amountChances)
        {
            chanceSum += e.chance;
        }
        return chanceSum;
    }
}