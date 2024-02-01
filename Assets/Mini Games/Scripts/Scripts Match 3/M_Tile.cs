using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * Author: Aleksandra Rusek
 * 
 * Tile class for representing individual tiles in a match-3 game.
 */
public class M_Tile : MonoBehaviour
{
    public int x;  /* The x-coordinate of the tile on the game board. */
    public int y;  /* The y-coordinate of the tile on the game board. */
    private M_Item _item; /* The item associated with the tile. */

    /** Gets or sets the item associated with the tile. */
    public M_Item Item
    {
        get => _item;

        set
        {
            if (_item == value) return;
            _item = value;
            icon.sprite = _item.sprite;
        }
    }

    public Image icon; /* The UI image component representing the icon of the item. */

    public Button button;  /* The button component associated with the tile. */

    /** Gets the left neighboring tile, if available. */
    public M_Tile Left => x > 0 ? M_Board.Instance.tiles[x - 1, y] : null;

    /** Gets the top neighboring tile, if available. */
    public M_Tile Top => y > 0 ? M_Board.Instance.tiles[x, y - 1] : null;

    /** Gets the right neighboring tile, if available. */
    public M_Tile  Right => x < M_Board.Instance.Width - 1 ? M_Board.Instance.tiles[x + 1, y] : null;

    /** Gets the bottom neighboring tile, if available. */
    public M_Tile Bottom => y < M_Board.Instance.Height - 1 ? M_Board.Instance.tiles[x, y + 1] : null;

    /** Gets an array of all neighboring tiles. */
    public M_Tile[] Neighbours => new[]
    {
        Left,
        Top,
        Right,
        Bottom,
    };

    /**
     * Start is called before the first frame update.
     * Adds a listener to the button's click event to notify the board when the tile is selected.
     */
    void Start()
    {
        button.onClick.AddListener(() => M_Board.Instance.Select(this));
    }

    /**
     * Gets a list of connected tiles of the same type.
     * @param exclude List of tiles to exclude from the search.
     * @return A list of connected tiles.
     */
    public List<M_Tile> GetConnectedTiles (List<M_Tile> exclude = null)
    {
        var result = new List<M_Tile> { this, };

        if(exclude == null)
        {
            exclude = new List<M_Tile> { this, };

        }
        else
        {
            exclude.Add(this);

        }
        foreach (var neighbour in Neighbours)  
        {

            if (neighbour == null || exclude.Contains(neighbour) || neighbour.Item != Item) { continue;  }

            result.AddRange(neighbour.GetConnectedTiles(exclude));

        }
        return result;
    }

}
