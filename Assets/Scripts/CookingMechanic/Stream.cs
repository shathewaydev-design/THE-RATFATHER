using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stream : MonoBehaviour
{
   private LineRenderer lineRenderer = null;
   private ParticleSystem splashParticle = null;
   private Coroutine pourRoutine = null;
   private Vector3 targetPosition = Vector3.zero;
   private void Awake()
   {
        lineRenderer = GetComponent<LineRenderer>();
        splashParticle = GetComponentInChildren<ParticleSystem>();
   }
   private void Start()
   {
        MoveToPosition(0, transform.position);//position of the stream currently
        MoveToPosition(1, transform.position);//position of the stream currently
   }
    public void Begin()
    {
        StartCoroutine(UpdateParticle());
        pourRoutine = StartCoroutine(BeginPour());//store this coroutine later
    }

    private IEnumerator BeginPour()
    {
        while(gameObject.activeSelf)
        {
            targetPosition = FindEndPoint();
            MoveToPosition(0, transform.position);
            AnimateToPosition(1, targetPosition);
            yield return null;
        }
    }
    public void End()
    {
        StopCoroutine(pourRoutine);
        pourRoutine = StartCoroutine(EndPour());
    }
    private IEnumerator EndPour()
    {
        //play splash particle at the end of the stream
        while(!HasReachedPosition(0, targetPosition))//while we has not reachedposition, we animate it
        {//if user stop pouring, we want the stream to animate to the ground and then disappear
            AnimateToPosition(0, targetPosition);
            AnimateToPosition(1, targetPosition);
            yield return null;
        }
        Destroy(gameObject);
    }

    private Vector3 FindEndPoint()//find the ground
    {
        //cast ray downwards to find where the stream should end
        RaycastHit hit;
        Ray ray = new Ray(transform.position, Vector3.down);
        Physics.Raycast(ray, out hit, 10f);
        Vector3 endPoint = hit.collider ? hit.point : ray.GetPoint(10f);
        return endPoint;
    }
    private void MoveToPosition(int index, Vector3 targetPosition)
    {
        lineRenderer.SetPosition(index, targetPosition);
    }
    private void AnimateToPosition(int index, Vector3 targetPosition)
    {
        Vector3 currentPosition = lineRenderer.GetPosition(index);
        Vector3 newPosition = Vector3.MoveTowards(currentPosition, targetPosition, Time.deltaTime * 1.75f);
        lineRenderer.SetPosition(index, newPosition);
    }
    private bool HasReachedPosition(int index, Vector3 targetPosition)//check if it's reached the ground
    {
        Vector3 currentPosition = lineRenderer.GetPosition(index);//does position on line renderer has reached the target position
        return currentPosition == targetPosition;
    }
    private IEnumerator UpdateParticle()
    {
        while(gameObject.activeSelf)
        {
            splashParticle.gameObject.transform.position = targetPosition;
            bool isHitting = HasReachedPosition(1, targetPosition);
            splashParticle.gameObject.SetActive(isHitting);
            
            yield return null;
        }
        
    }
    
}
