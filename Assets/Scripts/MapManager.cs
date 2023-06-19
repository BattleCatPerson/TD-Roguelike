using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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
    private void Awake()
    {
        if (Rounds == -1) Rounds = rounds;
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

        //if (row + 1 < rows.Count)
        //{
        //    availableButtons.Add(rows[row + 1].buttons[col * 2]);
        //    availableButtons.Add(rows[row + 1].buttons[col * 2 + 1]);
        //}
        
        //foreach (TreeRow row in rows)
        //{
        //    foreach (Button button in row.buttons)
        //    {
        //        if (!availableButtons.Contains(button)) button.interactable = false;
        //    }
        //}

        if (Rounds > 0)
        {
            for (int i = 0; i < options; i++)
            {
                var b = buttons[Random.Range(0, buttons.Count)];
                buttons.Remove(b);
                Instantiate(b, panel.transform);
            }
        }
    }
    public void LoadBattleScene(Button b)
    {
        //int index = rows[row + 1].buttons.IndexOf(b);
        //col = index;
        //row++;
        Rounds--;
        SceneManager.LoadScene(battleScene);
    }
}
