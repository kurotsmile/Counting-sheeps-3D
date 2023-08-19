using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Game_sheep_tap : MonoBehaviour
{
    public game_handle games;
    public Text txt_scores;
    public GameObject panel_gameover;

    public Transform[] point_create;
    public GameObject[] prefab_sheep;
    public Transform tag_apple;

    public GameObject apple2;
    public GameObject Fence;

    float timer_create = 2f;
    private int max_sheep=5;
    private float max_speed = 2f;
    public Transform area_all_sheep;
    private int scores=0;

    public Image img_btn_pause;
    public Sprite icon_play;
    public Sprite icon_pause;

    private bool is_pause = false;
    public Text txt_play_top_score;


    void Update()
    {
        if (this.is_pause == false)
        {
            this.timer_create -= Time.deltaTime;
            if (this.timer_create < 0)
            {
                for (int i = 0; i < Random.Range(2, this.max_sheep); i++)
                {
                    this.create_sheep();
                }
                this.max_speed += 0.2f;
                this.max_sheep += 1;
                this.timer_create = 10f;
            }
        }
    }

    public void game_reset()
    {
        this.timer_create = 2f;
        this.max_sheep = 5;
        this.max_speed = 2f;
        this.scores = 0;
        this.txt_scores.text = "0";
        this.is_pause = false;
        this.panel_gameover.SetActive(false);
        this.tag_apple.gameObject.SetActive(true);
        this.apple2.SetActive(false);
        this.img_btn_pause.sprite = this.icon_pause;
        this.delete_all_sheep();
        this.txt_play_top_score.text = PlayerPrefs.GetString("hight_socre", "Highest score") + " : " + PlayerPrefs.GetInt("top_socre2", 0);
    }

    public void add_scores()
    {
        this.scores += 1;
        this.txt_scores.text = this.scores + "";
    }

    public void play_game()
    {
        this.Fence.SetActive(false);
        this.game_reset();
    }

    public void Game_over()
    {
        this.pause_all_sheep(true);
        this.games.play_sound(3);
        this.panel_gameover.SetActive(true);
        this.tag_apple.gameObject.SetActive(false);
        this.apple2.SetActive(true);

        if (this.scores > PlayerPrefs.GetInt("top_socre2", 0))
        {
            PlayerPrefs.SetInt("top_socre2", this.scores);
            this.games.carrot.game.update_scores_player(this.scores, 1);
        }
    }

    [ContextMenu ("Create Sheep")]
    public void create_sheep()
    {
        int index_p = Random.Range(0, this.prefab_sheep.Length);
        GameObject sheep_obj = Instantiate<GameObject>(this.prefab_sheep[index_p]);
        sheep_obj.name = "sheep";
        sheep_obj.transform.SetParent(this.area_all_sheep);
        sheep_obj.GetComponent<Sheep>().enabled=false;
        sheep_obj.GetComponent<Sheep_game_2>().enabled = true;
        sheep_obj.GetComponent<Sheep_game_2>().target = tag_apple;
        sheep_obj.GetComponent<Sheep_game_2>().set_speed(this.max_speed);
        int index_point = Random.Range(0, this.point_create.Length);
        sheep_obj.transform.position = this.point_create[index_point].position;
    }

    public void play_an_pause_game()
    {
        if (this.is_pause)
            this.unPause();
        else
            this.pause();
    }

    public void pause()
    {
        this.img_btn_pause.sprite = this.icon_play;
        this.pause_all_sheep(true);
        this.is_pause = true;
    }

    public void unPause()
    {
        this.img_btn_pause.sprite = this.icon_pause;
        this.pause_all_sheep(false);
        this.is_pause = false;
    }

    public void pause_all_sheep(bool is_pause)
    {
        foreach(Transform sh in this.area_all_sheep)
        {
            if (is_pause)
            {
                sh.GetComponent<Sheep_game_2>().Pause();
            }
            else
            {
                sh.GetComponent<Sheep_game_2>().unPause();
            }
            
        }
    }

    private void delete_all_sheep()
    {
        foreach (Transform sh in this.area_all_sheep)
        {
            Destroy(sh.gameObject);
        }
    }
}
