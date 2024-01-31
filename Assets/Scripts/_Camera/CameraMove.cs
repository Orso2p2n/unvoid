using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public float rotationTime;
    public float anglePerRotation;
    public Transform spawner;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            RotateCam(anglePerRotation);
        }
        
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            RotateCam(-anglePerRotation);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveCam(1);
            MoveSpawner(1);
        }
        
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveCam(-1);
            MoveSpawner(-1);
        }
    }

    private void RotateCam(float toRotate)
    {
        Vector3 newRot = transform.rotation.eulerAngles;
        newRot.y += toRotate;

        transform.DORotate(newRot, rotationTime).SetEase(Ease.OutBack);
    }

    private void MoveCam(float toMove)
    {
        Vector3 newPos = transform.position + Vector3.up * toMove;

        transform.DOMove(newPos, rotationTime).SetEase(Ease.OutBack);
    }
    
    private void MoveSpawner(float toMove)
    {
        Vector3 newPos = spawner.position + Vector3.up * toMove;

        spawner.DOMove(newPos, rotationTime).SetEase(Ease.OutBack);
    }
}
