using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Panel_sel_rank_item : MonoBehaviour
{

    public Button[] btn_sel;


    public void top_game1()
    {
        this.btn_sel[0].image.color = Color.yellow;
        this.btn_sel[1].image.color = Color.white;
    }

    public void top_game2()
    {
        this.btn_sel[0].image.color = Color.white;
        this.btn_sel[1].image.color = Color.yellow;
    }
}
