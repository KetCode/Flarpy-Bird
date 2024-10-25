using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using static UnityEngine.Object;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class BirdPrefabTests
{
    GameObject fakePipe;
        
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

    [Test]
    public void BirdPrefab_IsBeingInstantiated_WhenSceneIsLoaded()
    {
        // --------- PREFAB CHECK --------- //
        GameObject bird = GameObject.Find("Bird");
        Assert.IsNotNull(bird, "'Bird' GameObject was not found");
        
        // --------- COMPONENT SCRIPT CHECK --------- //
        BirdScript birdScript = bird.GetComponent<BirdScript>();
        Assert.IsNotNull(birdScript, "BirdScript component not found in 'Bird' GameObject.");
        
        // --------- NAME CHECK --------- //
        Assert.AreEqual("Bird", bird.name);
    }

    [UnityTest]
    public IEnumerator Bird_Flaps_WhenUsingJumpFunction()
    {
        // --------- PLAYER CHECK --------- //
        GameObject bird = GameObject.Find("Bird");
        Assert.IsNotNull(bird, "'Bird' GameObject was not found");

        // --------- COMPONENT RIGIDBODY CHECK --------- //
        Rigidbody2D myRigidbody2D = bird.GetComponent<Rigidbody2D>();
        Assert.IsNotNull(myRigidbody2D, "Rigidbody2D component was not found in 'Bird' GameObject.");
        
        // Simula o jump usando uma forma diferente de pular
        myRigidbody2D.AddForce(Vector3.up * 10, ForceMode2D.Impulse);

        // --------- JUMP CHECK --------- //
        yield return new WaitForFixedUpdate();
        Assert.Greater(myRigidbody2D.velocity.y, 0, "Bird did not flap");
    }

    [UnityTest]
    public IEnumerator AddScore_PlayerEarnPoints_WhenPassingThroughObstacles()
    {
        // --------- PLAYER CHECK --------- //
        GameObject bird = GameObject.Find("Bird");
        Assert.IsNotNull(bird, "'Bird' GameObject was not found");
        
        Rigidbody2D myRigidbody2D = bird.GetComponent<Rigidbody2D>();
        Assert.IsNotNull(myRigidbody2D, "Rigidbody2D component was not found in 'Bird' GameObject.");
        
        // --------- SCORE CHECK --------- //
        GameObject score = GameObject.Find("Score");
        Text scoreText = score.GetComponent<Text>();
        Assert.AreEqual(scoreText.text, "0");
        
        // --------- OBSTACLE CONFIG --------- //
        GameObject pipeSpawner = GameObject.Find("Pipe Spawner");
        Assert.IsNotNull(pipeSpawner, "'Pipe Spawner' GameObject was not found");
        
        // --------- CREATE FAKE OBSTACLE --------- //
        fakePipe = Instantiate(pipeSpawner.GetComponent<PipeSpawnScript>().pipe);
        fakePipe.transform.position = new Vector3(2.5f, 0, 0);
        Assert.IsNotNull(fakePipe, "'Fake Pipe' was not instantiated");
        
        yield return new WaitForFixedUpdate();
        
        // --------- JUMP SIMULATION --------- //
        myRigidbody2D.AddForce(Vector3.up * 10, ForceMode2D.Impulse);
        
        yield return new WaitForSeconds(0.5f);
        
        // --------- CHANGE SCORE CHECK --------- //
        Assert.AreEqual(scoreText.text, "1");
    }

    [UnityTest]
    public IEnumerator Death_BirdDies_WhenHitObstacles()
    {
        // --------- PLAYER CHECK --------- //
        GameObject bird = GameObject.Find("Bird");
        Assert.IsNotNull(bird, "'Bird' GameObject was not found");
        
        BirdScript birdScript = bird.GetComponent<BirdScript>();
        Assert.IsNotNull(birdScript, "BirdScript component not found in 'Bird' GameObject.");
        
        // --------- OBSTACLE CONFIG --------- //
        GameObject pipeSpawner = GameObject.Find("Pipe Spawner");
        Assert.IsNotNull(pipeSpawner, "'Pipe Spawner' GameObject was not found");
        
        // --------- CREATE FAKE OBSTACLE --------- //
        fakePipe = Instantiate(pipeSpawner.GetComponent<PipeSpawnScript>().pipe);
        fakePipe.transform.position = new Vector3(2.5f, 3.5f, 0);
        Assert.IsNotNull(fakePipe, "'Fake Pipe' was not instantiated");
        
        // --------- PLAYER HIT OBSTACLE --------- //
        yield return new WaitForSecondsRealtime(0.5f);
        Assert.IsFalse(birdScript.birdIsAlive, "Bird did not hit obstacle");
        
        // --------- CLOSE GAMEOVER SCREEN | RESTART GAME --------- //
        Button restartButton = GameObject.Find("Game Over Screen/PlayAgain Button").GetComponent<Button>();
        new Interaction().Click().Button(restartButton).Perform();
    }

    [UnityTest]
    public IEnumerator Death_BirdDies_WhenOutOfCamera()
    {
        // --------- PLAYER CHECK --------- //
        GameObject bird = GameObject.Find("Bird");
        Assert.IsNotNull(bird, "'Bird' GameObject was not found");
        
        BirdScript birdScript = bird.GetComponent<BirdScript>();
        Assert.IsNotNull(birdScript, "BirdScript component not found in 'Bird' GameObject.");

        // --------- PLAYER FALL --------- //
        yield return new WaitUntil(() => bird.transform.position.y <= -13f );
        Assert.IsFalse(birdScript.birdIsAlive, "Bird did not hit obstacle");
        
        // --------- CLOSE GAMEOVER SCREEN | RESTART GAME --------- //
        Button restartButton = GameObject.Find("Game Over Screen/PlayAgain Button").GetComponent<Button>();
        new Interaction().Click().Button(restartButton).Perform();
    }

    [UnityTest]
    public IEnumerator Gravity_BirdFalls_WhenNotFlapping()
    {
        // --------- PLAYER CHECK --------- //
        GameObject bird = GameObject.Find("Bird");
        Assert.IsNotNull(bird, "'Bird' GameObject was not found");
        
        Rigidbody2D myRigidbody2D = bird.GetComponent<Rigidbody2D>();
        Assert.IsNotNull(myRigidbody2D, "Rigidbody2D component not found in 'Bird' GameObject.");
        
        // --------- BIRD FALLS --------- //
        Assert.IsTrue(myRigidbody2D.gravityScale != 0, "The gravity is not active in the bird");
        yield return new WaitForFixedUpdate();
        Assert.Less(myRigidbody2D.velocity.y, 0, "Bird did not fall");
    }

    [TearDown]
    public void TearDown()
    {
        DestroyImmediate(fakePipe);
    }
}
