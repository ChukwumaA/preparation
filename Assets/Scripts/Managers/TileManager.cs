using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Managers
{
    public class TileManager : MonoBehaviour
    {
        public class Tile
        {
            public int X { get; set; }
            public int Y { get; set; }
            public bool Occupied { get; set; }
            public int AdjacentCount { get; set; }
            public bool IsIntersection { get; set; }

            public Tile left, right, up, down;

            public Tile(int xIn, int yIn)
            {
                X = xIn; Y = yIn;
                Occupied = false;
                left = right = up = down = null;
            }


        };

        public readonly List<Tile> tiles = new List<Tile>();

        //Call this Script when the game launches
        private void Start()
        {
            ReadTiles();
        }

//=====================================================================================================
        //hardcoded tile data: 1 = free tile, 0 = wall

        private void ReadTiles()
        {
            //hardwired data instead of reading from file (not feasible on web player)

            const string data = @"0000000000000000000000000000
0111111111111001111111111110
0100001000001001000001000010
0100001000001111000001000010
0100001000001001000001000010
0111111111111001111111111110
0100001001000000001001000010
0100001001000000001001000010
0111111001111001111001111110
0001001000001001000001001000
0001001000001001000001001000
0111001111111111111111001110
0100001001000000001001000010
0100001001000000001001000010
0111111001000000001001111110
0100001001000000001001000010
0100001001000000001001000010
0111001001111111111001001110
0001001001000000001001001000
0001001001000000001001001000
0111111111111111111111111110
0100001000001001000001000010
0100001000001001000001000010
0111001111111001111111001110
0001001001000000001001001000
0001001001000000001001001000
0111111001111001111001111110
0100001000001001000001000010
0100001000001001000001000010
0111111111111111111111111110
0000000000000000000000000000";

            int y = 31;
            using(var reader = new StringReader(data))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var x = 1;
                    for (var i = 0; i < line.Length; ++i)
                    {
                        var newTile = new Tile(x,y);

                        //if the tile we read is a valid tile (movable)
                        if (line[i] == '1')
                        {
                            //check for left-right neighbour
                            if (i != 0 && line[i - 1] == '1')
                            {
                                //assign each tile to the corresponding side of other tile
                                newTile.left = tiles[tiles.Count - 1];
                                tiles[tiles.Count - 1].right = newTile;

                                //adjust adjacent tile counts of each tile
                                newTile.AdjacentCount++;
                                tiles[tiles.Count - 1].AdjacentCount++;
                            }
                        }

                        //if the current tile is not movable
                        else newTile.Occupied = true;

                        //check for up-down neighbour, starting from second row (Y < 30)
                        var upNeighbour = tiles.Count - line.Length; //up neighbour index
                        if (y < 30 && !newTile.Occupied && !tiles[upNeighbour].Occupied)
                        {
                            tiles[upNeighbour].down = newTile;
                            newTile.up = tiles[upNeighbour];

                            //adjust adjacent tile counts of each tile
                            newTile.AdjacentCount++;
                            tiles[upNeighbour].AdjacentCount++;
                        }
                        tiles.Add(newTile);
                        x++;
                    }
                    y--;
                }
            }

            //after reading all tiles, determine the intersection tiles
            foreach (var tile in tiles.Where(tile => tile.AdjacentCount > 2))
            {
                tile.IsIntersection = true;
            }
        }

//======================================================================================================================
        //Draw lines between neighbour tiles (debug)
        private void DrawNeighbours()
        {
            foreach(var tile in tiles)
            {
                var pos = new Vector3(tile.X, tile.Y, 0);
                var up = new Vector3(tile.X+0.1f, tile.Y+1, 0);
                var down = new Vector3(tile.X-0.1f, tile.Y-1, 0);
                var left = new Vector3(tile.X-1, tile.Y+0.1f, 0);
                var right = new Vector3(tile.X+1, tile.Y-0.1f, 0);

                if(tile.up != null) Debug.DrawLine(pos, up);
                if(tile.down != null) Debug.DrawLine(pos, down);
                if(tile.left != null) Debug.DrawLine(pos, left);
                if(tile.right != null) Debug.DrawLine(pos, right);
            }
        }

//======================================================================================================================
        //Returns the index in the tiles list of a given tile's coordinates
        public static int Index(int x, int y)
        {
            //if the requested index is in bounds
            //The code below was commented out
            Debug.Log("Index called for X: " + x + ", Y: "+ y);
            if(x >= 1 && x <= 28 && y <= 31 && y >= 1)
                return (31 - y) * 28 + x - 1;

            //else, if the requested index is out of bounds
            //return closet in-bounds tile's index
            if(x < 1)   x =1;
            if(x > 28)  x = 28;
            if(y < 1)   y = 1;
            if(y > 31)  y = 31;

            return (31 - y) * 28 + x -1;

        }

        public int Index(Tile tile)
        {
            return (31 - tile.Y)* 28 + tile.X-1;
        }

//======================================================================================================================
        //Returns the distance between 2 tiles
        public static float Distance (Tile tile1, Tile tile2)
        {
            return Mathf.Sqrt( Mathf.Pow(tile1.X - tile2.X, 2) + Mathf.Pow(tile1.Y - tile2.Y, 2));
        }
    }
}
