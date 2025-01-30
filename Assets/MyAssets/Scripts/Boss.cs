using DG.Tweening;
using UnityEngine;

public class Boss : MonoBehaviour
{
    private Animator anim;
    public int health;
    [HideInInspector] public bool canWalk;
    private ParticleSystem dieParticle, playerHitParticle;
    public float walkSpeed;
    private Transform cam, player;
    private Animator transitionEffect;
    private GameObject winPanel, retryPanel;
    private int collectedItemsCount;

    private void Awake()
    {
        winPanel = FindObjectOfType<BossManager>().winPanel;
        retryPanel = FindObjectOfType<BossManager>().retryPanel;

        anim = GetComponent<Animator>();
        dieParticle = GetComponentInChildren<ParticleSystem>();
        playerHitParticle = GameObject.Find("PlayerHitParticle").GetComponent<ParticleSystem>();
        cam = FindObjectOfType<Camera>().transform;
        player = GameObject.Find("Player").transform;
        transitionEffect = GameObject.Find("Transition").GetComponent<Animator>();
        collectedItemsCount = ItemsManager.Instance.collectedItemsName.Count;
    }

    private void OnTriggerEnter(Collider collision)
    {
        print(collision.gameObject);
        Destroy(collision.gameObject);
        anim.SetTrigger("TakeDamage");
    }

    private void Win()
    {
        winPanel.transform.DOScale(new Vector3(1, 1, 1), .5f);
        winPanel.SetActive(true);
        AudioManager.Instance.Play("Win");
    }

    private void Retry()
    {
        retryPanel.transform.DOScale(new Vector3(1, 1, 1), .5f);
        retryPanel.SetActive(true);
        AudioManager.Instance.Play("Retry");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            anim.SetTrigger("TakeDamage");
        }

        if (canWalk)
        {
            transform.parent.Translate(Vector3.forward * walkSpeed * Time.deltaTime);
        }
    }

    public void Walk()
    {
        anim.SetBool("Walk", true);

        canWalk = true;
        Invoke(nameof(StopWalk), 3);
    }

    private void StopWalk()
    {
        canWalk = false;
    }

    //Dead
    public void AttackDone()
    {
        AudioManager.Instance.Play("BossHit");
        playerHitParticle.Play();

        cam.eulerAngles = new Vector3(cam.eulerAngles.x, 0, -24);
        player.localScale = new Vector3(player.localScale.x, 0.19f, player.localScale.z);
        Destroy(player.GetComponentInChildren<Animator>());

        transitionEffect.enabled = true;
        Invoke(nameof(Retry), 1);
    }

    public void CheckBossIsKilled()
    {
        if (collectedItemsCount >= health)
        {
            anim.SetBool("Die", true);
            AudioManager.Instance.Play("Die");
            dieParticle.Play();
            Invoke(nameof(Win), 5);
        }
        else
            Walk();
    }
}