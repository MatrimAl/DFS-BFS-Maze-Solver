using TMPro;
using UnityEngine;

public class MazeCell: MonoBehaviour
{
    [SerializeField]
    private GameObject _leftWall;

    [SerializeField]
    private GameObject _rightWall;

    [SerializeField]
    private GameObject _frontWall;

    [SerializeField]
    private GameObject _backWall;

    [SerializeField]
    private GameObject _unvisitedBlock;

    [SerializeField]
    private GameObject _bottomPlate;

    [SerializeField] 
      private TextMeshPro _cellText;

    public void SetVisited(string status)
    {
        CellText = status; // H�cre �zerindeki metni ayarla
        _unvisitedBlock.SetActive(false); // Gerekirse blo�u devre d��� b�rak
    }

    public string CellText
    {
        get { return _cellText.text; }
        set { _cellText.text = value; }
    }
    
    public void Block(){
        _unvisitedBlock.SetActive(false);
    }

    public void ClearLeftWall(){
        _leftWall.SetActive(false);
    }
    public void ClearRightWall(){
        _rightWall.SetActive(false);
    }

    public void ClearFrontWall(){
        _frontWall.SetActive(false);
    }
    public void ClearBackWall(){
        _backWall.SetActive(false);
    }

    public void SetActiveBottom()
    {
        _bottomPlate.SetActive(true);
    }

}
