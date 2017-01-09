namespace Cenobium
{
    using UnityEngine;

    public delegate void CollisionEvent(Collision sender);
    /// <summary>
    /// A MonoBehaviour with more tools and the ability to have its movement binded to a Matrix object
    /// Also supports grid movement. ex: 0,0 -> 0,1
    /// LookForward and the Like traverse the Matrix with the objects relative transform postition
    /// LookNorth and the Like traverse the Matrix with the World's Compass postition 
    /// </summary>
    public class OmniBehaviour : MonoBehaviour
    {
        #region New MonoBehaviour Fields - Deprecations removed and references cached
        Transform _transform = null;
        public new Transform transform { get { return _transform ? _transform : _transform = base.transform; } }
        RectTransform _rectransform = null;
        public RectTransform rectTransform { get { return _rectransform ? _rectransform : _rectransform = (RectTransform)(Transform)transform; } }
        GameObject _gameObject = null;
        public new GameObject gameObject { get { return _gameObject ? _gameObject : _gameObject = base.gameObject; } }
        Rigidbody _rgbdy = null;
        public new Rigidbody rigidbody { get { return _rgbdy ? _rgbdy : _rgbdy = GetComponent<Rigidbody>(); } }
        Rigidbody2D _rgbdy2D = null;
        public new Rigidbody2D rigidbody2D { get { return _rgbdy2D ? _rgbdy2D : _rgbdy2D = GetComponent<Rigidbody2D>(); } }
        AudioSource _audio = null;
        public new AudioSource audio { get { return _audio ? _audio : _audio = GetComponent<AudioSource>(); } }
        public new string tag { get { return gameObject.tag; } set { gameObject.tag = value; } }
        Collider _collider = null;
        public new Collider collider { get { return _collider ? _collider : _collider = GetComponent<Collider>(); } }
        Collider2D _collider2D = null;
        public new Collider2D collider2D { get { return _collider2D ? _collider2D : _collider2D = GetComponent<Collider2D>(); } }
        Renderer _renderer = null;
        public new Renderer renderer { get { return _renderer ? _renderer : _renderer = GetComponent<Renderer>(); } }
        SkinnedMeshRenderer _skinrenderer = null;
        public SkinnedMeshRenderer skinrenderer { get { return _skinrenderer ? _skinrenderer : _skinrenderer = GetComponentInChildren<SkinnedMeshRenderer>(true); } }
        MeshFilter _meshfilter = null;
        public MeshFilter meshfilter { get { return _meshfilter ? _meshfilter : _meshfilter = GetComponent<MeshFilter>(); } }
        UnityEngine.UI.Button _button = null;
        public UnityEngine.UI.Button button { get { return _button ? _button : _button = GetComponent<UnityEngine.UI.Button>(); } }
        UnityEngine.UI.Text _Text = null;
        public UnityEngine.UI.Text Text { get { return _Text ? _Text : _Text = GetComponentInChildren<UnityEngine.UI.Text>(); } }
        string _text = null;
        public string text { get { return Text.text; } set { Text.text = value; } }
        UnityEngine.UI.Image _image = null;
        public UnityEngine.UI.Image Image { get { return _image ? _image : _image = GetComponentInChildren<UnityEngine.UI.Image>(true); } }
        UnityEngine.UI.RawImage _rwimage = null;
        public UnityEngine.UI.RawImage rawimage { get { return _rwimage ? _rwimage : _rwimage = GetComponentInChildren<UnityEngine.UI.RawImage>(true); } }
        Texture2D _texture = null;
        public Texture2D texture { get { return _texture ? _texture : _texture = GetComponentInChildren<Texture2D>(true); } }
        Camera _camera = null;
        public new Camera camera { get { return _camera ? _camera : _camera = GetComponentInChildren<Camera>(true); } }

        #region Collision Events
        public event CollisionEvent CollisionEnter;
        void OnCollisionEnter(Collision c)
        {
            if (CollisionEnter != null)
                CollisionEnter(c);
        }
        public event CollisionEvent CollisionExit;
        void OnCollisionExit(Collision c)
        {
            if (CollisionExit != null)
                CollisionExit(c);
        }
        public event CollisionEvent CollisionStay;
        void OnCollisionStay(Collision c)
        {
            if (CollisionStay != null)
                CollisionStay(c);
        }
        #endregion

        #endregion

        #region Transform/Vector3 to Matrix Compatibility
        #region Fields
        Matrix<Vector3> MapField = null;
        public Matrix<Vector3> Map { set { MapField = value; } get { return MapField; } }
        float indexScale = 5;
        /// <summary>
        /// Distance | Units an object considers as '1' block of movement
        /// </summary>
        public float IndexScale { get { return indexScale; } set { if (value > 0) indexScale = value; } }
        public int ROWINDEX
        {
            get { return Mathf.RoundToInt(transform.position.x / IndexScale); }
        }
        public int COLINDEX
        {
            get { return Mathf.RoundToInt(transform.position.z / IndexScale); }
        }
        public int HEIINDEX
        {
            get { return Mathf.RoundToInt(transform.position.y / IndexScale); }
        }
        /// <summary>
        /// The index of the unit in x , y , z where x = Row , y = Height and z = Column
        /// </summary>
        public Vector3 POSINDEX
        {
            get { return new Vector3(ROWINDEX, HEIINDEX, COLINDEX); }
        }
        /// <summary>
        /// The World - Space Conversion of the object Postion - Index. This is POSINDEX * IndexScale
        /// </summary>
        public Vector3 POS
        {
            get { return POSINDEX * IndexScale; }
        }
        #endregion
        #region Methods
        #region Relative Movement
        public bool MoveRight(int distance = 1, MatrixConstraints mb = MatrixConstraints.NONE)
        {
            return (CanMove(LookRight(distance), mb));
        }
        public Vector3 LookRight(int distance = 1)
        {
            return transform.position + (transform.right * IndexScale * distance);
        }
        public bool MoveLeft(int distance = 1, MatrixConstraints mb = MatrixConstraints.NONE)
        {
            return (CanMove(LookLeft(distance), mb));
        }
        public Vector3 LookLeft(int distance = 1)
        {
            return LookRight(-distance);
        }
        public bool MoveForward(int distance = 1, MatrixConstraints mb = MatrixConstraints.NONE)
        {
            return (CanMove(LookForward(distance), mb));
        }
        public Vector3 LookForward(int distance = 1)
        {
            return transform.position + (transform.forward * IndexScale * distance);
        }
        public bool MoveBackward(int distance = 1, MatrixConstraints mb = MatrixConstraints.NONE)
        {
            return (CanMove(LookBackward(distance), mb));
        }
        public Vector3 LookBackward(int distance = 1)
        {
            return LookForward(-distance);
        }

        //Diagonals

        #endregion
        #region NSWE
        // changing move North changes South
        public bool MoveNorth(int distance = 1, MatrixConstraints mb = MatrixConstraints.NONE)
        {
            if (CanMove(LookNorth(distance), mb))
            {
                //transform.position = LookNorth(distance);
                return true;
            }
            return false;
        }
        public Vector3 LookNorth(int distance = 1)
        {
            return transform.position + (Vector3.forward * IndexScale * distance);
        }
        public bool MoveSouth(int distance = 1, MatrixConstraints mb = MatrixConstraints.NONE)
        {
            if (CanMove(LookSouth(distance), mb))
            {
                //transform.position = LookSouth(distance);
                return true;
            }
            return false;
        }
        public Vector3 LookSouth(int distance = 1)
        {
            return LookNorth(-distance);
        }

        // changing move East changes West
        public bool MoveEast(int distance = 1, MatrixConstraints mb = MatrixConstraints.NONE)
        {
            if (CanMove(LookEast(distance), mb))
            {
                //transform.position = LookEast(distance);
                return true;
            }
            return false;
        }
        public Vector3 LookEast(int distance = 1)
        {
            return transform.position + (Vector3.right * IndexScale * distance);
        }
        public bool MoveWest(int distance = 1, MatrixConstraints mb = MatrixConstraints.NONE)
        {
            if (CanMove(LookWest(distance), mb))
            {
                //transform.position = LookWest(distance);
                return true;
            }
            return false;
        }
        public Vector3 LookWest(int distance = 1)
        {
            return LookEast(-distance);
        }
        #endregion
        #region Diagonals NW NE SW SE
        // changing move North changes South
        public bool MoveNorthEast(int distance = 1, MatrixConstraints mb = MatrixConstraints.NONE)
        {
            return (CanMove(LookNorthEast(distance), mb));
        }
        public Vector3 LookNorthEast(int distance = 1)
        {
            return LookNorth(distance) + (Vector3.right * IndexScale * distance);
        }
        public bool MoveNorthWest(int distance = 1, MatrixConstraints mb = MatrixConstraints.NONE)
        {
            return (CanMove(LookNorthWest(distance), mb));
        }
        public Vector3 LookNorthWest(int distance = 1)
        {
            return LookNorth(distance) + (Vector3.left * IndexScale * distance);
        }

        // changing move East changes West
        public bool MoveSouthEast(int distance = 1, MatrixConstraints mb = MatrixConstraints.NONE)
        {
            return (CanMove(LookSouthEast(distance), mb));
        }
        public Vector3 LookSouthEast(int distance = 1)
        {
            return LookSouth() + (Vector3.right * IndexScale * distance);
        }
        public bool MoveSouthWest(int distance = 1, MatrixConstraints mb = MatrixConstraints.NONE)
        {
            return (CanMove(LookSouthWest(distance), mb));
        }
        public Vector3 LookSouthWest(int distance = 1)
        {
            return LookSouth() + (Vector3.left * IndexScale * distance);
        }
        #endregion
        // changing move Up changes Down
        public bool MoveUp(int distance = 1, MatrixConstraints mb = MatrixConstraints.NONE)
        {
            return (CanMove(LookUp(distance), mb));
        }
        public Vector3 LookUp(int distance = 1)
        {
            return transform.position + (Vector3.up * IndexScale * distance);
        }
        public bool MoveDown(int distance = 1, MatrixConstraints mb = MatrixConstraints.NONE)
        {
            return (CanMove(LookDown(distance), mb));
        }
        public Vector3 LookDown(int distance = 1)
        {
            return LookUp(-distance);
        }
        #region Matrix Movement Validation
        public bool CanMoveFree(Vector3 pos, MatrixConstraints mb = MatrixConstraints.NONE)
        {
            return CanMove(POS + pos, mb);
        }
        public virtual bool CanMove(Vector3 pos, MatrixConstraints mb = MatrixConstraints.NONE)
        {
            pos = Indexify(pos, indexScale);
            return !Map || mb == MatrixConstraints.NONE || ((!((mb & MatrixConstraints.ROW) == MatrixConstraints.ROW) || pos.x >= 0 && pos.x < Map.Row) && (!((mb & MatrixConstraints.COL) == MatrixConstraints.COL) || pos.z >= 0 && pos.z < Map.Column) && (!((mb & MatrixConstraints.HEIGHT) == MatrixConstraints.HEIGHT) || pos.y >= 0 && pos.y < Map.Height));
        }
        public static Vector3 Indexify(Vector3 pos2Convert, float scale = 1)
        {
            if (scale == 0) scale = 1;
            return new Vector3(UnityEngine.Mathf.RoundToInt(pos2Convert.x / scale), UnityEngine.Mathf.RoundToInt(pos2Convert.y / scale), UnityEngine.Mathf.RoundToInt(pos2Convert.z / scale));
        }

        public static bool SameSpot(OmniBehaviour a, OmniBehaviour b, MatrixConstraints mb = MatrixConstraints.ALL, bool ignoreHeight = false)
        {
            return mb != MatrixConstraints.NONE && (((mb & MatrixConstraints.ROW) == MatrixConstraints.ROW) ? a.ROWINDEX == b.ROWINDEX : true) && (((mb & MatrixConstraints.COL) == MatrixConstraints.COL) ? a.COLINDEX == b.COLINDEX : true) && (((mb & MatrixConstraints.HEIGHT) == MatrixConstraints.HEIGHT) ? (a.HEIINDEX == b.HEIINDEX || ignoreHeight) : true);
        }
        /// <summary>
        /// Returns true if <paramref name="row"/>, <paramref name="col"/>, <paramref name="elev"/> (matrix space) is in the same spot
        /// as <paramref name="b"/>.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="col">The col.</param>
        /// <param name="elev">The elev.</param>
        /// <param name="b">The b.</param>
        /// <param name="mb">The mb.</param>
        /// <param name="ignoreHeight">if set to <c>true</c> [ignore height].</param>
        /// <returns></returns>
        public static bool SameSpot(int row, int col, int elev, OmniBehaviour b, MatrixConstraints mb = MatrixConstraints.ALL, bool ignoreHeight = false)
        {
            return mb != MatrixConstraints.NONE && (((mb & MatrixConstraints.ROW) == MatrixConstraints.ROW) ? row == b.ROWINDEX : true) && (((mb & MatrixConstraints.COL) == MatrixConstraints.COL) ? col == b.COLINDEX : true) && (((mb & MatrixConstraints.HEIGHT) == MatrixConstraints.HEIGHT) ? (elev == b.HEIINDEX || ignoreHeight) : true);
        }
        #endregion
        public static bool Close(OmniBehaviour a, OmniBehaviour b, int distance = 1, bool ignoreHeight = false)
        {
            return (distance * a.IndexScale) < UnityEngine.Mathf.Abs((a.POSINDEX - b.POSINDEX).magnitude) && (ignoreHeight || a.HEIINDEX == b.HEIINDEX);
        }
        #endregion
        #endregion

        public virtual void OFF()
        {
            gameObject.SetActive(false);
        }
        public virtual void ON()
        {
            gameObject.SetActive(true);
        }
        public virtual void ON(Transform identity)
        {
            ON(transform.position, transform.rotation);
        }
        public virtual void ON(Vector3 pos, Quaternion rot = new Quaternion())
        {
            transform.position = pos;
            transform.rotation = rot;
            gameObject.SetActive(true);
        }

        public static readonly string newline = System.Environment.NewLine;
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <param name="inactive">if set to <c>true</c> [inactive].</param>
        /// <returns></returns>
        public static T[] GetCompNoRoot<T>(Transform obj, bool inactive = true) where T : Component
        {
            System.Collections.Generic.List<T> tList = new System.Collections.Generic.List<T>();
            foreach (Transform child in obj)
            {
                T[] scripts = child.GetComponentsInChildren<T>(inactive);
                if (scripts != null)
                {
                    foreach (T sc in scripts)
                        tList.Add(sc);
                }
            }
            return tList.ToArray();
        }
    }
}
