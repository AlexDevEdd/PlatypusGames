using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextScoreUI : MonoBehaviour
{
    public static TextScoreUI Instance { get; private set; }
    const int POOL_SIZE = 3;

    private class ActiveText
    {
        private Text uIText;
        private float maxTime;
        private float timer;
        private Vector3 itemPosition;

        public Text UIText { get => uIText; set => uIText = value; }
        public float MaxTime { get => maxTime; set => maxTime = value; }
        public float Timer { get => timer; set => timer = value; }
        public Vector3 ItemPosition { get => itemPosition; set => itemPosition = value; }

        public void MoveText(Camera camera)
        {
            float delta = 1.0f - (timer / maxTime);
            Vector3 pos = itemPosition + new Vector3(delta, delta, 0.0f);
            pos = camera.WorldToScreenPoint(pos);
            pos.z = 0.0f;

            UIText.transform.position = pos;
        }
    }

    [SerializeField] private Text _textPrefab;
    private Camera _camera;
    private Transform _transformParent;
    private List<ActiveText> _activeTextList = new List<ActiveText>();
    private Queue<Text> _textPool = new Queue<Text>();


    private void Awake() => Instance = this;

    void Start()
    {
        _camera = Camera.main;
        _transformParent = transform;

        for (int i = 0; i < POOL_SIZE; i++)
        {
            Text temp = Instantiate(_textPrefab, _transformParent);
            temp.gameObject.SetActive(false);
            _textPool.Enqueue(temp);
        }
    }

    void Update()
    {
        for (int i = 0; i < _activeTextList.Count; i++)
        {
            ActiveText activeText = _activeTextList[i];
            activeText.Timer -= Time.deltaTime;

            if (activeText.Timer <= 0.0f)
            {
                activeText.UIText.gameObject.SetActive(false);
                _textPool.Enqueue(activeText.UIText);
                _activeTextList.RemoveAt(i);
                --i;
            }
            else
            {
                var color = activeText.UIText.color;
                color.a = activeText.Timer / activeText.MaxTime;
                activeText.UIText.color = color;

                activeText.MoveText(_camera);
            }
        }
    }

    public void AddText(int score, Vector3 itemPos)
    {
        var t = _textPool.Dequeue();
        t.text = "+" + score.ToString();
        t.gameObject.SetActive(true);

        ActiveText activeText = new ActiveText() { MaxTime = 1.0f };
        activeText.Timer = activeText.MaxTime;
        activeText.UIText = t;
        activeText.ItemPosition = itemPos + Vector3.up;

        activeText.MoveText(_camera);
        _activeTextList.Add(activeText);
    }
}
