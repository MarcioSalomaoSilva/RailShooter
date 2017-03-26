using UnityEngine;
using System.Collections;
//
public class Camera_Controller : MonoBehaviour {
    //
    public static Camera_Controller instance;
    //
    public enum CameraState
    {
        Cockpit, Near, Far, Normal
    }
    public CameraState camState;
    //
    public bool cinematicMode;
    public bool lockedMode;
    //
    public UnityEngine.Camera cam;
    public GameObject pc;
    //
    Texture scope;
    //
    Vector3 offSet;
    float angle;
    // Use this for initialization
    void OnEnable() {
        Player_Input.Boost += Boost;
        Player_Input.Brake += Brake;
    }
    void Start() {
        //
        instance = this;
        //
        GetVariables();
    }
    // Update is called once per frame
    void Update() {
        //
        ApplyMovement();
        //
        SetReferencePosition();
        //
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 60, Player_Manager.instance.cameraSmooth * Time.deltaTime); // normalView 
    }
    //
    public void GetVariables() {
        cam = this.gameObject.GetComponent<UnityEngine.Camera>();
        pc = GameObject.Find("Ship");
    }
    //
    public void ApplyMovement() {
        //
        GetLocation();
        //
        this.transform.position = Vector3.Lerp(this.transform.position, offSet, (Player_Motor.instance.movementSpeed) * Time.deltaTime);
        transform.RotateAround(pc.transform.position, -Vector3.up, Player_Input.instance.rightVector.y * angle * Player_Manager.instance.universalScale);
        transform.RotateAround(pc.transform.position, Vector3.right, Player_Input.instance.rightVector.x * angle * Player_Manager.instance.universalScale);
        //
        Vector3 clampPosition = this.transform.localPosition;
        clampPosition.x = Mathf.Clamp(this.transform.localPosition.x, -Player_Manager.instance.horizantalClamp.x / 2 * Player_Manager.instance.universalScale, 
            Player_Manager.instance.horizantalClamp.y / 2 * Player_Manager.instance.universalScale);
        clampPosition.y = Mathf.Clamp(this.transform.localPosition.y, -Player_Manager.instance.verticalClamp.x / 2 * Player_Manager.instance.universalScale, 
            Player_Manager.instance.verticalClamp.y / 2 * Player_Manager.instance.universalScale);
        this.transform.localPosition = clampPosition;
        //
        Vector3 midPointPos = Vector3.Lerp(Camera_Reference.instance.posReference.transform.position, Camera_Reference.instance.shipReference.transform.position, 0.5f);
        Camera_Reference.instance.midpoint.transform.position = midPointPos;
        Vector3 targetDir = midPointPos - this.transform.position;
        Vector3 lookDirection = Vector3.RotateTowards(transform.forward, targetDir, 2f * Time.deltaTime, 0);
        this.transform.rotation = Quaternion.LookRotation(lookDirection);
    }
    //
    void Boost()
    {
        float farview = 70;
        float currentView;
        //
        currentView = Mathf.Lerp(cam.fieldOfView, farview, Player_Manager.instance.cameraSmooth * Time.deltaTime);
        cam.fieldOfView = currentView;
    }
    //
    void Brake()
    {
        float nearView = 45;
        float currentView;
        //
        currentView = Mathf.Lerp(cam.fieldOfView, nearView, Player_Manager.instance.cameraSmooth * Time.deltaTime);
        cam.fieldOfView = currentView;
        //
    }
    //
    void GetLocation() {
        switch (Player_Input.instance.cameraLocation) {
            case 4:
                offSet = Camera_Reference.instance.shipReference.transform.position;
                angle = 0;
                //
                this.transform.position = Player_Manager.instance.ship.transform.position;
                this.transform.rotation = Player_Manager.instance.ship.transform.rotation;
                if (Vector3.Distance(this.transform.position, offSet) < 0.15f) {
                    camState = CameraState.Cockpit;
                }
                break;
            case 3:
                offSet = Vector3.Lerp(Camera_Reference.instance.cameraNearReference.transform.position, Camera_Reference.instance.shipNearReference.transform.position, 0.15f);
                angle = 2f;
                if (Vector3.Distance(this.transform.position, offSet) < 0.15f) {
                    camState = CameraState.Near;
                }
                break;
            case 2:
                offSet = Vector3.Lerp(Camera_Reference.instance.cameraNormalReference.transform.position, Camera_Reference.instance.shipNormalReference.transform.position, 0.15f);
                angle = 1.75f;
                if (Vector3.Distance(this.transform.position, offSet) < 0.15f) {
                    camState = CameraState.Normal;
                }
                break;
            case 1:
                offSet = Vector3.Lerp(Camera_Reference.instance.cameraFarReference.transform.position, Camera_Reference.instance.shipFarReference.transform.position, 0.15f);
                angle = 1.5f;
                if (Vector3.Distance(this.transform.position, offSet) < 0.15f) {
                    camState = CameraState.Far;
                }
                break;
        }
    }
    //
    void SetReferencePosition()
    {
        Camera_Reference.instance.posReference.transform.localPosition = new Vector3(0, 0, Vector3.Distance(this.transform.position, Player_Manager.instance.ship.transform.position));
        Camera_Reference.instance.shipReference.transform.position = Player_Manager.instance.ship.transform.position;
    }
    //
    public void OnDrawGizmos()
    {
        if (Game_Manager.instance.debugging == Game_Manager.Debug.True || Game_Manager.instance.debugShip == true)
        {
            Gizmos.color = Color.grey;
            Gizmos.DrawLine(Camera_Reference.instance.posReference.transform.position, Camera_Reference.instance.shipReference.transform.position);
            Gizmos.color = new Color(1, 0.92f, 0.016f, 0.5f); // yellow
            Gizmos.DrawLine(this.transform.position, Camera_Reference.instance.midpoint.transform.position);
            Gizmos.color = new Color(1, 0, 0, 0.5f); // red
            Gizmos.DrawLine(this.transform.position, Camera_Reference.instance.shipReference.transform.position);
            Gizmos.color = new Color(0, 0, 1, 0.5f); // blue
            Gizmos.DrawLine(this.transform.position, Camera_Reference.instance.posReference.transform.position);
            Gizmos.DrawCube(Camera_Reference.instance.posReference.transform.position, Vector3.one / 3);
            Gizmos.color = new Color(1, 0, 0, 0.5f); // red
            Gizmos.DrawCube(Camera_Reference.instance.shipReference.transform.position, Vector3.one / 3);
            Gizmos.color = new Color(1, 0.92f, 0.016f, 0.5f); // yellow
            Gizmos.DrawCube(Camera_Reference.instance.midpoint.transform.position, Vector3.one / 3);
            //
            Gizmos.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);//grey
            Gizmos.DrawCube(Camera_Reference.instance.cameraNearReference.transform.position, Vector3.one / 4);
            Gizmos.DrawCube(Camera_Reference.instance.cameraNormalReference.transform.position, Vector3.one / 4);
            Gizmos.DrawCube(Camera_Reference.instance.cameraFarReference.transform.position, Vector3.one / 4);
            Gizmos.DrawLine(Camera_Reference.instance.cameraFarReference.transform.position, Camera_Reference.instance.shipReference.transform.position);
            //
            Gizmos.DrawCube(Camera_Reference.instance.shipNearReference.transform.position, Vector3.one / 4);
            Gizmos.DrawCube(Camera_Reference.instance.shipNormalReference.transform.position, Vector3.one / 4);
            Gizmos.DrawCube(Camera_Reference.instance.shipFarReference.transform.position, Vector3.one / 4);
            //
            Gizmos.DrawCube(Vector3.Lerp(Camera_Reference.instance.cameraNearReference.transform.position, Camera_Reference.instance.shipNearReference.transform.position, 0.15f), Vector3.one / 4);
            Gizmos.DrawCube(Vector3.Lerp(Camera_Reference.instance.cameraFarReference.transform.position, Camera_Reference.instance.shipFarReference.transform.position, 0.15f), Vector3.one / 4);
            Gizmos.DrawCube(Vector3.Lerp(Camera_Reference.instance.cameraNormalReference.transform.position, Camera_Reference.instance.shipNormalReference.transform.position, 0.15f), Vector3.one / 4);
            Gizmos.DrawLine(Vector3.Lerp(Camera_Reference.instance.cameraFarReference.transform.position, Camera_Reference.instance.shipFarReference.transform.position, 0.15f),
                Camera_Reference.instance.shipReference.transform.position);
            //
            Gizmos.DrawLine(this.transform.position, offSet);
            //
            Gizmos.DrawLine(Camera_Reference.instance.shipFarReference.transform.position, Camera_Reference.instance.cameraFarReference.transform.position);
            Gizmos.DrawLine(Camera_Reference.instance.shipNormalReference.transform.position, Camera_Reference.instance.cameraNormalReference.transform.position);
            Gizmos.DrawLine(Camera_Reference.instance.shipNearReference.transform.position, Camera_Reference.instance.cameraNearReference.transform.position);
            //
            Gizmos.DrawLine(Camera_Reference.instance.shipFarReference.transform.position, Camera_Reference.instance.shipReference.transform.position);
            //
            Gizmos.matrix = Matrix4x4.TRS(
                cam.transform.position,
                cam.transform.rotation,
                Vector3.one);

            Gizmos.DrawFrustum(
                Vector3.zero,
                cam.fieldOfView,
                cam.farClipPlane*0.01f,
                cam.nearClipPlane,
                cam.aspect);
        }
    }
}
