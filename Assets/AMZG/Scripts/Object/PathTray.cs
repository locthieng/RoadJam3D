using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathTray : MonoBehaviour
{
    public List<Tray> listTrays = new List<Tray>();
    public List<Car> listCars = new List<Car>();
    public bool isActive;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive)
        {
            CheckPathTray();
            return;
        } 
    }

    public void CheckPathTray()
    {
        for (int i = 0; i < listTrays.Count; i++)
        {
            if (listTrays[i].transform.position != listTrays[i].truePosition)
            {
                return;
            }    

            if (i == listTrays.Count - 1)
            {
                isActive = true;
            }
        } 
            
    }    
}
