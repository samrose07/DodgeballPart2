using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenScript : MonoBehaviour
{
    [SerializeField] private GameObject endLoc;
    [SerializeField] private float tweenTime;
    [SerializeField] private int timer = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        if(gameObject.name != "Ball")
        {
            Tween();
        }
        
        tweenTime = Random.Range(3, 10);
    }

    // Update is called once per frame
    private void Update()
    {
        float distance = Vector3.Distance(gameObject.transform.position, endLoc.transform.position);
        if (!LeanTween.isTweening() && gameObject.name != "Ball")
        {
            if (distance > 1) Tween();
        }
        
        
    }
    public void Tween()
    {
        LeanTween.cancel(gameObject);
        if (gameObject.name != "Ball") LeanTween.moveLocal(gameObject, endLoc.transform.position, 10f).setEaseInOutExpo();
        else LeanTween.moveLocal(gameObject, endLoc.transform.position, 1f);

    }
}
