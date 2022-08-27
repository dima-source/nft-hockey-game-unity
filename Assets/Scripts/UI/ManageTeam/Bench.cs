using System;
using System.Collections.Generic;
using System.Linq;
using Near.Models.Tokens;
using Runtime;
using UI.ManageTeam.DragAndDrop;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace UI.ManageTeam
{
    public class Bench: MonoBehaviour
    {
        [NonSerialized] public List<Token> Cards = new();
        [NonSerialized] public List<UISlot> Slots = new();
        public ManageTeamView manageTeamView;

        public List<UIPlayer> Players
        {
            get {return Slots.ConvertAll(slot => slot.uiPlayer);}
            set { }
        }

        public void ReplacePlayer(UIPlayer oldPlayer, UIPlayer newPlayer)
        {
            int index = Players.IndexOf(oldPlayer);
            if (index == -1)
            {
                throw new ApplicationException("Bench does not contain this player");
            }
            Slots[index].uiPlayer = newPlayer;

            index = Cards.IndexOf(oldPlayer.CardData);
            Cards[index] = newPlayer.CardData;
        }

        private UISlot CreateSlotWithPlayer(Token card)
        {
                UISlot benchSlot = Instantiate(Game.AssetRoot.manageTeamAsset.uiSlot, transform);
                benchSlot.slotPosition = SlotPositionEnum.Bench;
                benchSlot.GetComponent<Image>().color = new Color(255f, 255f, 255f, 0f);
                
                UIPlayer player = Instantiate(Game.AssetRoot.manageTeamAsset.fieldPlayer);
                player.CardData = card;
                player.SetData(card);
                player.canvasContent = manageTeamView.canvasContent;
                player.transform.SetParent(benchSlot.transform);
                player.transform.localPosition = Vector3.zero;
                player.RectTransform.sizeDelta = new Vector2(150, 225);
                player.RectTransform.localScale = benchSlot.RectTransform.localScale;
                
                benchSlot.uiPlayer = player;
                benchSlot.uiPlayer.uiSlot = benchSlot;
                Slots.Add(benchSlot);
                return benchSlot;
        }
        
        private UISlot CreateSlotWithPlayer(UIPlayer player)
        {
                UISlot benchSlot = Instantiate(Game.AssetRoot.manageTeamAsset.uiSlot, transform);
                benchSlot.slotPosition = SlotPositionEnum.Bench;
                benchSlot.GetComponent<Image>().color = new Color(255f, 255f, 255f, 0f);
                
                player.transform.SetParent(benchSlot.transform);
                player.transform.localPosition = Vector3.zero;
                player.RectTransform.sizeDelta = new Vector2(150, 225);
                player.RectTransform.localScale = benchSlot.RectTransform.localScale;
                
                benchSlot.uiPlayer = player;
                benchSlot.uiPlayer.uiSlot = benchSlot;
                Slots.Add(benchSlot);
                return benchSlot;
        }

        public UISlot AddPlayer(UIPlayer player)
        {
            if (Cards.Contains(player.CardData))
                throw new ApplicationException($"Such player is already in bench");
            Cards.Add(player.CardData);
            return CreateSlotWithPlayer(player);
        }

        private void DestroySlot(UISlot slot)
        {
            slot.uiPlayer.uiSlot = null;
            Destroy(slot.gameObject);
        }

        public void RemoveSlotWithinPlayer(UIPlayer player)
        {
            if (!Cards.Remove(player.CardData))
            {
                throw new ApplicationException("Cannot remove ui player from bench");
            }

            if (!Slots.Remove(player.uiSlot))
            {
                throw new ApplicationException("Cannot remove slot from bench");
            }
            
            DestroySlot(player.uiSlot);
            Cards.Remove(player.CardData);
        }

        private void DestroyAllSlots()
        {
            List<UISlot> slotsClone = new(Slots);
            foreach (var slot in slotsClone)
            {
                DestroySlot(slot);
                Slots.Remove(slot);
            }
        }
        
        private void RebuildSlots()
        {
            DestroyAllSlots();
            foreach (var player in Cards)
            {
                CreateSlotWithPlayer(player);
            }
        }
        public void OnEnable()
        {
            RebuildSlots();
        }
        
        public void OnDisable()
        {
            DestroyAllSlots();
        }
    }
}