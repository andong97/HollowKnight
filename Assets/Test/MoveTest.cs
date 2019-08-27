using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using UnityEngine.SceneManagement;

public enum playDir {
    Right=1,
    Left=-1
}
public enum AttackDir {
    Right=1,
    Left=-1,
    Up=3,
    Down=4
}
public enum ItemIDType {
    isDoubleJump=1,
    isFlash=0,
    iSAttakBack=2,
    isfireball=3
}

public class MoveTest : MonoBehaviour {
    private Vector3 LastJumpPosition;//记录最后一次跳跃位置
    private Animator P_animator;//获取动画状态机
    private Rigidbody2D P_body;//获取刚体
    private bool isGround;//判断是否在地面
    private bool isJumpState;//判断空中二段跳
    private float movement;
    public bool isFlash;//是否冲刺中
    private bool canFlash;//是否可以冲刺
    private bool isInput;//键盘是否允许输入
    private float jumpTime;//最长跳跃按键时间
    private bool isClimb;//是否处于爬墙状态
    private bool isTouchWall;//是否碰到墙壁
    private bool isDamage= false;//是否受伤
    private bool isDead;
    private GameObject HitEffect;//攻击到敌人的效果
    private AudioSource HitAU;//攻击的音效
    private bool isCollider;//是否发生碰撞
    public Transform FireBallPosition;//火球的发生位置
    public CharactorCtrl charaview;
    public int Play_hp=5;
    private bool[] ItemisEquip = new bool[24];
    private Vector3 Deadposition;
    public ItemEquip ItemEquipView;
    public AudioClip[] au;
    private AudioSource p_au;
    public Transform DashEffectPos;

    bool isattack = true;
    private GameObject attackEffect;//刀光特效
    GameObject attackEffectDir;
    float jumpCount = 0;
    bool isDoubleJump;


    public Transform groundpoint1;//设置检测对象1
    public Transform groundpoint2;//设置检测对象2

    public float rayDis;//射线检测的距离

    public playDir nowDir;//玩家方向
    public AttackDir attackDir;//攻击方向

    [Header("跳跃最短距离")]
    public float jumpHight;
    [Header("玩家的冲刺距离")]
    public float FlashDis;
    [Header("玩家的移动速度")]
    public float speed;
    // [Header("玩家重力控制")]
    //public float gravity;

    void Start() {
        ItemEquipView = GameObject.Find("Canvas").transform.Find("Bag").Find("EquipItem").GetComponent<ItemEquip>();
        charaview = GameObject.Find("Canvas").transform.Find("Charactor").GetComponent<CharactorCtrl>();
        P_animator = GetComponent<Animator>();
        P_body = GetComponent<Rigidbody2D>();
        p_au = GetComponent<AudioSource>();
        HitAU = transform.Find("AttackEffect").GetComponent<AudioSource>();
        movement = 0;
        jumpHight = 10.0f;
        speed = 5.0f;
        FlashDis = 8.0f;
        isFlash = false;
        isInput = true;
        nowDir = playDir.Right;
        attackEffect = transform.Find("AttackEffect").gameObject;
        ReadCharaterItem();
    }

