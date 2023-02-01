using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;

//
// This script allows us to create anchors with
// a prefab attached in order to visbly discern where the anchors are created.
// Anchors are a particular point in space that you are asking your device to track.
//
/*
 // 이 스크립트를 사용하여 앵커를 만들 수 있습니다.
// 앵커가 생성된 위치를 시각적으로 식별하기 위해 부착된 프리팹.
// 앵커는 장치에 추적을 요청하는 공간의 특정 지점입니다.
 
 */

[RequireComponent(typeof(ARAnchorManager))]
[RequireComponent(typeof(ARRaycastManager))]
[RequireComponent(typeof(ARPlaneManager))]
public class AnchorCreator : MonoBehaviour
{
    // This is the prefab that will appear every time an anchor is created.
    // 이것은 앵커가 생성될 때마다 나타나는 프리팹입니다.
    [SerializeField]
    GameObject m_AnchorPrefab;

    // SerializeField : 에디터의 인스펙터창에서만 이 변수를 수정 가능하게하고 다른 "스크립트"에서는 이 변수 손 못대도록 하고싶을때 사용
    [SerializeField]
    GameObject m_MainUICanvus; // 0레벨 UI (확인버튼)
    [SerializeField]
    GameObject m_OneUICanvus; // 1레벨 UI (테스트로그, 서버와 JSon 통신, 서버에서 이미지 가져와 보이기)



    public TestLogCompo testLog; // 테스트 로그용 컴포넌트

    public float diff;



    /*
    0레벨 : 프로그램 시작 ~ 앵커 놓고 확인버튼 누르기까지 (이전버튼 눌러서 돌아올수있음.)
    1레벨 : 0레벨에서 확인버튼 눌림 -> 0레벨 UI 숨김 , 1레벨 UI 보임/ 서버와 통신해서 JSon 파싱 시작
    이거 분리 하긴 해야하는데 어떻게 굴려볼지 잘 안그려진다
     */
    private int nowLevel = 0;

    public GameObject AnchorPrefab
    {
        get => m_AnchorPrefab;
        set => m_AnchorPrefab = value;
    }

    public GameObject MainUICanvus
    {
        get => m_MainUICanvus;
        set => m_MainUICanvus = value;
    }
    public GameObject OneUICanvus
    {
        get => m_OneUICanvus;
        set => m_OneUICanvus = value;
    }

    // On Awake(), we obtains a reference to all the required components.
    // The ARRaycastManager allows us to perform raycasts so that we know where to place an anchor.
    // The ARPlaneManager detects surfaces we can place our objects on.
    // The ARAnchorManager handles the processing of all anchors and updates their position and rotation.

    /*
     
// Awake()에서 필요한 모든 구성 요소에 대한 참조를 얻습니다.
    // ARRaycastManager를 사용하면 앵커를 배치할 위치를 알 수 있도록 레이캐스트를 수행할 수 있습니다.
    // ARPlaneManager는 객체를 배치할 수 있는 표면을 감지합니다.
    // ARAnchorManager는 모든 앵커의 처리를 처리하고 위치와 회전을 업데이트합니다.
     */

    void Awake()
    {
        m_RaycastManager = GetComponent<ARRaycastManager>();
        m_AnchorManager = GetComponent<ARAnchorManager>();
        m_PlaneManager = GetComponent<ARPlaneManager>();

    }

    void Update()
    {
        // 탭이 없으면 다음에 Update()를 호출할 때까지 아무 작업도 수행하지 않습니다.
        if (Input.touchCount == 0)
            return;
        // 만일 UI 오브젝트가 터치대상이 되었을 경우 이 경우에도 별도의 처리를 AnchorCreator.cs에선 안함.
        if (EventSystem.current.currentSelectedGameObject)
            return;

        Touch touch = Input.GetTouch(0);
        CheckTouchAction(touch);
        //SetAnchorObject(touch);
    }
    
