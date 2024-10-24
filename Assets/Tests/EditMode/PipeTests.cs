using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using System.Reflection;

public class PipeTests
{
    GameObject pipePrefab;
    GameObject pipeInstance;
    PipeMoveScript pipeMove;

    GameObject pipeSpawner;
    PipeSpawnScript pipeSpawn;
    
    [SetUp]
    public void SetUp()
    {
        pipePrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Pipe.prefab");
        Assert.IsNotNull(pipePrefab, "Pipe Prefab not found in Resources (Assets/Prefabs/).");
        
        pipeInstance = Object.Instantiate(pipePrefab);
        pipeMove = pipeInstance.GetComponent<PipeMoveScript>();
        Assert.IsNotNull(pipeMove, "PipeMoveScript component not found in 'Pipe' Prefab.");
        
        pipeSpawner = GameObject.Find("Pipe Spawner");
        pipeSpawn = pipeSpawner.GetComponent<PipeSpawnScript>();
        Assert.IsNotNull(pipeSpawn, "PipeSpawnScript component not found in 'Pipe Spawner' GameObject.");
    }
    
    [Test]
    public void PipePrefab_CorrectMovementProperties_ShouldBeSet()
    {
        Assert.IsNotNull(pipeMove, "PipeMoveScript component not found in 'Pipe' Prefab.");
        
        Assert.AreEqual(6, pipeMove.moveSpeed, "Speed should be 6.");
        Assert.IsNotNull(pipeMove.moveSpeed, "Speed not found in PipeMoveScript.");
        
        Assert.AreEqual(-40, pipeMove.deadZone, "Dead Zone should be -40.");
        Assert.IsNotNull(pipeMove.deadZone, "Dead Zone not found in PipeMoveScript.");
    }

    [Test]
    public void PipeSpawner_InitialPosition_ShouldBeCorrect()
    {
        Assert.AreEqual(pipeInstance.transform.position, pipeMove.transform.position, "Position should be pipe position");
    }

    [Test]
    public void PipeSpawner_CorrectInitialProperties_ShouldBeSet()
    {
        Assert.IsNotNull(pipeSpawn, "PipeSpawnScript component not found in 'Pipe Spawner' GameObject.");
        Assert.IsNotNull(pipeSpawn.pipe, "Pipe Prefab not found in 'Pipe Spawner' GameObject.");
        
        Assert.AreEqual(2.5, pipeSpawn.spawnRate, "SpawnRate should be 2.5.");
        Assert.IsNotNull(pipeSpawn.spawnRate, "Spawn Rate not found in PipeSpawnScript.");
        
        Assert.AreEqual(7, pipeSpawn.heightOffset, "Height Offset should be 7.");
        Assert.IsNotNull(pipeSpawn.heightOffset, "Height Offset not found in PipeSpawnScript.");
        
        // Para pegar uma propriedade privada dentro do script
        FieldInfo timerField = typeof(PipeSpawnScript).GetField("_timer", BindingFlags.NonPublic | BindingFlags.Instance);
        Assert.IsNotNull(timerField, "Field _timer not found in PipeSpawnScript.");
        
        // Transforma em float para pegar o valor
        float timerValue = (float)timerField.GetValue(pipeSpawn);
        Assert.AreEqual(0f, timerValue, "Timer value should be 0.");
    }
    
    [TearDown]
    public void TearDown()
    {
        if (pipeInstance != null)
        {
            Object.DestroyImmediate(pipeInstance);
        }
    }
}
