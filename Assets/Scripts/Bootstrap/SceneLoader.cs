using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader
{
    private static SceneLoader _instance;
    private float minimumLoadingTime = 2f; // 최소 로딩 시간 (초)

    private string nextSceneName = "MainGameScene1";

    private SceneLoader()
    {
    } // private 생성자

    public static SceneLoader Instance
    {
        get
        {
            if (_instance == null) _instance = new SceneLoader();
            return _instance;
        }
    }

    public async UniTask LoadScene(string sceneName)
    {
        try
        {
            await SceneManager.LoadSceneAsync(sceneName);
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to load scene '{sceneName}': {e.Message}");
            // 여기에 UI에 오류 메시지를 표시하는 코드를 추가할 수 있습니다.
        }
    }

    public async UniTask LoadSceneAsync(string sceneName, IProgress<float> progress = null)
    {
        try
        {
            var asyncOperation = SceneManager.LoadSceneAsync(sceneName);
            asyncOperation.allowSceneActivation = false;
            while (asyncOperation.progress < 0.9f)
            {
                await UniTask.Yield();
                progress?.Report(asyncOperation.progress);
            }

            // 마지막 10%는 수동으로 처리
            for (var i = asyncOperation.progress; i <= 1f; i += 0.1f)
            {
                progress?.Report(i);
                await UniTask.Delay(100);
            }

            asyncOperation.allowSceneActivation = true;
            await asyncOperation;
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to load scene '{sceneName}': {e.Message}");
            // 여기에 UI에 오류 메시지를 표시하는 코드를 추가할 수 있습니다.
        }
    }

    public async UniTask LoadNextSceneAsync()
    {
        try
        {
            // 최소 로딩 시간을 위한 딜레이 시작
            var minimumLoadingTimeTask = UniTask.Delay(TimeSpan.FromSeconds(minimumLoadingTime));
            // 진행률을 표시하기 위한 Progress 객체 생성
            var progress = new Progress<float>(OnLoadingProgressChanged);
            // 다음 씬 로딩 시작
            var loadSceneTask = LoadSceneAsync(nextSceneName, progress);
            // 두 작업이 모두 완료될 때까지 기다림
            await UniTask.WhenAll(minimumLoadingTimeTask, loadSceneTask);
            Debug.Log("Scene loading completed!");
        }
        catch (Exception e)
        {
            Debug.LogError($"Error loading next scene: {e.Message}");
        }

        Debug.Log("Loading scene complete");
    }

    private void OnLoadingProgressChanged(float progress)
    {
        // 여기에서 로딩 진행률 UI를 업데이트합니다.
        Debug.Log($"Loading progress: {progress * 100}%");
    }

    // 필요한 경우 nextSceneName과 minimumLoadingTime을 설정하는 메서드 추가
    public void SetNextScene(string sceneName)
    {
        nextSceneName = sceneName;
    }

    public void SetMinimumLoadingTime(float seconds)
    {
        minimumLoadingTime = seconds;
    }
}