    void CheckTouchAction(Touch touch) {
        if (nowLevel < 1) {
            TouchActionLevelZero(touch);
        }
        else if (nowLevel == 1) {
            TouchActionLevelOne(touch);
        }
    }
    // 레벨이 0일때 터치 이벤트
    private void TouchActionLevelZero(Touch touch) {
        if (touch.phase == TouchPhase.Ended && anchorObejct != null)
        {
            SetLevel(0); // 터치가 종료됐을경우 Zero 캔버스 표시
        }
        else
        {
            SetLevel(-1);// 터치가 아직 진행중일경우 Zero캔버스 숨김
        }
        SetAnchorObject(touch);
    }
    // 레벨이 1일때 터치 이벤트
    private void TouchActionLevelOne(Touch touch) {
        // 레벨이 1일땐 터치스크린에 직접 입력을 받는다기보다 버튼,UI로만 조작하기때문에 여기선 아무것도 안함. -> 3레벨쯤이 됐을때 터치 이벤트로 모델의 동작 제어등을 하게될듯. 
        return;
    }
    void SetAnchorObject(Touch touch)
    {

        if (m_RaycastManager.Raycast(touch.position, s_Hits, TrackableType.PlaneWithinPolygon))
        {

            /*
             // 레이캐스트 적중은 거리에 따라 정렬되므로 첫 번째
            // 가장 가까운 적중이 됩니다.
             */
            var hitPose = s_Hits[0].pose;
            var hitTrackableId = s_Hits[0].trackableId;
            var hitPlane = m_PlaneManager.GetPlane(hitTrackableId);


            /*
             // 이는 레이캐스트 히트에 해당하는 평면의 영역에 앵커를 연결합니다.
            // 그런 다음 해당 지점에서 선택한 프리팹의 인스턴스를 인스턴스화합니다.
            // 이 프리팹 인스턴스는 앵커의 부모가 되어 프리팹의 위치가 일관되도록 합니다.
            // ARPlane에 연결된 앵커는 ARPlane의 정확한 위치가 조정될 때 ARAnchorManager에 의해 자동으로 업데이트되기 때문에 앵커와 함께.
             */

            ARAnchor anchor = m_AnchorManager.AttachAnchor(hitPlane, hitPose);

            if (anchor == null)
            {
                //Debug.Log("Error creating anchor.");
                AddLog("Error creating anchor.");
            }
            else
            {
                // Stores the anchor so that it may be removed later. / m_AnchorPoints.Add(anchor);
                if (anchorObejct == null)
                {
                    // anchor의 위치(transform)에 프리팹 생성
                    anchorObejct = Instantiate(m_AnchorPrefab, anchor.transform);
                }
                else
                {
                    //float speed = 10 * Time.deltaTime;
                    //anchorObejct.transform.position = Vector3.MoveTowards(anchorObejct.transform.position, anchor.transform.position, speed);
                    anchorObejct.transform.position = anchor.transform.position;
                }
            }

        }
    }
    // 모든 캔버스 안보이게 초기화 -> 월드에 보여지던 오브젝트도 초기화
    private void DisabledAllCanvus() {
        if (m_MainUICanvus.activeSelf == true)
        {
            m_MainUICanvus.SetActive(false);
        }
        if (m_OneUICanvus.activeSelf == true)
        {
            m_OneUICanvus.SetActive(false);
        }
        if (nowShowingObject != null) {
            Destroy(nowShowingObject);
            nowShowingObject = null;
        }

    }
    private void ShowZeroCanvus() {
        m_OneUICanvus.SetActive(false);
        m_MainUICanvus.SetActive(true);
    }
    private void ShowOneCanvus() {
        m_MainUICanvus.SetActive(false);
        m_OneUICanvus.SetActive(true);
    }
    public void SetLevel(int l) {
        AddLog("SetLevel : "+l);
        this.nowLevel = l;
        switch (nowLevel) {
            case -1: DisabledAllCanvus(); break;
            case 0: ShowZeroCanvus(); break;
            case 1: ShowOneCanvus(); break;
            default: break;
        }
    }

    public void ShowObject(GameObject inst) {
        if (anchorObejct == null) {
            AddLog("ShowObject : anchor is NULL");
            return;
        }
        if (nowShowingObject != null) {
            Destroy(nowShowingObject);
        }
        // anchor의 위치(transform)에 프리팹 생성
        //anchorObejct = Instantiate(m_AnchorPrefab, anchor.transform);
        //anchorObejct.transform.position = anchor.transform.position;

        AddLog("~!ShowObject!~");
        //Instantiate(inst, anchor.transform);
        //inst.transform.position = anchorObejct.transform.position;
        nowShowingObject = inst;
        nowShowingObject.transform.SetPositionAndRotation(anchorObejct.transform.position, anchorObejct.transform.rotation);
    }

    private void AddLog(string log) {
        if (testLog != null) {
            testLog.AddLog(log);
        }
    }

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    GameObject anchorObejct = null;


    GameObject nowShowingObject = null;

    ARRaycastManager m_RaycastManager;

    ARAnchorManager m_AnchorManager;

    ARPlaneManager m_PlaneManager;
}
