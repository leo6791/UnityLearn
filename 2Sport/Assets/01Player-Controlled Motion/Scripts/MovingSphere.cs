using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSphere : MonoBehaviour
{
    private Vector2 playerInput;
    private Vector3 velocity;

    [SerializeField, Range(1, 100)]
    private float maxSpeed = 10f;

    [SerializeField, Range(1, 100)]
    private float maxAcceleration = 10f;

    [SerializeField]
    private Rect allowedArea = new Rect(-5f, -5f, 10f, 10f);

    [SerializeField, Range(0, 1)] private float bounciness = 0.5f;
    // Update is called once per frame
    void Update()
    {
        playerInput.x = Input.GetAxis("Horizontal");
        playerInput.y = Input.GetAxis("Vertical");
        //约束返回向量的模长
        playerInput = Vector2.ClampMagnitude(playerInput, 1f);

        //1、加速度,将输入转换成加速度
        //Vector3 acceleration = new Vector3(playerInput.x, 0f, playerInput.y) * maxSpeed;
        //速度根据加速度变化，就算加速度为0，也保持旧的速度值使小球匀速前进，但无法实时控制小球运动
        //velocity += acceleration * Time.deltaTime;
        //因为是在update中，不希望帧率影响输入的话需要Time.deltaTime来确定一帧移动

        //2、所需速度
        Vector3 desiredVelocity = new Vector3(playerInput.x, 0f, playerInput.y) * maxSpeed;
        //最大速度变化
        float maxSpeedChange = maxAcceleration * Time.deltaTime;
        //实际速度靠近目标值，最大速度变化限制
        //这样的效果是按下方向后小球加速，松开减速停止，而且平滑运动，快速响应
        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
        velocity.z = Mathf.MoveTowards(velocity.z, desiredVelocity.z, maxSpeedChange);

        Vector3 displacement = velocity * Time.deltaTime;
        //相对运动，如果每次都将displacement赋值给 transform.localPosition，那么就只能在一个范围内移动
        Vector3 newPos = transform.localPosition + displacement;
        //限制小球无法逃脱平面的范围,并增加反弹（模拟物理引擎效果）
        if (newPos.x < allowedArea.xMin)
        {
            newPos.x = allowedArea.xMin;
            velocity.x = -velocity.x * bounciness;
        }
        else if (newPos.x > allowedArea.xMax)
        {
            newPos.x = allowedArea.xMax;
            velocity.x = -velocity.x * bounciness;
        }

        if (newPos.z < allowedArea.yMin)
        {
            newPos.z = allowedArea.yMin;
            velocity.z = -velocity.z * bounciness;
        }
        else if (newPos.z > allowedArea.yMax)
        {
            newPos.z = allowedArea.yMax;
            velocity.z = -velocity.z * bounciness;
        }
        transform.localPosition = newPos;
    }
}
