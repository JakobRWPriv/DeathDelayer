using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public List<Item> ItemsWanted = new List<Item>();
    public List<Item> Items = new List<Item>();
    public SpriteRenderer[] SpeechBubbleItemSprites;

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }
    }

    public void RandomizeItemsWanted() {
            if (ItemsWanted[0] != null) {
                int randomNum = Random.Range(0, Items.Count);
                ItemsWanted[0] = Items[randomNum];
                SpeechBubbleItemSprites[0].sprite = Items[randomNum].itemSprite;
            }
            if (ItemsWanted[1] != null) {
                int randomNum = Random.Range(0, Items.Count);
                ItemsWanted[1] = Items[randomNum];
                SpeechBubbleItemSprites[1].sprite = Items[randomNum].itemSprite;
            }
            if (ItemsWanted[2] != null) {
                int randomNum = Random.Range(0, Items.Count);
                ItemsWanted[2] = Items[randomNum];
                SpeechBubbleItemSprites[2].sprite = Items[randomNum].itemSprite;
            }
            if (ItemsWanted[3] != null) {
                int randomNum = Random.Range(0, Items.Count);
                ItemsWanted[3] = Items[randomNum];
                SpeechBubbleItemSprites[3].sprite = Items[randomNum].itemSprite;
            } 
    }

    public void SpawnItem() {
        
    }
}
