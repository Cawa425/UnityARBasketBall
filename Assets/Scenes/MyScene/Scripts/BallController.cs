using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace Scenes.UX.MyScene
{
    [RequireComponent(typeof(Rigidbody))]
    public class BallController : MonoBehaviour
    {
        //Сила брсока
        public float throwForce = 100f;

        //Направление броска
        public float throwDirectionX = 0.17f;
        public float throwDirectionY = 0.67f;

        //Отступ камеры
        [SerializeField] private Vector3 ballCameraOffSet = new Vector3(0f, -0.4f, 2f);

        private Vector3 _startPosition;
        private Vector3 _direction;
        private float _startTime;
        private float _endTime;
        private float _duration;
        private bool _directionChosen;
        private bool _throwStarted;

        [SerializeField] private GameObject aRcam;
        [SerializeField] private ARSessionOrigin sessionOrigin;

        private Rigidbody rb;


        void Start()
        {
            Debug.Log("Мяч начал работу");
            
            rb = gameObject.GetComponent<Rigidbody>();
            sessionOrigin = GameObject.Find("AR Session Origin").GetComponent<ARSessionOrigin>();
            aRcam = sessionOrigin.transform.Find("AR Camera").gameObject;
            
            transform.parent = aRcam.transform;
            
            ResetBall();
            Debug.Log("Мяч готов");
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _startPosition = Input.mousePosition;
                _startTime = Time.time;
                
                _throwStarted = true;
                _directionChosen = false;
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
                rb.mass = 1;
                rb.useGravity = true;
                var force = new Vector3(aRcam.transform.right.x + _direction.x + throwDirectionX,
                    aRcam.transform.up.y + _direction.y + throwDirectionY,
                    aRcam.transform.forward.z + throwForce / _duration);
                rb.AddForce(force);

                _startTime = 0.0f;
                _duration = 0.0f;

                _startPosition = Vector3.zero;
                _direction = Vector3.zero;
                
                _throwStarted = false;
                _directionChosen = false;
            }

            //After 5 sec reset ball
            if (Time.time - _endTime >= 5 ) ResetBall();
        }

        private void ResetBall()
        {
            rb.mass = 0;
            rb.useGravity = false;
            rb.velocity = Vector3.zero;
            _endTime = 0.0f;

            var camPosition = aRcam.transform.position;
            Vector3 ballPos = new Vector3(camPosition.x +ballCameraOffSet.x,
                camPosition.y + ballCameraOffSet.y,
                camPosition.z + ballCameraOffSet.z);
            transform.position = ballPos;
        }
    }
}