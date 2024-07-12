
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

internal static class CreateMapTools
{
    internal delegate GameObject InstantiateFunc(GameObject _prefab);

    internal static List<Vector2> GetEventPointRow(this CreateMapEventValues values, List<int> nodeTreeData, int level, Vector2 focusV2)
    {
        List<Vector2> rtnList = new List<Vector2>();
        List<float> ratioAxis = new List<float>();

        // set new List
        int count = 0;
        for (int i = 0; i < nodeTreeData.Count; i++)
        {
            count += nodeTreeData[i];
            nodeTreeData[i] = count;
        }

        count += (nodeTreeData.Count) - 1;

        float coefX = values.eventArea.GetLength_X();
        float coefY = values.eventArea.GetLength_Y();

        if (true)
        {
            float tempSum = 0f;
            for (int i = 0; i < count + 1; i++)
            {
                tempSum += Random.Range(0.7f, 1f);
                ratioAxis.Add(tempSum);
            }

            for (int i = 0; i < ratioAxis.Count; i++)
            {
                ratioAxis[i] /= tempSum;
                ratioAxis[i] = -0.5f + ratioAxis[i];
            }
        }

        if (true)
        {
            float coeff = 3;
            float coef = Mathf.Min(coeff, count); // 2 3 4 5 5
            if (coef >= coeff) coef = 0;
            else
            {
                coef = 1 - ((coef + 1) / (coeff + 2));
            }

            for (int i = 0; i < count; i++)
            {
                float x = coefX * ratioAxis[i];
                x += focusV2.x;

                float temp = ratioAxis[i] + 0.5f * (ratioAxis[i + 1] - ratioAxis[i]);
                float y = coefY * (level) + values.eventArea.getRandomHeight(temp);

                rtnList.Add(new Vector2(x, y) + GetRandomPointInCircle(Vector2.one));
            }
        }

        return rtnList;
    }

    internal static float GetChildSumOfAxis(this CreateMapEventValues values, List<NodeScriptPerLevel> data, Vector2Int focusV2, int index)
    {
        float sum1 = data.getTransformByGridPos(focusV2).position.x;
        float sum2 = 0;
        if (focusV2.x > 0)
        {
            int count = 1;

            Vector2 range = data.getChildRangeByGridPos(new Vector2Int(focusV2.x, index));
            for (int i = (int)range.x; i < range.y + 1; i++)
            {
                sum2 += data.getTransformByGridPos(new Vector2Int(focusV2.x + 1, i)).position.x;
                count++;
            }

            sum2 /= count;
        }

        return (sum1 + sum2) / 2;
    }

    internal static Vector2 GetRandomPointInCircle(Vector2 coefV2)
    {
        float angle = Random.Range(0.0f, Mathf.PI * 2.0f); // ������ ������ ����
        float distance = Random.Range(0, 1f); // ������ �Ÿ��� ����

        Vector2 position = new Vector2(Mathf.Cos(angle) * coefV2.x, Mathf.Sin(angle) * coefV2.y) * distance; // ��ġ ���
        return position;
    }

    internal static List<Vector2> GetPointsRandomCurve(Vector2 startV2, Vector2 endV2, float gap, float coef)
    {
        List<Vector2> points = new List<Vector2>();
        Vector2 roadV2 = endV2 - startV2;
        float totalLength = roadV2.magnitude;
        float currentLength = 0;
        Vector2 currentPoint = startV2;

        Vector2 d = (endV2 - currentPoint).normalized;

        while (currentLength < totalLength)
        {
            Vector2 direction = (endV2 - currentPoint).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x);
            float gapWithRandomRange = gap * Random.Range(0.95f, 1.05f);
            float offsetX = gapWithRandomRange * Mathf.Cos(angle);
            float offsetY = gapWithRandomRange * Mathf.Sin(angle);

            Vector2 nextPoint = currentPoint + new Vector2(offsetX, offsetY);

            if ((endV2 - nextPoint).magnitude <= gapWithRandomRange)
            {
                break;
            }

            float randomCoef = coef * Random.Range(-1f, 1f);
            float perpendicularAngle = angle + Mathf.PI / 2;
            float shiftX = randomCoef * Mathf.Cos(perpendicularAngle);
            float shiftY = randomCoef * Mathf.Sin(perpendicularAngle);

            nextPoint += new Vector2(shiftX, shiftY);

            points.Add(nextPoint);
            currentPoint = nextPoint;
            currentLength += gapWithRandomRange;
        }

