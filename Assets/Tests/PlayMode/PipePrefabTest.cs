using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using Debug = UnityEngine.Debug;

public class PipePrefabTest
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
    public IEnumerator Destroy_OffScreen_ObjectDestruction()
    {
        bool isDestroyed = false;
        
        // --------- PIPE CHECK --------- //
        GameObject pipeClone = GameObject.Find("Pipe(Clone)");
        Assert.IsNotNull(pipeClone, "'Pipe Spawner' GameObject was not found");
        
        PipeMoveScript pipeMoveScript = pipeClone.GetComponent<PipeMoveScript>();
        pipeMoveScript.moveSpeed = 25f;
        
        // --------- PLAYER DESTRUCTION --------- //
        GameObject bird = GameObject.Find("Bird");
        Assert.IsNotNull(bird, "'Bird' GameObject was not found");
        Object.Destroy(bird);
        
        // --------- PIPE RENAME --------- //
        pipeClone.name = "PipeClone";
        
        // --------- POSITION PIPE CHECK --------- //
        while (pipeClone.transform.position.x >= -39.9f)
        {
            Debug.Log(pipeClone.transform.position.x);
            yield return null;
        }

        yield return new WaitForFixedUpdate();
        

        // --------- PIPE DESTROYED CHECK --------- //
        if (pipeClone == null) isDestroyed = true;
        /*for (var i = 0; i < 3; i++) // 3 tentativas
        {
            if (pipeClone == null)
            {
                isDestroyed = true;
                break;
            }
            yield return null;
        }*/

        Assert.IsTrue(isDestroyed, "'Pipe' GameObject was not null");
        //Assert.IsNull(pipeClone, "'Pipe' GameObject was not null");
    }

    [Test]
    [Order(2)]
    public void Pipe_IsBeingInstantiated_WhenSceneIsLoaded()
    {
        // --------- START WATCH --------- //
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        
        // --------- GAME OBJECT CHECK --------- //
        GameObject pipeSpawner = GameObject.Find("Pipe Spawner");
        Assert.IsNotNull(pipeSpawner, "'Pipe Spawner' GameObject was not found");
        
        // --------- COMPONENT SCRIPT CHECK --------- //
        PipeSpawnScript pipeScript = pipeSpawner.GetComponent<PipeSpawnScript>();
        Assert.IsNotNull(pipeScript, "PipeSpawnScript component was not found in 'Pipe Spawner' GameObject");
        
        // --------- NAME CHECK --------- //
        Assert.AreEqual("Pipe Spawner", pipeSpawner.name);
        
        // --------- GAME OBJECT INSTANTIATE CHECK --------- //
        GameObject pipeClone = null;
        while (pipeClone == null)
        {
            pipeClone = GameObject.Find("Pipe(Clone)");
        }
        Assert.IsNotNull(pipeClone, "O GameObject chamado 'Pipe(Clone)' não foi encontrado na cena.");
        
        // --------- CHECK LOAD TIME --------- //
        stopwatch.Stop();
        Debug.Log($"Tempo para instanciar o Pipe GameObject: {stopwatch.ElapsedMilliseconds} ms");
        Assert.IsTrue(stopwatch.ElapsedMilliseconds < 200, $"Tempo para instanciar o Pipe GameObject passou do limite de 200 ms.");
    }
}



/*public class MoverTests
{
    private GameObject testObject;
    private Mover moverScript;

    [SetUp]
    public void Setup()
    {
        testObject = new GameObject();
        moverScript = testObject.AddComponent<Mover>();
        moverScript.moveSpeed = 5f;
        moverScript.deadZone = -10f;
        testObject.transform.position = new Vector3(0, 0, 0);
    }

    [UnityTest]
    [Order(1)] // Define a prioridade como 1
    public IEnumerator TestObjectDestruction()
    {
        while (testObject.transform.position.x >= moverScript.deadZone)
        {
            moverScript.Update();
            yield return null;
        }

        Assert.IsNull(testObject);
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(testObject);
    }
}*/
