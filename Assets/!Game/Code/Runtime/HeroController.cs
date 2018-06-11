using System.Collections;
using UnityEngine.SceneManagement;
namespace Runner.Gameplay
{
    using System.Collections;
    using UnityEngine;

    public class HeroController : MonoBehaviour
    {

        #region EVENTS
        #endregion

        #region FIELDS
        [SerializeField] private GameplayController _GameplayController;

        [SerializeField]
        private Transform _LeftPoint;
        [SerializeField]
        private Transform _CenterPoint;
        [SerializeField]
        private Transform _RightPoint;
        [SerializeField]
        private Transform _CurrentPoint;
        [SerializeField]
        private Transform _NextPoint;
        [SerializeField]
        private Animator _Animator;
        private int side = 1;
        private float _Time;
        #endregion

        #region PUBLIC METHODS
        #endregion

        #region PRIVATE/PROTECTED METHODS
        private void OnTriggerEnter(Collider collider) {
            Debug.Log("TRG");
            _GameplayController.HitHandler();
        //    float dangerAngleX = collider.transform.rotation.eulerAngles.x;
        //    if (dangerAngleX < 180.0f || dangerAngleX > 320.0f)
        //        SceneManager.Instance.ChangeScene("Gameplay");
        }

        private IEnumerator InputLeft()
        {
            Debug.Log("InputLeft");
            if (side == 0)
                yield break;
            side--;
            _Animator.SetBool("Left", true);
            //yield return new WaitForSeconds(0.5f);
            if (side == 0)
                _NextPoint = _LeftPoint;
            else if (side == 1)
                _NextPoint = _CenterPoint;
            _Time = 0.0f;
            yield return new WaitForSeconds(0.5f);
            _Animator.SetBool("Left", false);
        }
        private IEnumerator InputRight()
        {
            Debug.Log("InputRight");
            if (side == 2)
                yield break;
            side++;
            _Animator.SetBool("Right", true);
            if (side == 2)
                _NextPoint = _RightPoint;
            else if (side == 1)
                _NextPoint = _CenterPoint;
            _Time = 0.0f;
            yield return new WaitForSeconds(0.5f);
            _Animator.SetBool("Right", false);
        }
        private IEnumerator InputUp()
        {
            Debug.Log("InputUp");
            _Animator.SetBool("Jump", true);
            yield return new WaitForSeconds(0.5f);
            _Animator.SetBool("Jump", false);
        }
        private IEnumerator InputDown()
        {
            Debug.Log("InputDown");
            _Animator.SetBool("Down", true);
            yield return new WaitForSeconds(0.5f);
            _Animator.SetBool("Down", false);
        }


        private void OnSwipeUp()
        {
            StartCoroutine(InputUp());
        }
        private void OnSwipeDown()
        {
            StartCoroutine(InputDown());
        }
        private void OnSwipeLeft()
        {
            if (_NextPoint == null)
                StartCoroutine(InputLeft());
        }
        private void OnSwipeRight()
        {
            if (_NextPoint == null)
                StartCoroutine(InputRight());
        }
        private void OnTap()
        {

        }
        #endregion

        #region IMPLEMENTATION OF: MonoBehaviour
        private void Start() {
            _NextPoint = _CenterPoint;
        }
        //private void Start()
        //{
        //    Runner.Input.Manager.Instance.OnTap += OnTap;
        //    Runner.Input.Manager.Instance.OnSwipeUp += OnSwipeUp;
        //    Runner.Input.Manager.Instance.OnSwipeDown += OnSwipeDown;
        //    Runner.Input.Manager.Instance.OnSwipeLeft += OnSwipeLeft;
        //    Runner.Input.Manager.Instance.OnSwipeRight += OnSwipeRight;
        //    _Animator.speed = (float)User.Manager.Instance.Difficulty;
        //    _Animator.SetBool("Jump", false);
        //    _Animator.SetBool("Down", false);
        //    _Animator.SetBool("Left", false);
        //    _Animator.SetBool("Right", false);
        //}
        //private void OnDestroy()
        //{
        //    if (Runner.Input.Manager.Instance == null)
        //        return;
        //    Runner.Input.Manager.Instance.OnTap -= OnTap;
        //    Runner.Input.Manager.Instance.OnSwipeUp -= OnSwipeUp;
        //    Runner.Input.Manager.Instance.OnSwipeDown -= OnSwipeDown;
        //    Runner.Input.Manager.Instance.OnSwipeLeft -= OnSwipeLeft;
        //    Runner.Input.Manager.Instance.OnSwipeRight -= OnSwipeRight;
        //}
        private void Update() {

            if (Input.GetKeyUp(KeyCode.W) || (Input.GetMouseButton(0) && Input.GetAxis("Mouse Y") > 0.2f)) {
                OnSwipeUp();
            } else if (Input.GetKeyUp(KeyCode.S) || (Input.GetMouseButton(0) && Input.GetAxis("Mouse Y") < -0.2f)) {
                OnSwipeDown();
            } else if (Input.GetKeyUp(KeyCode.A) || (Input.GetMouseButton(0) && Input.GetAxis("Mouse X") < -0.2f)) {
                OnSwipeLeft();
            } else if (Input.GetKeyUp(KeyCode.D) || (Input.GetMouseButton(0) && Input.GetAxis("Mouse X") > 0.2f)) {
                OnSwipeRight();
            }

            if (_NextPoint == null)
            {
            }
            else
            {
                _Time += Time.deltaTime;
                if (_Time > 0.5f)
                    _Time = 0.5f;
                transform.position = Vector3.Lerp(_CurrentPoint.position, _NextPoint.position, _Time / 0.5f);

                if (_Time == 0.5f)
                {
                    _CurrentPoint = _NextPoint;
                    _NextPoint = null;
                }
            }
        }
        #endregion

        #region PROPERTIES
        #endregion
    }
}