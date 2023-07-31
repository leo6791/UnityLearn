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
        //Լ������������ģ��
        playerInput = Vector2.ClampMagnitude(playerInput, 1f);

        //1�����ٶ�,������ת���ɼ��ٶ�
        //Vector3 acceleration = new Vector3(playerInput.x, 0f, playerInput.y) * maxSpeed;
        //�ٶȸ��ݼ��ٶȱ仯��������ٶ�Ϊ0��Ҳ���־ɵ��ٶ�ֵʹС������ǰ�������޷�ʵʱ����С���˶�
        //velocity += acceleration * Time.deltaTime;
        //��Ϊ����update�У���ϣ��֡��Ӱ������Ļ���ҪTime.deltaTime��ȷ��һ֡�ƶ�

        //2�������ٶ�
        Vector3 desiredVelocity = new Vector3(playerInput.x, 0f, playerInput.y) * maxSpeed;
        //����ٶȱ仯
        float maxSpeedChange = maxAcceleration * Time.deltaTime;
        //ʵ���ٶȿ���Ŀ��ֵ������ٶȱ仯����
        //������Ч���ǰ��·����С����٣��ɿ�����ֹͣ������ƽ���˶���������Ӧ
        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
        velocity.z = Mathf.MoveTowards(velocity.z, desiredVelocity.z, maxSpeedChange);

        Vector3 displacement = velocity * Time.deltaTime;
        //����˶������ÿ�ζ���displacement��ֵ�� transform.localPosition����ô��ֻ����һ����Χ���ƶ�
        Vector3 newPos = transform.localPosition + displacement;
        //����С���޷�����ƽ��ķ�Χ,�����ӷ�����ģ����������Ч����
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
