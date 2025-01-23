using UnityEngine;
using UnityEngine.UI;

public class Game_sheep_play : MonoBehaviour {

	[Header("Game Obj main")]
	public game_handle games;
	public GameObject[] sheep;
	public Transform point_start;
	public float time_create = 0f;
	public float speed_time=1f;
	public GameObject heart;
	int count_sheep=0;

	[Header("Ui Obj")]
	public GameObject panel_game_over;
	public Text txt_play_sleep_number;
	public Text txt_play_top_score;

	public Image img_btn_pause;
	public Sprite icon_play;
	public Sprite icon_pause;

	private bool is_run = true;

	public GameObject[] btn_act_sheep;
	public Transform  panel_heart;

	void Start () {
		this.create_sheep ();
		this.hide_all_act ();
	}

	void Update () {
		if (this.is_run) {
			this.time_create += this.speed_time * Time.deltaTime;
			if (time_create > 3f) {
				this.time_create = 0f;
				this.create_sheep ();
			}
		}
	}

	public void create_sheep(){
		int id_sheep=Random.Range(0,this.sheep.Length);
		GameObject sheep_sleep = Instantiate (this.sheep[id_sheep]);
		sheep_sleep.name = "sheep";
        Destroy(sheep_sleep.GetComponent<Sheep_game_2>());
        sheep_sleep.GetComponent<Sheep>().enabled = true;
        sheep_sleep.GetComponent<Sheep> ().id = id_sheep;
		sheep_sleep.transform.SetParent (this.transform);
		sheep_sleep.transform.localPosition = point_start.localPosition;
		Destroy (sheep_sleep, 15f);
	}

	public void clear_sheep(){
		this.hide_all_act ();
		this.is_run = true;
		this.txt_play_sleep_number.text = "0";
		this.count_sheep =0;
		foreach (Transform sheep_clone in this.transform) {
			if(sheep_clone.gameObject.name=="sheep")
				Destroy (sheep_clone.gameObject);
		}
		this.img_btn_pause.sprite = this.icon_pause;
		this.speed_time = 1f;
		this.time_create = 0f;
		this.games.carrot.clear_contain(this.panel_heart);
		this.add_heart ();
		this.add_heart ();
		this.add_heart ();
		this.panel_game_over.SetActive (false);
		this.txt_play_top_score.text = this.games.carrot.L("hight_socre", "Highest score") + " : " + PlayerPrefs.GetInt ("top_socre", 0);
	}

	public void add_sheep(){
		this.count_sheep++;
		this.txt_play_sleep_number.text = this.count_sheep.ToString();
		if (this.count_sheep <= 0) {
			Application.Quit ();
			GameObject.Find ("game_handle").GetComponent<game_handle> ().back_home ();
		}
		this.speed_time += 0.01f;
	}

	public void btn_pause(){
		this.games.carrot.play_sound_click();
		if (this.is_run) 
            this.pause();			
		else 
            this.unPause();			
	}

	public void pause(){
		this.img_btn_pause.sprite = this.icon_play;
		this.is_run = false;
		this.set_all_sheep_pause (true);
    }

	public void unPause(){
		this.img_btn_pause.sprite = this.icon_pause;
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

	private void hide_all_act(){
		foreach (GameObject btn in this.btn_act_sheep) {
			btn.SetActive (false);
		}
	}

	public void show_tag_act(int index){
		this.btn_act_sheep [index].SetActive (true);
	}

	public void hide_tag_act(int index){
		this.btn_act_sheep [index].SetActive (false);
	}

	public void jumb_act(int index){
		foreach (Transform sheep_clone in this.transform) {
			if (sheep_clone.gameObject.name == "sheep") {
				if (sheep_clone.gameObject.GetComponent<Sheep> ().id == index) {
					sheep_clone.gameObject.GetComponent<Sheep> ().jump_game ();
					this.games.play_sound(1);
					break;
				}
			}
		}
		this.btn_act_sheep [index].SetActive (false);
	}

	public void remove_heart(){
		foreach (Transform heart_item in this.panel_heart) {
			Destroy (heart_item.gameObject);
			break;
		}

		if(this.panel_heart.transform.childCount<=1){
			this.games.carrot.play_vibrate();
			this.panel_game_over.SetActive (true);
			this.set_all_sheep_pause (true);
			this.is_run = false;

			this.games.play_sound(3);

            if (this.count_sheep>PlayerPrefs.GetInt ("top_socre", 0)){
				PlayerPrefs.SetInt ("top_socre", this.count_sheep);
				if (this.games.carrot.user.get_id_user_login() != "") this.games.carrot.game.update_scores_player(this.count_sheep, 0);
			}
			this.games.ads.show_ads_Interstitial();
		}
	}

	void add_heart(){
		GameObject heart_item = Instantiate (this.heart);
		heart_item.transform.SetParent (this.panel_heart);
		heart_item.transform.localPosition = new Vector3 (heart_item.transform.localPosition.x, 0f, 0f);
		heart_item.transform.localScale = new Vector3 (1f, 1f, 1f);
		heart_item.transform.localRotation=Quaternion.Euler(Vector3.zero);
	}

	public void add_heart_to_game(){
		this.add_heart ();
		this.add_heart ();
		this.add_heart ();
		if(this.panel_game_over.activeInHierarchy){
			this.panel_game_over.SetActive (false);
			this.set_all_sheep_pause (true);
			this.is_run = true;
		}
	}
}
