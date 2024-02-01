using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cainos.LucidEditor;

/**
 * Edited by: Julia Bugaj
 * 
 * The Chest class represents an interactive chest in the game. It is an edited script that was originally included with PixelArtPlatformer assets, but it has been modified to meet the necessary requirements.
 */
namespace Cainos.PixelArtPlatformer_VillageProps
{
    public class Chest : MonoBehaviour
    {
        public GameObject itemToSpawn; /** The item to spawn when the chest is opened. */
        public Transform spawnPoint; /** The spawn point for the spawned item. */
        public GameObject obj; /** The associated GameObject for the chest. */

        [FoldoutGroup("Reference")]
        public Animator animator; /** Reference to the Animator component for chest animations. */

        /**
         * Property indicating whether the chest is open or closed.
         */
        [FoldoutGroup("Runtime"), ShowInInspector, DisableInEditMode]
        public bool IsOpened
        {
            get { return isOpened; }
            set
            {
                isOpened = value;
                animator.SetBool("IsOpened", isOpened);
            }
        }
        private bool isOpened; /** Private variable to store the internal state of the chest being open or closed. */


        /**
         * Initializes the chest's state based on whether the associated item has been collected.
         */
        private void Start()
        {
            if (Inventory.instance.HasKey(itemToSpawn.name))
            {
                if (obj != null)
                {
                    obj.SetActive(false);
                }
            }
        }
        /**
         * Opens the chest and spawns the item if it hasn't been collected.
         */
        [FoldoutGroup("Runtime"),Button("Open"), HorizontalGroup("Runtime/Button")]
        public void Open()
        {
            if (!Inventory.instance.HasKey(itemToSpawn.name))
            {
                IsOpened = true;
                Instantiate(itemToSpawn, spawnPoint.position, Quaternion.identity);
                Debug.Log("Item spawned");
            }
            else{
                Debug.Log("Item is already collected");
            }
        }
        /**
         * Closes the chest.
         */
        [FoldoutGroup("Runtime"), Button("Close"), HorizontalGroup("Runtime/Button")]
        public void Close()
        {
            IsOpened = false;
        }
    }
}
