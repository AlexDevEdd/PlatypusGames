using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    [SerializeField] private Text _scoreText;
    [HideInInspector] public IntReactiveProperty _currentScore = new IntReactiveProperty();

    private void Start() =>    
       _currentScore.SubscribeToText(_scoreText);

    public void ApplyScore(IntReactiveProperty scoreValue)
    {
        _currentScore.Value += scoreValue.Value;
    }
}
