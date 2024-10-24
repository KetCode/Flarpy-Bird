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
        // --------- SCENE NAME CHECK --------- //
        Assert.IsTrue(SceneManager.GetActiveScene().name == "Menu");
        
        yield return new WaitForFixedUpdate();
    }

    [UnityTest]
    public IEnumerator PlayGame_GameStart_Success()
    {
        // --------- SCENE NAME CHECK --------- //
        string sceneName = SceneManager.GetActiveScene().name;
        Assert.That(sceneName, Is.EqualTo("Menu"));
        
        // --------- PLAY BUTTON CHECK --------- //
        Button playButton = GameObject.Find("Background/Button Container/PlayButton").GetComponent<Button>();
        Assert.IsNotNull(playButton, "'PlayButton' GameObject not found in scene.");
        
        // --------- CLICK BUTTON --------- //
        new Interaction().Click().Button(playButton).Perform();
        yield return new WaitForDomainReload();
        
        // --------- CHANGE SCENE CHECK --------- //
        sceneName = SceneManager.GetActiveScene().name;
        Assert.That(sceneName, Is.EqualTo("Game"));
    }

    [UnityTest]
    public IEnumerator ExitGame_GameClose_Success()
    {
        // --------- SCENE NAME CHECK --------- //
        string sceneName = SceneManager.GetActiveScene().name;
        Assert.That(sceneName, Is.EqualTo("Menu"));
        
        // --------- QUIT BUTTON CHECK --------- //
        Button quitButton = GameObject.Find("Background/Button Container/QuitButton").GetComponent<Button>();
        Assert.IsNotNull(quitButton, "'PlayButton' GameObject not found in scene.");
        
        // --------- CLICK BUTTON --------- //
        new Interaction().Click().Button(quitButton).Perform();
        yield return new WaitForDomainReload();
        
        // --------- EXIT GAME --------- //
        // Application.Quit() não encerra o editor durante os testes, então não é possível verificar se a aplicação realmente fecha.
        Assert.IsTrue(Application.isPlaying, "The application must be running after clicking Exit.");
    }
}
