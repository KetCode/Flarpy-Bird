using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class GameOverScreenTest
{
    [SetUp]
    public void SetUp()
    {
        SceneManager.LoadScene("Game");
    }
    
    [UnityTest]
    [Order(1)]
    public IEnumerator TestSceneLoading()
    {
        // --------- SCENE NAME CHECK --------- //
        Assert.IsTrue(SceneManager.GetActiveScene().name == "Game");
        
        yield return new WaitForFixedUpdate();
    }

    [UnityTest]
    public IEnumerator PlayAgain_ClickButton_RestartGame()
    {
        // --------- GAME LOGIC CONFIG --------- //
        GameObject logic = GameObject.Find("Logic Manager");
        Assert.IsNotNull(logic, "'Logic Manager' GameObject was not found in the scene.");
        
        LogicScript logicScript = logic.GetComponent<LogicScript>();
        Assert.IsNotNull(logicScript, "'Logic Script' component was not found in Logic Manager.");
        
        // --------- POPUP APPEARS --------- //
        logicScript.gameOverScreen.SetActive(true);
        Assert.IsTrue(logicScript.gameOverScreen.activeSelf, "'Game Over Screen' was not active.");
        
        // --------- CLICK PLAY AGAIN BUTTON --------- //
        Button playButton = GameObject.Find("Game Over Screen/PlayAgain Button").GetComponent<Button>();
        Assert.IsNotNull(playButton, "'Play Again' Button was not found in the scene.");

        new Interaction().Click().Button(playButton).Perform();
        yield return new WaitForDomainReload();
        
        string sceneName = SceneManager.GetActiveScene().name;
        Assert.That(sceneName, Is.EqualTo("Game"));
        Assert.IsTrue(logicScript.gameOverScreen == null || !logicScript.gameOverScreen.activeSelf, "'Game Over Screen' is still active even when clicking on Play Again Button.");
    }
}
