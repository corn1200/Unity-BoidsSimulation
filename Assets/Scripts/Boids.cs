using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boids : MonoBehaviour
{
    [SerializeField] private BoidUnit boidUnitPrefab;
    public int boidCount;
    public float spawnRange = 30;
    public Vector2 speedRange;

    //Cohesion - 주변 무리의 중심 방향으로 이동
    //Alignment - 주변 무리가 향하는 평균 방향으로 전환
    //Separation - 뭉쳐있는 무리를 피해 이동
    public float cohesionWeight = 1;
    public float alignmentWeight = 1;
    public float separationWeight = 1;

    public float boundsWeight = 1;
    public float egoWeight = 1;

    public bool randomColor = false;
    public bool protectiveColor = false;
    public Color[] GizmoColors;

    void Start()
    {
        for (int i = 0; i < boidCount; i++)
        {
            Vector3 randomVec = Random.insideUnitSphere;
            randomVec *= spawnRange;
            Quaternion randomRot = Quaternion.Euler(0, Random.Range(0, 360f), 0);
            BoidUnit currUnit = Instantiate(boidUnitPrefab, this.transform.position + randomVec, randomRot);
            currUnit.transform.SetParent(this.transform);
            currUnit.InitializeUnit(this, Random.Range(speedRange.x, speedRange.y));
        }
    }

    void Update()
    {

    }
}
