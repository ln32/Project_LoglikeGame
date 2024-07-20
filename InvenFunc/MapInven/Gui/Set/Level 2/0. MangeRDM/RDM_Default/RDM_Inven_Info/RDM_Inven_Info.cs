using System;
using UnityEngine;

public class RDM_Inven_Info : MonoBehaviour, iRoot_DDO_Manager
{
    [SerializeField] internal RDM_ValueSet _REF;
    [SerializeField] internal MapRDM_ItemInfoCtrl mapRDM_ItemInfoCtrl;
    [SerializeField] protected GUI_InvenSetManager m_inven;
    [SerializeField] protected InputCtrl_withRDM ctrl;

    public void SetClickEvent_RDM(SlotGUI_InvenSlot _gui)
    {
        ctrl.SetClickEvent(_gui);
    }

    public void SetSlotTransform_OnDrag(IDragDropObj _targetDDO)
    {
        if (_targetDDO.GetTransform_ItemGUI() == null)
            return;

        _REF._inputM.setDuringState(false);
        ctrl.SetSlotTransform_OnDrag(_targetDDO);
        ctrl.SetParent_ItemTrans(_REF._AboveOfAll.transform);
        mapRDM_ItemInfoCtrl.SetItemInfo_byItemDataUnit(ctrl.GetMyFocusingItem_toShowInfo());
    }

    // �巡�� ���� �� �̺�Ʈ �߻����� �ʾ� �Ͼ�� �Լ�.
    public void ReturnToInit_EndDrag()
    {
        if (!ctrl.IsDragObjExist())
            return;

        _REF._inputM.setDuringState(false);
        ctrl.ReturnToInit_EndDrag();
        mapRDM_ItemInfoCtrl.SetItemInfo_byItemDataUnit(ctrl.GetMyFocusingItem_toShowInfo());
    }

    // OnOver Event �߻� �� ȣ��
    public void SetEvent_OnEnter(IResponedByDrop _currRBD)
    {
        // �巡�� ���� ���� ��
        if (!ctrl.IsDragObjExist())
        {
            HandleNonDragEnter(_currRBD);
            return;
        }

        // �巡�� ��� ���� ��
        HandleDragEnter(_currRBD);
    }

    public void SetEvent_OnExit(IResponedByDrop _currRBD = null)
    {
        _currRBD.GetTargetSlotGUI().SetColor_DEFAULT();
        ctrl.SetEvent_OnExit(_currRBD);
        mapRDM_ItemInfoCtrl.SetItemInfo_byItemDataUnit(ctrl.GetMyFocusingItem_toShowInfo());
    }

    public SGT_GUI_ItemData GetInvenSGT()
    {
        return _REF._SGT_GUI_ItemData;
    }

    public bool IsDragObjExist()
    {
        return ctrl.IsDragObjExist();
    }

    // �巡�� ���� ���� ���� OnEnter �̺�Ʈ ó��
    private void HandleNonDragEnter(IResponedByDrop _currRBD)
    {
        if (_currRBD is iInvenSlot invenSlot && invenSlot.GetTransform_ItemGUI() != null)
        {
            ctrl.SetEvent_OnEnter(_currRBD);
        }

        if (ctrl.IsSameDrag(_currRBD))
        {
            _currRBD.GetTargetSlotGUI().SetColor_ONFOCUS();
        }
        else
        {
            _currRBD.GetTargetSlotGUI().SetColor_ONFOCUS();
        }

        mapRDM_ItemInfoCtrl.SetItemInfo_byItemDataUnit(ctrl.GetMyFocusingItem_toShowInfo());
    }

    // �巡�� ��� ���� ���� OnEnter �̺�Ʈ ó��
    private void HandleDragEnter(IResponedByDrop _currRBD)
    {
        if (!ctrl.IsSameDrag(_currRBD))
        {
            if (ctrl.targetDDO.IsInteractable_byGetRBD(this, _currRBD))
            {
                ctrl.SetCurrRBD(_currRBD);
                _currRBD.GetTargetSlotGUI().SetColor_ABLE();
            }
            else
            {
                ctrl.SetCurrRBD(null);
                _currRBD.GetTargetSlotGUI().SetColor_DISABLE();
            }
        }
        else if (_currRBD is RBD_CasherZone && ctrl.targetDDO.IsInteractable_byGetRBD(this, _currRBD))
        {
            ctrl.SetCurrRBD(_currRBD);
            _currRBD.GetTargetSlotGUI().SetColor_ABLE();
        }
        else
        {
            _currRBD.GetTargetSlotGUI().SetColor_DISABLE();
        }

        mapRDM_ItemInfoCtrl.SetItemInfo_byItemDataUnit(ctrl.GetMyFocusingItem_toShowInfo());
    }
}

[Serializable]
internal class RDM_ValueSet
{
    [SerializeField] internal MyInputManager _inputM;
    [SerializeField] internal Canvas _AboveOfAll;
    [SerializeField] internal SGT_GUI_ItemData _SGT_GUI_ItemData;
}
