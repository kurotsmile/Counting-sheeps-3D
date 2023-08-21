using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Purchasing;
using System.Collections;
using System.Collections.Generic;
using System;
using Carrot;

public class game_handle : MonoBehaviour {
    public Carrot.Carrot carrot;

    public GameObject game_home;
	public GameObject game_sleep_play;
	public GameObject game_sheep_play;
    public GameObject game_sheep_tap;

	public GameObject panel_home;
	public GameObject panel_sleep_select;
	public GameObject panel_play_sleep;
	public GameObject panel_play_sheep;
    public GameObject panel_play_sheep_tap;

	public AudioSource[] sounds;
    public AudioClip sound_click_AudioClip;


	public Color32 color_sel;
	public Color32 color_nomal;

    public Sprite sp_setting_model_end_on;
    public Sprite sp_setting_model_end_off;

    private Carrot_Box_Item box_model_end;
    private Carrot_Window_Input box_inp_customer_sleep;

    void Start () {
        this.carrot.Load_Carrot(check_exit_app);
        this.carrot.shop.onCarrotPaySuccess += this.onBuySuccessCarrotPay;
        this.carrot.game.load_bk_music(this.sounds[0]);
        this.carrot.change_sound_click(this.sound_click_AudioClip);

		Screen.sleepTimeout = (int)SleepTimeout.NeverSleep;
        this.hide_all_panel();
        this.game_home.SetActive(true);
        this.panel_home.SetActive(true);
    }

    void check_exit_app()
    {
        if (this.panel_play_sheep.activeInHierarchy)
        {
            this.back_home();
            this.carrot.set_no_check_exit_app();
        }
        else if (this.panel_play_sleep.activeInHierarchy)
        {
            this.back_home();
            this.carrot.set_no_check_exit_app();
        }
        else if (this.panel_sleep_select.activeInHierarchy)
        {
            this.back_home();
            this.carrot.set_no_check_exit_app();
        }
    }

    private void hide_all_panel()
    {
        this.panel_sleep_select.SetActive(false);
        this.panel_play_sheep.SetActive(false);
        this.panel_play_sleep.SetActive(false);
        this.panel_play_sheep_tap.SetActive(false);
        this.game_sleep_play.SetActive(false);
        this.game_sheep_play.SetActive(false);
        this.game_sheep_tap.SetActive(false);
        this.game_home.SetActive(false);
        this.panel_home.SetActive(false);
    }


    public void show_account_me()
    {
        this.check_pause_game();
        this.carrot.show_login();
    }
		
	public void go_to_sleep_select(){
        this.carrot.play_sound_click();
        this.carrot.ads.show_ads_Interstitial();
        Screen.sleepTimeout = (int)SleepTimeout.NeverSleep;
        this.hide_all_panel();
		this.panel_sleep_select.SetActive (true);
        this.game_home.SetActive(true);
	}

	public void back_home(){
        this.carrot.play_sound_click();
        this.carrot.ads.show_ads_Interstitial();
        this.hide_all_panel();
        this.panel_home.SetActive(true);
        this.game_home.SetActive(true);
	}

    public void go_to_sheep_play()
    {
        this.carrot.play_sound_click();
        this.carrot.ads.show_ads_Interstitial();
        this.hide_all_panel();
        this.panel_play_sheep.SetActive(true);
        this.game_sheep_play.SetActive(true);
        this.game_sheep_tap.GetComponent<Game_sheep_tap>().Fence.SetActive(true);
    }

    public void go_to_sleep_play(int number_sheep){
        this.carrot.play_sound_click();
        this.hide_all_panel();
        this.game_sleep_play.SetActive(true);
		this.panel_play_sleep.SetActive (true);
        this.game_sleep_play.GetComponent<Game_sleep_play> ().clear_sheep(number_sheep);
        this.game_sheep_tap.GetComponent<Game_sheep_tap>().Fence.SetActive(true);
    }

    public void btn_show_customer_sleep_play()
    {
        this.carrot.play_sound_click();
        this.box_inp_customer_sleep=this.carrot.show_input(PlayerPrefs.GetString("goto_sleep"),"Enter the number of sheep you want to count","10");
        this.box_inp_customer_sleep.inp_text.contentType = InputField.ContentType.IntegerNumber;
        this.box_inp_customer_sleep.set_act_done(this.act_done_customer_sleep);
    }

    private void act_done_customer_sleep(string s_txt) 
    {
        int num_sheep = int.Parse(s_txt);
        this.go_to_sleep_play(num_sheep);
        if (this.box_inp_customer_sleep != null) this.box_inp_customer_sleep.close();
    }

