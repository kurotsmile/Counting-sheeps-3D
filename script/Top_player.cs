using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Top_player : MonoBehaviour
{
    public Sprite icon;
    public GameObject prefab_item_category;
    public GameObject prefab_item_top_player;
    public Sprite[] iconRank;
    private string type_temp = "0";

    private Carrot.Carrot_Box box_top_player;

    public void show()
    {
        this.type_temp = "0";
        WWWForm frm = this.GetComponent<game_handle>().carrot.frm_act("list_top_player");
        frm.AddField("type", this.type_temp);
        this.GetComponent<game_handle>().carrot.send(frm, get_list_top_player);
    }

    public void show2()
    {
        this.type_temp = "1";
        WWWForm frm = this.GetComponent<game_handle>().carrot.frm_act("list_top_player");
        frm.AddField("type", this.type_temp);
        this.GetComponent<game_handle>().carrot.send(frm, get_list_top_player);
    }

    public void close()
    {
        this.GetComponent<game_handle>().check_unPause_game();
    }

    private void get_list_top_player(string s_data)
    {
        this.GetComponent<game_handle>().check_pause_game();
        if (this.box_top_player != null) this.box_top_player.close();
        this.box_top_player = this.GetComponent<game_handle>().carrot.Create_Box("top_player");
        this.box_top_player.set_icon(this.icon);
        this.box_top_player.set_title(PlayerPrefs.GetString("top_player", "Top Player"));

        IList list_top_game = (IList)Carrot.Json.Deserialize(s_data);

        GameObject item_category = Instantiate(this.prefab_item_category);
        item_category.transform.SetParent(this.box_top_player.area_all_item);
        item_category.transform.localPosition = new Vector3(item_category.transform.localPosition.x, item_category.transform.localPosition.y, 0f);
        item_category.transform.localScale = new Vector3(1f, 1f, 1f);
        item_category.transform.localRotation = Quaternion.Euler(Vector3.zero);
        item_category.GetComponent<Panel_sel_rank_item>().btn_sel[0].onClick.AddListener(this.show);
        item_category.GetComponent<Panel_sel_rank_item>().btn_sel[1].onClick.AddListener(this.show2);
        item_category.GetComponent<Panel_sel_rank_item>().btn_sel[0].GetComponentInChildren<Text>().text = PlayerPrefs.GetString("play_game", "Game 1");
        item_category.GetComponent<Panel_sel_rank_item>().btn_sel[1].GetComponentInChildren<Text>().text = PlayerPrefs.GetString("play_game2", "Game 2");
        if (this.type_temp == "1")
            item_category.GetComponent<Panel_sel_rank_item>().top_game2();
        else
            item_category.GetComponent<Panel_sel_rank_item>().top_game1();


        for (int i = 0; i < list_top_game.Count; i++)
        {
            IDictionary player = (IDictionary)list_top_game[i];
            GameObject item_top_player = Instantiate(this.prefab_item_top_player);
            item_top_player.transform.SetParent(this.box_top_player.area_all_item);
            item_top_player.transform.localPosition = new Vector3(item_top_player.transform.localPosition.x, item_top_player.transform.localPosition.y, 0f);
            item_top_player.transform.localScale = new Vector3(1f, 1f, 1f);
            item_top_player.transform.localRotation = Quaternion.Euler(Vector3.zero);
            if (i < 3)
            {
                item_top_player.GetComponent<Panel_top_player>().img_rank.sprite = this.iconRank[i];
            }
            else
            {
                item_top_player.GetComponent<Panel_top_player>().img_rank.sprite = this.iconRank[2];
            }
            item_top_player.GetComponent<Panel_top_player>().txt_name_player.text = player["name_user"].ToString();
            item_top_player.GetComponent<Panel_top_player>().txt_player_scores.text = player["scores"].ToString();
            item_top_player.GetComponent<Panel_top_player>().txt_index.text = (i + 1).ToString();
            item_top_player.GetComponent<Panel_top_player>().txt_scores.text = PlayerPrefs.GetString("scores", "scores");
            item_top_player.GetComponent<Panel_top_player>().id_user = player["id_user"].ToString();
            this.GetComponent<game_handle>().carrot.get_img(player["avatar_user"].ToString(), item_top_player.GetComponent<Panel_top_player>().img_avatar_player);
        }

        this.box_top_player.set_act_before_closing(this.close);
    }
}
