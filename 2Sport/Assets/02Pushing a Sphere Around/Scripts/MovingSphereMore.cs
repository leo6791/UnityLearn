using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSphereMore : MonoBehaviour
{
    private Vector2 playerInput;
    private Vector3 velocity;
    private Rigidbody body;
    Vector3 desiredVelocity;
    bool desireJump;
    bool onGround;

    [SerializeField, Range(1f, 100f)]
    private float maxSpeed = 10f;

    [SerializeField, Range(0f, 100f)]
    private float maxAcceleration = 10f, maxAirAcceleration = 1f;
    [SerializeField, Range(0f, 10f)]
    private float jumpHeight = 1f;
    [SerializeField, Range(0, 5)]
    int maxAirJump = 2;
    int jumpPhase = 0;
    private void Awake()
    {
        body = transform.GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {
        playerInput.x = Input.GetAxis("Horizontal");
        playerInput.y = Input.GetAxis("Vertical");
        //约束返回向量的模长
        playerInput = Vector2.ClampMagnitude(playerInput, 1f);
        //所需速度
        desiredVelocity = new Vector3(playerInput.x, 0f, playerInput.y) * maxSpeed;
        //可能不会调用下一帧的fixedUpdate，用布尔或运算来检查变化值是否与之前的值相同
        //这样做就是保证FixedUpdate中跳跃生效后才显式改变desireJump=false
        desireJump |= Input.GetButtonDown("Jump");

    }
    // 利用FixedUpdate以固定的时间步长调整步速
    // 默认为0.02（每秒50次）,但可以通过fixedDeltatime来改变FixedUpdate的更新速率
    private void FixedUpdate()
    {
        UpdateState();
        //根据在地面或空中选择加速度
        float acceleration = onGround ? maxAcceleration : maxAirAcceleration;
        //最大速度变化
        float maxSpeedChange = acceleration * Time.deltaTime;
        //实际速度靠近目标值，最大速度变化限制
        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
        velocity.z = Mathf.MoveTowards(velocity.z, desiredVelocity.z, maxSpeedChange);
        //跳跃
        if (desireJump)
        {
            Jump();
            desireJump = false;
        }
        body.velocity = velocity;
        onGround = false;
    }
    void UpdateState()
    {
        //物理碰撞会影响速度，先将基础速度从刚体中检索出来
        velocity = body.velocity;
        if (onGround)
            jumpPhase = 0;
    }
    void Jump()
    {
        //空中跳跃
        if (onGround || jumpPhase < maxAirJump)
        {
            jumpPhase += 1;
            // 跳跃克服重力，按照公式来算垂直方向的速度
            float jumpSpeed = Mathf.Sqrt(-2 * Physics.gravity.y * jumpHeight);
            //限制向上的速度
            if (velocity.y > 0f)
            {
                //已经有向上的速度，则在将其添加到速度的Y分量之前，将其从跳跃速度中减去,并限制不为负数
                jumpSpeed = Mathf.Max(jumpSpeed - velocity.y, 0f);
            }
            velocity.y += jumpSpeed;
        }

    }

    private void OnCollisionEnter(Collision other)
    {
        EvaluaeCollision(other);
    }
    private void OnCollisionStay(Collision other)
    {
        EvaluaeCollision(other);
    }
    void EvaluaeCollision(Collision other)
    {
        //可以通过Collision的contactCount属性找到接触点的数量。
        //GetContact方法遍历所有点,访问该点的法线属性。
        //确定接触的是地面而不是墙体
        for (int i = 0; i < other.contactCount; i++)
        {
            Vector3 normal = other.GetContact(i).normal;
            //如果平面是水平的，则其法线将指向垂直，因此其Y分量应正好为1。如果是这种情况，则我们正在接触地面。但是，让我们宽容一些，接受0.9或更大的Y分量。
            onGround |= normal.y >= 0.9f;
        }
    }
}
