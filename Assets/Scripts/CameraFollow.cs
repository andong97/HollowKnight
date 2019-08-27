using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraFollow : MonoBehaviour {
    public Transform target;
    private Vector3 cameraPosition;
    private float shakeDetal=0.5f;
    private bool isShake;
    private int shakenum = 20;
    public int ylimit;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

    void LateUpdate()
    {
        if (isShake) {
            Cameshake();
            return;
        }
        cameraPosition = target.position;
        cameraPosition = new Vector3(Mathf.Clamp(cameraPosition.x, 0, ylimit), Mathf.Clamp(cameraPosition.y, 0f, ylimit), -10);
        this.transform.position = cameraPosition;
        
    }
    public  void Cameshake() {
        isShake = true;
        shakenum -= 1;
        if (shakenum > 0)
        {
            this.transform.DOMove(new Vector3(
                transform.position.x + Random.Range(-0.1f, 0.1f),
                transform.position.y + Random.Range(-0.1f, 0.1f),
                -10
                ), 0.1f);
            if (shakenum == 1) {
                this.transform.DOMove(cameraPosition, 0.1f);
            }
        }
        else
        {
            isShake = false;
            shakenum = 20;
        }
    }
}
