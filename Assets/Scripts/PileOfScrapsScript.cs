using UnityEngine;

public class PileOfScrapsScript : MonoBehaviour
{

    //public GameObject scrapReplacement;

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
        if (other.gameObject.CompareTag("SoldatoBoss"))
        {
            if (SoldatoScript.Instance.currentState == SoldatoScript.State.ChargeDash)
            {
                // apply damage to boss
                SoldatoScript.Instance.TakeDamage();


                // destroy or replace game object with smaller pile of scraps
                //Instantiate(scrapReplacement, gameObject.transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Hasn't charged into it!");
            }













            //SoldatoScript.Instance.TakeDamage();


            //Instantiate(scrapReplacement, transform.position, Quaternion.identity);
            //Destroy(gameObject);


        }
    }

}
