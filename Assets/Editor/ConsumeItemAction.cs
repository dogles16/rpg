﻿/// ---------------------------------------------
/// Ultimate Inventory System
/// Copyright (c) Opsive. All Rights Reserved.
/// https://www.opsive.com
/// ---------------------------------------------

namespace Opsive.UltimateInventorySystem.Demo.ItemActions
{
    using Opsive.UltimateInventorySystem.Core.AttributeSystem;
    using Opsive.UltimateInventorySystem.Core.DataStructures;
    using Opsive.UltimateInventorySystem.Demo.CharacterControl;
    using Opsive.UltimateInventorySystem.ItemActions;
    using UnityEngine;

    /// <summary>
    /// Demo Item action used to consume an item.
    /// </summary>
    [System.Serializable]
    public class ConsumeItemAction : ItemAction
    {
        protected int m_HealAmount;
        protected int m_ManaAmount;

        public int HealAmount => m_HealAmount;
        public int ManaAmount => m_ManaAmount;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ConsumeItemAction()
        {
            m_Name = "Consume";
        }

        /// <summary>
        /// Can the item action be invoked.
        /// </summary>
        /// <param name="itemInfo">The item info.</param>
        /// <param name="itemUser">The item user (can be null).</param>
        /// <returns>True if it can be invoked.</returns>
        protected override bool CanInvokeInternal(ItemInfo itemInfo, ItemUser itemUser)
        {
            var item = itemInfo.Item;
            var inventory = itemInfo.Inventory;
            var character = itemUser.GetComponent<CharStats>();
            if(character != null && inventory.MainItemCollection.HasItem((1, item)))
            {
                if (item.GetAttribute<Attribute<int>>("HealAmount") != null || item.GetAttribute<Attribute<int>>("ManaAmount") != null)
                {
                    if (character.healthTotal != character.healthMax)
                    {
                        return true;
                    }
                    else if (character.magicTotal != character.magicMax)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
            
            
        }

        /// <summary>
        /// Consume the item.
        /// </summary>
        /// <param name="itemInfo">The item info.</param>
        /// <param name="itemUser">The item user (can be null).</param>
        protected override void InvokeActionInternal(ItemInfo itemInfo, ItemUser itemUser)
        {
            var item = itemInfo.Item;
            var inventory = itemInfo.Inventory;
            var character = itemUser.GetComponent<CharStats>();
            inventory.MainItemCollection.RemoveItem(item);
            m_HealAmount = item.GetAttribute<Attribute<int>>("HealAmount").GetValue();
            m_ManaAmount = item.GetAttribute<Attribute<int>>("ManaAmount").GetValue();
            EventManager.TriggerEvent("PlayerHeal");
            character.Heal(m_HealAmount);
            character.AddMana(m_ManaAmount);
        }
    }
}