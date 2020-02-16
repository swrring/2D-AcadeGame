using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int score;
    public int stage;
    public int totalscore;
    public int PlayerHP;
    public PlayerMove player;
    public GameObject[] StageIndex;
    public Image[] UI_hp;
    public Text UI_stage;
    public Text UI_score;
    public GameObject UI_button;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {

            if (PlayerHP > 1)
            {
                PlayerRestart();                

            }
            HPdown();
        }
    }

    // Start is called before the first frame update

    public void NextStage()
    {
        if (stage < StageIndex.Length - 1)
        {
            StageIndex[stage].SetActive(false);
            stage++;
           
            totalscore += score;
            score = 0;
            UI_stage.text = (stage + 1).ToString();
            PlayerRestart();
            StageIndex[stage].SetActive(true);
        }
        else
        {
            Time.timeScale = 0;          
            Text btText = UI_button.GetComponentInChildren<Text>();
            btText.text = "게임 클리어!\n게임을 다시 시작 하시겠습니까?";
            UI_button.SetActive(true);
            Debug.Log("게임 클리어");
        }
    }
   public void HPdown()
    {
        if(PlayerHP > 1)
        {
            PlayerHP--;
            UI_hp[PlayerHP].color = new Color(1, 1, 1, 0.3f);
        }
        else
        {
            UI_hp[0].color = new Color(1, 1, 1, 0.3f);
            player.onDie();
            UI_button.SetActive(true);
        }    
    }
    void PlayerRestart()
    {
        player.PlayerPosionZero();
        player.transform.position = new Vector3(0, 0, -1);
    }
    private void Update()
    {
        UI_score.text = (totalscore+score).ToString();
    }
    public void GameRestart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
