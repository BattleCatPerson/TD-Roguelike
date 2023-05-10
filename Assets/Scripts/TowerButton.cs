using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TowerButton : MonoBehaviour
{
    [SerializeField] TowerShoot tower;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] float changeTextSize;
    public void ChangeTower()
    {
        if (SpawnTower.resources >= tower.Cost) SpawnTower.SetTower(tower);
        else StartCoroutine(ChangeTextAndWaitForSeconds("Not Enough Resources!", 0.5f));
    }

    public IEnumerator ChangeTextAndWaitForSeconds(string message, float time)
    {
        float initialSize = text.fontSize;
        string initialText = text.text;
        text.fontSize = changeTextSize;
        text.text = message;
        yield return new WaitForSeconds(time);
        text.fontSize = initialSize;
        text.text = initialText;
    }
}
