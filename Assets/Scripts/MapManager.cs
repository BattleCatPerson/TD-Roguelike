using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MapManager : MonoBehaviour
{
    public static int row = 0;
    public static int col = 0;
    public static MapManager instance;
    public static bool started;
    [SerializeField] List<TreeRow> rows;
    [SerializeField] List<Button> availableButtons;
    [SerializeField] string battleScene;
    [SerializeField] List<float> startingResources;
    [SerializeField] float startingHealth;

    public static int Rounds = -1;
    [SerializeField] int rounds;
    [SerializeField] List<Button> buttons;
    [SerializeField] GameObject panel;
    [SerializeField] int options;

    [SerializeField] TextMeshProUGUI roundText;
    private void Awake()
    {
        if (Rounds == -1) Rounds = 0;
        instance = this;
        if (HealthManager.health == -1 && !started)
        {
            SpawnTower.resources = startingResources;
            HealthManager.health = startingHealth;
            started = true;
        }
    }

    void Start()
    {
        if (Rounds < rounds)
        {
            roundText.text = "Round " + (Rounds + 1) + "/" + rounds;
            for (int i = 0; i < options; i++)
            {
                var b = buttons[Random.Range(0, buttons.Count)];
                buttons.Remove(b);
                Instantiate(b, panel.transform);
            }
        }
        else
        {
            roundText.text = "Complete";
        }
    }
    public void LoadBattleScene(Button b)
    {
        //int index = rows[row + 1].buttons.IndexOf(b);
        //col = index;
        //row++;
        Rounds++;
        SceneManager.LoadScene(battleScene);
    }
}
