using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FieldOfView : MonoBehaviour {
    public float viewRadius;
    [Range(1, 360)]
    public float viewAngle;

    int enemylayermask;
    int obstaclelayermask;

    public List<Transform> visibleTargets = new List<Transform>();
    //public List<Transform> visibleTargets = new ();

    public float meshResolution;

    public int meshResolveIterations;

    public float edgeDstThreshold;

    public float maskCutawayDst = .1f;

    Light light;

    public MeshFilter viewMeshFilter;
    Mesh viewMesh;

    private void Start()
    {
        light = GetComponentInChildren<Light>();
        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;
        enemylayermask = 1 << LayerMask.NameToLayer("Targets");
        obstaclelayermask = 1 << LayerMask.NameToLayer("Obstacles");
        StartCoroutine("FindTargetWithDelay", .2f);
    }

    private void Update()
    {
        light.spotAngle = viewAngle;
        DrawFieldOfView();
    }

    IEnumerator FindTargetWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisbleTarget(enemylayermask, obstaclelayermask);
        }
    }

    void FindVisbleTarget(int hitMask, int dontHitMask)
    {
        visibleTargets.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, hitMask);

        foreach (var i in targetsInViewRadius)
        {
            Transform target = i.transform;

            Vector3 dirToTarget = (target.position - transform.position).normalized;

            if (!(Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)) continue;

            float dstToTarget = Vector3.Distance(transform.position, target.position);

            if (Physics.Raycast(transform.position, dirToTarget, dstToTarget, dontHitMask)) continue;

            visibleTargets.Add(target);
        }
        
    }

    void DrawFieldOfView()
    {
        int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
        float stepAngleSize = viewAngle / stepCount;
        List<Vector3> ViewPoints = new List<Vector3>();
        ViewCastInfo oldViewCast = new ViewCastInfo();

        for (int i = 0; i <= stepCount; i++)
        {
            float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;
            ViewCastInfo newViewCast = ViewCast(angle);
            if(i > 0)
            {
                bool edgeDstThresholdExceeded = Mathf.Abs(oldViewCast.dst - newViewCast.dst) > edgeDstThreshold;
                if (oldViewCast.hit != newViewCast.hit || (oldViewCast.hit && newViewCast.hit && edgeDstThresholdExceeded))//hit 정보를 레이어로 검사해서 같은 레이어가 맞더라도 거리가 멀면 테두리 검사 실행
                {
                    EdgeInfo edge = FindEdge(oldViewCast, newViewCast);//findedge 함수에 들어가서 분석해도 좋은데, 대충 raycast를 쐈을때 자연스럽지 않게 표시되는 테두리 부분을 찾는 함수임. 궁금하면 내일 그려드림
                    if(edge.pointA != Vector3.zero)//내가 들어가면 안되니 제로벡터는 제외
                    {
                        ViewPoints.Add(edge.pointA);
                    }
                    if (edge.pointB != Vector3.zero)
                    {
                        ViewPoints.Add(edge.pointB);
                    }
                    
                }
            }
            ViewPoints.Add(newViewCast.point);
            oldViewCast = newViewCast;
        }

        int vertexCount = ViewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangle = new int[(vertexCount - 2) * 3];


        vertices[0] = new Vector3(0, 0, 0);//삼각형이 처음 생성될 위치
        for(int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(ViewPoints[i]) + Vector3.forward * maskCutawayDst + new Vector3(0,2,0); //내가 아닌 삼각형의 정점(꼭짓점) 좌표 저장~~

            if (i < vertexCount - 2)//매쉬를 생성할 때 vertices에 들어갈 정점의 인덱스 위치를 표현.
            {
                triangle[i * 3] = 0;
                triangle[i * 3 + 1] = i + 1;
                triangle[i * 3 + 2] = i + 2;
            }
        }

        viewMesh.Clear();
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangle;
        viewMesh.RecalculateNormals();
    }

    EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
    {
        float minAngle = minViewCast.angle;
        float maxAngle = maxViewCast.angle;
        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

        for(int i = 0; i < meshResolveIterations; i++)
        {
            
            float angle = (minAngle + maxAngle) / 2;
            ViewCastInfo newViewCast = ViewCast(angle);

            bool edgeDstThresholdExceeded = Mathf.Abs(minViewCast.dst - newViewCast.dst) > edgeDstThreshold;
            if (newViewCast.hit == minViewCast.hit && !edgeDstThresholdExceeded)
            {
                minAngle = angle;
                minPoint = newViewCast.point;
            }
            else
            {
                maxAngle = angle;
                maxPoint = newViewCast.point;
            }
        }

        return new EdgeInfo(minPoint, maxPoint);
    }

    ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 dir = DirFromAngle(globalAngle, true);
        RaycastHit hitinfo;

        if(Physics.Raycast(transform.position, dir, out hitinfo, viewRadius, obstaclelayermask)){
            return new ViewCastInfo(true, hitinfo.point, hitinfo.distance, globalAngle);
        }
        
        return new ViewCastInfo(false, transform.position + dir * viewRadius, viewRadius, globalAngle);
    }


    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y; //플레이어의 y축 각도 값 더함
        }

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float dst;
        public float angle;

        public ViewCastInfo(bool _hit, Vector3 _point, float _dst, float _angle)
        {
            hit = _hit;
            point = _point;
            dst = _dst;
            angle = _angle;
        }
    }

    public struct EdgeInfo
    {
        public Vector3 pointA;
        public Vector3 pointB;

        public EdgeInfo(Vector3 _pointA, Vector3 _pointB)
        {
            pointA = _pointA;
            pointB = _pointB;
        }
    }

}
