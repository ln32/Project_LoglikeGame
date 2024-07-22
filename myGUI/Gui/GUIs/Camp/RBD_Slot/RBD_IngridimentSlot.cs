using UnityEngine;

public class RBD_IngridimentSlot : MonoBehaviour, IResponedByDrop
{
    public GUI_IngridiSlot myGUI_Slot;

    [SerializeField] internal GUI_Ctrl myGUI_CTRL;
    [SerializeField] internal GUI_ItemUnit _GUI_ItemUnit;
    [SerializeField] internal int _index;

    private iRoot_DDO_Manager cash = null;

    public iRoot_DDO_Manager GetDDO_Manager()
    {
        if (cash != null)
            return cash;
        cash = transform.root.GetComponent<iRoot_DDO_Manager>();

        return transform.root.GetComponent<iRoot_DDO_Manager>();
    }

    public iSlotGUI GetTargetSlotGUI()
    {
        return myGUI_CTRL;
    }

    public void DDO_Event_byInvenSlot(SlotGUI_InvenSlot _src)
    {
        if (true)
        {
            RDM_Info_CampCook _RDM_CampCook = GetDDO_Manager() as RDM_Info_CampCook;
            if (_RDM_CampCook)
            {
                _RDM_CampCook.SetIngredient_byInvenSlot(_src, this);
                return;
            }
        }
    }

    public void SetDefault()
    {
        myGUI_Slot.SetDefault();
        if (_GUI_ItemUnit != null)
        {
            Destroy(_GUI_ItemUnit.gameObject);
            _GUI_ItemUnit = null;
        }
    }
}
