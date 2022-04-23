using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidUnit : MonoBehaviour
{
    Vector3 targetVec;
    [SerializeField] float speed;
    Boids myBoids;

    List<BoidUnit> neighbours = new List<BoidUnit>();
    [SerializeField] LayerMask boidUnitLayer;

    public void InitializeUnit(Boids _boids, float _speed)
    {
        myBoids = _boids;
        speed = _speed;
    }
    void Start()
    {

    }

    void Update()
    {
        FindNeighbour();

        Vector3 cohesionVec = CalculateCohesionVector();
        Vector3 alignmentVec = CalculateAlignmentVector();

        targetVec = cohesionVec;

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
            neighbours.Add(colls[i].GetComponent<BoidUnit>());
        }
    }

    public Vector3 CalculateCohesionVector()
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
            //이웃이 없으면 vector3.zero 반환
            return cohesionVec;
        }

        //중심 위치로의 벡터 찾기
        cohesionVec /= neighbours.Count;
        cohesionVec -= transform.position;
        return cohesionVec;
    }

    public Vector3 CalculateAlignmentVector()
    {
        Vector3 alignmentVec = Vector3.zero;
        if(neighbours.Count > 0)
        {
            //이웃들이 향하는 방향의 평균 방향으로 이동
            for(int i = 0; i < neighbours.Count; i++)
            {
                alignmentVec += neighbours[i].transform.forward;
            }
        }
        else
        {
            //이웃이 없으면 그냥 앞
            return alignmentVec;
        }

        alignmentVec /= neighbours.Count;
        return alignmentVec;
    }
}