    public void go_to_sheep_tap()
    {
        this.carrot.play_sound_click();
        this.hide_all_panel();
        this.game_sheep_tap.SetActive(true);
        this.panel_play_sheep_tap.SetActive(true);
        this.game_sheep_tap.GetComponent<Game_sheep_tap>().play_game();
    }


	public void game_rate(){
        this.carrot.show_rate();
	}

	private void change_setting_model_end(){
		if (PlayerPrefs.GetInt ("is_sheep_end", 0)==0) {
			PlayerPrefs.SetInt ("is_sheep_end",1);
		} else {
			PlayerPrefs.SetInt ("is_sheep_end",0);
		}
        this.reload_item_setting_model_end();
	}

	public void open_setting(){
        this.check_pause_game();

        Carrot.Carrot_Box box_setting=this.carrot.Create_Setting();
        box_setting.set_act_before_closing(this.act_close_setting);

        this.box_model_end = box_setting.create_item_of_index("count_end", 1);
        box_model_end.set_title("When finished counting sheep");
        box_model_end.check_type();
        this.reload_item_setting_model_end();

        box_model_end.set_act(this.change_setting_model_end);
    }

    private void reload_item_setting_model_end()
    {
        if (PlayerPrefs.GetInt("is_sheep_end", 0) == 0)
        {
            box_model_end.set_icon(this.sp_setting_model_end_off);
            box_model_end.set_tip("Off");
            box_model_end.set_key_lang_tip("count_end_1");
        }
        else
        {
            box_model_end.set_icon(this.sp_setting_model_end_on);
            box_model_end.set_tip("On");
            box_model_end.set_key_lang_tip("count_end_2");
        }
        box_model_end.set_key_lang_title("count_end_tip");
        box_model_end.load_lang_data();
    }

    private void act_close_setting(List<string> list_item_change)
    {
        this.check_unPause_game();
    }

    public void show_store_music()
    {
        this.carrot.game.show_list_music_game();
    }

    public void show_top_player()
    {
        this.carrot.game.Show_List_Top_player();
    }

    public void show_app_other()
    {
        this.carrot.show_list_carrot_app();
    }

    public void check_pause_game()
    {
        if (this.game_sheep_play.activeInHierarchy)
        {
            this.game_sheep_play.GetComponent<Game_sheep_play>().pause();
        }
        else if (this.game_sheep_tap.activeInHierarchy)
        {
            this.game_sheep_tap.GetComponent<Game_sheep_tap>().pause();
        }
        else if (this.game_sleep_play.activeInHierarchy)
        {
            this.game_sleep_play.GetComponent<Game_sleep_play>().game_pause();
        }
    }


    public void check_unPause_game()
    {
        if (this.game_sheep_play.activeInHierarchy)
        {
            this.game_sheep_play.GetComponent<Game_sheep_play>().unPause();
        }
        else if (this.game_sheep_tap.activeInHierarchy)
        {
            this.game_sheep_tap.GetComponent<Game_sheep_tap>().unPause();
        }
        else if (this.game_sleep_play.activeInHierarchy)
        {
            this.game_sleep_play.GetComponent<Game_sleep_play>().game_unPause();
        }
    }

    public void share_app()
    {
        this.carrot.show_share();
    }

    public void btn_show_select_lang()
    {
        this.carrot.show_list_lang();
    }

    private void onBuySuccessCarrotPay(string id_product)
    {
        if (id_product == this.carrot.shop.get_id_by_index(2))
        {
            this.carrot.show_msg(PlayerPrefs.GetString("shop", "shop"), PlayerPrefs.GetString("buy_heart_success", "Buy heart success! You can continue the game"), Carrot.Msg_Icon.Success);
            this.game_sheep_play.GetComponent<Game_sheep_play>().add_heart_to_game();
        }
    }

    public void show_help()
    {
        this.carrot.play_sound_click();
        this.carrot.show_msg(PlayerPrefs.GetString("help", "Help"), PlayerPrefs.GetString("home_tip", "Sheep anti-insomnia counting app with two main features is going to bed and gaming to help you get rid of straightness and deep sleep. Go to bed: the app will give you a large number of sheep for you to count and drop into songs that make your mind easy to go to sleep: you choose the right time to touch you Sheep help him jump over the fence and you will get points and the second game you have to protect your food by hitting the rescues who are trying to steal your fruit."),Carrot.Msg_Icon.Question);
    }

    public void play_sound(int index_sound)
    {
        if (this.carrot.get_status_sound()) this.sounds[index_sound].Play();
    }

    public void btn_buy_heart()
    {
        this.carrot.buy_product(2);
    }
}
