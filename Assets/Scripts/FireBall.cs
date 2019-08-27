using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour {
    private Rigidbody2D F_body;
    public playDir AttackDir;
	// Use this for initialization
	void Start () {
        F_body = GetComponent<Rigidbody2D>();
        transform.localScale = new Vector2((int)AttackDir*0.5f, 0.5f);
	}
	
	// Update is called once per frame
	void Update () {
        Move();
	}

    void Move() {
        F_body.velocity = new Vector2(20f *(int)AttackDir,0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("22");
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<EnemyOneCtrl>().OnDamage((AttackDir)((int)AttackDir));
        }
        else if (other.gameObject.tag == "Enemy2")
        {
            other.gameObject.GetComponent<EnemyTwoCtrl>().OnDamage((AttackDir)((int)AttackDir));
        }
        Destroy(this.gameObject);
    }
}
