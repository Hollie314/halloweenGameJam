using TMPro;
using UnityEngine;

public class UpdateUI : MonoBehaviour
{
    public TextMeshProUGUI textAltar;
    public TextMeshProUGUI textCanvas;
    public int stolenCount = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    public void UpdateText()
    {
        stolenCount++;
        textAltar.text = $"Altar \n({stolenCount}/7)";
        textCanvas.text = $"Stolen items : {stolenCount}/7";
    }
}
