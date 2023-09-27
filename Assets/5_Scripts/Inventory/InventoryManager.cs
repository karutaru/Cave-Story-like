using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField]
    private GameObject inventory_UI;
    [SerializeField]
    private Image mainInventory_1;
    [SerializeField]
    private Image mainInventory_2;

    [SerializeField]
    private Image inventory_Black;
    [SerializeField]
    private Image inventory_1_Black;
    [SerializeField]
    private Image inventory_2_Black;

    [SerializeField]
    private Image inventory_1_Menu;
    [SerializeField]
    private Image inventory_1_Pick_1;
    [SerializeField]
    private Image inventory_1_Pick_2;
    [SerializeField]
    private Image inventory_1_Pick_3;
    [SerializeField]
    private Image inventory_1_Pick_4;
    [SerializeField]
    private Image inventory_1_Pick_5;
    [SerializeField]
    private Image inventory_1_Pick_6;
    [SerializeField]
    private Image inventory_1_Pick_7;
    [SerializeField]
    private Image inventory_1_Pick_8;
    [SerializeField]
    private Image inventory_1_Pick_9;
    [SerializeField]
    private Image inventory_1_Pick_10;

    [SerializeField]
    private Image inventory_2_Menu;


    public int inventory_menu;

    public int cursor_vertical = 1;
    public int cursor_horizontal = 1;


    private void Start()
    {
        InventorySelect_Delete();

    }

    private void Update()
    {
        InventoryActivate();

        InventoryCursor();

        InventorySelect();

        //InventoryPop();

        InventoryCursorLimit();
    }



    private void InventoryActivate()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (inventory_UI.activeSelf)
            {
                inventory_UI.SetActive(false);
                Time.timeScale = 1;
                InventoryInitialization();
            }
            else
            {
                inventory_UI.SetActive(true);
                Time.timeScale = 0;
            }
        }
    }

    private void InventoryInitialization()
    {
        cursor_vertical = 1;
        cursor_horizontal = 1;
    }

    private void InventoryCursor()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            cursor_vertical--;
            if (cursor_vertical <= 0)
            {
                cursor_vertical = 1;
            }
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            cursor_vertical++;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            cursor_horizontal++;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            cursor_horizontal--;
            if (cursor_horizontal <= 0)
            {
                cursor_horizontal = 1;
            }
        }
    }

    private void InventorySelect()
    {
        if (cursor_vertical == 1)
        {
            inventory_Black.enabled = true;
        }
        else
        {
            inventory_Black.enabled = false;
        }


        if (cursor_vertical == 1 && cursor_horizontal == 1)
        {
            InventorySelect_Delete();
            mainInventory_1.enabled = true;
            mainInventory_2.enabled = false;

            inventory_1_Menu.enabled = true;

            inventory_2_Black.enabled = false;
            inventory_1_Black.enabled = false;
        }

        if (cursor_vertical == 1 && cursor_horizontal == 2)
        {
            InventorySelect_Delete();
            mainInventory_2.enabled = true;
            mainInventory_1.enabled = false;

            inventory_2_Menu.enabled = true;

            inventory_1_Black.enabled = false;
            inventory_2_Black.enabled = false;
        }

        if (cursor_vertical == 1 && cursor_horizontal >= 3)
        {
            cursor_horizontal = 2;
        }

        //if (cursor_vertical >= 2 && cursor_horizontal == 1)
        //{
        //    inventory_1_Black.enabled = true;
        //    inventory_2_Black.enabled = false;
        //}

        //if (cursor_vertical >= 2 && cursor_horizontal == 2)
        //{
        //    inventory_2_Black.enabled = true;
        //    inventory_1_Black.enabled = false;
        //}



        if (cursor_vertical == 2 && cursor_horizontal == 1)
        {
            InventorySelect_Delete();
            inventory_1_Pick_1.enabled = true;
        }
        if (cursor_vertical == 3 && cursor_horizontal == 1)
        {
            InventorySelect_Delete();
            inventory_1_Pick_2.enabled = true;
        }
        if (cursor_vertical == 4 && cursor_horizontal == 1)
        {
            InventorySelect_Delete();
            inventory_1_Pick_3.enabled = true;
        }
        if (cursor_vertical == 2 && cursor_horizontal == 2)
        {
            InventorySelect_Delete();
            inventory_1_Pick_4.enabled = true;
        }
        if (cursor_vertical == 3 && cursor_horizontal == 2)
        {
            InventorySelect_Delete();
            inventory_1_Pick_5.enabled = true;
        }
        if (cursor_vertical == 3 && cursor_horizontal == 3)
        {
            InventorySelect_Delete();
            inventory_1_Pick_6.enabled = true;
        }
        if (cursor_vertical == 3 && cursor_horizontal == 4)
        {
            InventorySelect_Delete();
            inventory_1_Pick_7.enabled = true;
        }
        if (cursor_vertical == 4 && cursor_horizontal == 2)
        {
            InventorySelect_Delete();
            inventory_1_Pick_8.enabled = true;
        }
        if (cursor_vertical == 4 && cursor_horizontal == 3)
        {
            InventorySelect_Delete();
            inventory_1_Pick_9.enabled = true;
        }
        if (cursor_vertical == 4 && cursor_horizontal == 4)
        {
            InventorySelect_Delete();
            inventory_1_Pick_10.enabled = true;
        }
    }

    //private void InventoryPop()
    //{
    //    if (inventory_menu == 1)
    //    {
    //        mainInventory_1.enabled = true;
    //        mainInventory_2.enabled = false;

    //        inventory_1_Menu.enabled = true;
    //    }
    //    if (inventory_menu == 2)
    //    {
    //        mainInventory_2.enabled = true;
    //        mainInventory_1.enabled = false;

    //        inventory_2_Menu.enabled = true;
    //    }
    //}

    private void InventoryCursorLimit()
    {
        if (inventory_menu == 1)
        {

        }
        if (inventory_menu == 2)
        {

        }

        if (cursor_vertical >= 5)
        {
            cursor_vertical = 4;
        }

        if (cursor_horizontal >= 5)
        {
            cursor_horizontal = 4;
        }

        if (cursor_vertical == 2 && cursor_horizontal >= 3)
        {
            cursor_vertical++;
        }
    }

    private void InventorySelect_Delete()
    {
        inventory_1_Menu.enabled = false;
        inventory_1_Pick_1.enabled = false;
        inventory_1_Pick_2.enabled = false;
        inventory_1_Pick_3.enabled = false;
        inventory_1_Pick_4.enabled = false;
        inventory_1_Pick_5.enabled = false;
        inventory_1_Pick_6.enabled = false;
        inventory_1_Pick_7.enabled = false;
        inventory_1_Pick_8.enabled = false;
        inventory_1_Pick_9.enabled = false;
        inventory_1_Pick_10.enabled = false;

        inventory_2_Menu.enabled = false;
    }
}
