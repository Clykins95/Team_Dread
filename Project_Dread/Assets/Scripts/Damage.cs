using UnityEngine;
using System.Collections;

public class Damage : MonoBehaviour
{

    enum DamageType {moving, stationary, DOT, homing}

    [SerializeField] DamageType type;
    [SerializeField] Rigidbody rb;
    [SerializeField] int damageAmt;
    [SerializeField] float damageRate;
    [SerializeField] int speed;
    [SerializeField] int destroyTime;
    

    bool isDamaging;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (type == DamageType.moving || type == DamageType.homing)
        {
            Destroy(gameObject, destroyTime);

            if (type == DamageType.moving)
            {
                rb.linearVelocity = transform.forward * speed;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (type == DamageType.homing)
        {
            rb.linearVelocity = (GameManager.instance.player.transform.position - transform.position).normalized * speed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
       
        if (other.isTrigger)
            return;
        
        IDamage dmg = other.GetComponent<IDamage>();

        if (dmg != null && type != DamageType.DOT)
        {
            dmg.TakeDamage(damageAmt);
        }

        if (type == DamageType.homing || type == DamageType.moving)
        {
            Destroy(gameObject);
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.isTrigger)
            return;
        
        IDamage dmg = other.GetComponent<IDamage>();

        if (dmg != null && type == DamageType.DOT && !isDamaging)
        {
            StartCoroutine(DamageOther(dmg));
        }
    }

    IEnumerator DamageOther(IDamage d)
    {

        isDamaging = true;
        d.TakeDamage(damageAmt);
        yield return new WaitForSeconds(damageRate);
        isDamaging = false;

    }
}
