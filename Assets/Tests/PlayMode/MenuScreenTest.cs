using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class MenuScreenTest
{
    [SetUp]
    public void SetUp()
    {
        SceneManager.LoadScene("Menu");
    }
    
    [UnityTest]
    [Order(1)]
    public IEnumerator TestSceneLoading()
    {
        // --------- LOAD SCENE --------- //
        var testScene = SceneManager.GetActiveScene();
        yield return SceneManager.LoadSceneAsync("Menu", LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Menu"));
        
        Assert.IsTrue(SceneManager.GetActiveScene().name == "Menu");
        
        // --------- CLEAN UP --------- //
        SceneManager.SetActiveScene(testScene);
        yield return SceneManager.UnloadSceneAsync("Menu");
    }

    [UnityTest]
    public IEnumerator PlayGame_GameStart_Success()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        Assert.That(sceneName, Is.EqualTo("Menu"));
        
        Button playButton = GameObject.Find("Background/Button Container/PlayButton").GetComponent<Button>();
        Assert.IsNotNull(playButton, "'PlayButton' GameObject not found in scene.");
        
        new Interaction().Click().Button(playButton).Perform();
        yield return new WaitForDomainReload();
        
        sceneName = SceneManager.GetActiveScene().name;
        Assert.That(sceneName, Is.EqualTo("Game"));
    }

    [UnityTest]
    public IEnumerator ExitGame_GameClose_Success()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        Assert.That(sceneName, Is.EqualTo("Menu"));
        
        Button quitButton = GameObject.Find("Background/Button Container/QuitButton").GetComponent<Button>();
        Assert.IsNotNull(quitButton, "'PlayButton' GameObject not found in scene.");
        
        new Interaction().Click().Button(quitButton).Perform();
        yield return new WaitForDomainReload();
        
        // Application.Quit() não encerra o editor durante os testes, então não é possível verificar se a aplicação realmente fecha.
        Assert.IsTrue(Application.isPlaying, "The application must be running after clicking Exit.");
    }
}
