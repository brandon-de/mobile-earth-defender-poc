using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatableObject : MonoBehaviour {

    public float startTimeBtwnShots;
    private float timeBtwnShots = 0f;
    public Bullet bullet = null;
    public Transform TouchArea = null;
    public Transform ShotPoint = null;
    public Transform ShotDirection = null;
    private GameObject pDebugPrefab1;
    private GameObject pDebugPrefab2;
    public float glPlFltDeltaLimit;
    public float glPlFltDeltaReduce;
    public int glPlIntLapsBeforeStopping;
    public bool glPlBoolCanRotate { get; set; }
    private float glPrFltDeltaRotation;
    private float glPrFltPreviousRotation;
    private float glPrFltCurrentRotation;
    private int glPrIntCurrentLaps;
    private float glPrFloatRotation;
    private float glPrFltQuarterRotation;
    private bool boolCountRotations;
    private bool _touchAreaPressed;


    void Start()
    {
        //this.pDebugPrefab1 = Instantiate(debugPrefab, transform.position, Quaternion.identity);
        //this.pDebugPrefab2 = Instantiate(debugPrefab, transform.position, Quaternion.identity);        
        
        glPrIntCurrentLaps = glPlIntLapsBeforeStopping;
        glPrFloatRotation = 0f;
        glPlBoolCanRotate = true;
        boolCountRotations = true;
    }

    // Update is called once per frame
    void Update()
    {
        RotateThis();
        CountRotations();

        if(this.CheckIfChildObjectPressed(this.TouchArea))
        {
            _touchAreaPressed = true;
        }
        else if(Input.GetMouseButtonUp(0))
        {
            _touchAreaPressed = false;
        }
    }

    private void CountRotations()
    {
        if (boolCountRotations)
        {
            if (Mathf.Sign(glPrFltDeltaRotation) == 1)
            {
                glPrFloatRotation += glPrFltDeltaRotation;
            }

            if (glPrFloatRotation >= 360)
            {
                glPrFloatRotation -= 360;
                glPrIntCurrentLaps -= 1;
                if (glPrIntCurrentLaps <= 0)
                {
                    glPlBoolCanRotate = false;
                }
            }
        }
    }

    private void RotateThis()
    {
        if (Input.GetMouseButtonDown(0) && glPlBoolCanRotate)
        {

            // Get initial rotation of this game object
            glPrFltDeltaRotation = 0f;
            glPrFltPreviousRotation = angleBetweenPoints(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
        else if (Input.GetMouseButton(0) && glPlBoolCanRotate && _touchAreaPressed)
        {
            // Rotate along the mouse under Delta Rotation Limit
            glPrFltCurrentRotation = angleBetweenPoints(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
            glPrFltDeltaRotation = Mathf.DeltaAngle(glPrFltCurrentRotation, glPrFltPreviousRotation);
            if (Mathf.Abs(glPrFltDeltaRotation) > glPlFltDeltaLimit)
            {
                glPrFltDeltaRotation = glPlFltDeltaLimit * Mathf.Sign(glPrFltDeltaRotation);
            }
            glPrFltPreviousRotation = glPrFltCurrentRotation;
            transform.Rotate(Vector3.back * Time.deltaTime, glPrFltDeltaRotation);

            //Debug.Log(string.Format("current mouse pos x:{0} y:{1}", Input.mousePosition.x, Input.mousePosition.y));

            Vector3 diff = this.ShotDirection.position - transform.position;
            float rotZ = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            var newRot = Quaternion.Euler(0f, 0f, rotZ + 90);

            if (timeBtwnShots <= 0)
            {
                var bullet = Instantiate(this.bullet, this.ShotPoint.position, this.ShotDirection.rotation);
                bullet.SetBulletTarget(this.ShotDirection);
                timeBtwnShots = startTimeBtwnShots;
            }
            else
            {
                timeBtwnShots -= Time.deltaTime;
            }
        }
        else
        {
            // Inertia
            transform.Rotate(Vector3.back * Time.deltaTime, glPrFltDeltaRotation);
            glPrFltDeltaRotation = Mathf.Lerp(glPrFltDeltaRotation, 0, glPlFltDeltaReduce * Time.deltaTime);
        }
    }

    private float angleBetweenPoints(Vector2 v2Position1, Vector2 v2Position2)
    {
        Vector2 v2FromLine = v2Position2 - v2Position1;
        Vector2 v2ToLine = new Vector2(1, 0);

        //Debug.Log(string.Format("faked mouse pos x:{0} y:{1}", this.ShotDirection.position.x, this.ShotDirection.position.y));

        
        //Vector2 mirroredMousePos = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - this.pDebugPrefab1.transform.position) + this.pDebugPrefab1.transform.position;
        //this.pDebugPrefab2.transform.position = new Vector3(-mirroredMousePos.x, -mirroredMousePos.y);
        //Debug.Log(string.Format("mirroed mouse pos x:{0} y:{1}", -mirroredMousePos.x, -mirroredMousePos.y));
        //Debug.Log(string.Format("mirroed mouse pos x:{0} y:{1}", this.pDebugPrefab1.transform.position.x, this.pDebugPrefab1.transform.position.y));

        //this.pDebugPrefab1.transform.position = new Vector3(-Input.mousePosition.x, -Input.mousePosition.y);
        //this.pDebugPrefab1.transform.position = new Vector3(-v2FromLine.x, -v2FromLine.y);
        //this.pDebugPrefab2.transform.position = new Vector3(v2ToLine.x, v2ToLine.y);
        //Debug.Log(string.Format("Current position of debug x:{0} y:{1}", this.debugPrefab.transform.position.x, this.debugPrefab.transform.position.y));

        float fltAngle = Vector2.Angle(v2FromLine, v2ToLine);

        // If rotation is more than 180
        Vector3 v3Cross = Vector3.Cross(v2FromLine, v2ToLine);
        if (v3Cross.z > 0)
        {
            fltAngle = 360f - fltAngle;
        }

        return fltAngle;
    }

    private bool CheckIfChildObjectPressed(Transform child)
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.collider == child.GetComponent<Collider>())
                {
                    Debug.Log("Mouse in touch area");
                    return true;
                }
            }
        }

        return false;
    }
}
