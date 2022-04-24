using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidUnit : MonoBehaviour
{
    Boids myBoids;
    List<BoidUnit> neighbours = new List<BoidUnit>();

    Vector3 targetVec;
    Vector3 egoVector;
    float speed;

    float additionalSpeed = 0;

    MeshRenderer myMeshRenderer;
    TrailRenderer myTrailRenderer;
    [SerializeField] private Color myColor;

    [SerializeField] float obstacleDistance;
    [SerializeField] float FOVAngle = 120;
    [SerializeField] float maxNeighbourCount = 50;
    [SerializeField] float neighbourDistance = 10;

    [SerializeField] LayerMask boidUnitLayer;
    [SerializeField] LayerMask obstacleLayer;

    Coroutine findNeighbourCoroutine;
    Coroutine calculateEgoVecotrCoroutine;
    public void InitializeUnit(Boids _boids, float _speed)
    {
        myBoids = _boids;
        speed = _speed;

        myTrailRenderer = GetComponentInChildren<TrailRenderer>();
        myMeshRenderer = GetComponentInChildren<MeshRenderer>();

        //색상 설정
        if (myBoids.randomColor)
        {
            myColor = new Color(Random.value, Random.value, Random.value);
            myMeshRenderer.material.color = myColor;
        }
        else
        {
            myColor = myMeshRenderer.material.color;
        }
    }
    void Start()
    {

    }

    void Update()
    {
        FindNeighbour();

        Vector3 cohesionVec = CalculateCohesionVector() * myBoids.cohesionWeight;
        Vector3 alignmentVec = CalculateAlignmentVector() * myBoids.alignmentWeight;
        Vector3 separationVec = CalculateSeparationVector() * myBoids.separationWeight;
        //추가적인 규칙
        Vector3 boundsVec = CalculateBoundsVector() * myBoids.boundsWeight;

        targetVec = cohesionVec + alignmentVec + separationVec + boundsVec;

        targetVec = Vector3.Lerp(this.transform.forward, targetVec, Time.deltaTime);
        this.transform.rotation = Quaternion.LookRotation(targetVec);
        this.transform.position += targetVec * speed * Time.deltaTime;
    }

    private void FindNeighbour()
    {
        neighbours.Clear();

        Collider[] colls = Physics.OverlapSphere(transform.position, 20f, boidUnitLayer);
        for (int i = 0; i < colls.Length; i++)
        {
            if (Vector3.Angle(transform.forward, colls[i].transform.position - transform.position) <= FOVAngle)
            {
                neighbours.Add(colls[i].GetComponent<BoidUnit>());
            }
            if (i > maxNeighbourCount)
            {
                break;
            }
        }
    }

    private Vector3 CalculateCohesionVector()
    {
        Vector3 cohesionVec = Vector3.zero;
        if (neighbours.Count > 0)
        {
            //이웃 unit들의 위치 더하기
            for (int i = 0; i < neighbours.Count; i++)
            {
                cohesionVec += neighbours[i].transform.position;
            }
        }
        else
        {
            //이웃이 없으면 Vector.zero 반환
            return cohesionVec;
        }

        //중심 위치로의 벡터 찾기
        cohesionVec /= neighbours.Count;
        cohesionVec -= transform.position;
        return cohesionVec;
    }

    private Vector3 CalculateAlignmentVector()
    {
        Vector3 alignmentVec = transform.forward;
        if (neighbours.Count > 0)
        {
            //이웃들이 향하는 방향의 평균 방향으로 이동
            for (int i = 0; i < neighbours.Count; i++)
            {
                alignmentVec += neighbours[i].transform.forward;
            }
        }
        else
        {
            //이웃이 없으면 그냥 forawrd로 이동
            return alignmentVec;
        }

        alignmentVec /= neighbours.Count;
        return alignmentVec;
    }

    private Vector3 CalculateSeparationVector()
    {
        Vector3 separationVec = Vector3.zero;
        if (neighbours.Count > 0)
        {
            //이웃들을 피하는 방향으로 이동
            for (int i = 0; i < neighbours.Count; i++)
            {
                separationVec += (transform.position - neighbours[i].transform.position);
            }
        }
        else
        {
            // 이웃이 없으면 그냥 Vector.zero 반환
            return separationVec;
        }
        separationVec /= neighbours.Count;
        return separationVec;
    }

    private Vector3 CalculateBoundsVector()
    {
        Vector3 offsetToCenter = myBoids.transform.position - transform.position;
        return offsetToCenter.magnitude >= myBoids.spawnRange ? offsetToCenter.normalized : Vector3.zero;
    }

    public void DrawVectorGizmo(int _depth)
    {
        for (int i = 0; i < neighbours.Count; i++)
        {
            if (_depth + 1 < myBoids.GizmoColors.Length - 1)
            {
                neighbours[i].DrawVectorGizmo(_depth + 1);
            }
            Debug.DrawLine(this.transform.position, neighbours[i].transform.position, myBoids.GizmoColors[_depth + 1]);
            Debug.DrawLine(this.transform.position, this.transform.position + targetVec, myBoids.GizmoColors[0]);
        }
    }
}
