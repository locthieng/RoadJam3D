using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NgoUyenNguyen.GridSystem;
public struct StageTransform
{
    public string Name;
    public Vector3 Position;
    public Vector3 Rotation;
    public Vector3 Scale;
}

public class SingleLevelController : MonoBehaviour
{
    public static SingleLevelController Instance { get; set; }

    public BaseGrid grid;

    public virtual void SetUp()
    {
        Instance = this;


    }

    public virtual void StartLevel()
    {
    }

    public virtual void ResetLevel()
    {
    }


    private void OnDestroy()
    {
    }

    public virtual void ShowSingleFinishEffects()
    {

    }

    public virtual void RefreshCharacterSkins()
    {
    }

    public virtual void SetCharacterSkin(ShopItemData data)
    {
    }
}
