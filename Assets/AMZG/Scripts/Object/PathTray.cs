using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathTray : MonoBehaviour
{
    public List<Tray> listTrays = new List<Tray>();
    public List<Car> listCars = new List<Car>();
    public bool isActive;

    public Transform goal;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (!isActive /*&& SingleLevelController.Instance.isStart*/)
        {
            CheckPathTray();
            return;
        } 
    }

    public void CheckPathTray()
    {
        for (int i = 0; i < listTrays.Count; i++)
        {
            if (!listTrays[i].isActive) return;

            if (i == listTrays.Count - 1)
            {
                isActive = true;
                SetUp();
            }
        } 
    }

    public void CheckCar()
    {
        if (listCars.Count > 0) return;

        foreach (var tray in listTrays)
        {
            var mat = tray.GetComponent<Renderer>().material;
            LeanTween.value(tray.gameObject, 1f, 0f, 1f)
                .setOnUpdate((float a) =>
                {
                    var c = mat.color;
                    c.a = a;
                    mat.color = c;
                    DragObject3D d = tray.GetComponent<DragObject3D>();
                    d.CheckTray();
                })
                .setOnComplete(() => tray.gameObject.SetActive(false));
        }
    }

    public void SetUp()
    {
        for (int i = 0; i < listCars.Count; i++)
        {
            listCars[i].isActive = true;
        }
    }
}
