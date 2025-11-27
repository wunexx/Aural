using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerWeaponInventory : PlayerComponentBase, IUpdatable
{
    [Header("Settings")]
    [SerializeField] private int inventorySize = 3;
    [SerializeField] private float scrollThreshold = 0.1f;

    [Header("References")]
    [SerializeField] private GameObject inventorySlotPrefab;
    [SerializeField] private Transform slotParent;
    [SerializeField] private UpdateManager updateManager;
    [SerializeField] private WeaponSO startingWeapon;
    [SerializeField] private GameObject weaponInteractablePrefab;

    [Header("UI Colors")]
    [SerializeField] private Color selectedBgColor = new Color(0f, 0f, 0f, 0.8f);
    [SerializeField] private Color normalBgColor = new Color(0f, 0f, 0f, 0.6f);

    private PlayerWeapon playerWeapon;
    private int currentIndex = 0;

    private WeaponSO[] inventory;
    private Slot[] slots;

    private bool scrollProcessed = false;

    private class Slot
    {
        public GameObject root;
        public Image icon;
        public Image background;
    }

    private void OnEnable() => updateManager.AddUpdatable(this);
    
    private void OnDisable() => updateManager.RemoveUpdatable(this);
    
    private void Start()
    {
        playerWeapon = GetComponent<PlayerWeapon>();

        inventory = new WeaponSO[inventorySize];
        slots = new Slot[inventorySize];

        CreateInventoryUI();

        inventory[0] = startingWeapon;
        playerWeapon.ChangeWeapon(startingWeapon);
        currentIndex = 0;

        UpdateAllSlotUI();
        HighlightCurrentSlot();
    }

    public void OnUpdate()
    {
        HandleScrollInput();
    }

    private void SwitchWeapon(int direction)
    {
        int newIndex = currentIndex;

        for (int i = 0; i < inventorySize; i++)
        {
            newIndex = (newIndex + direction + inventorySize) % inventorySize;
            if (inventory[newIndex] != null)
            {
                currentIndex = newIndex;
                playerWeapon.ChangeWeapon(inventory[currentIndex]);
                HighlightCurrentSlot();
                return;
            }
        }
    }

    private void HandleScrollInput()
    {
        Vector2 scrollInput = Mouse.current?.scroll.ReadValue() ?? Vector2.zero;

        if (Mathf.Abs(scrollInput.y) < scrollThreshold)
        {
            scrollProcessed = false;
            return;
        }

        if (scrollProcessed) return;

        scrollProcessed = true;
        int direction = scrollInput.y > 0 ? -1 : 1;
        SwitchWeapon(direction);
    }
    void SelectWeapon(int index)
    {
        currentIndex = index;
        playerWeapon.ChangeWeapon(inventory[currentIndex]);
        HighlightCurrentSlot();
    }
    public void TryAddWeapon(WeaponSO weapon)
    {
        if (weapon == null) return;
        //Debug.Log($"Trying to add {weapon}!");

        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] == null)
            {
                inventory[i] = weapon;
                UpdateSlotUI(i);
                return;
            }
        }

        GameObject pickup = Instantiate(weaponInteractablePrefab, transform.position, Quaternion.identity);
        WeaponInteractable weaponInteractable = pickup.GetComponent<WeaponInteractable>();
        weaponInteractable.SetWeapon(inventory[currentIndex]);

        inventory[currentIndex] = weapon;
        UpdateSlotUI(currentIndex);
        playerWeapon.ChangeWeapon(weapon);
        HighlightCurrentSlot();
    }

    private void CreateInventoryUI()
    {
        for (int i = 0; i < inventorySize; i++)
        {
            GameObject slotObj = Instantiate(inventorySlotPrefab, slotParent);
            slotObj.name = $"InventorySlot_{i}";

            Slot slot = new Slot { root = slotObj };

            Transform iconObj = slotObj.transform.Find("Icon");
            slot.icon = iconObj.GetComponent<Image>();

            Image bgImage = slotObj.GetComponent<Image>();
            slot.background = bgImage;

            slots[i] = slot;
        }
    }

    private void UpdateSlotUI(int index)
    {
        if (index < 0 || index >= slots.Length) return;

        Slot slot = slots[index];

        slot.icon.sprite = inventory[index]?.sprite;
        slot.icon.SetNativeSize();
        slot.icon.enabled = inventory[index] != null;

        slot.background.color = (index == currentIndex) ? selectedBgColor : normalBgColor;
    }

    private void UpdateAllSlotUI()
    {
        for (int i = 0; i < inventorySize; i++)
            UpdateSlotUI(i);
    }

    private void HighlightCurrentSlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            Slot slot = slots[i];
            slot.background.color = (i == currentIndex) ? selectedBgColor : normalBgColor;
        }
    }
}
