using UnityEngine;
using UnityEngine.SceneManagement;

public class JunkYardEnterance : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void OnTriggerEnter(Collider other)
    {

        //Debug.Log("Something entered: " + other.name);

        if (other.gameObject.CompareTag("Player"))
        {
            if (GameManager.Instance.CheckStatus())
            {
                SceneManager.LoadScene("FirstBossScene(FINAL)");
            }
        }
    }
}
