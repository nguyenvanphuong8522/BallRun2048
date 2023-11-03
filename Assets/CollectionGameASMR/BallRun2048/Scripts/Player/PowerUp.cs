using Unity.VisualScripting;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public int id;
    public MovementPlayer movementPlayer;
    public AnimationBall animationBall;
    public bool canPowerDown = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PowerUp powerUpCollision = collision.gameObject.GetComponent<PowerUp>();
            if (powerUpCollision.id == id)
            {
                if (movementPlayer.canMove)
                {
                    collision.gameObject.SetActive(false);
                    PowerUpBall();
                }
                else if (powerUpCollision.DistanceCompare() < DistanceCompare() && !powerUpCollision.movementPlayer.canMove)
                {
                    collision.gameObject.SetActive(false);
                    PowerUpBall();
                }
            }
            else
            {
                AudioManager.instance.PlayShot("collisionBall");
            }
        }

        if (collision.gameObject.CompareTag("Trap"))
        {
            if (canPowerDown)
            {
                if(id == 2)
                {
                    GameManager.Instance.ResetLevel();
                }
                else
                {
                    collision.gameObject.GetComponent<BoxCollider>().enabled = false;
                    PowerDown();
                }
            }
        }
    }

    public float DistanceCompare()
    {
        return GameManager.Instance.gameObject.transform.position.z - transform.position.z;
    }
    public void PowerUpBall()
    {
        int i = index(id * 2);
        ObjectPool_BallRun2048 _objectPool = ObjectPool_BallRun2048.instance;
        GameObject newplayer = _objectPool.Get(_objectPool.balls[i]);
        newplayer.SetActive(true);
        newplayer.transform.position = transform.position;
        PowerUp newPowerUp = newplayer.GetComponent<PowerUp>();
        MovementPlayer newMovementPlayer = newPowerUp.movementPlayer;
        if (!movementPlayer.canMove)
        {
            newMovementPlayer.canMove = false;
        }
        else
        {
            newMovementPlayer.canMove = true;
            newPowerUp.canPowerDown = true;
            GameManager.Instance.CameraFollow.player = newplayer;
            newPowerUp.animationBall.SetScale();
        }
        AudioManager.instance.PlayShot("mergeBall");
        ObjectPool_BallRun2048.instance.Return(gameObject);
    }
    public void PowerDown()
    {
        int i = index(id / 2);
        movementPlayer.lastMousePos = Vector2.zero;
        gameObject.GetComponent<SphereCollider>().enabled = false;
        ObjectPool_BallRun2048 objectPool = ObjectPool_BallRun2048.instance;
        GameObject newplayer = objectPool.Get(objectPool.balls[i]);
        GameObject playerClone = objectPool.Get(objectPool.powerUps[i]);
        newplayer.SetActive(true);
        playerClone.SetActive(true);
        PowerUp newPowerUp = newplayer.GetComponent<PowerUp>();
        newplayer.transform.position = transform.position;
        playerClone.transform.position = transform.position;
        GameManager.Instance.CameraFollow.player = newplayer;
        newPowerUp.movementPlayer.canMove = true;
        newPowerUp.canPowerDown = true;
        movementPlayer.canMove = false;
        AudioManager.instance.PlayShot("decreasePower");
        ObjectPool_BallRun2048.instance.Return(gameObject);
    }

    public int index(int id)
    {
        if (id <= 2)
            return 0;
        else
            return 1 + index(id / 2);
    }
}
