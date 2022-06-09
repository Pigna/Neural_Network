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

    [SerializeField]
    private GameObject Neuron;

    private bool InfoMenuOpen = false;

    private GameObject _selected;

    // Start is called before the first frame update
    void Start()
    {
        SelectedPanel.localScale = new Vector3(0, 0);
        CloseBtn.onClick.AddListener(() => this.ToggleInfoMenu(false));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Pause()
    {
        //TODO: Pause the simulation.
    }
    
    public void ToggleInfoMenu(bool value)
    {
        InfoMenuOpen = value;
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

    public void UpdateSelectedInfo(GameObject selected)
    {
        Ship _ship = selected.GetComponent<Ship>();
        int[] layers = _ship.neuralNetwork.getLayers();
        float[][] neurons = _ship.neuralNetwork.getNeurons();
        float[][][] weights = _ship.neuralNetwork.getWeights();

        GameObject pnl = GameObject.Find("Pnl_Selected");

        for (int i = 1; i < layers.Length; i++)
        {
            Debug.Log("Layer: " + i);
            for (int j = 0; j < neurons[i].Length; j++)
            {
                Debug.Log("Neuron: " + j);
                Vector3 position = new Vector3(100 + (100 * i), 400 - (25 * j), 0);
                Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                //Create neuron
                GameObject _obj = Instantiate(Neuron, position, rotation);
                _obj.GetComponent<TMPro.TextMeshProUGUI>().text = neurons[i][j] + "";

                _obj.transform.parent = pnl.transform;
                for (int k = 0; k < neurons[i - 1].Length; k++)
                {
                    //Create connection
                }
            }
        }
    }
}
