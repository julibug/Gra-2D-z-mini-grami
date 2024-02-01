using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine.UI;

/**
 * Author: Aleksandra Rusek
 * 
 * Class for managing the game board in a match-3 game.
 */
public class M_Board : MonoBehaviour
{
    public static M_Board Instance { get; private set; } /* The singleton instance of the M_Board class. */
    [SerializeField] private AudioClip collectSound; /* The sound to play when collecting tiles. */
    [SerializeField] private AudioSource _audioSource; /* The audio source to play sound effects. */
    public M_Row[] rows; /* The rows of tiles on the game board. */
    public M_Tile[,] tiles { get; private set; } /** The 2D array of tiles representing the game board. */
    public int Width => tiles.GetLength(0); /** The width of the game board. */
    public int Height=> tiles.GetLength(1); /** The height of the game board. */

    private M_Tile _selectedTile1;  /* The first selected tile for swapping. */
    private M_Tile _selectedTile2; /* The second selected tile for swapping. */
    private readonly List<M_Tile> _selection = new List<M_Tile>(); /* The list to store the selected tiles for swapping. */
    private const float TweenDuration = 0.25f; /* The duration of the swap animation. */
    private bool isSwapping = false; /* Flag indicating whether a swap animation is in progress. */
    private bool isWaiting = false; /* Flag indicating whether the game is waiting before shuffling the board. */
    private float waitStartTime;   /* The start time of the waiting period. */
    public Text noMoves;  /* The text displaying a message when there are no possible moves. */
    private int consecutiveMatches = 0; /* The count of consecutive matches for triggering special effects. */
    private int comboCount = 0; /* The count of tiles matched in a single combo. */
    [SerializeField] private float timeRemainingSet = 10f; /* Variable holding the number of seconds until the hint is displayed. */
    private bool isTimerRunning = true; /* Flag representing if the time is running. */
    private float timeRemaining = 0f; /* Variable holding the number of seconds until the hint is displayed. */
    public GameObject hintParticles; /* The particle system prefab for hint effects. */
    private GameObject hint1; /* Reference to the first hint particle effect. */
    private GameObject hint2; /* Reference to the second hint particle effect. */
    private bool hintState = false; /* Flag indicating whether hint particles are currently active. */

    /**
     * The awake method to set up the singleton instance.
     */
    public void Awake() => Instance = this;

    /**
     * The start method to initialize the game board.
     */
    void Start()
    {
        noMoves.gameObject.SetActive(false);
        tiles = new M_Tile[rows.Max(row => row.Rtiles.Length), rows.Length];

        for (var y = 0; y < Height; y++)
        {
            for (var x =0; x < Width; x++)
            {
                var tile = rows[y].Rtiles[x];
                tile.x = x;
                tile.y = y;
                tile.Item = M_ItemDatabase.Items[Random.Range(0, M_ItemDatabase.Items.Length)];
                tiles[x, y] = tile;

            }
        }
        while (CanPop())
        {
            ShuffleBoard();
        }
        timeRemaining = timeRemainingSet;
    }
    /**
     * The update method to check for possible moves and trigger shuffling and check if hint needs to be displayed.
     */
    public void Update()
    {
        if (!HasPossibleMoves() && !isWaiting)
        {
            noMoves.gameObject.SetActive(true);
            isWaiting = true;
            waitStartTime = Time.time;
        }
        if (isWaiting && Time.time - waitStartTime >= 2.5f)
        {
            ShuffleBoard();
            noMoves.gameObject.SetActive(false);
            isWaiting = false;
        }
        if (isTimerRunning)
        {
            timeRemaining -= Time.deltaTime;
            if (timeRemaining <= 0f)
            {
                isTimerRunning = false;
                ShowHint();
            }
        }

    }

    /**
     * Method to handle tile selection.
     * @param tile The selected tile.
     */
    public async void Select(M_Tile tile)
    {
        if (isSwapping)
        {
            return;
        }
        if (!_selection.Contains(tile)) 
        { 
            if (_selection.Count > 0)
            {
                if (System.Array.IndexOf(_selection[0].Neighbours, tile) != -1)
                {
                    _selection.Add(tile);
                }
                else
                {
                    _selection.Clear();
                    _selection.Add(tile);
                }
            }
            else
            {
                _selection.Add(tile);
            } 
        }

        if (_selection.Count < 2) return;
        if (_selection.Count >= 2)
        {
            isSwapping = true;
            await Swap(_selection[0], _selection[1]);
            isSwapping = false;
        }

        if (CanPop())
        {
            Pop();
        }
        else
        {
            isSwapping = true;
            await Swap(_selection[0], _selection[1]);
            isSwapping = false;
        }
        _selection.Clear();
    }

