using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFireBall : MonoBehaviour {
    public GameObject target;
    private bool isBoom;
    private Animator boom_animator;
    private Vector3 position;
    private Rigidbody2D boom_body;
    private bool isok;
    // Use this for initialization
    void Start() {
        target = GameObject.Find("Player");
        position = target.transform.position;
        boom_animator = GetComponent<Animator>();
        boom_body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {
       
        Move();
    }
    void Move()
    {
            StartCoroutine(Boom());
    }
    IEnumerator Boom() {
        yield return new WaitForSeconds(0.5f);
        if (boom_body != null) {
            boom_body.AddForce((position - transform.position).normalized * 30f);
        }

    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Boss")
        {
            isBoom = true;
            boom_animator.SetTrigger("IsBoom");
            Destroy(transform.GetComponent<Rigidbody2D>());
            StartCoroutine(destory());
        }
        if (collision.tag == "Player") {
            if (transform.position.x - collision.transform.position.x > 0) {
                collision.gameObject.GetComponent<MoveTest>().OnDamage(Vector2.left);
            }
            if (transform.position.x - collision.transform.position.x < 0)
            {
                collision.gameObject.GetComponent<MoveTest>().OnDamage(Vector2.right);
            }
        }
    }
    IEnumerator destory() {
        yield return new WaitForSeconds(0.5f);
        Destroy(this.gameObject);
    }
}
