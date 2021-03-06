﻿using UnityEngine;
using System.Collections;
using System;
using VRTK;

public class BagInteraction : MonoBehaviour
{
    [SerializeField]
    public Material mRimLight;
    private Material oldMaterial;

    public delegate void TriggerEnter(Collider other);
    public static event TriggerEnter OnEntered;
    public delegate void TriggerExit();
    public static event TriggerExit OnExited;

    public bool mIsBagOnLeft = true;
    private bool mManageMode = true;
    private bool mIsItemOverBag = false;
    private bool mWasTriggerEnteredSkipped = false;
    private Collider mItemOverBagCollider;
    private ushort bagHapticVibration = 500;

    public void SubscribeTriggers()
    {
        HVRControllerManager.Instance.RightEvents.TriggerReleased += DoTriggerReleasedRight;
        HVRControllerManager.Instance.LeftEvents.TriggerReleased += DoTriggerReleasedLeft;
    }

    public void UnsubscribeTriggers()
    {
        HVRControllerManager.Instance.RightEvents.TriggerReleased -= DoTriggerReleasedRight;
        HVRControllerManager.Instance.LeftEvents.TriggerReleased -= DoTriggerReleasedLeft;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject != null)
        {
            VRTK_InteractableObject obj = other.gameObject.GetComponent<VRTK_InteractableObject>();
            if(obj != null)
            {
                if (obj.IsGrabbed())
                {
                    mWasTriggerEnteredSkipped = false;
                    if (OnEntered != null)
                    {
                        OnEntered(other);
                    }

                    mIsItemOverBag = true;
                    mItemOverBagCollider = other;

                    oldMaterial = other.gameObject.GetComponent<MeshRenderer>().materials[0];
                    Material[] newMaterials = new Material[other.gameObject.GetComponent<MeshRenderer>().materials.Length];
                    newMaterials[0] = mRimLight;
                    other.gameObject.GetComponent<MeshRenderer>().materials = newMaterials;
                }
                else
                {
                    mWasTriggerEnteredSkipped = true;
                }
            }
            else
            {
                Debug.LogError("BRODY DOESN'T LIKE ERROR CHECKING BUT HERE IS A MESSAGE");
            }
        }

    }

    void OnTriggerExit(Collider other)
    {
        // Temporary to avoid null ref
        if (other == null)
        {
            mWasTriggerEnteredSkipped = false;
            mIsItemOverBag = false;
        }
        else
        {
            mWasTriggerEnteredSkipped = false;
            if (other.gameObject.GetComponent<VRTK_InteractableObject>().IsGrabbed())
            {
                if (OnExited != null)
                {
                    OnExited();
                }

                mIsItemOverBag = false;

                Material[] newMaterials = new Material[other.gameObject.GetComponent<MeshRenderer>().materials.Length];
                newMaterials[0] = oldMaterial;
                other.gameObject.GetComponent<MeshRenderer>().materials = newMaterials;
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<VRTK_InteractableObject>().IsGrabbed())
        {
            if (mWasTriggerEnteredSkipped)
            {
                OnTriggerEnter(other);
            }
            VRTK_InteractableObject heldInteractableObject = other.gameObject.GetComponent<VRTK_InteractableObject>();
            GameObject heldControllerGameObject = heldInteractableObject.GetGrabbingObject();
            if (heldControllerGameObject != null)
            {
                VRTK_ControllerActions heldControllerActions = heldControllerGameObject.GetComponent<VRTK_ControllerActions>();
                heldControllerActions.TriggerHapticPulse(bagHapticVibration);
            }
        }
    }

    void UndoItemOverBag()
    {
        if ((!mManageMode) && (mIsItemOverBag))
        {
            OnTriggerExit(mItemOverBagCollider);
        }
    }

    void SwitchModes()
    {
        UndoItemOverBag();

        if (mManageMode)
        {
            GameManager.Instance.Player.BackPack.DestroyInventoryUI();
        }
        else
        {
            GameManager.Instance.Player.BackPack.SpawnInventoryUI(true);
        }
        transform.GetChild(0).GetChild(1).gameObject.SetActive(!mManageMode);
        transform.GetComponent<CapsuleCollider>().enabled = mManageMode;
        transform.GetChild(0).GetChild(0).gameObject.SetActive(mManageMode);
        mManageMode = !mManageMode;
    }

    void DoTriggerReleasedLeft(object sender, ControllerInteractionEventArgs e)
    {
        if (mIsBagOnLeft)
        {
            SwitchModes();
        }
        else
        {
            if (mItemOverBagCollider != null)
            {
                GameObject item = mItemOverBagCollider.gameObject;
                GameManager.Instance.Player.BackPack.AddItemToInventory(item.GetComponent<ItemFactoryData>().ItemEnum);
                //Destroy(mItemOverBagCollider.gameObject);
                HVRItemFactory.DestroyObject(mItemOverBagCollider.gameObject);
            }
        }
    }

    void DoTriggerReleasedRight(object sender, ControllerInteractionEventArgs e)
    {
        if (!mIsBagOnLeft)
        {
            SwitchModes();
        }
        else
        {
            if (mIsItemOverBag)
            {
                if (mItemOverBagCollider != null)
                {
                    GameObject item = mItemOverBagCollider.gameObject;
                    GameManager.Instance.Player.BackPack.AddItemToInventory(item.GetComponent<ItemFactoryData>().ItemEnum);
                    Destroy(mItemOverBagCollider.gameObject);
                }
            }
        }
    }

    void OnDestroy()
    {
        UndoItemOverBag();
    }
}