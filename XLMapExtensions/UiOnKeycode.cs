using UnityEngine;


namespace UiOnKeycode

{
    public class UiOnKeycode : MonoBehaviour
    {
        [Header("Ui Element")]
        [Tooltip("Place your UI panel here")]
        public GameObject UiMenu;
        [Header("Control To Open Ui Menu")]
        [Tooltip("Select the Keyboard command to open your UI")]
        [SerializeField] private KeyCode MenuKey = KeyCode.H;

        private void Update()
        {
            if (Input.GetKeyDown(MenuKey))

            {
                UiMenu.SetActive(!UiMenu.activeInHierarchy);
                Cursor.visible = UiMenu.activeInHierarchy;
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = UiMenu.activeInHierarchy ? 0f : 1f;
            }

            if (UiMenu.activeInHierarchy)
            {
                AudioListener.pause = true;
            }
            else
            {
                AudioListener.pause = false;
            }
        }

    }
}
