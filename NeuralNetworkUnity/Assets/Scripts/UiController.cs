using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI GenNr;

    [SerializeField]
    private RectTransform SelectedPanel;

    [SerializeField]
    private TextMeshProUGUI SelectedNr;

    [SerializeField]
    private Button CloseBtn;

    private bool InfoMenuOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        SelectedPanel.localScale = new Vector3(0, 0);
        CloseBtn.onClick.AddListener((UnityEngine.Events.UnityAction)this.ToggleInfoMenu);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Pause()
    {
        //TODO: Pause the simulation.
    }
    
    public void ToggleInfoMenu()
    {
        InfoMenuOpen = !InfoMenuOpen;
        if (InfoMenuOpen)
        {
            SelectedPanel.localScale = new Vector3(1, 1);
        }
        else
        {
            SelectedPanel.localScale = new Vector3(0, 0);
        }
    }

    public void UpdateGenNr(int GenNumber)
    {
        GenNr.text = "Gen: " + GenNumber.ToString();
    }
}
