using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MovementPlayer : MonoBehaviour
{
    public float speed;
    public float speedForward;
    public Rigidbody rb;

    public Vector2 lastMousePos = Vector3.zero;
    public Vector2 currentMousePos;
    public PowerUp powerUp;
    public Vector3 direction;
    public GameManager gameManager;
    public GameObject _target1;
    public Vector3 winDirection = Vector3.zero;
    public float foreceDecrease;

    public bool win = false;
    public bool canMove = false;
    private void Start()
    {
        gameManager = GameManager.Instance;
        _target1 = GameManager.Instance.target1;
    }
    private void OnEnable()
    {
        gameObject.GetComponent<SphereCollider>().enabled = true;
        lastMousePos = Vector2.zero;
    }
    private void FixedUpdate()
    {
            if (canMove)
            {
            if (Input.GetMouseButton(0))
            {
                currentMousePos = Input.mousePosition;
                if (lastMousePos == Vector2.zero)
                {
                    lastMousePos = currentMousePos;
                }
                else
                {
                    direction = currentMousePos - lastMousePos;
                    direction = new Vector3(direction.x, 0, 0);
                    Vector3.Normalize(direction);
                }
                lastMousePos = currentMousePos;
            }
            rb.velocity = new Vector3(direction.x * speed * Time.deltaTime, -10, speedForward);
            }

            if (win)
            {
                MoveWin(powerUp.index(powerUp.id));
            }
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            direction = new Vector3(0, -10, rb.velocity.z);
            lastMousePos = Vector2.zero;
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("WinCheck") && canMove)
        {
            win = true;
        }

        if (other.gameObject.CompareTag("cucu"))
        {
            if(other.gameObject.name == powerUp.id.ToString())
            SetWin();
        }
    }

    public void MoveWin(int index)
    {
        canMove = false;
        if(_target1 == gameManager.target1)
        {
            winDirection = _target1.transform.position - transform.position;
            Vector3 a = new Vector3(_target1.transform.position.x, 0, _target1.transform.position.z);
            Vector3 b = new Vector3(transform.position.x, 0, transform.position.z);
            if (Vector3.Distance(a, b) > 1f)
            {
                rb.velocity = new Vector3(winDirection.normalized.x, 0, winDirection.normalized.z) * 20;
            }
            else
            {
                _target1 = gameManager.winGrades[index];
            }
        }
        else
        {
            rb.constraints = RigidbodyConstraints.FreezePositionX;
            rb.AddForce((_target1.transform.position - transform.position).normalized * foreceDecrease, ForceMode.Impulse);
        }
    }
    public void SetWin()
    {
        win = false;
        UiManager.instance.SetUiWin();
        _target1 = gameObject;
        rb.velocity = Vector3.zero;
        gameManager.sceneFlow.NextLevel();
    }
}