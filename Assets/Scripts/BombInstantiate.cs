using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombInstantiate : MonoBehaviour
{
    [SerializeField]GameObject bomb;
    float secondsToWait;

    private void Start()
    {
        secondsToWait = Random.Range(2f, 3.5f);
        StartCoroutine(InstantiateBomb(bomb, secondsToWait));
    }

    IEnumerator InstantiateBomb(GameObject bomb, float secondstoWait)
    {
        while(true)
        {
            Instantiate(bomb, this.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(secondstoWait);
        }
    }
}
