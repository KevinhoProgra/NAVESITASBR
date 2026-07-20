using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;
public class CONTROLREMAP : MonoBehaviour
{
    [Header("Configuración del Input")]
    [SerializeField] private PlayerInput playerInput; // Objeto con el componente PlayerInput
    [SerializeField] private string actionName;       // Nombre exacto de la acción en tu mapa
    [SerializeField] private int bindingIndex = 0;    // Índice de la tecla (0 para botones simples)

    [Header("UI Componentes")]
    [SerializeField] private TMP_Text buttonText;     // El texto interno del botón
    [SerializeField] private Button bindingButton;    // El componente Button de este objeto

    private InputActionRebindingExtensions.RebindingOperation rebindOperation;

    private void Start()
    {
        if (bindingButton == null) bindingButton = GetComponent<Button>();
        if (buttonText == null) buttonText = GetComponentInChildren<TMP_Text>();

        // Escuchar cuando el jugador hace click en el botón morado
        bindingButton.onClick.AddListener(() => StartRebinding());

        // Lee la tecla actual asignada y la pone en el texto
        UpdateBindingDisplay();
    }

    private void UpdateBindingDisplay()
    {
        InputAction action = playerInput.actions.FindAction(actionName);
        if (action != null)
        {
            // Traduce el path interno (ej: "<Keyboard>/w") a algo estético (ej: "W")
            buttonText.text = InputControlPath.ToHumanReadableString(
                action.bindings[bindingIndex].effectivePath,
                InputControlPath.HumanReadableStringOptions.OmitDevice
            );
        }
    }

    private void StartRebinding()
    {
        InputAction action = playerInput.actions.FindAction(actionName);
        if (action == null) return;

        // Desactivamos temporalmente el mapa para que la nave no se mueva mientras configuras
        playerInput.actions.Disable();

        buttonText.text = "Presiona...";
        bindingButton.interactable = false;

        // Iniciamos la operación interactiva del nuevo Input System
        rebindOperation = action.PerformInteractiveRebinding(bindingIndex)
            .WithControlsExcluding("<Mouse>/delta") // Evita que mover el mouse asigne un eje
            .WithControlsExcluding("<Pointer>/position")
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => FinishRebinding())
            .OnCancel(operation => FinishRebinding());

        rebindOperation.Start();
    }

    private void FinishRebinding()
    {
        rebindOperation.Dispose();
        playerInput.actions.Enable();
        bindingButton.interactable = true;

        UpdateBindingDisplay(); // Actualiza el botón con la nueva tecla
    }
}