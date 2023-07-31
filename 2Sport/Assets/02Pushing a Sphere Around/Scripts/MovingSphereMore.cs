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
        //Լ������������ģ��
        playerInput = Vector2.ClampMagnitude(playerInput, 1f);
        //�����ٶ�
        desiredVelocity = new Vector3(playerInput.x, 0f, playerInput.y) * maxSpeed;
        //���ܲ��������һ֡��fixedUpdate���ò��������������仯ֵ�Ƿ���֮ǰ��ֵ��ͬ
        //���������Ǳ�֤FixedUpdate����Ծ��Ч�����ʽ�ı�desireJump=false
        desireJump |= Input.GetButtonDown("Jump");

    }
    // ����FixedUpdate�Թ̶���ʱ�䲽����������
    // Ĭ��Ϊ0.02��ÿ��50�Σ�,������ͨ��fixedDeltatime���ı�FixedUpdate�ĸ�������
    private void FixedUpdate()
    {
        UpdateState();
        //�����ڵ�������ѡ����ٶ�
        float acceleration = onGround ? maxAcceleration : maxAirAcceleration;
        //����ٶȱ仯
        float maxSpeedChange = acceleration * Time.deltaTime;
        //ʵ���ٶȿ���Ŀ��ֵ������ٶȱ仯����
        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
        velocity.z = Mathf.MoveTowards(velocity.z, desiredVelocity.z, maxSpeedChange);
        //��Ծ
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
        //������ײ��Ӱ���ٶȣ��Ƚ������ٶȴӸ����м�������
        velocity = body.velocity;
        if (onGround)
            jumpPhase = 0;
    }
    void Jump()
    {
        //������Ծ
        if (onGround || jumpPhase < maxAirJump)
        {
            jumpPhase += 1;
            // ��Ծ�˷����������չ�ʽ���㴹ֱ������ٶ�
            float jumpSpeed = Mathf.Sqrt(-2 * Physics.gravity.y * jumpHeight);
            //�������ϵ��ٶ�
            if (velocity.y > 0f)
            {
                //�Ѿ������ϵ��ٶȣ����ڽ�����ӵ��ٶȵ�Y����֮ǰ���������Ծ�ٶ��м�ȥ,�����Ʋ�Ϊ����
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
        //����ͨ��Collision��contactCount�����ҵ��Ӵ����������
        //GetContact�����������е�,���ʸõ�ķ������ԡ�
        //ȷ���Ӵ����ǵ��������ǽ��
        for (int i = 0; i < other.contactCount; i++)
        {
            Vector3 normal = other.GetContact(i).normal;
            //���ƽ����ˮƽ�ģ����䷨�߽�ָ��ֱ�������Y����Ӧ����Ϊ1�������������������������ڽӴ����档���ǣ������ǿ���һЩ������0.9������Y������
            onGround |= normal.y >= 0.9f;
        }
    }
}
