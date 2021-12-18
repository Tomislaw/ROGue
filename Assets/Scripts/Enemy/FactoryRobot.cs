using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class FactoryRobot : MonoBehaviour
{
    public List<GameObject> enemyRoster = new List<GameObject>();
    public float spawnTime = 10;
    public float moveSpeed = 10;
    public Transform spawnLocation;
    public Transform moveLocation;

    private float timeToNextSpawn;
    private int currentSpawn = 0;

    private static readonly int Open = Animator.StringToHash("Open");
    protected Animator animator;
    void Start()
    {
        timeToNextSpawn = spawnTime;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timeToNextSpawn -= Time.fixedDeltaTime;
        if(timeToNextSpawn <= 0)
        {
            timeToNextSpawn = spawnTime;
            SpawnEnemies();
            animator.SetBool(Open, true);
        }
    }

    private void SpawnEnemies()
    {
        if (enemyRoster.Count == 0)
            return;

        if(currentSpawn >= enemyRoster.Count || currentSpawn < 0){
            currentSpawn = 0;
        }
        var newObject = Instantiate(enemyRoster[currentSpawn]);
        StartCoroutine(MoveFromTo(newObject.transform));

        currentSpawn++;

    }

    IEnumerator MoveFromTo(Transform objectToMove)
    {
        float step = (moveSpeed / (spawnLocation.position - moveLocation.position).magnitude) * Time.fixedDeltaTime;
        float t = 0;
        while (t <= 1.0f || objectToMove == null)
        {
            t += step;
            objectToMove.position = Vector3.Lerp(spawnLocation.position, moveLocation.position, t);
            yield return new WaitForFixedUpdate();       
        }
        objectToMove.position = moveLocation.position;
        animator.SetBool(Open, false);
    }
}
