using UnityEngine;

// i'm pretty sure we're using doors you blast open with guns, this is just for class.

public class door : MonoBehaviour
{

    [SerializeField] GameObject doorModel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        IOpen open = other.GetComponent<IOpen>();
        if (open != null && (!other.isTrigger))
        {
            doorModel.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IOpen open = other.GetComponent<IOpen>();
        if (open != null)
        {
            doorModel.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
