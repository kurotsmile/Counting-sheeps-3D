using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep_game_2 : MonoBehaviour
{
    public Transform target;
    private float speed = 1f;
    private Animator ani;
    private bool is_die=false;
    private bool is_run =true;

    void Start()
    {
        this.ani = this.GetComponent<Animator>();
        this.ani.Play("run");
    }

    public void set_speed(float max_speed)
    {
        this.speed = Random.Range(1f, max_speed);
    }
    // Start is called before the first frame update
    void OnMouseDown()
    {
        if (is_die == false)
        {
            if (this.is_run)
            {
                ani.Play("Die");
                this.GetComponent<BoxCollider>().enabled = false;
                this.is_die = true;
                Destroy(this.gameObject, 0.8f);
                GameObject.Find("game_handle").GetComponent<game_handle>().play_sound(2);
                GameObject.Find("Game_sheep_tap").GetComponent<Game_sheep_tap>().add_scores();
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (this.is_die == false)
        {
            if (this.is_run)
            {
                float step = speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, target.position, step);
                transform.LookAt(this.target);
                if (Vector3.Distance(transform.position, target.position) < 0.001f)
                {
                    target.position *= -1.0f;
                }
            }
        }
    }

    public void Pause()
    {
        ani.Play("Idle");
        this.is_run = false;
    }

    public void unPause()
    {
        ani.Play("run");
        this.is_run = true;
    }


    void OnTriggerEnter(Collider col)
    {
        if (this.is_die==false)
        {
            if (col.name == "Apple")
            {
                this.is_run = false;
                GameObject.Find("Game_sheep_tap").GetComponent<Game_sheep_tap>().Game_over();
            }
        }
    }
}