        return points;
    }

    internal static List<SpriteRenderer> setBackgroundSprites(this CreateMapBackgroundValues values, Vector2 stdPos, int index, int level)
    {
        List<SpriteRenderer> filledByBG = new();

        Fill_Circle();
        return filledByBG;

        void Fill_Circle()
        {
            float coefX = values.backgroundArea.GetLength_X() / 2;
            float coefY = values.backgroundArea.GetLength_Y() / 2;
            Vector2 stdV2 = new Vector2(-values.backgroundArea.GetLength_X() / 2, -values.backgroundArea.GetLength_Y() / 2);

            for (int y = 0; y < values.countY; y++)
            {
                for (int x = 0; x < values.countX; x++)
                {
                    var target = values.BgObjDataSet.GetDataSet_byStage(level);
                    if (target == null)
                        continue;

                    float offset_X = x - (values.countX / 2);
                    float offset_Y = y - (values.countY / 2);
                    offset_X /= (values.countX / 2);
                    offset_Y /= (values.countY / 2);

                    Vector2 dir = new Vector2(offset_X, offset_Y);

                    if (dir == Vector2.zero)
                        continue;

                    //dir += size;
                    dir += dir.normalized * new Vector2(coefX, coefY) * values.coef_Outline;
                    dir += GetRandomPointInCircle(new Vector2(values.coefX, values.coefY));

                    InstantSprite(Object.Instantiate(target).transform, dir);
                }
            }

            //( -1, 1 ) -> ( -1 , 1 )
            Vector2 retouchV2(Vector2 inputV2)
            {
                inputV2.x = retouch(inputV2.x);
                inputV2.y = retouch(inputV2.y);

                return inputV2*values.coef_Outline;

                float retouch(float input)
                {
                    return input;

                    if (input == 0)
                        return 0;
                    if (input == 1)
                        return 0;

                    float temp = 1 - Mathf.Abs(input);
                    input = Mathf.Pow(temp, values.coef_CircleLength)*(input/ Mathf.Abs(input));

                    return input;
                }
            }
        }

        void InstantSprite(Transform insTrans, Vector2 tempV2)
        {
            insTrans.parent = values.parentTransform;
            insTrans.localPosition = (Vector3)(stdPos + tempV2);
            insTrans.localPosition = (Vector3)((Vector2)insTrans.localPosition) + Vector3.forward * insTrans.localPosition.y;
            insTrans.localScale *= Random.Range(0.8f, 1f);
            filledByBG.Add(insTrans.GetComponent<SpriteRenderer>());
        }
    }

    internal static List<SpriteRenderer> setBackgroundSprites_BOSS(this CreateMapBackgroundValues values, Vector2 stdPos, int index, int level)
    {
        List<SpriteRenderer> filledByBG = new();

        Fill_Circle();
        return filledByBG;

        void Fill_Circle()
        {
            float coefX = values.backgroundArea.GetLength_X();
            float coefY = values.backgroundArea.GetLength_Y();

            for (int y = 0; y < values.countY * 2; y++)
            {
                for (int x = 0; x < values.countX * 2; x++)
                {
                    float offset_X = x - (values.countX);
                    float offset_Y = y - (values.countY);
                    offset_X /= (values.countX);
                    offset_Y /= (values.countY);

                    Vector2 dir = new Vector2(offset_X, offset_Y);

                    if (dir == Vector2.zero)
                        continue;

                    dir += (dir.normalized * new Vector2(coefX, coefY) * values.coef_Outline) * 1.05f;

                    //dir += size;
                    dir += GetRandomPointInCircle(new Vector2(values.coefX, values.coefY)) * 2;


                    var target = values.BgObjDataSet.GetDataSet_byStage(-1);
                    InstantSprite(Object.Instantiate(target).transform, dir);
                }
            }
        }

        void InstantSprite(Transform insTrans, Vector2 tempV2)
        {
            insTrans.parent = values.parentTransform;
            insTrans.localPosition = (Vector3)(stdPos + tempV2);
            insTrans.localPosition = (Vector3)((Vector2)insTrans.localPosition) + Vector3.forward * insTrans.localPosition.y;
            insTrans.localScale *= Random.Range(0.8f, 1f);
            filledByBG.Add(insTrans.GetComponent<SpriteRenderer>());
        }
    }

    internal static List<NodeController> setEventArea_Root(this CreateMapEventValues values, List<Transform> transformList)
    {
        List<NodeController> eventAreaSprites = new();

        Quaternion spawnRotation = Quaternion.identity;
        Transform tempObj;

        tempObj = Object.Instantiate(values.prefab).transform;
        transformList.Add(tempObj);
        tempObj.parent = values.parentTransform;
        tempObj.rotation = spawnRotation;

        NodeController node = _GetSpriteRenderer_EventNode(tempObj);
        eventAreaSprites.Add(node);
        node.ActiveDetailed(); // Root?
        return eventAreaSprites;

        NodeController _GetSpriteRenderer_EventNode(Transform instant)
        {
            return instant.GetComponent<NodeController>();
        }

        void setNodeSprite(List<EventDataSet> eventSpriteDB, NodeController target, int input)
        {
            int index_X = input / 100;
            int index_Y = input % 100;

            target.sillhouette = eventSpriteDB[index_X].sil_Sprite;
            target.detail = eventSpriteDB[index_X].real_Sprite[index_Y];
            return;
        }
    }



    // 1227DBG_4
    internal static void setEvent_byPatrol(this CreateMapVisual cm, NodeScriptPerLevel target, Vector2 _rangeV2)
    {
        if (cm.focusingNode.y == -1)
            return;

        _ValueSet_EventCoef _value = cm._value;
        Dictionary<int, int> currIncountState = _value.currIncountState;

        // ���� ��Ŀ�� ��� ���� ���߱�
        if (true)
        {
            Dictionary<int, int> currDict = currIncountState;
            var currToShow = cm._value.currIncountState_ToShow;
            int focusingIndex = cm.eventObjectList[cm.focusingNode.x].eventIndex[cm.focusingNode.y];

            if (focusingIndex != -1)
            {
                int renewCoef = 0;

                if (true)
                {
                    int focusingFloor = cm.focusingNode.x;
                    int getFocusedCoef = _value.getCoef_Gap_onFocused(focusingIndex);
                    renewCoef = focusingFloor + getFocusedCoef;
                }

                // dict ctrl
                if (true)
                {
                    currDict.Remove(focusingIndex);
                    currDict.Add(focusingIndex, renewCoef);
                }

                // list ctrl
                if (true)
                {
                    for (int i = 0; i < currToShow.Count; i++)
                    {
                        if (currToShow[i].x == focusingIndex)
                        {
                            currToShow[i] = new Vector2Int(currToShow[i].x, renewCoef);
                        }
                    }
                }
            }
        }

        // ���θ��� �̺�Ʈ ����Ʈ ������ ���� ��ȸ�ϸ� ��
        if (true)
        {
            Dictionary<int, int> oldDict = currIncountState;
            Dictionary<int, int> newDict = new();
            List<Vector2Int> newList_toShow = new();

            // ������ Ű �� ���� 
            if (true)
            {
                foreach (KeyValuePair<int, int> kv in oldDict)
                {
                    int compareValue = cm.focusingNode.x;

                    if (compareValue < kv.Value)
                    {
                        newDict.Add(kv.Key, kv.Value);
                        newList_toShow.Add(new Vector2Int(kv.Key, kv.Value));
                    }
                }

                cm._value.currIncountState = newDict;
                oldDict.Clear();

                cm._value.currIncountState_ToShow = newList_toShow;
            }

            // ���� Ű���� ���� ���൵ ����
            if (true)
            {
                foreach (var kv in oldDict)
                {
                    int agingCoef = (int)(kv.Value);//_valueSet.getCoef_Aging(kv.Key);
                    newDict.Add(kv.Key, agingCoef);

                    newList_toShow.Add(new Vector2Int(kv.Key, agingCoef));
                }
                cm._value.currIncountState = newDict;

                oldDict.Clear();
                cm._value.currIncountState_ToShow = newList_toShow;
            }
        }


        return;
    }

    // 1227DBG_3z
    internal static void setEvent_byHistory(this CreateMapVisual cm, NodeScriptPerLevel target, Vector2 _rangeV2)
    {
        cm._value.ApplyCurrState(target.eventIndex, new Vector2Int((int)_rangeV2.x, (int)_rangeV2.y),cm.focusingNode);
    }

    // 1227DBG_3
    internal static void ActiveDetailed_RealTime(this NodeController values, int _data)
    {
        if (_data < 0)
            return;

        values.detail = values.myDetailData[_data %= 100];
    }

    internal static List<NodeController> setEventArea(this CreateMapEventValues values, List<Vector2> createList,
        List<Transform> transformList, List<int> eventList)
    {
        List<NodeController> eventAreaSprites = new();

        Vector3 spawnPosition;
        Quaternion spawnRotation;

        for (int i = 0; i < createList.Count; i++)
        {
            spawnPosition = createList[i];
            spawnPosition.z = spawnPosition.y;
            spawnRotation = Quaternion.identity;

            Transform tempObj;
            tempObj = Object.Instantiate(values.prefab).transform;
            transformList.Add(tempObj);
            tempObj.parent = values.parentTransform;
            tempObj.name = i + " pos ";
            tempObj.position = spawnPosition;
            tempObj.rotation = spawnRotation;

            NodeController temp = tempObj.GetComponent<NodeController>();
            setNodeSprite(values.eventSpriteDB, temp, eventList[i]);
            temp.SetSprite();
            eventAreaSprites.Add(temp);
            createList[i] = tempObj.position;
        }

        return eventAreaSprites;

        // 1227DBG_2
        void setNodeSprite(List<EventDataSet> eventSpriteDB, NodeController target, int input)
        {
            int index_X = input / 100;
            int index_Y = input % 100;

            target.sillhouette = eventSpriteDB[index_X].sil_Sprite;
            target.myDetailData = eventSpriteDB[index_X].real_Sprite;
            target.detail = eventSpriteDB[index_X].real_Sprite[index_Y];
            return;
        }
    }

    internal static List<NodeController> setEventArea_BOSS(this CreateMapEventValues values, List<Vector2> createList,
    List<Transform> transformList, List<int> eventList)
    {
        List<NodeController> eventAreaSprites = new();

        Vector3 spawnPosition;
        Quaternion spawnRotation;

        for (int i = 0; i < createList.Count; i++)
        {
            spawnPosition = createList[i];
            spawnPosition.z = spawnPosition.y;
            spawnRotation = Quaternion.identity;

            Transform tempObj;
            tempObj = Object.Instantiate(values.lastBOSS).transform;
            transformList.Add(tempObj);
            tempObj.parent = values.parentTransform;
            tempObj.name = "BOSS";
            tempObj.position = spawnPosition;
            tempObj.rotation = spawnRotation;

            NodeController temp = tempObj.GetComponent<NodeController>();
            setNodeSprite(values.eventSpriteDB, temp, eventList[i]);
            temp.SetSprite();
            eventAreaSprites.Add(temp);
            createList[i] = tempObj.position;
        }

        return eventAreaSprites;

        // 1227DBG_2
        void setNodeSprite(List<EventDataSet> eventSpriteDB, NodeController target, int input)
        {
            int index_X = input / 100;
            int index_Y = input % 100;

            target.sillhouette = eventSpriteDB[index_X].sil_Sprite;
            target.myDetailData = eventSpriteDB[index_X].real_Sprite;
            target.detail = eventSpriteDB[index_X].real_Sprite[0]; 
            return;
        }
    }



    // V3 data -> int
    internal static int terrainDataToIndex(Vector3 terrainData)
    {
        float MAX = Mathf.Max(terrainData.x, terrainData.y, terrainData.z);
        float[] tmpAry = { terrainData.x, terrainData.y, terrainData.z };

        int i = 0;
        for (; i < tmpAry.Length; i++)
        {
            if (tmpAry[i] == MAX)
                break;
        }

        return i;
    }

    internal static void SetTerrain(this List<NodeScriptPerLevel> _data, Vector2Int _src, Vector2Int _dst)
    {
        Vector3 _srcTerrain = _data[_src.x].nodeTerrainData[_src.y];
        Vector3 _dstTerrain = _data[_dst.x].nodeTerrainData[_dst.y];

        _data[_dst.x].terrainScale[_dst.y] = _data[_src.x].terrainScale[_src.y] + Vector3.Distance(_srcTerrain, _dstTerrain);
    }

    internal static void setRoadAreaSetBySet(this CreateRoadValues values, NodeScriptPerLevel nextLevel, NodeScriptPerLevel createdLevel, int childStd)
    {
        for (int index = 0; index < nextLevel.connectingData.Count; index++)
        {
            for (int dest = (int)nextLevel.connectingData[index].x; dest < nextLevel.connectingData[index].y + 1; dest++)
            {
                int source = index + childStd;
                RoadSet temp = setRoadArea(new Vector2Int(source, dest));
                setRoadTerrainData(temp);
                createdLevel.roadSetList.Add(temp);
            }
        }

        return;

        RoadSet setRoadArea(Vector2Int route)
        {
            RoadSet roadAreaSprites = new();
            Vector3 srcPosV3 = nextLevel.transformList[route.x].position;
            Vector3 desPosV3 = createdLevel.transformList[route.y].position;

            List<Vector2> dotPos = GetPointsRandomCurve(srcPosV3, desPosV3, values.gap, values.coef);

            roadAreaSprites.nameV3 = new Vector3Int(createdLevel.level, route.x, route.y);


            Transform targetTransf = null;

            if (values.isBool_RoadVis)
                targetTransf = SetState_onInit(roadAreaSprites.nameV3 + "");

            targetTransf.position = srcPosV3;
            roadAreaSprites._transform = targetTransf;

            for (int j = 0; j < dotPos.Count; j++)
            {
                Vector3 spawnPosition = dotPos[j];

                if (Vector2.Distance(srcPosV3, spawnPosition) < values.ignoreRange)
                    continue;
                if (Vector2.Distance(desPosV3, spawnPosition) < values.ignoreRange)
                    continue;

                spawnPosition.z = spawnPosition.y;

                Quaternion spawnRotation = Quaternion.identity;

                Transform tempObj = Object.Instantiate(values.roadObject).transform;
                tempObj.parent = targetTransf;
                tempObj.position = spawnPosition;
                tempObj.rotation = spawnRotation;
                roadAreaSprites.roadSet.Add(tempObj.GetChild(0).GetComponent<SpriteRenderer>());
            }

            return roadAreaSprites;

            Transform SetState_onInit(string _name)
            {
                Transform unitRoad = new GameObject().transform;
                unitRoad.name = "Road - " + _name;
                unitRoad.parent = values.parentTransform;
                return unitRoad;
            }
        }

        void setRoadTerrainData(RoadSet _data)
        {
            _data.srcTerrain = nextLevel.nodeTerrainData[_data.nameV3.y];
            _data.desTerrain = createdLevel.nodeTerrainData[_data.nameV3.z];
        }
    }

    internal static void setRoadSpriteByData(this CreateMapBackgroundValues values, NodeScriptPerLevel _createdLevel)
    {
        int std_CUTOFF = 10;
        //float std_DISTANCE = 0.8f;
        for (int i = 0; i < _createdLevel.roadSetList.Count; i++)
        {
            RoadSet target = _createdLevel.roadSetList[i];

            for (int pointIndex = 0; pointIndex < target.roadSet.Count; pointIndex++)
            {
                if (pointIndex < std_CUTOFF)
                    continue;
                if (target.roadSet.Count - pointIndex < std_CUTOFF)
                    continue;

                Transform prev = target.roadSet[pointIndex - 1].transform;
                Transform curr = target.roadSet[pointIndex].transform;
                Transform next = target.roadSet[pointIndex + 1].transform;
            }
        }

        return;
    }


    internal static NodeScriptPerLevel tryAddLevel(this List<NodeScriptPerLevel> data, int level)
    {
        if (data.Count != level)
        {
            return data[level];
        }

        NodeScriptPerLevel rtn = new(level);
        data.Add(rtn);
        return rtn;
    }

    internal static Transform getTransformByGridPos(this List<NodeScriptPerLevel> data, Vector2Int focusNodePos)
    {
        if (data[focusNodePos.x].transformList == null)
        { return null;   }

        return data[focusNodePos.x].transformList[focusNodePos.y];
    }

    internal static int setConnectedData(this List<Vector2> target, List<int> data)
    {
        if (target == null || data == null)
        {
            return 0;
        }

        int curr = 0;
        int temp = 0;
        for (int i = 0; i < data.Count; i++)
        {
            if (i != 0 && i != data.Count - 1)
            {
                temp++;
            }

            int next = data[i] + temp;
            target.Add(new Vector2(curr, next));
            curr = next;
        }

        if (target.Count == 1)
        {
            target[0] = new Vector2(0, target[0].y - 1);
        }

        return 1;
    }

    internal static Vector2 getChildRangeByGridPos(this List<NodeScriptPerLevel> data, Vector2Int gridNodePos, int std = 0)
    {
        if (gridNodePos.x == 0)
            return new Vector2(0, data[0].transformList.Count);

        if (data[gridNodePos.x].connectingData.Count == 0)
            return Vector2.one * -1;

        return data[gridNodePos.x].connectingData[gridNodePos.y] + Vector2.one * std;
    }

    internal static Vector2Int getChildRangeByGridPos_FocusStd(this List<NodeScriptPerLevel> data, Vector2Int gridNodePos)
    {
        int stdValue = 0;
        if (gridNodePos.x - 1 >= 0)
        {
            stdValue = data[gridNodePos.x - 1].childStd;
        }

        if (gridNodePos.x == 0)
            return new Vector2Int(0, data[1].transformList.Count - 1);

        Vector2 temp = data[gridNodePos.x].connectingData[gridNodePos.y - stdValue];
        return new Vector2Int((int)temp.x, (int)temp.y);
    }

    public static void SetNodeMaterialActive(this SpriteRenderer targetNode)
    {
        targetNode.transform.GetComponent<NodeController>().ActiveDetailed();
    }
}