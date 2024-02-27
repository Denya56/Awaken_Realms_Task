using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Dice dice;
    [SerializeField] private TMP_Text resultText;
    [SerializeField] private TMP_Text totalText;
    [SerializeField] private Button rollButton;
    private int total;

    private void Start()
    {
        total = 0;
        EventManager.Instance.OnRollEnd += SetResultKnown;
        EventManager.Instance.OnRollStart += SetResultDefault;
    }

    private void SetResultDefault()
    {
        resultText.SetText($"Result: ?");
    }

    private void SetResultKnown(string result)
    {
        // 9 is represented as 9. to distinguish it from 6 on a dice
        // remove "." from result to properly parse it to int
        if (result.Contains("."))   
            result = result.Replace(".", "");
        resultText.SetText($"Result: {result}");
        AddTotal(result);
        SetTotal(total);
    }

    private void AddTotal(string currentResult)
    {
        total += int.Parse(currentResult);
    }
    private void SetTotal(int total)
    {
        totalText.SetText($"Total: {total}");
    }

    public void Roll()
    {
        EventManager.Instance.HandleOnButtonThrow();
    }
}
