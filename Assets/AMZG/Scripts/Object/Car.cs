using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Car : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    [SerializeField] public bool isMoving;
    public bool isActive;
    [SerializeField] private PathTray pathTray;
    private int a = 0;

    private void Update()
    {
        if (gameObject.activeSelf)
        {
            MoveCup();
        }    
    }

    public void MoveCup()
    {
        if (isActive && pathTray.isActive)
        {
            isActive = false;
            if (a >= pathTray.listTrays.Count)
            {
                LeanTween.move(gameObject, pathTray.goal.position, 0f).setSpeed(speed).setOnComplete(() =>
                {
                    gameObject.SetActive(false);
                    pathTray.listCars.Remove(this);
                    pathTray.CheckCar();
                });
            }
            else
            {
                LeanTween.move(gameObject, pathTray.listTrays[a].transform.position, 0f).setSpeed(speed).setOnComplete(() =>
                {
                    a++;
                    isActive = true;
                    MoveCup();
                });
            }
        }
    }    
}
