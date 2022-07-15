using Curves;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PuzzleController : MonoBehaviour
{
    public List<Texture2D> PuzzleImages;
    public SpriteRenderer Background;
    public Transform TilesParent;
    public Material ShadowMaterial;
    public int puzzleSize = 100;
    Color trans = new Color(0.0f, 0.0f, 0.0f, 0.0f);
    private List<Vector3> mBezierPoints = new List<Vector3>();
    private Texture2D mBaseTexture;
    public GameObject[,] mGameObjects;
    private int mTilesX;
    private int mTilesY;
    private float ortographicSize;
    private Vector3 cameraPos;
    public int currentPuzzleIndex = 0;
    public float scaleTime = 1.0f; 

    Vec2[] mCurvyCoords = new Vec2[]
    {
            new Vec2(0, 0),
            new Vec2(0.35f, 0.15f),
            new Vec2(0.37f, 0.05f),
            new Vec2(0.37f, 0.05f),
            new Vec2(0.40f, 0),
            new Vec2(0.38f, -0.05f),
            new Vec2(0.38f, -0.05f),
            new Vec2(0.20f, -0.20f),
            new Vec2(0.50f, -0.20f),
            new Vec2(0.50f, -0.20f),
            new Vec2(0.80f, -0.20f),
            new Vec2(0.62f, -0.05f),
            new Vec2(0.62f, -0.05f),
            new Vec2(0.60f, 0),
            new Vec2(0.63f, 0.05f),
            new Vec2(0.63f, 0.05f),
            new Vec2(0.65f, 0.15f),
            new Vec2(1, 0)
    };
    // Start is called before the first frame update
    void Start()
    {
        SetProperValues();
        CreateNewPuzzle();
    }

    public void ClearTiles()
    {
        foreach(Transform tile in TilesParent.GetComponentInChildren<Transform>())
        {
            Destroy(tile.gameObject);
        }
    }

    public void CreateNewPuzzle()
    {
        if(currentPuzzleIndex >= PuzzleImages.Count)
        {
            SceneManager.LoadScene(1);
            return;
        }
        CreateJigsawTiles();
        RePositionCamera(mTilesX, mTilesY);
        ShufflePuzzles();
    }

    public IEnumerator PrepareNextMap()
    {
        Debug.Log("Before");
        ClearTiles();
        float time = 0;
        while (time < scaleTime)
        {
            Camera.main.orthographicSize -= 0.1f;
            time += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(1.5f);

        currentPuzzleIndex += 1;
        CreateNewPuzzle();
    }

    public void DisplayColorBackground()
    {
        Texture2D tex = PuzzleImages[currentPuzzleIndex];
        Texture2D new_tex = new Texture2D(tex.width + 40, tex.height + 40, TextureFormat.ARGB32, 1, true);
        for (int i = 20; i < tex.width + 20; ++i)
        {
            for (int j = 20; j < tex.height + 20; ++j)
            {
                Color col = tex.GetPixel(i - 20, j - 20);
                col.a = 1.0f;
                new_tex.SetPixel(i, j, col);
            }
        }
        new_tex.Apply();

        Sprite sprite = SpriteUtils.CreateSpriteFromTexture2D(new_tex, 0, 0, mBaseTexture.width, mBaseTexture.height);
        Background.sprite = sprite;
    }

    public void ChangeMap()
    {
        DisplayColorBackground();
        StartCoroutine(PrepareNextMap());
    }

    private void SetProperValues()
    {
        for(int i=0; i< mCurvyCoords.Length; i++)
        {
            var vec = mCurvyCoords[i];
            mCurvyCoords[i] = new Vec2(vec.x * puzzleSize, vec.y * puzzleSize);
        }
    }

    public void RePositionCamera(int numTilesX, int numTilesY)
    {
        // We set the size of the camera. 
        // You can implement your own way of doing this.
        Camera.main.orthographicSize = numTilesX < numTilesY ?
            numTilesX * puzzleSize : numTilesY * puzzleSize;

        // Set the position of the camera to be at the
        // centre of the board.
        Camera.main.transform.position = new Vector3(
            (numTilesX * puzzleSize + 40) / 2,
            (numTilesY * puzzleSize + 40) / 2,
            -1000.0f);

        ortographicSize = Camera.main.orthographicSize;
        cameraPos = Camera.main.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void Shuffle(GameObject obj)
    {
        // get a random point within the region.
        float y = Random.Range
                        (Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).y, Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y - puzzleSize);
        float x = Random.Range
            (Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x, Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x - puzzleSize);

        // final position of the tile.
        Vector3 pos = new Vector3(x, y, 0.0f);

        StartCoroutine(Coroutine_MoveOverSeconds(obj, pos, 1.0f));
    }

    private IEnumerator Coroutine_MoveOverSeconds(GameObject objectToMove, Vector3 end, float seconds)
    {
        float elapsedTime = 0;
        Vector3 startingPos = objectToMove.transform.position;
        while (elapsedTime < seconds)
        {
            objectToMove.transform.position = Vector3.Lerp(startingPos, end, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
        objectToMove.transform.position = end;
    }

    public void ShufflePuzzles()
    {
        for (int i = 0; i < mTilesX; ++i)
        {
            for (int j = 0; j < mTilesY; ++j)
            {
                Shuffle(mGameObjects[i, j]);
            }
        }

    }

    public void CreateJigsawTiles()
    {
        // Load the main image.
        Texture2D tex = PuzzleImages[currentPuzzleIndex];
        if (!tex.isReadable)
        {
            Debug.Log("Texture is not readable");
            return;
        }

        mTilesX = tex.width / puzzleSize;
        mTilesY = tex.height / puzzleSize;

        // add 20 pixel border around.
        Texture2D new_tex = new Texture2D(tex.width + 40, tex.height + 40, TextureFormat.ARGB32, 1, true);
        for (int i = 20; i < tex.width + 20; ++i)
        {
            for (int j = 20; j < tex.height + 20; ++j)
            {
                Color col = tex.GetPixel(i - 20, j - 20);
                col.a = 1.0f;
                new_tex.SetPixel(i, j, col);
            }
        }
        new_tex.Apply();
        mBaseTexture = new_tex;

        // create the bezier curve.
        CreateBezierCurve();

        mGameObjects = new GameObject[mTilesX, mTilesY];
        for (int i = 0; i < mTilesX; ++i)
        {
            for (int j = 0; j < mTilesY; ++j)
            {
                CreateSpriteGameObject(i, j);
            }
        }

        // now make the background image light transparent.
        for (int i = 20; i < tex.width + 20; ++i)
        {
            for (int j = 20; j < tex.height + 20; ++j)
            {
                Color col = tex.GetPixel(i - 20, j - 20);
                col.a = 0.4f;
                new_tex.SetPixel(i, j, col);
            }
        }
        new_tex.Apply();
        mBaseTexture = new_tex;

        Sprite sprite = SpriteUtils.CreateSpriteFromTexture2D(mBaseTexture, 0, 0, mBaseTexture.width, mBaseTexture.height);
        Background.sprite = sprite;

        //RelocateCamera();
    }

    void CreateBezierCurve()
    {
        // use bezier curve.
        Bezier bez = new Bezier(mCurvyCoords.OfType<Vec2>().ToList());

        for (int i = 0; i < puzzleSize; i++)
        {
            Vec2 bp = bez.ValueAt(i / (float)puzzleSize);
            Vector3 p = new Vector3(bp.x, bp.y, 0.0f);

            mBezierPoints.Add(p);
        }
    }

    void CreateSpriteGameObject(int i, int j)
    {
        GameObject obj = new GameObject();
        obj.name = "Tile_" + i.ToString() + "_" + j.ToString();
        SplitTile tile = obj.AddComponent<SplitTile>();

        tile.ShadowMaterial = ShadowMaterial;
        tile.puzzleSize = puzzleSize;
        tile.mIndex = new Vector2Int(i, j);
        mGameObjects[i, j] = obj;

        if (TilesParent != null)
        {
            obj.transform.SetParent(TilesParent);
        }

        SpriteRenderer spren = obj.AddComponent<SpriteRenderer>();
        tile.mSpriteRenderer = spren;

        obj.transform.position = new Vector3(i * puzzleSize, j * puzzleSize, 0.0f);

        // create a new tile texture.
        Texture2D mTileTexture = CreateTileTexture(i, j);

        tile.mDirections[0] = GetRandomDirection(0);
        tile.mDirections[1] = GetRandomDirection(1);
        tile.mDirections[2] = GetRandomDirection(2);
        tile.mDirections[3] = GetRandomDirection(3);

        // check for bottom and left tile.
        if (j > 0)
        {
            SplitTile downTile = mGameObjects[i, j - 1].GetComponent<SplitTile>();
            if (downTile.mDirections[0] == Direction.UP)
            {
                tile.mDirections[2] = Direction.DOWN_REVERSE;
            }
            else
            {
                tile.mDirections[2] = Direction.DOWN;
            }
        }

        // check for bottom and left tile.
        if (i > 0)
        {
            SplitTile downTile = mGameObjects[i - 1, j].GetComponent<SplitTile>();
            if (downTile.mDirections[1] == Direction.RIGHT)
            {
                tile.mDirections[3] = Direction.LEFT_REVERSE;
            }
            else
            {
                tile.mDirections[3] = Direction.LEFT;
            }
        }

        if (i == 0)
        {
            tile.mDirections[3] = Direction.NONE;
        }
        if (i == mTilesX - 1)
        {
            tile.mDirections[1] = Direction.NONE;
        }
        if (j == 0)
        {
            tile.mDirections[2] = Direction.NONE;
        }
        if (j == mTilesY - 1)
        {
            tile.mDirections[0] = Direction.NONE;
        }
        for (int d = 0; d < tile.mDirections.Length; ++d)
        {
            if (tile.mDirections[d] != Direction.NONE)
                ApplyBezierMask(mTileTexture, tile.mDirections[d]);
        }

        mTileTexture.Apply();

        // Set the tile texture to the sprite.
        Sprite sprite = SpriteUtils.CreateSpriteFromTexture2D(mTileTexture, 0, 0, puzzleSize + 40, puzzleSize + 40);
        spren.sprite = sprite;

        obj.AddComponent<BoxCollider2D>();
    }

    Texture2D CreateTileTexture(int indx, int indy)
    {
        int w = puzzleSize + 40; 
        int h = puzzleSize + 40;

        Texture2D new_tex = new Texture2D(w, h, TextureFormat.ARGB32, 1, true);

        int startX = indx * puzzleSize;
        int startY = indy * puzzleSize;
        for (int i = 0; i < puzzleSize + 40; ++i)
        {
            for (int j = 0; j < puzzleSize + 40; ++j)
            {
                Color color = mBaseTexture.GetPixel(i + startX, j + startY);
                new_tex.SetPixel(i, j, color);
                if (i < 20 && j < 20)
                {
                    new_tex.SetPixel(i, j, trans);
                }
                if (i >= puzzleSize + 20 && j < 20)
                {
                    new_tex.SetPixel(i, j, trans);
                }
                if (i >= puzzleSize + 20 && j >= puzzleSize + 20)
                {
                    new_tex.SetPixel(i, j, trans);
                }
                if (i < 20 && j >= puzzleSize + 20)
                {
                    new_tex.SetPixel(i, j, trans);
                }
            }
        }
        return new_tex;
    }

    public enum Direction
    {
        UP,
        UP_REVERSE,
        RIGHT,
        RIGHT_REVERSE,
        DOWN,
        DOWN_REVERSE,
        LEFT,
        LEFT_REVERSE,
        NONE,
    }

    Direction GetRandomDirection(int side)
    {
        float rand = Random.Range(0.0f, 1.0f);
        switch (side)
        {
            case 0:
                {
                    if (rand < 0.5f) return Direction.UP;
                    else return Direction.UP_REVERSE;
                }
            case 1:
                {
                    if (rand < 0.5f) return Direction.RIGHT;
                    else return Direction.RIGHT_REVERSE;
                }
            case 2:
                {
                    if (rand < 0.5f) return Direction.DOWN;
                    else return Direction.DOWN_REVERSE;
                }
            case 3:
                {
                    if (rand < 0.5f) return Direction.LEFT;
                    else return Direction.LEFT_REVERSE;
                }
        }
        return Direction.UP;
    }

    void ApplyBezierMask(Texture2D mTileTexture, Direction dir)
    {
        switch (dir)
        {
            case Direction.UP:
                {
                    for (int i = 0; i < puzzleSize; ++i)
                    {
                        int y = -GetInterpolatedY(mBezierPoints, i);

                        for (int j = puzzleSize + 20 + y; j < puzzleSize + 40; ++j)
                        {
                            mTileTexture.SetPixel(i + 20, j, trans);
                        }
                        mTileTexture.SetPixel(i + 20, puzzleSize + 20 + y, Color.gray);
                    }
                    break;
                }
            case Direction.UP_REVERSE:
                {
                    for (int i = 0; i < puzzleSize; ++i)
                    {
                        int y = GetInterpolatedY(mBezierPoints, i);

                        for (int j = puzzleSize + 20 + y; j < puzzleSize + 40; ++j)
                        {
                            mTileTexture.SetPixel(i + 20, j, trans);
                        }
                        mTileTexture.SetPixel(i + 20, puzzleSize + 20 + y, Color.gray);
                    }
                    break;
                }
            case Direction.RIGHT:
                {
                    for (int j = 0; j < puzzleSize; ++j)
                    {
                        int x = -GetInterpolatedY(mBezierPoints, j);

                        //mTileTexture.SetPixel(120 + x, j + 20, Color.gray);
                        for (int i = puzzleSize + 19 + x; i < puzzleSize + 40; ++i)
                        {
                            mTileTexture.SetPixel(i, j + 20, trans);
                        }
                    }
                    break;
                }
            case Direction.RIGHT_REVERSE:
                {
                    for (int j = 0; j < puzzleSize; ++j)
                    {
                        int x = GetInterpolatedY(mBezierPoints, j);

                        //mTileTexture.SetPixel(120 + x, j + 20, Color.gray);
                        for (int i = puzzleSize + 21 + x; i < puzzleSize + 40; ++i)
                        {
                            mTileTexture.SetPixel(i, j + 20, trans);
                        }
                    }
                    break;
                }
            case Direction.DOWN:
                {
                    for (int i = 0; i < puzzleSize; ++i)
                    {
                        int y = GetInterpolatedY(mBezierPoints, i);

                        //mTileTexture.SetPixel(i + 20, y + 20, trans);
                        for (int j = 0; j < y + 19; ++j)
                        {
                            mTileTexture.SetPixel(i + 20, j, trans);
                        }
                    }
                    break;
                }
            case Direction.DOWN_REVERSE:
                {
                    for (int i = 0; i < puzzleSize; ++i)
                    {
                        int y = -GetInterpolatedY(mBezierPoints, i);

                        //mTileTexture.SetPixel(i + 20, y + 20, trans);
                        for (int j = 0; j < y + 19; ++j)
                        {
                            mTileTexture.SetPixel(i + 20, j, trans);
                        }
                    }
                    break;
                }
            case Direction.LEFT:
                {
                    for (int j = 0; j < puzzleSize; ++j)
                    {
                        int x = GetInterpolatedY(mBezierPoints, j);

                        //mTileTexture.SetPixel(x + 20, j, trans);
                        for (int i = 0; i < x + 19; ++i)
                        {
                            mTileTexture.SetPixel(i, j + 20, trans);
                        }
                    }
                    break;
                }
            case Direction.LEFT_REVERSE:
                {
                    for (int j = 0; j < puzzleSize; ++j)
                    {
                        int x = -GetInterpolatedY(mBezierPoints, j);

                        //mTileTexture.SetPixel(x + 20, j + 20, trans);
                        for (int i = 0; i < x + 21; ++i)
                        {
                            mTileTexture.SetPixel(i, j + 20, trans);
                        }
                    }
                    break;
                }
        }

        int GetInterpolatedY(List<Vector3> mBezierPoints, int x)
        {
            for (int i = 1; i < mBezierPoints.Count; ++i)
            {
                if (mBezierPoints[i].x >= x)
                {
                    float x1 = mBezierPoints[i - 1].x;
                    float x2 = mBezierPoints[i].x;

                    float y1 = mBezierPoints[i - 1].y;
                    float y2 = mBezierPoints[i].y;

                    float y = (x - x1) * (y2 - y1) / (x2 - x1) + y1;
                    return (int)y;
                }
            }
            return (int)mBezierPoints[mBezierPoints.Count - 1].y;
        }
    }

    public bool HasCompleted()
    {
        for (int i = 0; i < mTilesX; ++i)
        {
            for (int j = 0; j < mTilesY; ++j)
            {
                if (mGameObjects[i, j].transform.position.x != i * puzzleSize ||
                    mGameObjects[i, j].transform.position.y != j * puzzleSize)
                    return false;
            }
        }
        return true;
    }

}
