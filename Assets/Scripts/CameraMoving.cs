using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMoving : MonoBehaviour
{
    [SerializeField]
    private float camRotLerpSpeed;
    [SerializeField]
    private float camPosLerpSpeed;
    public GameObject Player;
    public float zoomCam = 1f;
    public float maxZoom = 2f;
    public float minZoom = 1f;
    public float CamVecRotSpeed = 1;

    public GameObject camera;
    float camRot = 0;//카메라 벡터 위치 각도 돌려주는거, 로컬 각도 아님
    private Vector3 nextCamPos;
    protected Vector3 cameraVec;

    PlayerIO _playerio;
    InputAction QEAction;

    // Start is called before the first frame update
    void Start()
    {
        _playerio = new PlayerIO();
        QEAction = _playerio.KeyBoard.Action;//PlayerIO에 있는 Action을 가져옴. 자세한건 new input system을 찾아보세요 앞 QE는 Q와 E를 눌렀을때 나오는 액션
        QEAction.Enable();//꼭 활성화 해줘야함
        cameraVec = camera.transform.position - Player.transform.position;
        nextCamPos = (transform.position + cameraVec) * zoomCam;
        
    }

    // Update is called once per frame
    void Update()
    {
        camRot += QEAction.ReadValue<float>();
        ZoomCam();
        CameraLerpPos(Player, camera, CamVecRotSpeed);
        CameraLerpRot();
    }

    //private void FixedUpdate()
    //{
    //}

    void CameraLerpRot()
    {
        Quaternion target = Quaternion.LookRotation(Player.transform.position - transform.position, Vector3.up);// 대상 바라보게 하는 코드
        transform.rotation = Quaternion.Lerp(transform.rotation, target, camRotLerpSpeed * Time.deltaTime);
        
    }

    void CameraLerpPos(GameObject player, GameObject camera, float speed)
    {
        Quaternion reRot = Quaternion.Euler(0, -camRot * speed, 0);
        nextCamPos = (Player.transform.position + reRot * cameraVec / zoomCam);//기즈모로 표시해둔 빨간 원 
        camera.transform.position = Vector3.Lerp(camera.transform.position, nextCamPos, camPosLerpSpeed * Time.deltaTime);
    }

    void ZoomCam()
    {
        zoomCam += ((Input.GetAxis("Mouse ScrollWheel")));//스크롤 리턴값 -0.1 ~ 0.1
        zoomCam = Mathf.Clamp(zoomCam, minZoom, maxZoom);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(nextCamPos, 0.3f);
        Gizmos.DrawLine(transform.position, nextCamPos);
    }

}
