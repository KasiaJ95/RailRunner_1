using UnityEngine;
using System.Collections;


namespace RailRunner {
    public class World : MonoBehaviour {
        #region FIELDS
        [SerializeField] private GameObject [] _Cubes;
        [SerializeField] private MeshRenderer _GroundRenderer;

        private Transform _PrevTransform;

        private int _FirstSpawnCount;
        float _GroundSpeed;
        #endregion

        #region PRIVATE/PROTECTED METHODS
        private void Start() {
            _GroundSpeed = new float[] { 1.0f, 1.5f, 2.0f }[MenuController.DifficultySelected] * 0.9f;
            float groundScale = new float[] { 15.0f, 22.5f, 30.0f }[MenuController.DifficultySelected];
            _GroundRenderer.material.mainTextureScale = new Vector2(groundScale, groundScale);
            StartCoroutine(SpawnCubes());
        }
        private void Update() {
            _GroundRenderer.material.mainTextureOffset -= new Vector2(_GroundSpeed * Time.deltaTime, 0.0f);
        }
        #endregion

        private IEnumerator SpawnCubes()  {
            while (true) {
                yield return new WaitForSeconds(0.5f);

                _FirstSpawnCount++;

                GameObject newCube = Instantiate<GameObject>(_Cubes[
                    _FirstSpawnCount < 4 ? 0 : Random.Range(0, _Cubes.Length)]);
                newCube.transform.parent = null;
                newCube.transform.rotation = Quaternion.identity;
                newCube.transform.localScale = Vector3.one;
                if (_PrevTransform != null) {
                    newCube.transform.position = _PrevTransform.position + new Vector3(0.0f, 0.0f, 10.0f);
                } else {
                    newCube.transform.position = new Vector3(0.0f, -6.0f, 30.0f);
                                }
                _PrevTransform = newCube.transform;
                StartCoroutine(UpdateCube(newCube.transform));            
            }
        }

        private IEnumerator UpdateCube(Transform cubeTransform) {
            while(cubeTransform.position.z > -10 ) {
                cubeTransform.position -= cubeTransform.forward * Time.deltaTime * 10f;
                yield return null;
            }
            Destroy(cubeTransform.gameObject);
        }
      
    }
}
