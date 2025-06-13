using UnityEngine;
using System.Collections;

public class Destructable : MonoBehaviour, IDamage
{

    [SerializeField] int HP;
    [SerializeField] Renderer model;

    Color colorOrig;

    public void TakeDamage(int amount)
    {

        HP -= amount;

        if (HP <= 0)
        {
            Destroy(gameObject);

        } else
        {

            StartCoroutine(FlashWhite());
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        colorOrig = model.material.color; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator FlashWhite()
    {

        model.material.color = Color.white;
        yield return new WaitForSeconds(0.1f);
        model.material.color = colorOrig;
    }
}
