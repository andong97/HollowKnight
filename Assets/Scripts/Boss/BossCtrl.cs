using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum BossDir {
    Right=1,
    Left=-1
}

public enum AttackType {
    Flash=0,
    FireBall=1,
    Dash=2,
    AttackDown=3,
}

public class BossCtrl : MonoBehaviour {
    private bool isInCamera;
    private bool isDead;
    private BossDir nowDir;
    private  GameObject target;
    private Rigidbody2D boss_body;
    private Animator boss_animator;
    public Transform FireBallPosition;
    public Transform Effect;
    private int hp=10;
    private bool[] state;
    public AudioClip[] au;
    private AudioSource boss_au;

    // Use this for initialization
    void Start () {
        state = new bool[4];
        target = GameObject.Find("Player");
        boss_animator = GetComponent<Animator>();
        boss_body = GetComponent<Rigidbody2D>();
        boss_au = GetComponent<AudioSource>();

    }
	
	// Update is called once per frame
	void Update () {
        if (isDead) {
            return;
        }
        if (GameCtrl.isGamePause || GameCtrl.isOpenBag) {
            return;
        }
        ISinCamera();
        BossAI();
    }

    void BossAI(){
        //判断是否有其他状态
        for (int i = 0; i < state.Length; i++) {
            if (state[i]) {
                return;
            }
        }
        int attacktype= Random.Range(0, 3);
        state[attacktype] = true;
        if (attacktype == 0)
        {
            AttackDown();
        }
        else if (attacktype == 1)
        {
            FireBall();
        }
        else if (attacktype == 2) {
            DashAttack();
        }
    }

    /// <summary>
    /// 受伤
    /// </summary>
    public void onDamage() {
        hp -= 1;
        if (hp <= 0) {
            Dead();
        }
    }

    /// <summary>
    /// 释放火球
    /// </summary>
    void FireBall() {
        boss_animator.SetBool("IsFireBall",true);
        GameObject go = Resources.Load<GameObject>("Prefabs/Boss/BossFireBall");
        Instantiate<GameObject>(go, FireBallPosition.position,Quaternion.identity);
        StartCoroutine(fireball());
    }
    IEnumerator fireball() {
        yield return new WaitForSeconds(0.5f);
        boss_animator.SetBool("IsFireBall", false);
        boss_au.clip = au[1];
        boss_au.Play();
        yield return new WaitForSeconds(3f);
        for (int i = 0; i < state.Length; i++)
        {
            state[i] = false;
        }
    }

    /// <summary>
    /// 瞬移闪现
    /// </summary>
    void FlashMove() {
        boss_animator.SetBool("IsFlash", true);
        boss_au.clip = au[0];
        boss_au.Play();
    }

    /// <summary>
    /// 左右的移动冲击
    /// </summary>
    void DashAttack() {
        StartCoroutine(dashattack());
    }
    IEnumerator dashattack()
    {
            boss_animator.SetTrigger("IsFlash");
            yield return new WaitForSeconds(0.2f);
            transform.position = new Vector2(-10, -2.5f);
            switchDir();
            boss_animator.SetBool("IsDash", true);
            yield return new WaitForSeconds(0.2f);
            boss_body.velocity = new Vector2(10.0f * (int)nowDir, 0);
            yield return new WaitForSeconds(3);
            boss_animator.SetBool("IsDash", false);
            Born();
    }

    /// <summary>
    /// 向下冲击
    /// </summary>
    void AttackDown() {
        state[(int)AttackType.AttackDown] = true;  
        StartCoroutine(attackdown());
    }
    IEnumerator attackdown()
    {
        boss_animator.SetTrigger("IsFlash");
        yield return new WaitForSeconds(0.2f);
        transform.position = new Vector2(target.transform.position.x, target.transform.position.y + 7);
        yield return new WaitForSeconds(0.5f);
        boss_animator.SetBool("IsAttackDown", true);
        yield return new WaitForSeconds(0.5f);
        boss_body.velocity = new Vector2(0, -25);
        yield return new WaitForSeconds(0.1f);
        boss_au.clip = au[2];
        boss_au.Play();
        yield return new WaitForSeconds(1);
        boss_animator.SetBool("IsAttackDown", false);
        Born();
    }

    /// <summary>
    /// 判断方向
    /// </summary>
    void switchDir()
    {
        if (transform.position.x - target.transform.position.x > 0) {
            nowDir = BossDir.Left;
        }
        if (transform.position.x - target.transform.position.x < 0) {
            nowDir = BossDir.Right;
        }
        transform.localScale = new Vector2((int)nowDir, 1);
    }

    /// <summary>
    /// 死亡
    /// </summary>
    void Dead() {
        isDead = true;
        boss_animator.SetBool("IsDash", false);
        StopAllCoroutines();
        boss_animator.SetTrigger("IsDead");
        Destroy(this.GetComponent<Rigidbody2D>());
        Destroy(this.GetComponent<BoxCollider2D>());
        StartCoroutine(victor());
    }
    IEnumerator victor() {
        GameObject v= GameObject.Find("Canvas").transform.Find("Victor").gameObject;
        yield return new WaitForSeconds(1);
        GameCtrl.Victor(v);
    }
    void ISinCamera() {
        if (transform.position.x > Camera.main.transform.position.x + 15)
        {
            isInCamera = false;
        }
        else if (transform.position.x < Camera.main.transform.position.x - 15)
        {
            isInCamera = false;
        }
        else
            isInCamera = true;
    }

    void Born() {
        boss_body.velocity = new Vector2(0, 0);
        transform.position = new Vector2(Random.Range(-10, 10), 1);
        boss_animator.SetTrigger("IsFlashD");
        switchDir();
        StartCoroutine(nextattack());
    }
    IEnumerator nextattack() {
        yield return new WaitForSeconds(3f);
        for (int i = 0; i < state.Length; i++) {
            state[i] = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player") {
            other.gameObject.GetComponent<MoveTest>().OnDamage(new Vector2((int)nowDir,0));
            StartCoroutine(notbox());
        }
    }
    IEnumerator notbox() {
        this.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        yield return new WaitForSeconds(1f);
        this.gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
    }
}
