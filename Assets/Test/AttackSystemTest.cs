using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSystemTest : MonoBehaviour {

    private playDir nowDir;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        getPlayDir();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy") {
            other.gameObject.GetComponent<EnemyOneCtrl>().OnDamage(transform.parent.parent.GetComponent<MoveTest>().attackDir);
        }
        if (other.tag == "Enemy2") {
            other.gameObject.GetComponent<EnemyTwoCtrl>().OnDamage(transform.parent.parent.GetComponent<MoveTest>().attackDir);
        }
        if (other.tag == "Wall") {

        }
        if (other.tag == "Boss") {
            other.gameObject.GetComponent<BossCtrl>().onDamage();
        }
    }

    void getPlayDir()
    {
        nowDir = transform.parent.parent.GetComponent<MoveTest>().nowDir;
    }
}
