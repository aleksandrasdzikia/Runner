using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2V : MonoBehaviour
{
    private Vector3 start = Vector3.zero;
    private Vector3 startPosition = Vector3.zero;
    private Vector3 startRotation = Vector3.zero;

    private float distance = 0f;
    private float speed = 0f;
    private float speedMultiplier = 1f;
    private float touchDuration = 0f;
    public float maxSpeed = 1.0f;
    public float maxSpeedMultiplier = 1.0f;
    public float accelaration = 1.0f;
    public float deaccelaration = 3.0f;
    public ParticleSystem runParticles;

    public bool touchMoveEnabled = true;
    public Camera mainCamera;
    //public Level level;

    private Vector2 currentTouchMove = Vector2.zero;

    private bool dead = false;

    public GameObject mesh;

    private Animator animator;
    private Animator offsetAnimator;
    public bool running = false;

    private Direction direction = Direction.Forward;
    private Direction wannaBeDirection = Direction.Forward;
    private Vector3 directionVector = Vector3.forward;

    private Vector3 lastOffsetVector = Vector3.zero;
    private Vector3 specialDirection = Vector3.zero;
    private bool runningStraight = true;

    private List<Vector3> debugLines = new List<Vector3>();

    private float rotating = 1f;
    private Quaternion lastRotation = Quaternion.identity;
    private Quaternion lastLookRotation = Quaternion.identity;

    private bool climbing = false;

    private bool tapEnable = true;
    private bool freezeEnable = true; // (Freeze Tap Collsion)

    private void Awake()
    {
        this.runParticles.Stop();
    }

    void Start()
    {
        this.startPosition = this.transform.position; 
        this.animator = mesh.GetComponent<Animator>();
    }

    void Update()
    {

        this.checkStates();
        this.parseInputs();
        this.handleSpeed();
        this.move();
    }

    void checkStates()
    {
            AnimatorStateInfo asi = this.animator.GetCurrentAnimatorStateInfo(0);
            AnimatorTransitionInfo ati = this.animator.GetAnimatorTransitionInfo(0);
            this.climbing = asi.IsName("Climb") || ati.IsName("Run -> Climb") || ati.IsName("Climb -> Run");
    }

    void parseInputs()
    {
        bool space = Input.GetKeyDown("space");
        bool r = Input.GetKeyDown("r");
        bool minus = Input.GetKeyDown("q") || Input.GetKeyDown("-");
        bool plus = Input.GetKeyDown("=") || Input.GetKeyDown("w") || Input.GetKeyDown("+");

        if (plus)
            this.maxSpeed += 0.25f;
        if (minus)
            this.maxSpeed -= 0.25f;

        if ((space) && !this.running)
        {
            this.startRunning();
            this.runParticles.Play();
        }

        if (this.running && space)
        {
            //if()
            {
                //ADD FUNCTION OF ROTATING OBJECTS
            }

        }

        if (r)
            this.reset();

        // TOUCH INPUTS

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (this.touchMoveEnabled)
            {
                this.touchDuration = 0f;
                if (tapEnable)
                {
                    this.tapMove();
                }
                this.touchMoveEnabled = false;
                this.currentTouchMove = Vector2.zero;
                return;
            }
        }

        else
        {
            if (this.touchDuration > 0f)
            {
                this.touchDuration = 0f;
                this.tapMove();
            }
            this.touchMoveEnabled = true;
        }
    }

    public void tapMove()
    {
        if (!this.running)
            this.startRunning();
        else
        {
            if (freezeEnable)
            {
                //ADD FUNCTION OF ROTATING OBJECTS
            }
        }
    }



    public void startRunning()
    {
        this.running = true;
        this.animator.SetTrigger("StartRun");
    }

    void handleSpeed()
    {
        if (running)
        {
            float maximumSpeed = this.getMaxSpeed();
            if (this.speed > maximumSpeed)
                this.speed -= this.deaccelaration * Time.deltaTime;
            else
            {
                this.speed += this.accelaration * Time.deltaTime;
                if (this.speed > maximumSpeed)
                    this.speed = maximumSpeed;
            }
            this.animator.SetFloat("RunSpeed", this.getSpeed());
        }
        else
        {
            this.speed -= this.deaccelaration * Time.deltaTime;
            if (this.speed < 0)
                this.speed = 0;
        }
    }

    void die(DeathType type)
    {
        switch (type)
        {
            case DeathType.Instant:
            default:
                this.reset();
                break;
        }
    }

    void changeDirection(int dir)
    {
        int newDirection = (((int)this.direction) - dir) % 4;
        if (newDirection < 0)
            newDirection = 4 - newDirection;
        this.wannaBeDirection = (Direction)newDirection;
        //Debug.Log("Wanting to change direction  " + dir + " to " + this.wannaBeDirection);

        bool actionDone = false;
        Collider[] colliders = Physics.OverlapSphere(transform.position, 0f); //0.25f
        List<IAction> safetyNets = new List<IAction>();
        foreach (Collider collider in colliders)
        {
            foreach (IAction action in collider.GetComponents<IAction>())
            {
                if (action is ActionRun)
                    actionDone = this.parseAction(action, collider) || actionDone;
                else if (action is ActionSafetyNetForRun)
                    safetyNets.Add(action);
            }
        }

        if (!actionDone)
            foreach (IAction action in safetyNets)
                actionDone = this.parseAction(action) || actionDone;

        if (!actionDone)
        {
            this.speed = 0.5f;
        }
    }

    bool setRunDirection(ActionRun runAction, Direction newDirection)
    {
        Vector3 newDirectionVector = runAction.getDirectionVector(this.transform.position, newDirection);
        this.lastOffsetVector = this.calculateLineOffset(this.transform.position);
        if (newDirectionVector != Vector3.zero)
        {
            this.directionVector = newDirectionVector;
            this.direction = newDirection;
            this.runningStraight = false;
            this.specialDirection = runAction.getSpecialDirection();
            this.speedMultiplier = runAction.getSpeedMultiplier(this.direction);
            this.maxSpeedMultiplier = runAction.getMaxSpeedMultiplier(this.direction);
            string newAnimation = runAction.getSpecialAnimation();
            if (newAnimation != null && newAnimation != "")
                this.animator.SetTrigger(newAnimation);
            return true;
        }
        else
        {
            return false;
        }
    }

    bool parseAction(IAction action, Collider collider = null, bool trigger = false)
    {
        if (action is ActionTriggerRun)
        {
            ActionTriggerRun runActionTrigger = (ActionTriggerRun)action;
            return this.setRunDirection(runActionTrigger, runActionTrigger.getDirection());
        }
        else if (action is ActionRun)
        {
            ActionRun actionRun = action as ActionRun;
            if (trigger)
                return this.setRunDirection(actionRun, actionRun.GetTriggerDirection());
            if (this.wannaBeDirection != this.direction)
                return this.setRunDirection(actionRun, this.wannaBeDirection);
        }
        else if (action is ActionSafetyNetForRun)
        {
            if (this.wannaBeDirection != this.direction)
                return ((ActionSafetyNetForRun)action).EarlyMove(this.wannaBeDirection);
        }
        else if (action is ActionTranslate)
        {
            ActionTranslate translateAction = (ActionTranslate)action;
            this.transform.Translate(translateAction.getTranslateVector(), Space.World);
            return true;
        }

        else if (action is ActionDeath)
        {
            this.die(((ActionDeath)action).getType());
        }

        return false;
    }

    Vector3 calculateLineOffset(Vector3 position)
    {
        float x = position.x - Mathf.Round(position.x);
        float y = position.y - Mathf.Round(position.y);
        float z = position.z - Mathf.Round(position.z);
        return new Vector3(x, y, z);
    }

    void move()
    {
        Vector3 beforePosition = this.transform.position;
        this.transform.Translate(this.directionVector * this.getSpeed() * Time.deltaTime, Space.World);
        Vector3 velocity = this.transform.position - beforePosition;

        this.rotate(velocity);

        if (!this.runningStraight)
        {
            Vector3 currentOffsetVector = this.calculateLineOffset(this.transform.position);
            float a = 0f;
            float b = 0f;
            if (direction == Direction.Forward || direction == Direction.Backward)
            {
                a = this.lastOffsetVector.x;
                b = currentOffsetVector.x;
            }
            else if (direction == Direction.Right || direction == Direction.Left)
            {
                a = this.lastOffsetVector.z;
                b = currentOffsetVector.z;
            }

            this.runningStraight = (a >= 0f && b <= 0f) || (a <= 0f && b >= 0f);

            if (this.runningStraight)
                this.directionVector = this.specialDirection != Vector3.zero ? this.specialDirection : DirectionVector.getVector(this.direction);
        }
    }

    void rotate(Vector3 velocity)
    {
        if (specialDirection != Vector3.zero)
            velocity += DirectionVector.getVector(this.direction) * 2f;
        velocity.y = 0f;

        Quaternion lookRotation = Quaternion.identity;
        if (velocity != Vector3.zero)
            lookRotation = Quaternion.LookRotation(velocity);
        float angle = lookRotation.y - this.transform.rotation.y;
        if (this.lastLookRotation != lookRotation)
        {
            this.rotating = 0f;
            this.lastRotation = this.transform.rotation;
            this.lastLookRotation = lookRotation;
        }
        else if (this.rotating != 1f)
        {
            this.rotating += Time.deltaTime * this.speed;
        }

        if (this.rotating > 1f)
        {
            this.rotating = 1f;
        }
        this.transform.rotation = Quaternion.Lerp(lastRotation, lookRotation, this.rotating * 2f);
    }

    void reset()
    {
        this.transform.position = this.startPosition;
        //cameraPosition.transform.position = this.startPosition;
        this.transform.eulerAngles = this.startRotation;
        this.dead = false;
        this.wannaBeDirection = this.direction;
        this.debugLines.Clear();
        this.running = false;
        this.speed = 0f;
        this.animator.Play("Idle");
        this.specialDirection = Vector3.zero;
        this.mainCamera.GetComponent<CameraFollow>().reset();
        //this.level.reset();
        this.tapEnable = true;
        this.runParticles.Stop();
    }

    public void OnTriggerEnter(Collider collider)
    {
        foreach (IAction action in collider.GetComponents<IAction>())
            this.parseAction(action, collider, true);
    }

    public float getSpeed()
    {
        return this.speed * this.speedMultiplier;
    }

    public float getMaxSpeed()
    {
        return this.maxSpeed * this.maxSpeedMultiplier;
    }

    public bool doAction(IAction action)
    {
        return this.parseAction(action);
    }

    void debug()
    {
        Debug.DrawLine(this.transform.position, this.transform.position + DirectionVector.getVector(this.direction) * 2f, Color.red);
        this.debugLines.Add(this.transform.position);
        for (int i = 1; i < this.debugLines.Count; i++)
            Debug.DrawLine(this.debugLines[i - 1], this.debugLines[i], Color.blue);

    }

}
