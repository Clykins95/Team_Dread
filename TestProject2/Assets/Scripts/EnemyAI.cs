using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamage
{
    [SerializeField] float HP;
    [SerializeField] int turnSpeed;
    [SerializeField] Renderer model;
    Color colorOrig;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator anim;
    float animSpeedTrans = 1;  
    Vector3 playerDirection;

    Collider swordCol;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        colorOrig = model.material.color;
        GameManager.instance.UpdateGameGoal(1);
    }

    // Update is called once per frame
    void Update()
    {
        // i told ya i'd remove it
        agent.SetDestination(GameManager.instance.player.transform.position);

        // still being funky but i'll get it eventually
        float agentSpeedCur = agent.velocity.normalized.magnitude;
        float animSpeedCur = anim.GetFloat("Speed");
        anim.SetFloat("Speed", Mathf.Lerp(animSpeedCur, agentSpeedCur, Time.deltaTime * animSpeedTrans));
    }

    public void TakeDamage(float amount)
    {
        HP -= amount;

        if (HP <= 0)
        {
            Destroy(gameObject);
            GameManager.instance.UpdateGameGoal(-1);
        }
        else
        {

            StartCoroutine(FlashRed());
        }
    }

    IEnumerator FlashRed()
    {

        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = colorOrig;
    }

    void faceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDirection.x, 0, playerDirection.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * turnSpeed);
    }

    void setAnimations()
    {
        float agentSpeedCur = agent.velocity.normalized.magnitude;
        float animSpeedCur = anim.GetFloat("Speed");

        anim.SetFloat("Speed", Mathf.Lerp(animSpeedCur, agentSpeedCur, Time.deltaTime * animSpeedTrans));
    }

    // no. -_-
    public void swordColOn()
    {
        if (swordCol)
        {
            swordCol.enabled = true;
        }
    }
    public void swordColOff()
    {
        if (swordCol)
        {
            swordCol.enabled = false;
        }
    }
}
