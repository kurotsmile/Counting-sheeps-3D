using Carrot;
using Firebase.Firestore;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[FirestoreData]
public struct Good_Night_Data
{
	public string id { get; set; }
	[FirestoreProperty]
	public string msg { get; set; }
	[FirestoreProperty]
	public Carrot.Carrot_Rate_user_data user { get; set; }
	[FirestoreProperty]
	public string lang { get; set; }
	[FirestoreProperty]
	public string date_create { get; set; }
}

public class Game_sleep_play : MonoBehaviour {

	[Header("Obj Game")]
	public game_handle games;
	public GameObject[] sheep;
	public GameObject panel_good_night;
	public GameObject panel_write_good_night;
	public GameObject panel_account_login;

	public Text txt_good_night_msg;
	public Text txt_good_night_name;
	public Text txt_good_night_date;

	[Header("Account current")]
	public Text txt_good_night_account_login_username;
	public Image img_goood_night_account_login_avt;
	public Image img_good_night_avatar;
	private string id_user_good_night_view;

	public InputField inp_gn_name;
	public InputField inp_gn_msg;

	public Transform point_start;
	public float time_create = 0f;
	int count_sheep=0;
    int count_sheep_cur = 0;
	public Text txt_playe_sleep_number;
	public Text txt_playe_music_name;

	public Image img_btn_pause;
	public Image img_btn_pause_music;
	public Image img_btn_msg;
	public Sprite icon_play;
	public Sprite icon_pause;
	public Sprite icon_msg_yes;
	public Sprite icon_msg_no;
	public Sprite icon_good_night_avt_default;

	private bool is_run = true;
	private bool is_show_msg=true;

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
		this.panel_good_night.SetActive (false);
		this.panel_write_good_night.SetActive (false);
	}

	public void update_name_music(){
		this.txt_playe_music_name.text = PlayerPrefs.GetString ("sel_music_music","Good Night Baby");
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

	public void good_night(){
		if (this.is_show_msg) {
			this.StopAllCoroutines ();
			if (this.panel_good_night.activeInHierarchy) {
				this.panel_good_night.SetActive (false);
			} else {
				this.img_good_night_avatar.sprite = this.icon_good_night_avt_default;

			}
		}
	}
	private void get_good_night(string s_data)
	{
		IDictionary data = (IDictionary)Carrot.Json.Deserialize(s_data);
		this.txt_good_night_msg.text = data["msg"].ToString();
		this.txt_good_night_name.text = data["name_user"].ToString();
		this.txt_good_night_date.text = data["date"].ToString();
		this.id_user_good_night_view = data["id_user"].ToString();
		if (data["avatar_user"].ToString() != "") this.games.carrot.get_img(data["avatar_user"].ToString(), this.img_good_night_avatar);
		this.panel_good_night.SetActive(true);
	}

	public void submit_good_night(){
		if (this.panel_account_login.activeInHierarchy == false) {
			if (this.inp_gn_name.text == null || this.inp_gn_name.text.Length <= 5) {
				this.games.carrot.show_msg(PlayerPrefs.GetString("good_night_write", "Write Good Night"), PlayerPrefs.GetString("error_name", "Your name must not be longer than 5 characters"), Carrot.Msg_Icon.Error);
				return;
			}
		}

		if (this.inp_gn_msg.text == null || this.inp_gn_msg.text.Length <= 10) {
			this.games.carrot.show_msg(PlayerPrefs.GetString("good_night_write", "Write Good Night"), PlayerPrefs.GetString("error_msg", "The message can not be blank and is greater than 5 characters"),Carrot.Msg_Icon.Error);
			return;
		}
		WWWForm frm_send_goodnight = this.games.carrot.frm_act("send_good_night");
		frm_send_goodnight.AddField("msg", this.inp_gn_msg.text);
		if (this.games.carrot.user.get_id_user_login()!="")
		{
			frm_send_goodnight.AddField("user_name", this.games.carrot.user.get_id_user_login());
			frm_send_goodnight.AddField("user_type", "1");
		}
		else
		{
			frm_send_goodnight.AddField("user_name", this.inp_gn_name.text);
			frm_send_goodnight.AddField("user_type", "0");
		}

		Good_Night_Data good_night = new Good_Night_Data();
		good_night.msg = this.inp_gn_msg.text;


        if (this.games.carrot.user.get_id_user_login() != "")
        {
			Carrot.Carrot_Rate_user_data user_login = new Carrot_Rate_user_data();
			user_login.name = this.games.carrot.user.get_data_user_login("name");
			user_login.id = this.games.carrot.user.get_id_user_login();
			user_login.lang = this.games.carrot.user.get_lang_user_login();
			user_login.avatar = this.games.carrot.user.get_data_user_login("avatar");
			good_night.user = user_login;
		}

		this.games.carrot.db.Collection("good_night").AddAsync(good_night);

		this.games.carrot.show_msg(PlayerPrefs.GetString("good_night_write", "Write Good Night"), PlayerPrefs.GetString("good_night_success", "Good night success!!!"), Carrot.Msg_Icon.Success);
		this.btn_close_write_good_night();
	}

	public void btn_show_msg(){
		this.games.carrot.play_sound_click();
		if (this.is_show_msg) {
			this.StopAllCoroutines ();
			this.img_btn_msg.sprite = icon_msg_yes;
			this.panel_good_night.SetActive (false);
			this.is_show_msg = false;
		} else {
			this.img_btn_msg.sprite = icon_msg_no;
			this.is_show_msg = true;
		}
	}

	public void btn_show_write_good_night(){
		this.games.carrot.play_sound_click();
        this.btn_pause();
		this.panel_write_good_night.SetActive (true);
		string id_user_login =this.games.carrot.user.get_id_user_login();
		if (id_user_login != "") {
			this.panel_account_login.SetActive (true);
			this.txt_good_night_account_login_username.text =this.games.carrot.user.get_data_user_login("name");
		} else{
			this.panel_account_login.SetActive (false);
		}
	}

    public void btn_close_write_good_night()
    {
		this.games.carrot.play_sound_click();
        this.panel_write_good_night.SetActive(false);
        this.game_unPause();
    }

	public void btn_view_user_good_night(){
		if (this.id_user_good_night_view != "") {
			this.games.carrot.play_sound_click();
			string lang_user_login = this.games.carrot.user.get_lang_user_login();
			this.games.carrot.user.show_user_by_id(this.id_user_good_night_view, lang_user_login);
		}
	}

}
