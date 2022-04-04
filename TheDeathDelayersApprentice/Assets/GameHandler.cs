using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{
    public List<Item> ItemsWanted = new List<Item>();
    public List<Item> Items = new List<Item>();
    public SpriteRenderer[] SpeechBubbleItemSprites;
    public Animator[] speechBubbleAnimators;

    public float gameTimer = 45;
    public float gameTimerMax;

    public RectTransform timerBar;

    public float testValue;

    public ItemSpawner[] caterpillarSpawners;

    bool hasRestarted = false;

    void Start() {
        hasRestarted = false;
        gameTimerMax = gameTimer;
        RandomizeItemsWanted();
    }

    void Update() {
        gameTimer -= Time.deltaTime;
        timerBar.sizeDelta = new Vector2(gameTimer * (testValue / 100f), timerBar.sizeDelta.y);

        if (gameTimer > gameTimerMax) {
            gameTimer = gameTimerMax;
        }
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }

        if (gameTimer < 0 && !hasRestarted) {
            hasRestarted = true;
            StartCoroutine(LoadSceneCo("SampleScene"));
        }
    }

    public void RandomizeItemsWanted() {
            if (ItemsWanted[0] == null) {
                int randomNum = Random.Range(2, 6);
                ItemsWanted[0] = Items[randomNum];
                SpeechBubbleItemSprites[0].sprite = Items[randomNum].itemSprite;
                CheckForWhatToSpawn(randomNum);
            }
            if (ItemsWanted[1] == null) {
                int randomNum = Random.Range(2, 6);
                ItemsWanted[1] = Items[randomNum];
                SpeechBubbleItemSprites[1].sprite = Items[randomNum].itemSprite;
                CheckForWhatToSpawn(randomNum);
            }
            if (ItemsWanted[2] == null) {
                int randomNum = Random.Range(2, 6);
                ItemsWanted[2] = Items[randomNum];
                SpeechBubbleItemSprites[2].sprite = Items[randomNum].itemSprite;
                CheckForWhatToSpawn(randomNum);
            }
            if (ItemsWanted[3] == null) {
                int randomNum = Random.Range(2, 6);
                ItemsWanted[3] = Items[randomNum];
                SpeechBubbleItemSprites[3].sprite = Items[randomNum].itemSprite;
                CheckForWhatToSpawn(randomNum);
            } 
    }

    public void CheckForWhatToSpawn(int num) {
        if (num == 2) {
            SpawnCaterpillar();
        } else if (num == 3) {
            SpawnCaterpillarOrange();
        }
    }

    public void CheckForCorrectItem(Item item) {
        int i = 0;
        foreach(Item itm in ItemsWanted) {
            if (itm.itemIndex == item.itemIndex) {
                speechBubbleAnimators[i].SetTrigger("NewItem");
                gameTimer += 10f;
                StartCoroutine(NewItemCo(i));
                break;
            }
            i++;
        }
    }

    IEnumerator NewItemCo(int i) {
        yield return new WaitForSeconds(0.5f);
        ItemsWanted[i] = null;
        print("CORRECT");
        RandomizeItemsWanted();
    }

    public void SpawnCaterpillar() {
        int randomNum = Random.Range(0, caterpillarSpawners.Length - 1);
        GameObject go = Instantiate(Items[2].parentTransform.gameObject, caterpillarSpawners[randomNum].transform.position, Quaternion.identity);
        go.SetActive(true);
    }

    public void SpawnCaterpillarOrange() {
        int randomNum = Random.Range(0, caterpillarSpawners.Length - 1);
        GameObject go = Instantiate(Items[3].parentTransform.gameObject, caterpillarSpawners[randomNum].transform.position, Quaternion.identity);
        go.SetActive(true);
    }

    IEnumerator LoadSceneCo(string sceneName) {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone) {
            yield return null;
        }
    }
}
