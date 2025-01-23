using UnityEngine;
using UnityEngine.UI;

public class Game_sleep_play : MonoBehaviour {

	[Header("Obj Game")]
	public game_handle games;
	public GameObject[] sheep;

	public Text txt_good_night_msg;
	public Text txt_good_night_name;
	public Text txt_good_night_date;

	public Transform point_start;
	public float time_create = 0f;
	int count_sheep=0;
    int count_sheep_cur = 0;
	public Text txt_playe_sleep_number;
	public Text txt_playe_music_name;

	public Image img_btn_pause;
	public Image img_btn_pause_music;
	public Sprite icon_play;
	public Sprite icon_pause;
	public Sprite icon_good_night_avt_default;

	private bool is_run = true;

	void Start () {
		this.create_sheep ();
	}

	void Update () {
		if (this.is_run) {
			this.time_create += 1f * Time.deltaTime;
			if (time_create > 3f) {
				this.time_create = 0f;
				this.create_sheep ();
			}
		}
	}

	public void create_sheep(){
		GameObject sheep_sleep = Instantiate (this.sheep[Random.Range(0,this.sheep.Length)]);
		sheep_sleep.name = "sheep";
        Destroy(sheep_sleep.GetComponent<Sheep_game_2>());
        sheep_sleep.transform.SetParent (this.transform);
		sheep_sleep.transform.localPosition = point_start.localPosition;
		Destroy (sheep_sleep, 15f);
	}
		
	public void clear_sheep(int number_sheep){
		this.is_run = true;
		this.txt_playe_sleep_number.text = number_sheep.ToString();
		this.count_sheep = number_sheep;
        this.count_sheep_cur = 0;
		foreach (Transform sheep_clone in this.transform) {
			if(sheep_clone.gameObject.name=="sheep")
			Destroy (sheep_clone.gameObject);
		}
		this.img_btn_pause.sprite = this.icon_pause;
		this.img_btn_pause_music.sprite = this.icon_play;
		this.update_name_music ();
	}

	public void update_name_music(){
		this.txt_playe_music_name.text = this.games.carrot.L("sel_music_music","Good Night Baby");
	}

	public void add_sheep(){
        this.count_sheep_cur++;
		this.txt_playe_sleep_number.text = this.count_sheep_cur.ToString()+"/"+this.count_sheep;
		if (this.count_sheep_cur>=this.count_sheep) {
			if (PlayerPrefs.GetInt("is_sheep_end", 0) == 0) {
				Application.Quit ();
			}else{
				Screen.sleepTimeout = 5;
			}
			this.games.back_home ();
			this.games.carrot.play_vibrate();
		}
		this.games.play_sound(1);
	}

	public void btn_pause(){
		if (this.is_run) {
            this.game_pause();
		} else {
            this.game_unPause();
		}
	}

	public void game_pause(){
		this.img_btn_pause.sprite = this.icon_play;
		this.img_btn_pause_music.sprite = this.icon_pause;
		this.is_run = false;
		this.set_all_sheep_pause (true);
	}

	public void game_unPause(){
		this.img_btn_pause.sprite = this.icon_pause;
		this.img_btn_pause_music.sprite = this.icon_play;
		this.is_run = true;
		this.set_all_sheep_pause (false);
	}

	private void set_all_sheep_pause(bool is_pause){
		foreach (Transform sheep_clone in this.transform) {
			if (sheep_clone.gameObject.name == "sheep") {
                if (is_pause)
                    sheep_clone.gameObject.GetComponent<Sheep>().pause();
                else
                    sheep_clone.gameObject.GetComponent<Sheep>().unPause();
			}
		}
	}
}
