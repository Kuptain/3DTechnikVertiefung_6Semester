using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnPlayer : MonoBehaviour
{
    public Text spaceText;
    public GameObject playerObject;
    public GameObject previewCamera;
    public GameObject planetObject;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(BlinkText());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            spaceText.gameObject.SetActive(false);
            previewCamera.SetActive(false);
            playerObject.SetActive(true);
            planetObject.GetComponent<Animator>().enabled = false;
        }
    }
    IEnumerator BlinkText()
    {
        spaceText.enabled = false;
        yield return new WaitForSeconds(0.3f);
        spaceText.enabled = true;
        yield return new WaitForSeconds(0.6f);
        if (spaceText.gameObject.activeSelf)
        {
            StartCoroutine(BlinkText());
        }      

    }
}
