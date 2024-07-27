

using UnityEngine;
using UnityEngine.UIElements;

public class WordGameUIController : MonoBehaviour
{
    private UIDocument uiDocument;
    private Label wordLabel;
    private Label meaningLabel;
    private Button correctAnswerButton;
    private Button wrongAnswerButton;

    private void OnEnable()
    {
        uiDocument = GetComponent<UIDocument>();
        var root = uiDocument.rootVisualElement;

        wordLabel = root.Q<Label>("word-label");
        meaningLabel = root.Q<Label>("meaning-label");
        correctAnswerButton = root.Q<Button>("correct-answer-button");
        wrongAnswerButton = root.Q<Button>("wrong-answer-button");

        correctAnswerButton.clicked += OnCorrectAnswerClicked;
        wrongAnswerButton.clicked += OnWrongAnswerClicked;

        DisplayWordExample();
    }

    private void DisplayWordExample()
    {
        // 예시 단어를 표시합니다
        wordLabel.text = "Apple";
        meaningLabel.text = "A round fruit with red or green skin and white flesh";
        correctAnswerButton.text = "Apple";
        wrongAnswerButton.text = "Banana";

        // 버튼 순서를 랜덤화합니다
        if (Random.value > 0.5f)
        {
            (correctAnswerButton.text, wrongAnswerButton.text) = (wrongAnswerButton.text, correctAnswerButton.text);
        }
    }

    private void OnCorrectAnswerClicked()
    {
        Debug.Log("Correct Answer!");
        // 여기에 정답을 선택했을 때의 로직을 추가할 수 있습니다
    }

    private void OnWrongAnswerClicked()
    {
        Debug.Log("Wrong Answer!");
        // 여기에 오답을 선택했을 때의 로직을 추가할 수 있습니다
    }
}