    void Update() {
        if (GameCtrl.isGamePause) {
            return;
        }
        OpenBag();
        if (GameCtrl.isOpenBag) {
            return;
        }
        if (isDead) {
            transform.position = Deadposition;
            return;
        }
        Move();
        Jumping();
        Flash();
        //Climb();
        FlashZeroG();
        Attack();
        StateMachineBehaviour();
        skillFireBall();
        GetHp();
    }
    void OpenBag() {
        if (Input.GetKeyDown(KeyCode.B) && !GameCtrl.isOpenBag)
        {
            if (!PlayerPrefs.HasKey("CharacterItem"))
            {
                return;
            }
            string savedata = PlayerPrefs.GetString("BuyenItem");
            List<ItemData> SaveItem = JsonMapper.ToObject<List<ItemData>>(savedata);
            if (SaveItem.Count==0) {
                return;
            }          
            GameCtrl.isOpenBag = true;
            GameObject.Find("UI").transform.Find("Canvas/Bag").gameObject.SetActive(true);                        
            ReadCharaterItem();
            string equipdata = PlayerPrefs.GetString("CharacterItem");
            List<ItemData> characterItem = JsonMapper.ToObject<List<ItemData>>(equipdata);
            ItemEquipView.DisPlay(characterItem);
        }
        else if (Input.GetKeyDown(KeyCode.B) && GameCtrl.isOpenBag)
        {
            GameCtrl.isOpenBag = false;
            GameObject.Find("UI").transform.Find("Canvas/Bag").gameObject.SetActive(false);
            ReadCharaterItem();
        }
    }
    /// <summary>
    /// 人物移动控制
    /// </summary>
    void Move() {     
        if (!isInput) {
            return;
        }
        movement = Input.GetAxis("Horizontal");
        if (isCollider)
        {
            movement = 0;
        }
        if (movement > 0f)
        {
            P_body.velocity = new Vector2(movement * speed, P_body.velocity.y);
            transform.localScale = new Vector2(1, 1);
            nowDir = playDir.Right;
            
        }
        else if (movement < 0f) {
            P_body.velocity = new Vector2(movement * speed, P_body.velocity.y);
            transform.localScale = new Vector2(-1, 1);
            nowDir = playDir.Left;
            //p_au.clip = au[2];
            //p_au.Play();
        }
        else
        {
            P_body.velocity = new Vector2(0, P_body.velocity.y);
        }
    }

