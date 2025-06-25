using UnityEngine;
using System.Collections;
using Unity.AI.Navigation;

public class Destructable : MonoBehaviour, IDamage
{

    [SerializeField] float HP;
    [SerializeField] Renderer model;

    Color colorOrig;

    public void TakeDamage(float amount)
    {

        HP -= amount;

        if (HP <= 0)
        {
            //NavMeshSurface
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
