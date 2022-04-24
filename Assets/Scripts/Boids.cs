using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

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
    public float obstacleWeight = 10;
    public float egoWeight = 1;

    public BoidUnit currUnit;
    [SerializeField] LayerMask unitLayer;

    [SerializeField] CinemachineVirtualCamera originCam;
    [SerializeField] CinemachineFreeLook unitCam;

    public bool cameraFollowUnit = false;
    public bool randomColor = false;
    public bool protectiveColor = false;
    public Color[] GizmoColors;

    public Slider cohesionSlider;
    public Slider alignmentSlider;
    public Slider separationSlider;
    public Toggle colorToggle;

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
        cohesionWeight = cohesionSlider.value;
        alignmentWeight = alignmentSlider.value;
        separationWeight = separationSlider.value;
        colorToggle.onValueChanged.AddListener((value) =>
        {
            randomColor = value;
        });

        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * 1000, Color.red, 3f);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, unitLayer))
            {
                currUnit = hit.transform.GetComponent<BoidUnit>();
            }
            else
            {
                currUnit = null;
            }
        }

        if (currUnit)
        {
            currUnit.DrawVectorGizmo(0);
        }

        if (cameraFollowUnit && currUnit != null)
        {
            originCam.gameObject.SetActive(false);
            unitCam.gameObject.SetActive(true);
            unitCam.Follow = currUnit.transform;
            unitCam.LookAt = currUnit.transform;
        }
        else
        {
            originCam.gameObject.SetActive(true);
            unitCam.gameObject.SetActive(false);
        }
    }
}
