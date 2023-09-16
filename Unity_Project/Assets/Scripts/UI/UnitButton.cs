
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    // for the Buttons
    public class UnitButton : MonoBehaviour
    {
        [SerializeField] private Button unitButton;
        [SerializeField] private TextMeshProUGUI buttonName;

        private int enemyIndex;
        private HUDView HUDView;

        private void Start()
        {
            HUDView = GetComponentInParent<HUDView>(); // for the scroll view
        }
        private void OnEnable()
        {
            unitButton.onClick.AddListener(ChangeEnemy);
        }

        private void OnDisable()
        {
            unitButton.onClick.RemoveListener(ChangeEnemy);
        }
        private void ChangeEnemy()
        {
            HUDView.ChangeEnemyTeam(enemyIndex); // for the positions also
        }

        public void UpdateIndex(int enemyIndex) // to update the team UI.Text
        {
            buttonName.text = $"Enemy {enemyIndex}";
            this.enemyIndex = enemyIndex;
        }
    }

}
