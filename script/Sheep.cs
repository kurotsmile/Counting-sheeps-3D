using UnityEngine;

public class Sheep : MonoBehaviour
{
	private float speed = 1f;
	public Animator ani;
	public Rigidbody rig;
	private bool is_run = true;
	private bool is_jump_game = false;
	public int id = -1;

	void Start()
	{
		ani.Play("run");
	}

	void Update()
	{
		if (this.is_run)
		{
			this.speed = 1f * Time.deltaTime;
			transform.position += new Vector3(0, 0, this.speed);
		}
	}

	public void jump()
	{
		ani.Play("Atk");
		this.rig.AddForce(0, 300f, 110f);
		GameObject.Find("Game_sleep_play").GetComponent<Game_sleep_play>().add_sheep();
	}

	public void jump_game()
	{
		if (this.is_jump_game)
		{
			ani.Play("Atk");
			this.rig.AddForce(0, Random.Range(230f, 301f), Random.Range(60f, 111f));
			this.is_jump_game = false;
		}
	}

	void OnTriggerEnter(Collider col)
	{
		if (this.is_run)
		{
			if (col.gameObject.name == "jump")
			{
				this.jump();
			}

			if (col.gameObject.name == "add")
			{
				this.id = -1;
				GameObject.Find("Game_sheep_play").GetComponent<Game_sheep_play>().add_sheep();
			}

			if (col.gameObject.name == "show_tag")
			{
				GameObject.Find("Game_sheep_play").GetComponent<Game_sheep_play>().show_tag_act(this.id);
				this.is_jump_game = true;
			}

			if (col.gameObject.name == "die")
			{
				ani.Play("Die");
				this.rig.AddForce(0, 50f, -50f);
				this.is_run = false;
				Destroy(this.GetComponent<BoxCollider>());
				Destroy(this.GetComponent<Rigidbody>());
				GameObject.Find("Game_sheep_play").GetComponent<Game_sheep_play>().hide_tag_act(this.id);
				if (PlayerPrefs.GetInt("is_sound_mute", 0) == 0)
				{
					GameObject.Find("game_handle").GetComponent<game_handle>().play_sound(2);
				}
				GameObject.Find("Game_sheep_play").GetComponent<Game_sheep_play>().remove_heart();
				Destroy(this.gameObject, 0.8f);
			}
		}
	}

	public void pause()
	{
		ani.Play("Idle");
		this.is_run = false;
	}

	public void unPause()
	{
		ani.Play("run");
		this.is_run = true;
	}

	void OnMouseDown()
	{
		if (this.id != -1)
		{
			this.jump_game();
			GameObject.Find("game_handle").GetComponent<game_handle>().play_sound(1);
			GameObject.Find("Game_sheep_play").GetComponent<Game_sheep_play>().hide_tag_act(this.id);
		}
	}
}
