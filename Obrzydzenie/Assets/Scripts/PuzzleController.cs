using System;
using Curves;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Transformers;
using Random = UnityEngine.Random;

public class PuzzleController : MonoBehaviour
{
    public List<Texture2D> puzzleImages;
    public SpriteRenderer background;
    public Transform tilesParent;
    public Material shadowMaterial;
    public int puzzleSize = 100;
    private readonly Color trans = new(0.0f, 0.0f, 0.0f, 0.0f);
    private readonly List<Vector3> mBezierPoints = new();
    private Texture2D mBaseTexture;
    private GameObject[,] mGameObjects;
    private int mTilesX;
    private int mTilesY;
    public int currentPuzzleIndex;
    public float scaleTime = 1.0f; 

    private readonly Vec2[] mCurvyCoords = {
            new(0, 0),
            new(0.35f, 0.15f),
            new(0.37f, 0.05f),
            new(0.37f, 0.05f),
            new(0.40f, 0),
            new(0.38f, -0.05f),
            new(0.38f, -0.05f),
            new(0.20f, -0.20f),
            new(0.50f, -0.20f),
            new(0.50f, -0.20f),
            new(0.80f, -0.20f),
            new(0.62f, -0.05f),
            new(0.62f, -0.05f),
            new(0.60f, 0),
            new(0.63f, 0.05f),
            new(0.63f, 0.05f),
            new(0.65f, 0.15f),
            new(1, 0)
    };

    private void Start()
    {
        SetProperValues();
        CreateNewPuzzle();
    }

    private void ClearTiles()
    {
        foreach(Transform tile in tilesParent.GetComponentInChildren<Transform>())
        {
            Destroy(tile.gameObject);
        }
    }

    private void CreateNewPuzzle()
    {
        if(currentPuzzleIndex >= puzzleImages.Count)
        {
            SceneManager.LoadScene(1);
            return;
        }
        CreateJigsawTiles();
        ShufflePuzzles();
    }

