using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonManager : MonoBehaviour
{
    // spawners
    public int currentWave;
    public int enemiesToKill;
    public bool waveFinished;
    public List<Transform> spawnPoints;
    public int currentTotalEnemies;
    public int maxTotalEnemies;

    // enemies
    public List<GameObject> enemyPrefabs;
    public Transform enemyParent;
    public float spawnRate = 2.5f;

    // ui
    public Text waveText;
    public GameObject initialDialogBox;
    public GameObject waveTextHolder;
    public Text enemiesLeftText;
    public GameObject enemiesLefTextHolder;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(InitialCoroutine());

        waveTextHolder.SetActive(true);

        currentWave = 1;

        // start wave
        StartCoroutine(PrepWaveCoroutine());
    }


    private IEnumerator InitialCoroutine() {
        yield return new WaitForSeconds(4f);
        initialDialogBox.SetActive(false);
    }


    private int GetEnemiesToKill() {
        return currentWave * 5 + (int)Mathf.Pow((currentWave-1), 2) * 4;
    }


    private int GetMaxTotalEnemies() {
        return 2 * currentWave + 4;
    }


    private void WaveComplete() {
        waveFinished = true;

        // show wave cleared
        enemiesLefTextHolder.SetActive(false);
        waveText.text = "WAVE " + currentWave.ToString() + " CLEARED";
        
        //Debug.Log("Best wave = " + bestWave.initialValue.ToString());

        currentWave += 1;

        // prep and start next wave
        StartCoroutine(PrepWaveCoroutine());
    }



    // SPAWNERS
    private void SpawnEnemy() {
        GameObject enemyPrefab = enemyPrefabs[Random.Range(0,enemyPrefabs.Count)];
        Transform spawnpoint = spawnPoints[Random.Range(0,spawnPoints.Count)];
        if (enemyPrefab && spawnpoint) {
            currentTotalEnemies += 1;
            GameObject enemy = Instantiate(enemyPrefab, spawnpoint.position, Quaternion.identity, enemyParent);
            enemy.GetComponent<Enemy>().DiedEvent.AddListener(EnemyDiedHandler);
        }
    }


    private IEnumerator PrepWaveCoroutine() {
                
        yield return new WaitForSeconds(2.5f);

        // start next wave
        waveFinished = false;
        enemiesToKill = GetEnemiesToKill();
        maxTotalEnemies = GetMaxTotalEnemies();
        currentTotalEnemies = 0;
        
        // show new enemies to kill
        waveText.text = "WAVE " + currentWave.ToString();
        enemiesLeftText.text = "ENEMIES: " + enemiesToKill.ToString();
        enemiesLefTextHolder.SetActive(true);
        

        StartCoroutine(StartWaveCoroutine());
    }

    private IEnumerator StartWaveCoroutine() {
        while (gameObject.activeInHierarchy) {
            if (waveFinished) {
                break;
            }

            yield return new WaitForSeconds(spawnRate);
            if (currentTotalEnemies < maxTotalEnemies && 
                    enemiesToKill - currentTotalEnemies > 0 && !waveFinished) {

                SpawnEnemy();
            }

        }
    }


    private void EnemyDiedHandler() {
        currentTotalEnemies -= 1;
        enemiesToKill -= 1;

        enemiesLeftText.text = "ENEMIES: " + enemiesToKill.ToString();
        
        if (enemiesToKill == 0) {
            WaveComplete();
        }
    }

}
