using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamage
{
    [SerializeField] int HP;
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;

    [SerializeField] int FOV;
    [SerializeField] int turnSpeed;
   // [SerializeField] int speed;
   
    

    Color colorOrig;
    
  

   /* private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerinrange = true;
        }
    }*/

    Vector3 playerDirection;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        colorOrig = model.material.color;
        GameManager.instance.UpdateGameGoal(1);
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(GameManager.instance.player.transform.position);
    }


    

    /*bool canSeePlayer()
    {
        playerDirection = GameManager.instance.player.transform.position - headLevel.position;
        angleToPlayer = Vector3.Angle(playerDirection, transform.forward);
        RaycastHit hit;
        Debug.DrawRay(headLevel.position, playerDirection);

        if (Physics.Raycast(headLevel.position, playerDirection, out hit))
        {

            if (angleToPlayer < FOV && hit.collider.CompareTag("Player"))
            {
                agent.SetDestination(GameManager.instance.player.transform.position);
                Debug.Log("canSeePlayer");
                return true;
            }
            
        }
        Debug.Log("blind");
        return false;
    }*/
    public void TakeDamage(int amount)
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

   

}
