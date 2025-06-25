using UnityEngine;
using System.Collections;

public class checkpoint : MonoBehaviour
{

    [SerializeField] Renderer model;

    Color colorOrig;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        colorOrig = model.material.color;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && GameManager.instance.player.transform.position != transform.position) {
            GameManager.instance.playerSpawn.transform.position = transform.position;
            StartCoroutine(checkpointFeedback());
        }
    }

    IEnumerator checkpointFeedback()
    {
        model.material.color = colorOrig;
        GameManager.instance.checkpointPopup.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        GameManager.instance.checkpointPopup.SetActive(false);
        model.material.color = Color.green;
    }
}
