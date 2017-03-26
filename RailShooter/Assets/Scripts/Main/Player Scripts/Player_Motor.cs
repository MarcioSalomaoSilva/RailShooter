using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
//
public class Player_Motor : MonoBehaviour {
	//
	public static Player_Motor instance;
	//
	public enum CurrentMode 
	{
		Rail, Range
	}
	public CurrentMode curMode;
	//
	float speed;
	Vector3[] path;
    public Vector3 currentWayPoint;
    int targetIndex;
    GameObject[] targets;
    List<GameObject> finalTargets;
    public Vector3 target;
    //
    //public float transition;
    //
    public bool requestPath;
    public bool lookAtPath;
    bool pressed;
    //
    public float movementSpeed;
    public float tiltSpeed;
    public float breakSpeed;
    public float boostSpeed;
    //
    void OnEnable() {
        Player_Input.Brake += Brake;
        Player_Input.Boost += Boost;
        Player_Input.TiltLeft += TiltLeft;
        Player_Input.TiltRight += TiltRight;
        //
        Player_Stats.ChangeMode += ChangeMode;
    }
    void Start () {
		//
		instance = this;
		//
		speed = Player_Manager.instance.forwardSpeed * Time.deltaTime;
        SetMovementSpeed();
        curMode = CurrentMode.Range;
        //GetTargets();
    }
    //
    void Update() {
        CheckMode();
        CheckMovementSpeed();
        //
        //transition += Time.deltaTime / 3;
    }
    void SetMovementSpeed() {
        movementSpeed = Player_Manager.instance.movementSpeed; 
        tiltSpeed  = movementSpeed * 2f;
        breakSpeed = movementSpeed * 0.6f;
        boostSpeed = movementSpeed * 2.5f;
    }
    //
    void ChangeMode() {
        if (curMode == CurrentMode.Rail) {
            lookAtPath = false;
            //StopCoroutine("FollowPath");
            curMode = CurrentMode.Range;
        } else if (curMode == CurrentMode.Range) {
            //Player_Targeting.GetTargets(this.transform.position);
            //PathRequestManager.RequestPath(transform.position, target, OnPathFound);
            curMode = CurrentMode.Rail;
        } else {
            lookAtPath = false;
            StopCoroutine("FollowPath");
            curMode = CurrentMode.Range;
        }
    }
	//
    public void CheckMode () {
        
        switch (curMode)
        {
            case CurrentMode.Rail:
                this.transform.localPosition += Player_Input.instance.leftVector * movementSpeed * Time.deltaTime;
                this.transform.parent.position += this.transform.parent.forward * (movementSpeed*4) * Time.deltaTime;
                ApplyScreenCenterRail();
                ApplyClamp(18f, 8.2f, 11.5f, 7f, 9.6f, 5.3f);
                Rotation(Player_Input.instance.leftVector, movementSpeed);
                break;
            case CurrentMode.Range:
                //this.transform.parent.position += new Vector3(0, Player_Input.instance.leftVector.y * movementSpeed * Time.deltaTime); // use translate instead
                //this.transform.parent.Rotate(new Vector3(0, Player_Input.instance.leftVector.x, 0) * Time.deltaTime * movementSpeed * 4);
                //this.transform.parent.position += this.transform.parent.forward * movementSpeed * Time.deltaTime; // use translate instead
                ApplyScreenCenterRange();
                Rotation(Player_Input.instance.leftVector, movementSpeed);
                
                break;
        }
    }
    public void Brake() {
        movementSpeed = breakSpeed;
    }
    public void Boost() {
        movementSpeed = boostSpeed;
    }
    //
    public void TiltRight() {
        if (Player_Input.instance.leftVector.x > Player_Manager.instance.controllerDeadZone) {
            movementSpeed = tiltSpeed;
        }
        //
        this.transform.localRotation = Quaternion.Slerp(this.transform.localRotation, Quaternion.Euler(0, 0, Player_Input.instance.right *  - 100),
            Player_Manager.instance.rotationSmooth);
    }
    public void TiltLeft() {
        if (Player_Input.instance.leftVector.x < -Player_Manager.instance.controllerDeadZone) {
            movementSpeed = tiltSpeed;
        }
        //
        this.transform.localRotation = Quaternion.Slerp(this.transform.localRotation, Quaternion.Euler(0, 0, Player_Input.instance.left *   100), 
            Player_Manager.instance.rotationSmooth);
    }
    //
    public void CheckMovementSpeed() {
        switch (Player_Input.instance.curState) {
            case Player_Input.CurrentState.Cooling:
                movementSpeed = breakSpeed;
                break;
            case Player_Input.CurrentState.ModeChange:
                movementSpeed = breakSpeed;
                break;
            default:
                movementSpeed = Player_Manager.instance.movementSpeed;
                break;
        }
    }
    //
    public void Rotation (Vector3 leftVector, float movementSpeed) {
        //
        Quaternion origenRotation = Quaternion.LookRotation(Vector3.zero);
        Quaternion turnRotation = CalculateTurnRotation(); //CalculateTurnRotation(); // Quaternion.Euler(0, 0, 45)
        Quaternion vectorRotation = Quaternion.LookRotation(leftVector);
        float slerpValue = leftVector.magnitude / 2;
        //
        Quaternion finalRotation = Quaternion.Slerp(origenRotation, vectorRotation * turnRotation, slerpValue);
        //
        if (leftVector.magnitude > Player_Manager.instance.controllerDeadZone/2) {
            //if (leftVector.x > Player_Manager.instance.controllerDeadZone)
            this.transform.localRotation = Quaternion.Slerp (this.transform.localRotation, finalRotation, Player_Manager.instance.rotationSmooth * Time.deltaTime*20); // forward, leftvector rotates and leftvector points in the direction// * Quaternion.Euler(0, 0, -45)
        } else {
			this.transform.localRotation = Quaternion.Slerp (this.transform.localRotation, Quaternion.LookRotation (Vector3.zero), Player_Manager.instance.rotationSmooth);
		}
	}
    public Quaternion CalculateTurnRotation() {
        Quaternion turnRotation = Quaternion.Euler(0,0, CalculateAngle(Player_Input.instance.leftVector)) ;
        return turnRotation;
    }
    public float CalculateAngle(Vector3 vector) {
        float angle;
        //
        bool dirForward = false;
        bool dirBackward = false;
        bool dirLeft = false;
        bool dirRight = false;
        //
        if (vector.x > Player_Manager.instance.controllerDeadZone/2) {
            dirForward = true;
        }
        if (vector.x < -Player_Manager.instance.controllerDeadZone/2) {
            dirBackward = true;
        }
        if (vector.y < -Player_Manager.instance.controllerDeadZone/2) {
            dirLeft = true;
        }
        if (vector.y > Player_Manager.instance.controllerDeadZone/2) {
            dirRight = true;
        }
        if (dirForward) {
            if (dirLeft) {
                //curCooling = CurrentCooling.WeaponLeft;
                angle = 0;
            } else if (dirRight) {
                //curCooling = CurrentCooling.WeaponRight;
                angle = 0;
            } else {
                //curCooling = CurrentCooling.Cannon;
                angle = 0;
            }
        } else if (dirBackward) {
            if (dirLeft) {
                //curCooling = CurrentCooling.EngineLeft;
                angle = 0;
            } else if (dirRight) {
                //curCooling = CurrentCooling.EngineRight;
                angle = 0;
            } else {
                //curCooling = CurrentCooling.Generator;
                angle = 0;
            }
        } else if (dirLeft) {
            //curCooling = CurrentCooling.WingLeft;
            angle = 0;
        } else if (dirRight) {
            //curCooling = CurrentCooling.WingRight;
            angle = 0;
        } else {
            //curCooling = CurrentCooling.Shield;
            angle = 0;
        }
        return angle;
    }
    //
	public void ApplyScreenCenterRail (){
        if (Player_Input.instance.leftVector.magnitude < Player_Manager.instance.controllerDeadZone){
            this.transform.position = Vector3.Lerp(this.transform.position, Camera_Reference.instance.posReference.transform.position, Time.deltaTime/2);
        }
    }
    public void ApplyScreenCenterRange () {
        this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, Vector3.zero, Time.deltaTime / 2);
    }
    //
    public void ApplyClamp(float farX, float farY, float normalX, float normalY, float nearX, float nearY)
	{
        if (Camera_Controller.instance.camState == Camera_Controller.CameraState.Far) {
            this.transform.localPosition = 
                new Vector3(Mathf.Clamp(this.transform.localPosition.x, -farX * Player_Manager.instance.universalScale, farX * Player_Manager.instance.universalScale), 
                Mathf.Clamp(this.transform.localPosition.y, -farY * Player_Manager.instance.universalScale, farY * Player_Manager.instance.universalScale), 0);
        }
        if (Camera_Controller.instance.camState == Camera_Controller.CameraState.Normal) {
            this.transform.localPosition = 
                new Vector3(Mathf.Clamp(this.transform.localPosition.x, -normalX * Player_Manager.instance.universalScale, normalX * Player_Manager.instance.universalScale), 
                Mathf.Clamp(this.transform.localPosition.y, -normalY * Player_Manager.instance.universalScale, normalY * Player_Manager.instance.universalScale), 0);
        }
        if (Camera_Controller.instance.camState == Camera_Controller.CameraState.Near) {
            this.transform.localPosition = 
                new Vector3(Mathf.Clamp(this.transform.localPosition.x, -nearX * Player_Manager.instance.universalScale, nearX * Player_Manager.instance.universalScale), 
                Mathf.Clamp(this.transform.localPosition.y, -nearY * Player_Manager.instance.universalScale, nearY * Player_Manager.instance.universalScale), 0);
        }
        //
        if (Camera_Controller.instance.camState == Camera_Controller.CameraState.Cockpit )
        {
            Vector3 clampPosition = this.transform.localPosition;
            clampPosition.x = Mathf.Clamp(this.transform.localPosition.x, -Player_Manager.instance.horizantalClamp.x / 2 * Player_Manager.instance.universalScale, 
                Player_Manager.instance.horizantalClamp.y / 2 * Player_Manager.instance.universalScale);
            clampPosition.y = Mathf.Clamp(this.transform.localPosition.y, -Player_Manager.instance.verticalClamp.x / 2 * Player_Manager.instance.universalScale, 
                Player_Manager.instance.verticalClamp.y / 2 * Player_Manager.instance.universalScale);
            transform.localPosition = clampPosition;
        }
        else
        {
            Vector3 pos = Camera_Controller.instance.cam.WorldToViewportPoint(this.transform.position);
            pos.x = Mathf.Clamp01(pos.x);
            pos.y = Mathf.Clamp01(pos.y);
            this.transform.position = Vector3.Lerp(this.transform.position, Camera_Controller.instance.cam.ViewportToWorldPoint(pos), Time.deltaTime * 9);
        }
    }
    //
    public void OnPathFound (Vector3[] newPath, bool pathSuccessful) {
		if (pathSuccessful) {
			path = newPath;
			StopCoroutine ("FollowPath");
			StartCoroutine ("FollowPath");
		}
	}
    //
    IEnumerator FollowPath() {
        currentWayPoint = path[0];
        //
        while (true)
        {
            if (transform.parent.position == currentWayPoint)
            {
                targetIndex++;
                if (targetIndex >= path.Length)
                {
                    yield break;
                }
                currentWayPoint = path[targetIndex];
            }
            this.transform.parent.position = Vector3.MoveTowards(transform.parent.position, currentWayPoint, speed);
            //
            Vector3 newDirection = Vector3.RotateTowards(this.transform.parent.forward, currentWayPoint - this.transform.parent.position, Time.deltaTime / 5, 0);
            this.transform.parent.rotation = Quaternion.LookRotation(newDirection);
            if(Vector3.Distance(this.transform.position, target) < 100)
            {
                lookAtPath = true;
            }
            //for (int i = 0; i < path.Length; i += 2)
            //{
            //    Vector3[] curMoveSeg = new Vector3[3];
            //    //
            //    float pathIndex = i + 1;
            //    //
            //    if (pathIndex == path.Length)
            //    {
            //        curMoveSeg[0] = this.transform.parent.position;
            //        curMoveSeg[1] = path[i];
            //        curMoveSeg[2] = path[i];
            //    }
            //    else
            //    {
            //        curMoveSeg[0] = this.transform.parent.position;
            //        curMoveSeg[1] = path[i];
            //        curMoveSeg[2] = path[i + 1];
            //    }
            //    //
            //    while (this.transform.parent.position != curMoveSeg[2])
            //    {
            //        transition = Mathf.Clamp01(transition);
            //        float oneMinusT = 1f - transition;
            //        Vector3 position = oneMinusT * oneMinusT * curMoveSeg[0] + 2f * oneMinusT * transition * curMoveSeg[1] + transition * transition * curMoveSeg[2];
            //        Vector3 direction = 2f * (1f - transition) * (curMoveSeg[1] - curMoveSeg[0]) + 2f * transition * (curMoveSeg[2] - curMoveSeg[1]);
            //        this.transform.parent.position = Vector3.MoveTowards(this.transform.parent.position, position, speed);


            //        yield return null;
            //    }
            //}
            //if (transform.parent.position == path[path.Length-1]) {
            //    curMode = CurrentMode.Range;
            //}
            yield return null;
        }
    }
    //
    public void OnDrawGizmos(){
        //
        Gizmos.color = new Color(1, 0, 0, 0.5f); // red
        Gizmos.DrawCube(target, Vector3.one*9);
        //
        if (Game_Manager.instance.debugging == Game_Manager.Debug.True || Game_Manager.instance.debugShip == true)
        {
            if (path != null)
            {
                for (int i = targetIndex; i < path.Length; i++)
                {
                    Gizmos.color = Color.black;
                    Gizmos.DrawCube(path[i], Vector3.one);
                    //
                    if (i == targetIndex)
                    {
                        Gizmos.DrawLine(transform.position, path[i]);
                    }
                    else
                    {
                        Gizmos.DrawLine(path[i - 1], path[i]);
                    }
                }
            }
        }
    }
}
