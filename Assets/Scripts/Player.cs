using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	private Vector3 start = Vector3.zero;
	private Vector3 startPosition = Vector3.zero;

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
	public Level level;

	private Vector2 currentTouchMove = Vector2.zero;

	private bool dead = false;

	public GameObject mesh;

	private Animator animator;
	private Animator offsetAnimator;
	private bool running = false;

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

	private float highScore = 0f;
	private float currentScore = 0f;

	private bool rolling = false;
	private bool climbing = false;
	public bool inAir = false;
    private float jumping = -1f;
	private Vector3 startJumpingVector = Vector3.zero;
	private Vector3 endJumpingVector = Vector3.zero;

	public GameObject offsetObject;
	public GameObject hoverBoard;

	public bool boarding = false;
    public bool tapEnable = true;
    public bool freezeEnable = true; // (Freeze Tap Collsion)

    //private Vector2 fingerDown; //-----------------------------------------------------------
    //private Vector2 fingerUp;
    //public bool detectSwipeOnlyAfterRelease = false;                                        

    //public float SWIPE_THRESHOLD = 20f; //----------------------------------------------------

    private void Awake()
    {
        this.runParticles.Stop();
    }

    void Start()
	{
		this.startPosition = this.transform.position; //
		this.animator = mesh.GetComponent<Animator>();
        //this.offsetAnimator = offsetObject.GetComponent<Animator>();
        //this.SetupBoarding();
    }

	void Update()
	{

        this.checkStates();
		this.parseInputs();
		this.handleSpeed();
        this.move();

        // this.debug();

        // this.inAir = animator.GetCurrentAnimatorStateInfo(0).IsName("Jump");
        // this.checkInputs();
        // this.misc();
        // this.findAction();
        // this.doAction();
    }

	void checkStates() {
		if (this.boarding) {
			AnimatorStateInfo asi = this.offsetAnimator.GetCurrentAnimatorStateInfo(0);
			AnimatorTransitionInfo ati = this.offsetAnimator.GetAnimatorTransitionInfo(0);
			this.inAir = asi.IsName("Hop") || ati.IsName("Hover -> Hop") || ati.IsName("Slide -> Hop");
		} else {
			AnimatorStateInfo asi = this.animator.GetCurrentAnimatorStateInfo(0);
			AnimatorTransitionInfo ati = this.animator.GetAnimatorTransitionInfo(0);
			this.rolling = asi.IsName("Slide") /*|| asi.IsName("Roll") || ati.IsName("Run -> Roll")*/ || ati.IsName("Run -> Slide") /*|| ati.IsName("Roll -> Run")*/ || ati.IsName("Slide -> Run") /*|| ati.IsName("Idle -> Roll")*/ || ati.IsName("Idle -> Slide");
            this.inAir = asi.IsName("Jump") || ati.IsName("Run -> Jump");
			this.climbing = asi.IsName("Climb") || ati.IsName("Run -> Climb") || ati.IsName("Climb -> Run");
            //this.inAirColl = asi.IsName("Jump");
        }
	}

	void parseInputs() {
		bool space = Input.GetKeyDown("space");
		bool left = Input.GetKeyDown("left");
		bool right = Input.GetKeyDown("right");
		bool up = Input.GetKeyDown("up");
		//bool down = Input.GetKeyDown("down");
		bool r = Input.GetKeyDown("r");
		bool minus = Input.GetKeyDown("q") || Input.GetKeyDown("-");
        bool plus = Input.GetKeyDown("=") || Input.GetKeyDown("w") || Input.GetKeyDown("+");

		if (plus)
			this.maxSpeed += 0.25f;
		if (minus)
			this.maxSpeed -= 0.25f;

		if ((left || right || space) && !this.running)
        {
            this.startRunning();
            this.runParticles.Play();
        }
			

		if (this.running && (left || right || space))
        {
            if (this.direction == Direction.Forward)
            {
                this.changeDirection(-1);
            }
            else if (this.direction == Direction.Right)
            {
                this.changeDirection(1);
            }
        }
			//this.changeDirection(left ? 1 : -1);

		if (this.running && up)
			this.jump();

        //if (this.running && down)
			//this.roll();

		if (r)
			this.reset();

        // touch inputs



        if (Input.touchCount > 0) {
			Touch touch = Input.GetTouch(0);
			if (this.touchMoveEnabled) {
   //             if (this.touchDuration >= 0.5f)
   //             {
                    this.touchDuration = 0f;
                if(tapEnable)
                {
                    this.tapMove();
                }
                    this.touchMoveEnabled = false;
                    this.currentTouchMove = Vector2.zero;
                    return;
                }
    //            this.touchDuration += Time.deltaTime;
				//if (touch.phase == TouchPhase.Moved) {
				//	this.currentTouchMove += touch.deltaPosition;
				//	if (this.currentTouchMove.magnitude > 10f) { //3f default
				//		if (!this.running)
				//			this.startRunning();
				//		float angle = Vector2.Angle(Vector2.up, this.currentTouchMove) * Mathf.Sign(this.currentTouchMove.x);
				//		if (angle >= 45f && angle <= 135f) {
    //                        Debug.Log(angle);
    //                        this.changeDirection(-1); // Right
				//		} else if (angle >= 135f || angle <= -135f) {
				//			this.roll();
    //                        Debug.Log(angle);
    //                    } else if (angle >= -150f && angle <= -45f) {
				//			this.changeDirection(1); //Left
    //                        Debug.Log(angle);
    //                    } else if (angle <= 0f && angle >= -45f) {
				//			this.jump();
    //                        Debug.Log(angle);
    //                    }

				//		this.touchDuration = 0f;
				//		this.currentTouchMove = Vector2.zero;
				//		this.touchMoveEnabled = false;
				//	}
				//}
		//	}
		}

        else {
			if (this.touchDuration > 0f) {
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
            if(freezeEnable)
            {
                if (this.direction == Direction.Forward)
                {
                    this.changeDirection(-1);
                }
                else if (this.direction == Direction.Right)
                {
                    this.changeDirection(1);
                }
            }
        }
    }
 
	void startRunning() {
		this.running = true;
        this.animator.SetTrigger("StartRun");
        //if (this.boarding)
        //this.offsetAnimator.SetTrigger("StartHover");
        //else this.animator.SetTrigger("StartRun");

    }

	void handleSpeed() {
		if (running) {
			float maximumSpeed = this.getMaxSpeed();
			if (this.speed > maximumSpeed)
				this.speed -= this.deaccelaration * Time.deltaTime;
			else {
				this.speed += this.accelaration * Time.deltaTime;
				if (this.speed > maximumSpeed)
					this.speed = maximumSpeed;
			}
			this.animator.SetFloat("RunSpeed", this.getSpeed());
        } else {
			this.speed -= this.deaccelaration * Time.deltaTime;
			if (this.speed < 0)
				this.speed = 0;
		}
	}

	void jump() {
		if (!this.climbing && !this.inAir) {
			if (this.boarding) {
				this.offsetAnimator.SetTrigger("StartHop");
				Collider[] colliders = Physics.OverlapSphere(transform.position, 0f);
				foreach (Collider collider in colliders) {
					foreach (IAction action in collider.GetComponents<IAction>()) {
						if (action is ActionJump)
							this.parseAction(action, collider);
					}
				}
			}
			else this.animator.SetTrigger("StartJump");
		}
	}

	void roll() {
		if (this.rolling || this.climbing || this.boarding)
			return;
		//if (Random.value >= 0.5f)
            //this.animator.SetTrigger("StartRoll");
            //else this.animator.SetTrigger("StartSlide");
            this.animator.SetTrigger("StartSlide");
    }

	void die(DeathType type) {
		switch (type) {
			case DeathType.Roll:
				if (!this.rolling)
					this.reset();
				else this.currentScore += this.getSpeed();
				break;
			case DeathType.Gap:
				if (!inAir)
					this.reset();
				else this.currentScore += this.getSpeed();
				break;
			case DeathType.Instant:
			default:
				this.reset();
				break;
		}
	}

	void changeDirection(int dir) {
		int newDirection = (((int) this.direction) - dir) % 4;
		if (newDirection < 0)
			newDirection = 4 - newDirection;
		this.wannaBeDirection = (Direction) newDirection;
		//Debug.Log("Wanting to change direction  " + dir + " to " + this.wannaBeDirection);

		bool actionDone = false;
		Collider[] colliders = Physics.OverlapSphere(transform.position, 0f); //0.25f
		List<IAction> safetyNets = new List<IAction>();
		foreach (Collider collider in colliders) {
			foreach (IAction action in collider.GetComponents<IAction>()) {
				if (action is ActionRun)
					actionDone = this.parseAction(action, collider) || actionDone;
				else if (action is ActionSafetyNetForRun)
					safetyNets.Add(action);
			}
		}

		if (!actionDone)
			foreach (IAction action in safetyNets)
				actionDone = this.parseAction(action) || actionDone;

		if (!actionDone) { //------------------ Pradeda vytis
            reset();
            //this.speed = 0.5f;
            //this.logHighscore();
        }
		else this.currentScore++;
	}

	bool setRunDirection(ActionRun runAction, Direction newDirection) {
		Vector3 newDirectionVector = runAction.getDirectionVector(this.transform.position, newDirection);
		this.lastOffsetVector = this.calculateLineOffset(this.transform.position);
		if (newDirectionVector != Vector3.zero) {
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
		} else {
			return false;
		}
	}

	bool parseAction(IAction action, Collider collider = null, bool trigger = false) {
		if (action is ActionPickUp) {
			ActionPickUp pickUpAction = (ActionPickUp) action;
			pickUpAction.executePickUp(this);
			return true;
		} else if (action is ActionTriggerRun) {
			ActionTriggerRun runActionTrigger = (ActionTriggerRun) action;
			return this.setRunDirection(runActionTrigger, runActionTrigger.getDirection());
		} else if (action is ActionRun) {
			ActionRun actionRun = action as ActionRun;
			if (trigger)
				return this.setRunDirection(actionRun, actionRun.GetTriggerDirection());
			if (this.wannaBeDirection != this.direction)
				return this.setRunDirection(actionRun, this.wannaBeDirection);
		} else if (action is ActionSafetyNetForRun) {
			if (this.wannaBeDirection != this.direction)
				return ((ActionSafetyNetForRun) action).EarlyMove(this.wannaBeDirection);
		} else if (action is ActionTranslate) {
			ActionTranslate translateAction = (ActionTranslate) action;
			this.transform.Translate(translateAction.getTranslateVector(), Space.World);
			return true;
		} else if (action is ActionSlide) {
			ActionSlide slideAction = action as ActionSlide;
			if (this.boarding && slideAction.slideStart) {
				if (this.inAir)
					this.offsetAnimator.SetBool("Slide", true);
				else {
					this.boarding = false;
					//this.SetupBoarding();
				}
			} else {
				this.offsetAnimator.SetBool("Slide", false);
				this.offsetAnimator.SetTrigger("StartHover");
			}
		} else if (action is ActionJump) {
			ActionJump jumpAction = action as ActionJump;
			if (this.boarding && this.jumping == -1f) {
				this.jumping = 0f;
				this.startJumpingVector = this.transform.position;
				this.endJumpingVector = jumpAction.GetTargetPosition();
			}
		} else if (action is ActionDeath) {
			this.die(((ActionDeath) action).getType());
		}

		return false;
	}

	// void checkInputs() {
	//     bool space = Input.GetKeyDown("space");
	//     if (this.dead) {
	//         if (space)
	//             this.reset();
	//         return;
	//     }
	//     if (!this.inAir && space)
	//         animator.Play("Jump");

	// }

	void misc() {
	}

	// void findAction() {
	//     if (this.dead)
	//         return;

		// if (this.currentAction == null) {
		//     Collider[] colliders = Physics.OverlapSphere(transform.position, 0.25f); //0.25f
		//     if (colliders.Length != 0)
		//         this.currentAction = colliders[0].GetComponent<ActionTrigger>().getAction();
		//     else this.currentAction = this.lastAction;
		// }
	// }

	// void doAction() {
	//     if (this.currentAction != null) {
	//         switch (this.currentAction.type) {
	//             case "gap-death":
	//                 if (!this.inAir)
	//                     this.dead = true;
	//                 break;
	//             default: break;
	//         }

	//         if (!this.dead)
	//             this.move();
	//     }
	// }

	Vector3 calculateLineOffset(Vector3 position) {
		float x = position.x - Mathf.Round(position.x);
		float y = position.y - Mathf.Round(position.y);
		float z = position.z - Mathf.Round(position.z);
		return new Vector3(x, y, z);
	}

	void Jumping() {
		this.transform.position = Vector3.Slerp(this.startJumpingVector, this.endJumpingVector, this.jumping);
		if (this.jumping >= 1f)
			this.jumping = -1f;
		else this.jumping += Time.deltaTime * this.getSpeed() * 0.4f;
	}

	void move() {
		if (this.jumping >= 0f) {
			this.Jumping();
			return;
		}
		Vector3 beforePosition = this.transform.position;
		this.transform.Translate(this.directionVector * this.getSpeed() * Time.deltaTime, Space.World);
		Vector3 velocity = this.transform.position - beforePosition;

		this.rotate(velocity);

		if (!this.runningStraight) {
			Vector3 currentOffsetVector = this.calculateLineOffset(this.transform.position);
			float a = 0f;
			float b = 0f;
			if (direction == Direction.Forward || direction == Direction.Backward) {
				a = this.lastOffsetVector.x;
				b = currentOffsetVector.x;
			} else if (direction == Direction.Right || direction == Direction.Left) {
				a = this.lastOffsetVector.z;
				b = currentOffsetVector.z;
			}

			this.runningStraight = (a >= 0f && b <= 0f) || (a <= 0f && b >= 0f);

			if (this.runningStraight)
				this.directionVector = this.specialDirection != Vector3.zero ? this.specialDirection : DirectionVector.getVector(this.direction);
		}
	}

	void rotate(Vector3 velocity) {
		if (specialDirection != Vector3.zero)
			velocity += DirectionVector.getVector(this.direction) * 2f;
		velocity.y = 0f;

		Quaternion lookRotation = Quaternion.identity;
		if (velocity != Vector3.zero)
			lookRotation = Quaternion.LookRotation(velocity);
		float angle = lookRotation.y - this.transform.rotation.y;
		if (this.lastLookRotation != lookRotation) {
			this.rotating = 0f;
			this.lastRotation = this.transform.rotation;
			this.lastLookRotation = lookRotation;
		} else if (this.rotating != 1f) {
			this.rotating += Time.deltaTime * this.speed;
		}

		if (this.rotating > 1f) {
			this.rotating = 1f;
		}
		this.transform.rotation = Quaternion.Lerp(lastRotation, lookRotation, this.rotating * 2f);
	}

	void reset() {
		this.transform.position = this.startPosition;
		this.dead = false;
		this.wannaBeDirection = this.direction;
		this.debugLines.Clear();
		this.running = false;
		this.speed = 0f;
		this.animator.Play("Idle");
		this.specialDirection = Vector3.zero;
		this.mainCamera.GetComponent<CameraFollow>().reset();
		this.level.reset();
		this.logHighscore();
        this.tapEnable = true;
        this.runParticles.Stop();
    }

	void logHighscore() {
		this.highScore = Mathf.Max(this.highScore, this.currentScore);
		Debug.Log("High score: " + this.highScore + ", this score: " + this.currentScore);
		this.currentScore = 0;
	}

	public void OnTriggerEnter(Collider collider) {
		foreach (IAction action in collider.GetComponents<IAction>())
			this.parseAction(action, collider, true);
	}

	public bool isInAir() {
		return this.inAir;
	}

	public float getSpeed() {
		return this.speed * this.speedMultiplier;
	}

	public float getMaxSpeed() {
		return this.maxSpeed * this.maxSpeedMultiplier;
	}

	public bool doAction(IAction action) {
		return this.parseAction(action);
	}

	void debug() {
		 Debug.DrawLine(this.transform.position, this.transform.position + DirectionVector.getVector(this.direction) * 2f, Color.red);
		 this.debugLines.Add(this.transform.position);
		 for (int i = 1; i < this.debugLines.Count; i++)
			Debug.DrawLine(this.debugLines[i - 1], this.debugLines[i], Color.blue);

	}

	//void SetupBoarding() {
	//	if (this.boarding) {
	//		this.hoverBoard.active = true;
	//		this.offsetAnimator.SetTrigger("Show");
	//	} else {
	//		this.hoverBoard.active = false;
	//		this.offsetAnimator.SetTrigger("Reset");
	//	}
	//}


}
