using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

/**
 * Author: Aleksandra Rusek
 * 
 * P_DragAndDrop class.
 * Handles drag-and-drop functionality for puzzle pieces.
 */
public class P_DragAndDrop : MonoBehaviour
{
    public GameObject SelectedPiece; /* Reference to the currently selected puzzle piece. */
    int OIL = 1; /* Sorting order index for the selected puzzle piece. */

    /**
     * Update is called once per frame. 
     * Handles moving the selected puzzle piece.
     */
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.transform != null && hit.transform.CompareTag("Puzzle")) 
            {
                if (!hit.transform.GetComponent<P_PiecesScript>().InRightPosition)
                {
                    SelectedPiece = hit.transform.gameObject;
                    SelectedPiece.GetComponent<P_PiecesScript>().Selected = true;
                    SelectedPiece.GetComponent<SortingGroup>().sortingOrder = OIL;
                    OIL++;
                }
            }
        }
        if (Input.GetMouseButtonUp(0)) 
        {
            if (SelectedPiece != null)
            {
                SelectedPiece.GetComponent<P_PiecesScript>().Selected = false;
                SelectedPiece = null;
            }
        }
        if (SelectedPiece != null) 
        {
            Vector3 MousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            SelectedPiece.transform.position = new Vector3(MousePoint.x, MousePoint.y, 0);
        }
    }
}
