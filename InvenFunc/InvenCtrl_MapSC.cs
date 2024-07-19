using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static GUI_MapScenario;

public class InvenCtrl_MapSC : MonoBehaviour
{
    protected MyInputManager MyInputM;
    protected RawImage myBlur1, myBlur2;
    protected GameObject CamObj;

    protected ProgressMap_preInput returnFunc;

    protected float reachTime = 0.2f;
    protected float myAlpha = 0.5f;
    protected float coefP = 3;
    protected float timer = 0;

    protected IEnumerator EffectScenechange_BlurMapToInven(ProgressMap_preInput func)
    {
        timer = 0;
        setColorByCoef(myBlur1, 0);
        setColorByCoef(myBlur2, 1);

        float target = 0;

        while (true)
        {
            timer += Time.deltaTime;

            if (timer > reachTime)
                break;

            target = (timer / reachTime); // 0 -> 1

            setColorByCoef(myBlur1, target * myAlpha);
            setColorByCoef(myBlur2, 1 - Mathf.Pow(target,coefP));

            yield return new WaitForSeconds(0.02f);
        }

        setColorByCoef(myBlur1, myAlpha);
        setColorByCoef(myBlur2, 0);
        func();
        timer = 0;
        yield return null;
    }

    protected IEnumerator EffectScenechange_BlurInvenToMap(ProgressMap_preInput progFunc)
    {
        timer = 0;
        float target = 0;

        while (true)
        {
            timer += Time.deltaTime;

            if (timer > reachTime)
                break;

            target = (1 - timer / reachTime); //1 -> 0
            setColorByCoef(myBlur1, target * myAlpha);
            setColorByCoef(myBlur2, 1 - Mathf.Pow(target, coefP));

            yield return new WaitForSeconds(0.02f);
        }

        setColorByCoef(myBlur1, 0);
        setColorByCoef(myBlur2, 1);

        returnFunc();
        progFunc();
        CamObj.SetActive(false);
        timer = 0;

        yield return null;
    }

    void setColorByCoef(RawImage rw, float coef)
    {
        if (rw == null) return;
        Color temp = new Color(rw.color.r, rw.color.g, rw.color.b, 0);
        temp.a = coef;
        rw.color = temp;
    }
}

public class MapScenario_InvenCtrl : InvenCtrl_MapSC
{
    public void BlurStart_InvenToMap()
    {
        MyInputM.SetInputObjActivate(false);
        returnFunc += () => MyInputM.SetInputObjActivate(true);
        ProgressMap_preInput progFunc = () => {; };

        if (timer != 0)
            return;

        StartCoroutine(EffectScenechange_BlurInvenToMap(progFunc));
    }

    public void BlurStart(ProgressMap_preInput func, ProgressMap_preInput rtnFunc)
    {
        MyInputM.SetInputObjActivate(false);
        func += () => MyInputM.SetInputObjActivate(true);

        if (timer != 0)
            return;

        returnFunc = rtnFunc;
        CamObj.SetActive(true);

        StartCoroutine(EffectScenechange_BlurMapToInven(func));
    }
}