    /// <summary>
    /// 人物的跳跃
    /// </summary>
    void Jumping() {
        isGround = (Physics2D.OverlapCircle(groundpoint1.position, 0.004f, 1 << LayerMask.NameToLayer("Ground"))||
           Physics2D.OverlapCircle(groundpoint2.position, 0.004f, 1 << LayerMask.NameToLayer("Ground"))
           );
        if (isGround) {
            canFlash = true;
            isDoubleJump = false;
            isCollider = false;
        }
        //isGround=Physics2D.Raycast(transform.position,Vector2.down, 0.665f, 1 << LayerMask.NameToLayer("Ground"));
        if (!isInput)
        {
            return;
        }
        //如果不在爬墙的状态
        if (!isClimb)
        {
            if (isJumpState)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    P_body.velocity = new Vector2(P_body.velocity.x, jumpHight);
                    isJumpState = false;
                    isDoubleJump = true;
                    jumpCount = 2;
                }
            }
            if ((Input.GetKeyDown(KeyCode.Space) && isGround))
            {
                jumpCount += 1;
                P_body.velocity = new Vector2(P_body.velocity.x, jumpHight);
                isJumpState = true;
                LastJumpPosition = transform.position;
            }
            if (!ItemisEquip[(int)ItemIDType.isDoubleJump])
            {
                isJumpState = false;
            }
        }
        //如果处于爬墙的状态
        if (isClimb) {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(climbjumpmove());
            }
            
        }

        //跳跃动画的改变
        if (jumpCount == 2) {
            P_animator.SetBool("IsDoubleJump", true);
            //isClimb = true;
            jumpCount = 0;
        }
        else
            P_animator.SetBool("IsDoubleJump", false);
    }

    /// <summary>
    /// 爬墙跳跃移动
    /// </summary>
    /// <returns></returns>
    IEnumerator climbjumpmove() {

        jumpCount = 0;
        P_animator.SetBool("IsClimb", false);
        isClimb = false;

        isInput = false;
        jumpCount += 1;
        isJumpState = true;
        if (nowDir == playDir.Left)
        {
            P_body.velocity = new Vector2(8.0f, jumpHight);
        }
        if (nowDir == playDir.Right)
        {
            P_body.velocity = new Vector2(-8.0f, jumpHight);
        }
        yield return new WaitForSeconds(0.15f);
        isInput = true;
    }

    /// <summary>
    /// 冲刺
    /// </summary>
    void Flash() {
        if (!ItemisEquip[(int)ItemIDType.isFlash]) {
            return;
        }
        if (Input.GetKeyDown(KeyCode.L)&&canFlash) {
            isFlash = true;
            canFlash = false;
            isInput = false;//锁住键盘输入
            if (nowDir == playDir.Right)
            {

                if (isClimb) {
                    P_body.velocity = new Vector2(-FlashDis, 0);
                    transform.localScale = new Vector2(-1, 1);
                }
                else
                    P_body.velocity = new Vector2(FlashDis, 0);
            }
            else if (nowDir == playDir.Left) {

                if (isClimb) {
                    P_body.velocity = new Vector2(FlashDis, 0);
                    transform.localScale = new Vector2(1, 1);
                }
                else
                    P_body.velocity = new Vector2(-FlashDis, 0);
            }
            P_animator.SetBool("IsFlash",true);

        }
    }

    /// <summary>
    /// 冲刺开始帧方法
    /// </summary>
    void FlashZeroG() {
        if (isFlash && !isGround) {
            P_body.gravityScale = 0;
            
        }

    }

    /// <summary>
    /// 冲刺结束帧方法
    /// </summary>
    void FlashFinsh() {
        P_body.velocity = Vector2.zero;
        isInput = true;
        isFlash = false;
        P_animator.SetBool("IsFlash", false);
        P_body.gravityScale = 2;

    }

    /// <summary>
    /// 攻击方法
    /// </summary>
    void Attack() {
        if (!isInput||isClimb) {
            return;
        }
        //不在空中的攻击
        if (Input.GetKeyDown(KeyCode.K)&&isGround)
        {
            if (Input.GetKey(KeyCode.W))
            {
                StartCoroutine(AttackLock(AttackDir.Up));
                return;
            }
            if (nowDir == playDir.Left) {
                StartCoroutine(AttackLock(AttackDir.Left));
            }
            if (nowDir == playDir.Right) {
                StartCoroutine(AttackLock(AttackDir.Right));
            }                      
        }
        //在空中的攻击方法
        if (Input.GetKeyDown(KeyCode.K) &&(isDoubleJump||!isGround))
        {
            if (Input.GetKey(KeyCode.W))
            {
                StartCoroutine(AttackLock(AttackDir.Up));
                return;
            }
            if (Input.GetKey(KeyCode.S))
            {
                Debug.Log(isDoubleJump);
                Debug.Log(isJumpState);
                Debug.Log("down");
                StartCoroutine(AttackLock(AttackDir.Down));
                return;
            }
            if (nowDir == playDir.Left)
            {
                StartCoroutine(AttackLock(AttackDir.Left));
            }
            if (nowDir == playDir.Right)
            {
                StartCoroutine(AttackLock(AttackDir.Right));
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param 攻击方向="dir"></param>
    /// <returns></returns>
    IEnumerator AttackLock(AttackDir dir) {
        attackEffectDir = null;
        isInput = false;
        switch (dir)
        {
            case AttackDir.Up:
                P_animator.SetTrigger("AttackUp");
                attackDir = AttackDir.Up;
                attackEffectDir = attackEffect.transform.Find("AttackUp").gameObject;
                AttackCheck(3);
                break;
            case AttackDir.Right:
                P_animator.SetTrigger("AttackRight");
                attackDir = AttackDir.Right;
                attackEffectDir = attackEffect.transform.Find("AttackLeft").gameObject;
                AttackCheck(1);
                break;
            case AttackDir.Left:
                P_animator.SetTrigger("AttackLeft");
                attackDir = AttackDir.Left;
                attackEffectDir = attackEffect.transform.Find("AttackLeft").gameObject;
                AttackCheck(-1);
                break;
            case AttackDir.Down:
                P_animator.SetTrigger("AttackDown");
                if (!isGround) {
                    attackEffectDir = attackEffect.transform.Find("AttackDown").gameObject;
                    attackDir = AttackDir.Down;
                    AttackCheck(4);
                }              
                break;
        }
        if (attackEffectDir) {
            attackEffectDir.SetActive(true);
        }
        yield return new WaitForSeconds(0.15f);
        if (attackEffectDir) {
            attackEffectDir.SetActive(false);
        }
        isInput = true;
    }

    void StateMachineBehaviour() {
        P_animator.SetFloat("Runing",Mathf.Abs(movement));
        P_animator.SetBool("IsJump", !isGround);

    }

    /// <summary>
    /// 锁住键盘
    /// </summary>
    void LockInput()
    {
        isInput = false;
    }

    /// <summary>
    /// 解锁键盘
    /// </summary>
    void UnLockInput()
    {
        isInput = true;
    }

    /// <summary>
    /// 攻击检测
    /// </summary>
    /// <param name="atkDir">攻击距离</param>
    /// 3向上，4向下，1向右，-1 向左
    void AttackCheck(int atkDir) {
        float attackDis = 1.8f;
        RaycastHit2D hit2d = new RaycastHit2D();
        switch (atkDir) {
            case 1:
                hit2d = Physics2D.Raycast(transform.position,Vector2.right,attackDis,1 << LayerMask.NameToLayer("Enemy"));
                break;
            case -1:
                hit2d = Physics2D.Raycast(transform.position, Vector2.left, attackDis, 1 << LayerMask.NameToLayer("Enemy"));
                break;
            case 3:
                hit2d = Physics2D.Raycast(transform.position, Vector2.up, attackDis, 1 << LayerMask.NameToLayer("Enemy"));
                break;
            case 4:
                hit2d = Physics2D.Raycast(transform.position, Vector2.down, attackDis, 1 << LayerMask.NameToLayer("Enemy"));
                break;
        }

        if (hit2d.collider != null) {
            if (atkDir == -1) {//向左攻击，人往右退
                StartCoroutine(AttackBack(2.0f,0));
            }
            if (atkDir == 1) {//向右攻击，人往左退
                StartCoroutine(AttackBack(-2.0f,0));
            }
            if (atkDir == 3){//向上攻击，不动
                StartCoroutine(AttackBack(0f,0f));
            }
            if (atkDir == 4) {//向下攻击，向上弹起
                StartCoroutine(AttackBack(0f,10.0f));
            }
        }
    }
    /// <summary>
    ///攻击后退效果，当前缺少特效未完成
    /// </summary>
    /// <param name="Hdis">水平阻力</param>
    /// <param name="Vdis">竖直阻力</param>
    /// <returns></returns>
    IEnumerator AttackBack(float Hdis,float Vdis) {
        P_body.velocity = new Vector2(Hdis, Vdis);
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>().Cameshake();
        GameObject go = Resources.Load<GameObject>("Prefabs/Effect/AtkEffect");
        HitAU.Play();
        GameObject HitEffect = Instantiate<GameObject>(go, attackEffectDir.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.2f);
        Destroy(HitEffect);
    }

    /// <summary>
    /// 受伤方法
    /// </summary>
    public void OnDamage(Vector2 dir) {
        if (!isDamage&&!isDead) {
            charaview.Damage();
            StartCoroutine(onDamageing(dir));
            Play_hp -= 1;
            if (Play_hp == 0) {
                Dead();
            }
        }
    }

    IEnumerator onDamageing(Vector2 dir) {
        isDamage = true;
        isInput = false;
        if ((int)nowDir == dir.x)
        {
            transform.localScale = new Vector2(-(int)nowDir, 1);
            P_body.velocity = new Vector2((int)nowDir * 8f, 3f);
        }
        else {
            P_body.velocity = new Vector2(-(int)nowDir * 8f, 3f);
        }
        P_animator.SetBool("IsDamage",true);
        yield return new WaitForSeconds(0.2f);
        P_animator.SetBool("IsDamage", false);
        isInput = true;
        yield return new WaitForSeconds(0.5f);
        isDamage = false;
    }

    void Dead() {
        isDead = true;
        Deadposition = transform.position;
        StartCoroutine(dead());
        Destroy(this.gameObject.GetComponent<BoxCollider2D>());
    }
    IEnumerator dead() {
        P_animator.SetBool("IsDead", true);
        P_body.velocity = new Vector2(-(int)nowDir * 8f, 3f); 
        yield return new WaitForSeconds(0.5f);
        P_animator.SetBool("IsDead", false);
        P_body.velocity = Vector2.zero;
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(0);
    }
    /// <summary>
    /// 碰撞爬墙
    /// </summary>
    /// <param name="other"></param>
    void OnCollisionEnter2D(Collision2D other)
    {
        isCollider = true;
        if (other.gameObject.tag == "Ground" && (other.contacts[0].normal == Vector2.right || other.contacts[0].normal == Vector2.left))
        {
            if (isGround)
            {
                return;
            }
            P_animator.SetBool("IsClimb", true);
            jumpCount = 0;
            isClimb = true;
        }
        if (other.gameObject.tag == "Enemy"|| other.gameObject.tag=="Enemy2"|| other.gameObject.tag == "Boss")
        {
            OnDamage(other.contacts[0].normal);
        }
        if (other.gameObject.tag == "Spike") {
            OnDamage(new Vector2(-(int)nowDir,3));
            StartCoroutine(waitspike());
        }
        if (other.gameObject.tag == "Flag") {
            SceneManager.LoadScene(2);
        }
    }
    IEnumerator waitspike() {
        yield return new WaitForSeconds(0.3f);
        transform.position = LastJumpPosition;
    }
    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Enemy2"|| other.gameObject.tag == "Boss")
        {
            OnDamage(other.contacts[0].normal);
        }
    }
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground") {
            jumpCount = 0;
            P_animator.SetBool("IsClimb", false);
            isClimb = false;
        }
        isCollider = false;
    }

    void skillFireBall() {
        if (!ItemisEquip[(int)ItemIDType.isfireball]) {
            return;
        }
        if (Input.GetKeyDown(KeyCode.O)) {
            if (charaview.soulnum < 0) {
                return;
            }
            if (isGround) {
                charaview.CostSoul();
                StartCoroutine(fireball());
            }
        }
    }
    IEnumerator fireball()
    {
        isInput = false;
        movement = 0;
        p_au.clip = au[0];
        p_au.Play();
        P_body.velocity = Vector2.zero;
        GameObject go = Resources.Load<GameObject>("Prefabs/FireBall");
        GameObject fireball = Instantiate(go, FireBallPosition.position, Quaternion.identity);
        fireball.GetComponent<FireBall>().AttackDir = nowDir;
        P_animator.SetTrigger("IsSkillAttack");
        yield return new WaitForSeconds(0.2f);
        isInput = true;
    }

    void GetHp() {
        if (!isGround) {
            return;
        }
        if (charaview.soulnum < 0)
        {
            return;
        }
        if (Play_hp > 5) {
            return;
        }
        if (Input.GetKeyDown(KeyCode.V)) {
            isInput = false;
            P_animator.SetBool("IsGetHp", true);
        }
        if (Input.GetKeyUp(KeyCode.V)) {
            isInput = true;
            P_animator.SetBool("IsGetHp", false);
            charaview.GetHp();
            Play_hp += 1;
        }
    }

    void ReadCharaterItem() {
        for (int i = 0; i < 24; i++) {
            ItemisEquip[i] = false;
        }
        if (!PlayerPrefs.HasKey("CharacterItem"))
        {
            return;
        }
        string data = PlayerPrefs.GetString("CharacterItem");
        List<ItemData> characterItem = JsonMapper.ToObject<List<ItemData>>(data);
        for (int i = 0; i < characterItem.Count; i++) {
            switch (characterItem[i].ItemID) {
                case 1:
                    ItemisEquip[0] = true;
                    break;
                case 2:
                    ItemisEquip[1] = true;
                    break;
                case 3:
                    ItemisEquip[2] = true;
                    break;
                case 4:
                    ItemisEquip[3] = true;
                    break;
                default:
                    break;
            }
        }
    }
}
