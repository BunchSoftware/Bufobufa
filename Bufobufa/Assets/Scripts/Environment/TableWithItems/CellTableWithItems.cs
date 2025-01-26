using Game.Environment.Item;
using Game.LPlayer;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Game.Environment.LTableWithItems
{
    public class CellTableWithItems : MonoBehaviour, ILeftMouseClickable
    {
        public UnityEvent<PickUpItem> OnPickUpItem;
        public UnityEvent<PickUpItem> OnPutItem;

        private TableWithItems tableWithItems;
        private TriggerObject triggerObject;
        private BoxCollider boxCollider;

        private PickUpItem currentItemInCell;

        private Player player;

        private bool isClick = false;

        public void Init(TableWithItems tableWithItems, Player player)
        {
            triggerObject = transform.Find("TriggerObject").GetComponent<TriggerObject>();
            boxCollider = GetComponent<BoxCollider>();

            this.tableWithItems = tableWithItems;
            this.player = player;

            triggerObject.OnTriggerStayEvent.AddListener((collider) =>
            {
                if (isClick)
                {
                    isClick = false;

                    if (player.PlayerPickUpItem && PutItem(player.GetPickUpItem()))
                    {
                        player.PutItem();
                        Debug.Log("� ������� ������� � Table");
                    }
                }
                else if (currentItemInCell != null && currentItemInCell.IsClicked)
                {
                    currentItemInCell.IsClicked = false;

                    if (player.PlayerPickUpItem == false)
                    {
                        player.PickUpItem(PickUpItem());
                        Debug.Log("� ������ ������� �� Table");
                    }
                }
            });
        }

        public void OnMouseLeftClickObject()
        {
            isClick = true;
        }

        public void OnMouseLeftClickOtherObject()
        {
            isClick = false;
        }

        public PickUpItem PickUpItem()
        {
            boxCollider.enabled = true;
            PickUpItem item = null;

            ScaleChooseObject scaleChooseObject = currentItemInCell.GetComponent<ScaleChooseObject>();

            if (scaleChooseObject != null)
                scaleChooseObject.RemoveComponent();

            item = currentItemInCell;

            OnPickUpItem?.Invoke(currentItemInCell);

            currentItemInCell = null;

            return item;
        }

        public bool PutItem(PickUpItem pickUpItem)
        {
            if (currentItemInCell == null)
            {
                OnPutItem?.Invoke(currentItemInCell);

                boxCollider.enabled = false;
                currentItemInCell = pickUpItem;
                currentItemInCell.transform.parent = transform;
                currentItemInCell.transform.position = transform.position;

                if (currentItemInCell.GetComponent<ScaleChooseObject>() == null)
                {
                    ScaleChooseObject scaleChooseObject = currentItemInCell.AddComponent<ScaleChooseObject>();
                    scaleChooseObject.coefficient = 1.15f;
                }

                return true;
            }

            return false;
        }
    }
}
