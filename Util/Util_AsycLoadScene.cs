using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Util_AsycLoadScene : MonoBehaviour
{
    [SerializeField] protected string nextSceneName = "Scene 0";
    [SerializeField] protected float output;

    [SerializeField] protected AsyncOperation asyncLoad;

    public void LoadScene_Asyc(string target)
    {
        if (asyncLoad != null)
        {
            return;
        }

        // �񵿱������� Scene�� �ҷ����� ���� Coroutine�� ����Ѵ�.
        StartCoroutine(LoadMyAsyncScene());

        IEnumerator LoadMyAsyncScene()
        {
            nextSceneName = target;

            asyncLoad = SceneManager.LoadSceneAsync(target);
            asyncLoad.allowSceneActivation = false;

            while (!(asyncLoad.isDone))
            {
                yield return new WaitForSeconds(0.02f);
            }

            yield return null;
        }
    }

    public void TimeToSwitchScene()
    {
        if (asyncLoad != null)
        {
            asyncLoad.allowSceneActivation = true;
        }
    }

    public bool IsLoadedScene()
    {
        return asyncLoad != null;
    }
}
