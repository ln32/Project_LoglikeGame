using CreateMapTools;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CreateStageScenario : MonoBehaviour
{
    internal Action<int> onClick;

    [SerializeField] internal CreateMapVisual createBackGroundSector;
    [SerializeField] internal CreateFloatingNode createFloatingNode;
    [SerializeField] internal Transform focusingNode;
    [SerializeField] internal Vector2Int focusingGridPos;

    [SerializeField] internal List<TouchableNode> currSelectable;
    [SerializeField] internal Camera MapCamera;

    public void SetOnClickFunc(Action<int> _onClick)
    {
        onClick = _onClick;
    }

    public void NewGame(int seed)
    {
        UnityEngine.Random.InitState(seed);
        this.setInitData();

        this.buildStem_byInit();
        this.buildLeaf_byInit();

        currSelectable.settingNextDestination(createBackGroundSector, createFloatingNode, onClick);
        this.setVisualObject();
    }

    public void LoadGame(int seed,int[] history)
    {
        UnityEngine.Random.InitState(seed);
        this.setInitData();

        this.buildStem_byInit();
        this.buildLeaf_byInit();

        for (int i = 0; i < history.Length; i++)
        {
            currSelectable.settingNextDestination(createBackGroundSector, createFloatingNode, history[i]);
            if(createFloatingNode.createNodeValues.maxLevel <= i)
            {
                return;
            }
            this.buildLeaf_byHistory(history[i], 
                (createFloatingNode.createNodeValues.maxLevel - i == 2)); // ���� �Ǵ� 
        }


        currSelectable.settingNextDestination(createBackGroundSector, createFloatingNode, onClick);
        this.setVisualObject();
    }

    public Vector3 ProgressMap(int inputChildIndex, ref Action task)
    {
        if (!createFloatingNode.IsInit())
            return Vector3.one*-1;

        if(createFloatingNode.createNodeValues.maxLevel == focusingGridPos.x)
        {
            return Vector3.zero;
        }

        // ���� ���� ��, �ش� ����� Ʈ������ ����
        this.buildLeaf(ref task, inputChildIndex);

        focusingNode = createBackGroundSector.GetFocusTransform();
        focusingGridPos = createFloatingNode.getFocusGridPos();

        Vector2Int range_Curr = createBackGroundSector.eventObjectList.getChildRangeByGridPos_FocusStd(focusingGridPos);
        Vector3 rtnV3 = createBackGroundSector.GetAxisX_CreatedNode();

        // �� ������ Ŭ�� �̺�Ʈ ���ķ� �̷���� �߰� �ڵ�
        task += () => currSelectable.settingNextDestination(createBackGroundSector, createFloatingNode, onClick);

        return rtnV3;
    }

    public int GetIndex_CurrFocusing()
    {
        Vector2Int temp = createBackGroundSector.focusingNode;
        return createBackGroundSector.eventObjectList[temp.x].eventIndex[temp.y];
    }
}

[Serializable]
public class EventNodeDataToPlace
{
    public Vector2Int focusGridPos;
    public int targetLevel;
    public List<int> nodeTreeData;
    public List<int> nodeEventData;
    public List<Vector3> nodeTerrainData;
}