    private IEnumerator PrepareNextMap()
    {
        Debug.Log("Before");
        ClearTiles();
        float time = 0;
        while (time < scaleTime)
        {
            time += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(1.5f);

        currentPuzzleIndex += 1;
        CreateNewPuzzle();
    }

    public void DisplayColorBackground()
    {
        var tex = puzzleImages[currentPuzzleIndex];
        var newTEX = new Texture2D(tex.width + 40, tex.height + 40, TextureFormat.ARGB32, 1, true);
        for (var i = 20; i < tex.width + 20; ++i)
        {
            for (var j = 20; j < tex.height + 20; ++j)
            {
                var col = tex.GetPixel(i - 20, j - 20);
                col.a = 1.0f;
                newTEX.SetPixel(i, j, col);
            }
        }
        newTEX.Apply();

        var sprite = SpriteUtils.CreateSpriteFromTexture2D(newTEX, 0, 0, mBaseTexture.width, mBaseTexture.height);
        background.sprite = sprite;
    }

    public void ChangeMap()
    {
        DisplayColorBackground();
        StartCoroutine(PrepareNextMap());
    }

    private void SetProperValues()
    {
        for(var i=0; i< mCurvyCoords.Length; i++)
        {
            var vec = mCurvyCoords[i];
            mCurvyCoords[i] = new Vec2(vec.x * puzzleSize, vec.y * puzzleSize);
        }
    }

    void Shuffle(GameObject obj)
    {
        // Define the frame multiplier
        const float frameMultiplier = 1.5f;

        // Get random point within the frame around the rectangle
        float x, y;
        if (Random.value < 0.5f)
        {
            // Top or bottom frame
            x = Random.Range(-puzzleSize * frameMultiplier, puzzleSize * (mTilesX + frameMultiplier - 1));
            y = Random.value < 0.5f ? -puzzleSize * frameMultiplier : puzzleSize * (mTilesY + frameMultiplier - 1);
        }
        else
        {
            // Left or right frame
            x = Random.value < 0.5f ? -puzzleSize * frameMultiplier : puzzleSize * (mTilesX + frameMultiplier - 1);
            y = Random.Range(-puzzleSize * frameMultiplier, puzzleSize * (mTilesY + frameMultiplier - 1));
        }

        // Final position of the tile
        var pos = new Vector3(x, y, 0.0f);

        StartCoroutine(Coroutine_MoveOverSeconds(obj, pos, 3.0f));
    }

    private static IEnumerator Coroutine_MoveOverSeconds(GameObject objectToMove, Vector3 end, float seconds)
    {
        float elapsedTime = 0;
        var startingPos = objectToMove.transform.position;
        while (elapsedTime < seconds)
        {
            objectToMove.transform.localPosition = Vector3.Lerp(startingPos, end, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
        objectToMove.transform.localPosition = end;
    }

    public void ShufflePuzzles()
    {
        for (var i = 0; i < mTilesX; ++i)
        {
            for (var j = 0; j < mTilesY; ++j)
            {
                Shuffle(mGameObjects[i, j]);
            }
        }

    }

    public void CreateJigsawTiles()
    {
        // Load the main image.
        var tex = puzzleImages[currentPuzzleIndex];
        if (!tex.isReadable)
        {
            Debug.Log("Texture is not readable");
            return;
        }

        mTilesX = tex.width / puzzleSize;
        mTilesY = tex.height / puzzleSize;

        // add 20 pixel border around.
        var newTEX = new Texture2D(tex.width + 40, tex.height + 40, TextureFormat.ARGB32, 1, true);
        for (var i = 20; i < tex.width + 20; ++i)
        {
            for (var j = 20; j < tex.height + 20; ++j)
            {
                var col = tex.GetPixel(i - 20, j - 20);
                col.a = 1.0f;
                newTEX.SetPixel(i, j, col);
            }
        }
        newTEX.Apply();
        mBaseTexture = newTEX;

        // create the bezier curve.
        CreateBezierCurve();

        mGameObjects = new GameObject[mTilesX, mTilesY];
        for (var i = 0; i < mTilesX; ++i)
        {
            for (var j = 0; j < mTilesY; ++j)
            {
                CreateSpriteGameObject(i, j);
            }
        }

        // now make the background image light transparent.
        for (var i = 20; i < tex.width + 20; ++i)
        {
            for (var j = 20; j < tex.height + 20; ++j)
            {
                var col = tex.GetPixel(i - 20, j - 20);
                col.a = 0.4f;
                newTEX.SetPixel(i, j, col);
            }
        }
        newTEX.Apply();
        mBaseTexture = newTEX;

        var sprite = SpriteUtils.CreateSpriteFromTexture2D(mBaseTexture, 0, 0, mBaseTexture.width, mBaseTexture.height);
        background.sprite = sprite;
        background.transform.localPosition = new Vector3(0, 0, 0.02f);

        //RelocateCamera();
    }

    void CreateBezierCurve()
    {
        // use bezier curve.
        var bez = new Bezier(mCurvyCoords.OfType<Vec2>().ToList());

        for (var i = 0; i < puzzleSize; i++)
        {
            var bp = bez.ValueAt(i / (float)puzzleSize);
            var p = new Vector3(bp.x, bp.y, 0.0f);

            mBezierPoints.Add(p);
        }
    }
    
    void CreateSpriteGameObject(int i, int j)
    {
        var obj = new GameObject
        {
            name = "Tile_" + i + "_" + j,
        };
        var tile = obj.AddComponent<SplitTile>();
        
        mGameObjects[i, j] = obj;

        if (tilesParent != null)
        {
            obj.transform.SetParent(tilesParent);
        }
        tile.correctPosition = new Vector3(i * puzzleSize, j * puzzleSize, 0.0f);

        var spren = obj.AddComponent<SpriteRenderer>();
        obj.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        obj.transform.position = new Vector3(i * puzzleSize, j * puzzleSize, 0);

        // create a new tile texture.
        var mTileTexture = CreateTileTexture(i, j);

        tile.mDirections[0] = GetRandomDirection(0);
        tile.mDirections[1] = GetRandomDirection(1);
        tile.mDirections[2] = GetRandomDirection(2);
        tile.mDirections[3] = GetRandomDirection(3);

        // check for bottom and left tile.
        if (j > 0)
        {
            var downTile = mGameObjects[i, j - 1].GetComponent<SplitTile>();
            if (downTile.mDirections[0] == Direction.Up)
            {
                tile.mDirections[2] = Direction.DownReverse;
            }
            else
            {
                tile.mDirections[2] = Direction.Down;
            }
        }

        // check for bottom and left tile.
        if (i > 0)
        {
            var downTile = mGameObjects[i - 1, j].GetComponent<SplitTile>();
            if (downTile.mDirections[1] == Direction.Right)
            {
                tile.mDirections[3] = Direction.LeftReverse;
            }
            else
            {
                tile.mDirections[3] = Direction.Left;
            }
        }

        if (i == 0)
        {
            tile.mDirections[3] = Direction.None;
        }
        if (i == mTilesX - 1)
        {
            tile.mDirections[1] = Direction.None;
        }
        if (j == 0)
        {
            tile.mDirections[2] = Direction.None;
        }
        if (j == mTilesY - 1)
        {
            tile.mDirections[0] = Direction.None;
        }
        for (var d = 0; d < tile.mDirections.Length; ++d)
        {
            if (tile.mDirections[d] != Direction.None)
                ApplyBezierMask(mTileTexture, tile.mDirections[d]);
        }

        mTileTexture.Apply();

        // Set the tile texture to the sprite.
        var sprite = SpriteUtils.CreateSpriteFromTexture2D(mTileTexture, 0, 0, puzzleSize + 40, puzzleSize + 40);
        spren.sprite = sprite;
        
        obj.AddComponent<BoxCollider>();
        obj.AddComponent<XRGrabInteractable>();
        
        var rigidBody = obj.GetComponent<Rigidbody>();
        var interactable = obj.GetComponent<XRGrabInteractable>();
        rigidBody.useGravity = false;
        rigidBody.isKinematic = true;
        interactable.movementType = XRBaseInteractable.MovementType.Kinematic;
        interactable.throwOnDetach = false;
        interactable.useDynamicAttach = true;
        interactable.selectExited.AddListener(tile.OnLetGo);
        interactable.selectEntered.AddListener(tile.OnPickUp);
        
        // delay this by 0.5 seconds.
        var grabTransformer = obj.GetComponent<XRGeneralGrabTransformer>();
        if (!grabTransformer)
        {
            grabTransformer = obj.AddComponent<XRGeneralGrabTransformer>();
        }
        grabTransformer.permittedDisplacementAxes = XRGeneralGrabTransformer.ManipulationAxes.X | XRGeneralGrabTransformer.ManipulationAxes.Y;
        grabTransformer.allowOneHandedScaling = false;
        grabTransformer.constrainedAxisDisplacementMode = XRGeneralGrabTransformer.ConstrainedAxisDisplacementMode
            .ObjectRelativeWithLockedWorldUp;
        interactable.trackRotation = false;
    }

    Texture2D CreateTileTexture(int indx, int indy)
    {
        var w = puzzleSize + 40; 
        var h = puzzleSize + 40;

        var newTEX = new Texture2D(w, h, TextureFormat.ARGB32, 1, true);

        var startX = indx * puzzleSize;
        var startY = indy * puzzleSize;
        for (var i = 0; i < puzzleSize + 40; ++i)
        {
            for (var j = 0; j < puzzleSize + 40; ++j)
            {
                var color = mBaseTexture.GetPixel(i + startX, j + startY);
                newTEX.SetPixel(i, j, color);
                if (i < 20 && j < 20)
                {
                    newTEX.SetPixel(i, j, trans);
                }
                if (i >= puzzleSize + 20 && j < 20)
                {
                    newTEX.SetPixel(i, j, trans);
                }
                if (i >= puzzleSize + 20 && j >= puzzleSize + 20)
                {
                    newTEX.SetPixel(i, j, trans);
                }
                if (i < 20 && j >= puzzleSize + 20)
                {
                    newTEX.SetPixel(i, j, trans);
                }
            }
        }
        return newTEX;
    }

    public enum Direction
    {
        Up,
        UpReverse,
        Right,
        RightReverse,
        Down,
        DownReverse,
        Left,
        LeftReverse,
        None,
    }

    Direction GetRandomDirection(int side)
    {
        var rand = Random.Range(0.0f, 1.0f);
        return side switch
        {
            0 => rand < 0.5f ? Direction.Up : Direction.UpReverse,
            1 => rand < 0.5f ? Direction.Right : Direction.RightReverse,
            2 => rand < 0.5f ? Direction.Down : Direction.DownReverse,
            3 => rand < 0.5f ? Direction.Left : Direction.LeftReverse,
            _ => Direction.Up
        };
    }

    void ApplyBezierMask(Texture2D mTileTexture, Direction dir)
    {
        switch (dir)
        {
            case Direction.Up:
                {
                    for (var i = 0; i < puzzleSize; ++i)
                    {
                        var y = -GetInterpolatedY(mBezierPoints, i);

                        for (var j = puzzleSize + 20 + y; j < puzzleSize + 40; ++j)
                        {
                            mTileTexture.SetPixel(i + 20, j, trans);
                        }
                        mTileTexture.SetPixel(i + 20, puzzleSize + 20 + y, Color.gray);
                    }
                    break;
                }
            case Direction.UpReverse:
                {
                    for (var i = 0; i < puzzleSize; ++i)
                    {
                        var y = GetInterpolatedY(mBezierPoints, i);

                        for (var j = puzzleSize + 20 + y; j < puzzleSize + 40; ++j)
                        {
                            mTileTexture.SetPixel(i + 20, j, trans);
                        }
                        mTileTexture.SetPixel(i + 20, puzzleSize + 20 + y, Color.gray);
                    }
                    break;
                }
            case Direction.Right:
                {
                    for (var j = 0; j < puzzleSize; ++j)
                    {
                        var x = -GetInterpolatedY(mBezierPoints, j);

                        //mTileTexture.SetPixel(120 + x, j + 20, Color.gray);
                        for (var i = puzzleSize + 19 + x; i < puzzleSize + 40; ++i)
                        {
                            mTileTexture.SetPixel(i, j + 20, trans);
                        }
                    }
                    break;
                }
            case Direction.RightReverse:
                {
                    for (var j = 0; j < puzzleSize; ++j)
                    {
                        var x = GetInterpolatedY(mBezierPoints, j);

                        //mTileTexture.SetPixel(120 + x, j + 20, Color.gray);
                        for (var i = puzzleSize + 21 + x; i < puzzleSize + 40; ++i)
                        {
                            mTileTexture.SetPixel(i, j + 20, trans);
                        }
                    }
                    break;
                }
            case Direction.Down:
                {
                    for (var i = 0; i < puzzleSize; ++i)
                    {
                        var y = GetInterpolatedY(mBezierPoints, i);

                        //mTileTexture.SetPixel(i + 20, y + 20, trans);
                        for (var j = 0; j < y + 19; ++j)
                        {
                            mTileTexture.SetPixel(i + 20, j, trans);
                        }
                    }
                    break;
                }
            case Direction.DownReverse:
                {
                    for (var i = 0; i < puzzleSize; ++i)
                    {
                        var y = -GetInterpolatedY(mBezierPoints, i);

                        //mTileTexture.SetPixel(i + 20, y + 20, trans);
                        for (var j = 0; j < y + 19; ++j)
                        {
                            mTileTexture.SetPixel(i + 20, j, trans);
                        }
                    }
                    break;
                }
            case Direction.Left:
                {
                    for (var j = 0; j < puzzleSize; ++j)
                    {
                        var x = GetInterpolatedY(mBezierPoints, j);

                        //mTileTexture.SetPixel(x + 20, j, trans);
                        for (var i = 0; i < x + 19; ++i)
                        {
                            mTileTexture.SetPixel(i, j + 20, trans);
                        }
                    }
                    break;
                }
            case Direction.LeftReverse:
                {
                    for (var j = 0; j < puzzleSize; ++j)
                    {
                        var x = -GetInterpolatedY(mBezierPoints, j);

                        //mTileTexture.SetPixel(x + 20, j + 20, trans);
                        for (var i = 0; i < x + 21; ++i)
                        {
                            mTileTexture.SetPixel(i, j + 20, trans);
                        }
                    }
                    break;
                }
            case Direction.None:
            default:
                throw new ArgumentOutOfRangeException(nameof(dir), dir, null);
        }

        return;

        int GetInterpolatedY(List<Vector3> mBezierPointsLocal, int x)
        {
            for (var i = 1; i < mBezierPointsLocal.Count; ++i)
            {
                if (!(mBezierPointsLocal[i].x >= x)) continue;
                var x1 = mBezierPointsLocal[i - 1].x;
                var x2 = mBezierPointsLocal[i].x;

                var y1 = mBezierPointsLocal[i - 1].y;
                var y2 = mBezierPointsLocal[i].y;

                var y = (x - x1) * (y2 - y1) / (x2 - x1) + y1;
                return (int)y;
            }
            return (int)mBezierPointsLocal[^1].y;
        }
    }

    public bool HasCompleted()
    {
        for (var i = 0; i < mTilesX; ++i)
        {
            for (var j = 0; j < mTilesY; ++j)
            {
                var tile = mGameObjects[i, j].GetComponent<SplitTile>();
                if (!tile.isCorrect)
                {
                    return false;
                }
            }
        }
        return true;
    }

}
