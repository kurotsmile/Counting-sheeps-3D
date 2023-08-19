using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Panel_top_player : MonoBehaviour {
	public Text txt_index;
    public Image img_rank;
    public Image img_avatar_player;
	public Text txt_name_player;
	public Text txt_scores;
	public Text txt_player_scores;
	public string id_user;


	public void view_player(){
		GameObject.Find ("game_handle").GetComponent<game_handle> ().carrot.user.show_user_by_id(this.id_user,PlayerPrefs.GetString("lang"));
	}


}
