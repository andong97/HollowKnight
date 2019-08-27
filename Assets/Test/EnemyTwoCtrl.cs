using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTwoCtrl : MonoBehaviour {
    private Vector3 StartPosition;
    private EnemyDir nowDir;
    private Animator E_animator;
    private Rigidbody2D E_body;
    private float speed = 3.0f;
    public float RayDis = 8.0f;
    private bool isDamage = false;
    private bool isAttack = false;
    private int hp = 3;
    private bool isDeath = false;
    private CharactorCtrl charaview;
    // Use this for initialization
    void Start () {
        StartPosition = transform.position;
        nowDir = EnemyDir.Right;
        E_animator = GetComponent<Animator>();
        E_body = GetComponent<Rigidbody2D>();
        charaview = GameObject.Find("UI").transform.Find("Canvas").Find("Charactor").GetComponent<CharactorCtrl>();
    }
	
	// Update is called once per frame
	void Update () {
        if (GameCtrl.isGamePause)
        {
            return;
        }
        if (isDeath)
        {
            return;
        }
        Move();
        Attack();
    }
    void Move()
    {
        if (isDamage)
        {
            return;
        }
        if (isAttack)
        {
            Vector2 playerH = (GameObject.FindGameObjectWithTag("Player").transform.position - transform.position);
            if (playerH.x > 0)
            {
                transform.Translate(Vector2.right * speed * Time.deltaTime, Space.Self);
            }
            else if (playerH.x < 0)
            {
                transform.Translate(Vector2.left * speed * Time.deltaTime, Space.Self);
            }
            return;
        }
        transform.Translate(Vector2.right * (int)nowDir * speed * Time.deltaTime, Space.Self);
        if (Vector2.Distance(transform.position, StartPosition) > 3f)
        {
            nowDir = (EnemyDir)(-(int)nowDir);
            transform.localScale = new Vector2((int)nowDir, 1);
        }
    }

    void Attack() {     
        bool isFindPlayer;
        isFindPlayer = Physics2D.Raycast(
            new Vector2(transform.position.x,transform.position.y-1.0f), 
            new Vector2((int)nowDir, 0), 
            RayDis,
            1 << LayerMask.NameToLayer("Player"));
        if (isFindPlayer)
        {
            StartPosition = transform.position;
            transform.Translate(Vector2.zero, Space.Self);
            isAttack = true;
            speed = 5.0f;
            if ((Mathf.Abs(GameObject.Find("Player").transform.position.x - transform.position.x)) < 2f)
            {
                StartCoroutine(AttackAnimation());
            }
        }
        if (!isFindPlayer)
        {
            isAttack = false;
            speed = 2.0f;
        }     
    }
    IEnumerator AttackAnimation() {
        E_animator.SetBool("IsAttack", true);
        yield return new WaitForSeconds(0.5f);
        E_animator.SetBool("IsAttack", false);
    }

    public void OnDamage(AttackDir nowAtkDir)
    {
        if (isDeath)
        {
            return;
        }
        StartCoroutine(onDamageing(nowAtkDir));
    }
    //被攻击受伤后退
    IEnumerator onDamageing(AttackDir nowAtkDir)
    {
        isDamage = true;
        if (nowAtkDir == AttackDir.Left)
        {
            E_body.velocity = new Vector2(-10f, 0);
        }
        else if (nowAtkDir == AttackDir.Right)
        {
            E_body.velocity = new Vector2(10f, 0);
        }
        else if (nowAtkDir == AttackDir.Down)
        {
        }
        hp -= 1;
        if (hp <= 0)
        {
            Death();
        }
        yield return new WaitForSeconds(0.2f);
        E_body.velocity = Vector2.zero;
        if ((int)nowDir == (int)nowAtkDir)
        {
            StartPosition = transform.position;
            nowDir = (EnemyDir)(-(int)nowDir);
            transform.localScale = new Vector2((int)nowDir, 1);
        }
        yield return new WaitForSeconds(0.3f);
        //E_body.velocity = Vector2.zero;
        isDamage = false;
    }

    //死亡特效未完成
    void Death()
    {
        Debug.Log("已死亡");
        // E_animator.speed = 0;
        E_animator.SetBool("IsDead", true);
        charaview.GetSoul();
        charaview.GetMoney(10);
        isDeath = true;
        //StartCoroutine(deathover());
    }
    //死亡失活
    void SetUnActive()
    {
        this.gameObject.SetActive(false);
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground") {
            nowDir = (EnemyDir)(-(int)nowDir);
            transform.localScale = new Vector2((int)nowDir,1);
        }
    }
}
