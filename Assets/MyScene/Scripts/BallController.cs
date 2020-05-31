using System;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace MyScene.Scripts
{
    [RequireComponent(typeof(Rigidbody))]
    public class BallController : MonoBehaviour
    {
        //Отступ камеры
        [SerializeField] private Vector3 ballCameraOffSet = new Vector3(0f, -0.1f, 0.2f);
        [SerializeField] private ARSessionOrigin sessionOrigin;
        [SerializeField] private GameObject aRcam;    

        //Направления броска
        private Vector3 _startPosition;
        private Vector3 _direction;


        //данные для броска
        public float throwForce = 100f;
        private float _startTime;
        private float _endTime;
        private float _duration;
        private bool _directionChosen;
        private static Rigidbody _rb;

        private float relaxTime;
        public float mass;
        void Awake()
        {
            Debug.Log("Мяч начал работу");
            sessionOrigin = GameObject.Find("AR Session Origin").GetComponent<ARSessionOrigin>();
            aRcam =  sessionOrigin.transform.Find("AR Camera").gameObject;
            _rb = transform.gameObject.GetComponent<Rigidbody>();
            transform.parent = aRcam.transform;
            ResetBall();

            Debug.Log(SaveSystem._Save.playerControll ? "Запущено первое управление" : "Запущено второе управление");

            Debug.Log("Мяч готов");
        }

        void Update()
        {
            if (!_directionChosen && relaxTime<=0f)
            {
                if (!SaveSystem._Save.playerControll) SecondTapsControll();
                else FirstCameraControll();
                
                
                _startTime = 0.0f;
                _duration = 0.0f;
                _directionChosen = false;
            }

            relaxTime -= Time.deltaTime;
            //After 5 sec autoreset ball
            if (Time.time - _endTime >= 8 && Math.Abs(_endTime) > 0) ResetBall();
        }


        public void ResetBall()
        {
            _rb.mass = 0;
            _rb.useGravity = false;
            _rb.velocity = Vector3.zero;
            _endTime = 0.0f;
            _startPosition = Vector3.zero;
            _direction = Vector3.zero;

            var ball = transform;

            //задает позицию и направление как у камеры
            ball.position = aRcam.transform.position;
            ball.LookAt(aRcam.transform.forward);

            //сдвигает на пользовательское значение
            ball.localPosition += ballCameraOffSet;
            Debug.Log("Мяч вернулся");

            relaxTime = 1f;
        }

        //управление камерой
        private void FirstCameraControll()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _startPosition = transform.position; //позиция старта броска
                _startTime = Time.time; //начало броска
                _directionChosen = false; //направление выбрано
            }
            else if (Input.GetMouseButtonUp(0))
            {
                _endTime = Time.time; //Конец броска
                _duration = _endTime - _startTime; //длительность броска
                _direction = transform.position - _startPosition; //направление броска
                _directionChosen = true;
            }

            if (_directionChosen)
            {
                _rb.mass = 1;
                _rb.useGravity = true;
                var force =new Vector3 (_direction.x *throwForce, _direction.y*throwForce  ,_direction.z/_duration);
                _rb.AddForce(force);

            }
        }

        //управление нажатиями
        private void SecondTapsControll()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _startPosition = Input.mousePosition; //позиция старта броска
                _startTime = Time.time; //начало броска
                _directionChosen = false; //направление выбрано
            }
            else if (Input.GetMouseButtonUp(0))
            {
                _endTime = Time.time;
                _duration = _endTime - _startTime;
                _direction = Input.mousePosition - _startPosition;
                Debug.Log($"Ты отжал кнопку в координатах {Input.mousePosition.x}x, {Input.mousePosition.y}y, {Input.mousePosition.z}z ");
                _directionChosen = true;
            }
            
            if (_directionChosen)
            {
                _rb.mass = mass;
                _rb.useGravity = true;
                var force = new Vector3 (_direction.x *throwForce, _direction.y*throwForce  ,_direction.z/_duration +_direction.y);
                _rb.AddForce(force);

            }
        }
    }
}