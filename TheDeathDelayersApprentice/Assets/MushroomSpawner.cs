using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomSpawner : MonoBehaviour
{
    public Item mushroom;
    float randomNum;

    public void Start() {
        StartCoroutine(MushroomCo());
    }

    public IEnumerator MushroomCo() {
        while(true) {
            randomNum = Random.Range(0, 4);

            if (randomNum == 1) {
                GameObject go = Instantiate(mushroom.parentTransform.gameObject, transform.position, Quaternion.identity);
                go.SetActive(true);
                print("GROOOW");
            } else {
                print("MISS");
            }

            yield return new WaitForSeconds(Random.Range(5f, 8f));
        }
    }
}
