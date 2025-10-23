/*using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Car : MonoBehaviour
{
    [SerializeField] public int idCar;
    [SerializeField] public List<Material> materialCar;
    [SerializeField] public bool isChoose = false;
    //[SerializeField] public bool isCheck = true;
    [SerializeField] public bool isChecking = false;
    [SerializeField] public bool isMoving;
    [SerializeField] public bool isTest;
    [SerializeField] private float vectorABY;

    private void Start()
    {
        isMoving = false;
    }

    private void Update()
    {
        if (isTest && !isMoving && SingleLevelController.Instance.varCountCup == 0 && SingleLevelController.Instance.isHaveNewTray)
        {
            isTest = false;
            CheckCupNew();
        }
    }

    public void CheckCountCup()
    {
        b = SingleLevelController.Instance.cups.Count;
    }

    public int a = 0;
    public int b = 0;

    public void MoveCup()
    {
        if (b == 0) return;
        for (int i = 0; i < SingleLevelController.Instance.listTransformCup.Count; i++)
        {
            if (transform.position == SingleLevelController.Instance.listTransformCup[i].position)
            {
                a = i;
                break;
            }
        }
        if (isChoose)
        {
            if (transform.position == SingleLevelController.Instance.listTransformCup[0].position)
            {
                for (int i = 0; i < SingleLevelController.Instance.listTrayOnQueue.Count; i++)
                {
                    if (idCup == SingleLevelController.Instance.listTrayOnQueue[i].idTray)
                    {
                        Tray tray = SingleLevelController.Instance.listTrayOnQueue[i];
                        tray.numberCupOnTrayInGame++;
                        int b = tray.numberCupOnTrayInGame;
                        if (b == tray.numberCupOnTray)
                        {
                            SingleLevelController.Instance.listTrayOnQueue.Remove(tray);
                        }
                        Vector3 pointA = tray.listTransformCupInTray[b - 1].position;
                        Vector3 pointB = transform.position;
                        Vector3 midPoint = new Vector3((pointA.x + pointB.x) / 2, (pointA.y + pointB.y) / 2, (pointA.z + pointB.z) / 2);
                        Vector3 pointC = new Vector3((midPoint.x + pointB.x) / 2, vectorABY, (midPoint.z + pointB.z) / 2);
                        Vector3 pointD = new Vector3((midPoint.x + pointA.x) / 2, vectorABY, (midPoint.z + pointA.z) / 2);
                        Vector3[] movePath = new Vector3[] {
                         pointB,
                         pointD,
                         pointC,
                         pointA,
                            };
                        transform.SetParent(tray.transform);
                        isMoving = true;
                        SingleLevelController.Instance.varCountCup--;
                        LeanTween.move(gameObject, movePath, 0.2f)
                            .setEase(LeanTweenType.animationCurve)
                            .setSpeed(10f).setEase(LeanTweenType.easeOutQuad).setOnComplete(() =>
                            {
                                LeanTween.scale(gameObject, new Vector3(0.8f, 0.8f, 0.8f), 0f);
                                BlinkTrayUnlock(tray);
                                //SingleLevelController.Instance.listCup.Remove(this);
                                //SingleLevelController.Instance.cups.Remove(this);
                                *//*if (SingleLevelController.Instance.cups.Count == 0)
                                {
                                    Debug.Log("cups.Count == 0");
                                    SingleLevelController.Instance.isRunning = false;
                                    SingleLevelController.Instance.CheckListCupWhenHaveTray();
                                }*//*
                                SingleLevelController.Instance.totalCup--;
                                GameUIController.Instance.UpdateTextTotalCup(SingleLevelController.Instance.totalCup);
                                //Debug.Log(tray.numberTest);
                                transform.position = tray.listTransformCupInTray[tray.numberTest].position;
                                tray.numberTest++;
                                if (tray.numberTest == tray.numberCupOnTray)
                                {
                                    SingleLevelController.Instance.CheckTrayOnQueue(tray);
                                }
                            });
                        break;
                    }
                }
            }
            else
            {
                if (a <= 0) return;
                MoveToPosition(SingleLevelController.Instance.listTransformCup[a - 1].position);
            }
        }
        else
        {
            if (a <= 0) return;
            MoveToPosition(SingleLevelController.Instance.listTransformCup[a - 1].position);
        }
    }

    private void CheckCupNew()
    {
        if (SingleLevelController.Instance.isHaveNewTray)
        {
            SingleLevelController.Instance.isHaveNewTray = false;
            SingleLevelController.Instance.isRunning = false;
            SingleLevelController.Instance.CheckListCup();
            SingleLevelController.Instance.MoveListCups();
        }
    }

    private float speedScale = 0.15f;
    private void BlinkTrayUnlock(Tray tray)
    {
        StartCoroutine(ShrinkAndReturn(tray));
    }
    IEnumerator ShrinkAndReturn(Tray tray)
    {
        Vector3 v = new Vector3(52f, 52f, 52f);
        Vector3 vt = new Vector3(45f, 45f, 45f);
        yield return StartCoroutine(ScaleTo(v, tray));
        yield return StartCoroutine(ScaleTo(vt, tray));
    }

    IEnumerator ScaleTo(Vector3 target, Tray tray)
    {
        Vector3 start = tray.transform.localScale;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / speedScale;
            tray.transform.localScale = Vector3.Lerp(start, target, t);
            yield return null;
        }
    }

    private void MoveToPosition(Vector3 targetPosition)
    {
        isMoving = true;
        LeanTween.move(gameObject, targetPosition, 0.1f)
                 //.setEase(LeanTweenType.animationCurve)
                 //.setSpeed(10f)
                 .setEase(LeanTweenType.easeOutQuad).setOnComplete(() =>
                 {
                     isMoving = false;
                     a--;
                     b--;
                     MoveCup();
                 });
    }
}
*/