    /**
     * Async method to swap two tiles.
     * @param tile1 The first tile to swap.
     * @param tile2 The second tile to swap.
     */
    public async Task Swap(M_Tile tile1, M_Tile tile2)
    {
        var icon1 = tile1.icon;
        var icon2 = tile2.icon;

        var icon1Transform = icon1.transform;
        var icon2Transform = icon2.transform;

        var sequence = DOTween.Sequence();

        sequence.Join(icon1Transform.DOMove(icon2Transform.position, TweenDuration))
                .Join(icon2Transform.DOMove(icon1Transform.position, TweenDuration));

        await sequence.Play()
                      .AsyncWaitForCompletion();

        icon1Transform.SetParent(tile2.transform);
        icon2Transform.SetParent(tile1.transform);

        tile1.icon = icon2;
        tile2.icon = icon1;

        var tile1Item = tile1.Item;

        tile1.Item = tile2.Item;
        tile2.Item = tile1Item;

    }


    /**
     * Method to check if popping is possible on the board.
     * @return True if popping is possible, false otherwise.
     */
    private bool CanPop()
    {
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                if (tiles[x, y].GetConnectedTiles().Skip(1).Count() >= 2 ) { return true; }
            }
        }
        consecutiveMatches = 0;
        return false;

    }

    /**
     * Method to initiate popping of matched tiles on the board.
     */
    private async void Pop()
    {
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                var tile = tiles[x, y];

                var connectedTiles = tile.GetConnectedTiles();

                if (connectedTiles.Skip(1).Count() < 2) continue;

                comboCount += connectedTiles.Count; 
                consecutiveMatches++; 
                var deflateSequence = DOTween.Sequence();

                foreach (var connectedTile in connectedTiles)
                {
                    deflateSequence.Join(connectedTile.icon.transform.DOScale(Vector3.zero, TweenDuration));
                }

                _audioSource.PlayOneShot(collectSound);

                if (comboCount >= 5)
                {
                    HitBoss();
                    M_Score.Instance.Score += 100;
                }
                if (consecutiveMatches >= 3)
                {
                    HitBoss();
                    M_Score.Instance.Score += 100;
                }

                M_Score.Instance.Score += tile.Item.value * connectedTiles.Count;

                await deflateSequence.Play()
                                     .AsyncWaitForCompletion();


                var inflateSequence = DOTween.Sequence();

                foreach(var connectedTile in connectedTiles)
                {
                    connectedTile.Item = M_ItemDatabase.Items[Random.Range(0, M_ItemDatabase.Items.Length)];
                    inflateSequence.Join(connectedTile.icon.transform.DOScale(Vector3.one, TweenDuration));
                }

                await inflateSequence.Play()
                                       .AsyncWaitForCompletion();

                x = 0;
                y = 0;

                comboCount = 0;

                SimpleHitBoss();
                ResetTimer();
                if (hintState == true)
                    DestroyHint();
                
            }
        }
    }

    /**
     * Method to check if there are possible moves on the board.
     * @return True if there are possible moves, false otherwise.
     */
    private bool HasPossibleMoves()
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                M_Tile currentTile = tiles[x, y];

                if (x < Width - 1)
                {
                    SwapTiles(currentTile, tiles[x + 1, y]);
                    if (CanPop())
                    {
                        SwapTiles(currentTile, tiles[x + 1, y]);
                        return true;
                    }
                    SwapTiles(currentTile, tiles[x + 1, y]);
                }

                if (y < Height - 1)
                {
                    SwapTiles(currentTile, tiles[x, y + 1]);
                    if (CanPop())
                    {
                        SwapTiles(currentTile, tiles[x, y + 1]);
                        return true;
                    }
                    SwapTiles(currentTile, tiles[x, y + 1]);
                }

                if (x > 0)
                {
                    SwapTiles(currentTile, tiles[x - 1, y]);
                    if (CanPop())
                    {
                        SwapTiles(currentTile, tiles[x - 1, y]);
                        return true;
                    }
                    SwapTiles(currentTile, tiles[x - 1, y]);
                }

                if (y > 0)
                {
                    SwapTiles(currentTile, tiles[x, y - 1]);
                    if (CanPop())
                    {
                        SwapTiles(currentTile, tiles[x, y - 1]);
                        return true;
                    }
                    SwapTiles(currentTile, tiles[x, y - 1]);
                }
            }
        }

        Debug.Log("No possible matches");
        return false;
    }

    /**
     * Method to swap tiles without modifying the board.
     * @param tile1 The first tile to swap.
     * @param tile2 The second tile to swap.
     */
    private void SwapTiles(M_Tile tile1, M_Tile tile2)
    {
        if (tile1 == null || tile2 == null)
        {
            Debug.LogWarning("One of the tiles is null. Swap aborted.");
            return;
        }

        M_Item tempItem = tile1.Item;
        tile1.Item = tile2.Item;
        tile2.Item = tempItem;
    }

    /**
     * Hits the boss character.
     */
    private void HitBoss()
    {
        M_WiggleBoss wiggleBossScript = FindObjectOfType<M_WiggleBoss>();
        if (wiggleBossScript != null)
        {
            wiggleBossScript.Hit();
        }
    }

    /**
     * Performs a simple hit on the boss character.
     */
    private void SimpleHitBoss()
    {
        M_WiggleBoss wiggleBossScript = FindObjectOfType<M_WiggleBoss>();
        if (wiggleBossScript != null)
        {
            wiggleBossScript.SimpleHit();
        }
    }

    /**
     * Counts the number of tiles of each type on the board.
     */
    public Dictionary<M_Item, int> CountTilesOfType()
    {
        Dictionary<M_Item, int> tileCounts = new Dictionary<M_Item, int>();

        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                M_Item item = tiles[x, y].Item;

                if (item != null)
                {
                    if (tileCounts.ContainsKey(item))
                    {
                        tileCounts[item]++;
                    }
                    else
                    {
                        tileCounts[item] = 1;
                    }
                }
            }
        }

        return tileCounts;
    }

    /**
     * Shuffles the board.
     */
    public void ShuffleBoard()
    {
        Dictionary<M_Item, int> originalTileCounts = CountTilesOfType();

        List<M_Tile> allTiles = new List<M_Tile>();
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                allTiles.Add(tiles[x, y]);
            }
        }

        for (int i = 0; i < allTiles.Count; i++)
        {
            int randomIndex = Random.Range(i, allTiles.Count);
            M_Tile temp = allTiles[i];
            allTiles[i] = allTiles[randomIndex];
            allTiles[randomIndex] = temp;
        }

        Dictionary<M_Item, int> remainingTileCounts = new Dictionary<M_Item, int>(originalTileCounts);

        foreach (M_Tile tile in allTiles)
        {
            List<M_Item> availableTypes = remainingTileCounts.Keys.ToList();
            M_Item randomType = availableTypes[Random.Range(0, availableTypes.Count)];

            tile.Item = randomType;

            remainingTileCounts[randomType]--;
            if (remainingTileCounts[randomType] == 0)
            {
                remainingTileCounts.Remove(randomType);
            }
        }

        if (CanPop())
        {
            ShuffleBoard();
        }

        ResetTimer();
    }

    /**
     * Destroys the hint particle effects if they are active.
     */
    private void DestroyHint()
    {
        if (hint1 != null)
            Destroy(hint1);
        if (hint2 != null)
            Destroy(hint2);
        hintState = false;
    }

    /**
     * Resets the timer for the hint.
     */
    private void ResetTimer()
    {
        timeRemaining = timeRemainingSet;
        isTimerRunning = true;
    }

    /**
     * Highlights and shows hint particles for potential moves on the board.
     * @param tile1 The first tile involved in the potential move.
     * @param tile2 The second tile involved in the potential move.
     */
    private void HighlightTiles(M_Tile tile1, M_Tile tile2)
    {

        if (hintParticles != null)
        {
            hint1 = Instantiate(hintParticles, tile1.transform.position, Quaternion.identity);
            hint2 = Instantiate(hintParticles, tile2.transform.position, Quaternion.identity);
            hintState = true;
        }

    }

    /**
    * Method to show a hint a pair of tiles that can be swapped for a match.
    */
    private void ShowHint()
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                M_Tile currentTile = tiles[x, y];

                if (x < Width - 1)
                {
                    SwapTiles(currentTile, tiles[x + 1, y]);
                    if (CanPop())
                    {
                        SwapTiles(currentTile, tiles[x + 1, y]);
                        HighlightTiles(currentTile, tiles[x + 1, y]);
                        return;
                    }
                    SwapTiles(currentTile, tiles[x + 1, y]);
                }

                if (y < Height - 1)
                {
                    SwapTiles(currentTile, tiles[x, y + 1]);
                    if (CanPop())
                    {
                        SwapTiles(currentTile, tiles[x, y + 1]);
                        HighlightTiles(currentTile, tiles[x, y + 1]);
                        return;
                    }
                    SwapTiles(currentTile, tiles[x, y + 1]);
                }

                if (x > 0)
                {
                    SwapTiles(currentTile, tiles[x - 1, y]);
                    if (CanPop())
                    {
                        SwapTiles(currentTile, tiles[x - 1, y]);
                        HighlightTiles(currentTile, tiles[x - 1, y]);
                        return;
                    }
                    SwapTiles(currentTile, tiles[x - 1, y]);
                }

                if (y > 0)
                {
                    SwapTiles(currentTile, tiles[x, y - 1]);
                    if (CanPop())
                    {
                        SwapTiles(currentTile, tiles[x, y - 1]);
                        HighlightTiles(currentTile, tiles[x, y - 1]);
                        return;
                    }
                    SwapTiles(currentTile, tiles[x, y - 1]);
                }
            }
        }
    }
}
