using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCameraSaver : MonoBehaviour
{
    private Vector3[] rotationTargets = new Vector3[8];
    private Vector3 currentTarget;
    public float saverSpeed = 5f;
    void Start()
    {
        rotationTargets[0] = new Vector3(350f, 350f, 0);
        rotationTargets[1] = new Vector3(10, 350f, 0);
        rotationTargets[2] = new Vector3(350f, 10, 0);
        rotationTargets[3] = new Vector3(10, 10, 0);
        rotationTargets[4] = new Vector3(0, 10, 0);
        rotationTargets[5] = new Vector3(10, 0, 0);
        rotationTargets[6] = new Vector3(-10, 0, 0);
        rotationTargets[7] = new Vector3(0, -10, 0);
        currentTarget = transform.eulerAngles;
        changeTarget();
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void changeTarget()
    {
        currentTarget = rotationTargets[Random.Range(0, 8)];
        LeanTween.rotateLocal(transform.gameObject, currentTarget, saverSpeed).setEaseInOutBack().setOnComplete(() => changeTarget());

    }
}
