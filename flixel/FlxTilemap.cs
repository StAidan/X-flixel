using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace org.flixel
{
    public struct BlockPoint
    {
        public int x;
        public int y;
        public int data;

        public BlockPoint(int X, int Y, int Data)
        {
            x = X;
            y = Y;
            data = Data;
        }
    }
    /**
     * This is a traditional tilemap display and collision class.
     * It takes a string of comma-separated numbers and then associates
     * those values with tiles from the sheet you pass in.
     * It also includes some handy static parsers that can convert
     * arrays or PNG files into strings that can be successfully loaded.
     */
    public class FlxTilemap : FlxObject
    {
        static public Texture2D ImgAuto;
        static public Texture2D ImgAutoAlt;

		/**
		 * No auto-tiling.
		 */
		public const int OFF = 0;
		/**
		 * Platformer-friendly auto-tiling.
		 */
		public const int AUTO = 1;
		/**
		 * Top-down auto-tiling.
		 */
		public const int ALT = 2;

		/**
		 * What tile index will you start colliding with (default: 1).
		 */
		public int collideIndex;
		/**
		 * The first index of your tile sheet (default: 0) If you want to change it, do so before calling loadMap().
		 */
		public int startingIndex;
		/**
		 * What tile index will you start drawing with (default: 1)  NOTE: should always be >= startingIndex.
		 * If you want to change it, do so before calling loadMap().
		 */
		public int drawIndex;
		/**
		 * Set this flag to use one of the 16-tile binary auto-tile algorithms (OFF, AUTO, or ALT).
		 */
		public int auto;
		/**
		 * Set this flag to true to force the tilemap buffer to refresh on the next render frame.
		 */
		public bool refresh;

		/**
		 * Read-only variable, do NOT recommend changing after the map is loaded!
		 */
		public int widthInTiles;
		/**
		 * Read-only variable, do NOT recommend changing after the map is loaded!
		 */
		public int heightInTiles;
		/**
		 * Read-only variable, do NOT recommend changing after the map is loaded!
		 */
		public int totalTiles;
		/**
		 * Rendering helper.
		 */
		protected Rectangle _flashRect;
		protected Rectangle _flashRect2;

		protected int[] _data;
		protected List<Rectangle> _rects;
		protected int _tileWidth;
		protected int _tileHeight;
		protected FlxObject _block;
		//protected var _callbacks:Array;
		protected int _screenRows;
		protected int _screenCols;
		protected bool _boundsVisible;

        protected Texture2D _tileBitmap;

        /**
         * The tilemap constructor just initializes some basic variables.
         */
        public FlxTilemap()
		{
            if (ImgAuto == null || ImgAutoAlt == null)
            {
                ImgAuto = FlxG.Content.Load<Texture2D>("flixel/autotiles");
                ImgAutoAlt = FlxG.Content.Load<Texture2D>("flixel/autotiles_alt");
            }

			auto = OFF;
			collideIndex = 1;
			startingIndex = 0;
			drawIndex = 1;
			widthInTiles = 0;
			heightInTiles = 0;
			totalTiles = 0;
			_flashRect2 = new Rectangle();
			_flashRect = _flashRect2;
			_data = null;
			_tileWidth = 0;
			_tileHeight = 0;
			_rects = null;
			_block = new FlxObject();
			_block.width = _block.height = 0;
			_block.@fixed = true;
			//_callbacks = new Array();
			@fixed = true;
            moves = false;
		}

		/**
		 * Load the tilemap with string data and a tile graphic.
		 * 
		 * @param	MapData			A string of comma and line-return delineated indices indicating what order the tiles should go in.
		 * @param	TileGraphic		All the tiles you want to use, arranged in a strip corresponding to the numbers in MapData.
		 * @param	TileWidth		The width of your tiles (e.g. 8) - defaults to height of the tile graphic if unspecified.
		 * @param	TileHeight		The height of your tiles (e.g. 8) - defaults to width if unspecified.
		 * 
		 * @return	A pointer this instance of FlxTilemap, for chaining as usual :)
		 */
        public FlxTilemap loadMap(string MapData, Texture2D TileGraphic)
        {
            return loadMap(MapData, TileGraphic, 0, 0);
        }
		public FlxTilemap loadMap(string MapData, Texture2D TileGraphic, int TileWidth, int TileHeight)
		{
			refresh = true;

            _tileBitmap = TileGraphic;

			//Figure out the map dimensions based on the data string
			string[] cols;
			string[] rows = MapData.Split('\n');
			heightInTiles = rows.Length;
			int r = 0;
			int c;

            cols = rows[r].Split(',');
            _data = new int[rows.Length * cols.Length];
            while(r < heightInTiles)
			{
                cols = rows[r++].Split(',');
                if (cols.Length <= 1)
				{
					heightInTiles = heightInTiles - 1;
					continue;
				}
				if(widthInTiles == 0)
					widthInTiles = cols.Length;
				c = 0;
				while(c < widthInTiles)
					_data[((r - 1) * widthInTiles) + c] = int.Parse(cols[c++]); //.push(uint(cols[c++]));
			}
			
			//Pre-process the map data if it's auto-tiled
			int i;
			totalTiles = widthInTiles*heightInTiles;
			if(auto > OFF)
			{
				collideIndex = startingIndex = drawIndex = 1;
				i = 0;
				while(i < totalTiles)
					autoTile(i++);
			}

			//Figure out the size of the tiles
			_tileWidth = TileWidth;
			if(_tileWidth == 0)
				_tileWidth = TileGraphic.Height;
			_tileHeight = TileHeight;
			if(_tileHeight == 0)
				_tileHeight = _tileWidth;
			_block.width = _tileWidth;
			_block.height = _tileHeight;
			
			//Then go through and create the actual map
			width = widthInTiles*_tileWidth;
			height = heightInTiles*_tileHeight;
			_rects = new List<Rectangle>();
			i = 0;
            while (i < totalTiles)
            {
                _rects.Add(Rectangle.Empty);
                updateTile(i++);
            }

			//Pre-set some helper variables for later
            _screenRows = (int)FlxU.ceil((float)FlxG.height / (float)_tileHeight) + 1;
			if(_screenRows > heightInTiles)
				_screenRows = heightInTiles;
            _screenCols = (int)FlxU.ceil((float)FlxG.width / (float)_tileWidth) + 1;
			if(_screenCols > widthInTiles)
				_screenCols = widthInTiles;
			
			generateBoundingTiles();
			refreshHulls();
			
			_flashRect.X = 0;
			_flashRect.Y = 0;
            _flashRect.Width = (int)(FlxU.ceil((float)FlxG.width / (float)_tileWidth) + 1) * _tileWidth; ;
            _flashRect.Height = (int)(FlxU.ceil((float)FlxG.height / (float)_tileHeight) + 1) * _tileHeight;
			
			return this;
		}

        /**
		 * Generates a bounding box version of the tiles, flixel should call this automatically when necessary.
		 */
        protected void generateBoundingTiles()
		{
        }

        /**
         * Draws the tilemap.
         */
        public override void render(SpriteBatch spriteBatch)
        {
            //NOTE: While this will only draw the tiles that are actually on screen, it will ALWAYS draw one screen's worth of tiles
            Vector2 _p = getScreenXY();
            int bX = (int)Math.Floor(-_p.X / _tileWidth);
            int bY = (int)Math.Floor(-_p.Y / _tileHeight);
            int eX = (int)Math.Floor((-_p.X + FlxG.width - 1) / _tileWidth);
            int eY = (int)Math.Floor((-_p.Y + FlxG.height - 1) / _tileHeight);

            if (bX < 0)
                bX = 0;
            if (bY < 0)
                bY = 0;
            if (eX >= widthInTiles)
                eX = widthInTiles - 1;
            if (eY >= heightInTiles)
                eY = heightInTiles - 1;

            int ri = bY * widthInTiles + bX;
            int cri;

            for (int iy = bY; iy <= eY; iy++)
            {
                cri = ri;
                for (int ix = bX; ix <= eX; ix++)
                {
                    if (_rects[cri] != Rectangle.Empty)
                    {
                        spriteBatch.Draw(_tileBitmap,
                            new Rectangle((ix * _tileWidth) + (int)Math.Floor(FlxG.scroll.X), (iy * _tileHeight) + (int)Math.Floor(FlxG.scroll.Y), _tileWidth, _tileHeight),
                            _rects[iy * widthInTiles + ix],
                            Color.White);
                    }
                    cri++;
                }
                ri += widthInTiles;
            }
        }

        /**
         * Checks for overlaps between the provided object and any tiles above the collision index.
         * 
         * @param	Core		The <code>FlxObject</code> you want to check against.
         */
        override public bool overlaps(FlxObject Core)
		{
			int d;
			
			int dd;
			List<BlockPoint> blocks = new List<BlockPoint>();
			
			//First make a list of all the blocks we'll use for collision
			int ix = (int)FlxU.floor((Core.x - x)/_tileWidth);
			int iy = (int)FlxU.floor((Core.y - y)/_tileHeight);
            int iw = (int)FlxU.ceil((float)Core.width / (float)_tileWidth) + 1;
            int ih = (int)FlxU.ceil((float)Core.height / (float)_tileHeight) + 1;
			int r = 0;
			int c;
			while(r < ih)
			{
				if(r >= heightInTiles) break;
				d = (iy+r)*widthInTiles+ix;
				c = 0;
				while(c < iw)
				{
					if(c >= widthInTiles) break;
					dd = _data[d+c];
					if(dd >= collideIndex)
						blocks.Add(new BlockPoint((int)(x+(ix+c)*_tileWidth),(int)(y+(iy+r)*_tileHeight),dd));
					c++;
				}
				r++;
			}
			
			//Then check for overlaps
			int bl = blocks.Count;
			int i = 0;
			while(i < bl)
			{
				_block.x = blocks[i].x;
				_block.y = blocks[i++].y;
				if(_block.overlaps(Core))
					return true;
			}
			return false;
		}

		/**
		 * Checks to see if a point in 2D space overlaps a solid tile.
		 * 
		 * @param	X			The X coordinate of the point.
		 * @param	Y			The Y coordinate of the point.
		 * @param	PerPixel	Not available in <code>FlxTilemap</code>, ignored.
		 * 
		 * @return	Whether or not the point overlaps this object.
		 */
        override public bool overlapsPoint(float X, float Y)
        {
            return overlapsPoint(X, Y, false);
        }
		override public bool overlapsPoint(float X, float Y, bool PerPixel)
		{
			return getTile((int)((X-x)/_tileWidth),(int)((Y-y)/_tileHeight)) >= this.collideIndex;
		}

		/**
		 * Called by <code>FlxObject.updateMotion()</code> and some constructors to
		 * rebuild the basic collision data for this object.
		 */
        override public void refreshHulls()
		{
			colHullX.x = 0;
			colHullX.y = 0;
			colHullX.width = _tileWidth;
			colHullX.height = _tileHeight;
			colHullY.x = 0;
			colHullY.y = 0;
			colHullY.width = _tileWidth;
			colHullY.height = _tileHeight;
		}

		/**
		 * <code>FlxU.collide()</code> (and thus <code>FlxObject.collide()</code>) call
		 * this function each time two objects are compared to see if they collide.
		 * It doesn't necessarily mean these objects WILL collide, however.
		 * 
		 * @param	Object	The <code>FlxObject</code> you're about to run into.
		 */
		override public void preCollide(FlxObject Object)
		{
			//Collision fix, in case updateMotion() is called
			colHullX.x = 0;
			colHullX.y = 0;
			colHullY.x = 0;
			colHullY.y = 0;
			
			int r;
			int c;
			int rs;
			int ix = (int)FlxU.floor((Object.x - x)/_tileWidth);
			int iy = (int)FlxU.floor((Object.y - y)/_tileHeight);
            int iw = ix + (int)FlxU.ceil((float)Object.width / (float)_tileWidth) + 1;
            int ih = iy + (int)FlxU.ceil((float)Object.height / (float)_tileHeight) + 1;
			if(ix < 0)
				ix = 0;
			if(iy < 0)
				iy = 0;
			if(iw > widthInTiles)
				iw = widthInTiles;
			if(ih > heightInTiles)
				ih = heightInTiles;
			rs = iy*widthInTiles;
			r = iy;
            colOffsets.Clear();
			while(r < ih)
			{
				c = ix;
				while(c < iw)
				{
                    if (_data[rs + c] >= collideIndex)
                    {
                        colOffsets.Add(new Vector2(x + c * _tileWidth, y + r * _tileHeight));
                    }
					c++;
				}
				rs += widthInTiles;
				r++;
			}
		}

		/**
		 * Check the value of a particular tile.
		 * 
		 * @param	X		The X coordinate of the tile (in tiles, not pixels).
		 * @param	Y		The Y coordinate of the tile (in tiles, not pixels).
		 * 
		 * @return	A uint containing the value of the tile at this spot in the array.
		 */
		public int getTile(int X, int Y)
		{
			return getTileByIndex(Y * widthInTiles + X);
		}

		/**
		 * Get the value of a tile in the tilemap by index.
		 * 
		 * @param	Index	The slot in the data array (Y * widthInTiles + X) where this tile is stored.
		 * 
		 * @return	A uint containing the value of the tile at this spot in the array.
		 */
		public int getTileByIndex(int Index)
		{
			return _data[Index];
		}

		/**
		 * Change the data and graphic of a tile in the tilemap.
		 * 
		 * @param	X				The X coordinate of the tile (in tiles, not pixels).
		 * @param	Y				The Y coordinate of the tile (in tiles, not pixels).
		 * @param	Tile			The new integer data you wish to inject.
		 * @param	UpdateGraphics	Whether the graphical representation of this tile should change.
		 * 
		 * @return	Whether or not the tile was actually changed.
		 */
        public bool setTile(int X, int Y, int Tile)
        {
            if ((X >= widthInTiles) || (Y >= heightInTiles))
                return false;
            return setTileByIndex(Y * widthInTiles + X, Tile, true);
        }
		public bool setTile(int X, int Y, int Tile, bool UpdateGraphics)
		{
			if((X >= widthInTiles) || (Y >= heightInTiles))
				return false;
			return setTileByIndex(Y * widthInTiles + X,Tile,UpdateGraphics);
		}

		/**
		 * Change the data and graphic of a tile in the tilemap.
		 * 
		 * @param	Index			The slot in the data array (Y * widthInTiles + X) where this tile is stored.
		 * @param	Tile			The new integer data you wish to inject.
		 * @param	UpdateGraphics	Whether the graphical representation of this tile should change.
		 * 
		 * @return	Whether or not the tile was actually changed.
		 */
		public bool setTileByIndex(int Index, int Tile, bool UpdateGraphics)
		{
			if(Index >= _data.Length)
				return false;
			
			bool ok = true;
			_data[Index] = Tile;
			
			if(!UpdateGraphics)
				return ok;
			
			refresh = true;
			
			if(auto == OFF)
			{
				updateTile(Index);
				return ok;
			}

			//If this map is autotiled and it changes, locally update the arrangement
			int i;
			int r = (int)(Index/widthInTiles) - 1;
			int rl = r + 3;
			int c = Index%widthInTiles - 1;
			int cl = c + 3;
			while(r < rl)
			{
				c = cl - 3;
				while(c < cl)
				{
					if((r >= 0) && (r < heightInTiles) && (c >= 0) && (c < widthInTiles))
					{
						i = r*widthInTiles+c;
						autoTile(i);
						updateTile(i);
					}
					c++;
				}
				r++;
			}
			
			return ok;
		}

		/**
		 * Bind a function Callback(Core:FlxCore,X:uint,Y:uint,Tile:uint) to a range of tiles.
		 * 
		 * @param	Tile		The tile to trigger the callback.
		 * @param	Callback	The function to trigger.  Parameters should be <code>(Core:FlxCore,X:uint,Y:uint,Tile:uint)</code>.
		 * @param	Range		If you want this callback to work for a bunch of different tiles, input the range here.  Default value is 1.
		 */
		public void setCallback(int Tile, int Callback, int Range)
		{
			FlxG.log("WARNING: FlxTilemap.setCallback()\nhas been temporarily deprecated.");
			//if(Range <= 0) return;
			//for(var i:uint = Tile; i < Tile+Range; i++)
			//	_callbacks[i] = Callback;
		}

		/**
		 * Call this function to lock the automatic camera to the map's edges.
		 * 
		 * @param	Border		Adjusts the camera follow boundary by whatever number of tiles you specify here.  Handy for blocking off deadends that are offscreen, etc.  Use a negative number to add padding instead of hiding the edges.
		 */
        public void follow()
        {
            follow(0);
        }
		public void follow(int Border)
		{
            FlxG.followBounds((int)x + Border * _tileWidth, (int)y + Border * _tileHeight, (int)width - Border * _tileWidth, (int)height - Border * _tileHeight);
		}

		/**
		 * Shoots a ray from the start point to the end point.
		 * If/when it passes through a tile, it stores and returns that point.
		 * 
		 * @param	StartX		The X component of the ray's start.
		 * @param	StartY		The Y component of the ray's start.
		 * @param	EndX		The X component of the ray's end.
		 * @param	EndY		The Y component of the ray's end.
		 * @param	Result		A <code>Point</code> object containing the first wall impact.
		 * @param	Resolution	Defaults to 1, meaning check every tile or so.  Higher means more checks!
		 * @return	Whether or not there was a collision between the ray and a colliding tile.
		 */
		public bool ray(int StartX, int StartY, int EndX, int EndY, Vector2 Result, int Resolution)
		{
			int step = _tileWidth;
			if(_tileHeight < _tileWidth)
				step = _tileHeight;
			step /= Resolution;
			int dx = EndX - StartX;
			int dy = EndY - StartY;
			float distance = (float)Math.Sqrt(dx*dx + dy*dy);
			int steps = (int)FlxU.ceil(distance/step);
			int stepX = dx/steps;
			int stepY = dy/steps;
			int curX = StartX - stepX;
			int curY = StartY - stepY;
			int tx;
			int ty;
			int i = 0;
			while(i < steps)
			{
				curX += stepX;
				curY += stepY;
				
				if((curX < 0) || (curX > width) || (curY < 0) || (curY > height))
				{
					i++;
					continue;
				}
				
				tx = curX/_tileWidth;
				ty = curY/_tileHeight;
				if(_data[ty*widthInTiles+tx] >= collideIndex)
				{
					//Some basic helper stuff
					tx *= _tileWidth;
					ty *= _tileHeight;
					int rx = 0;
					int ry = 0;
					int q;
					int lx = curX-stepX;
					int ly = curY-stepY;
					
					//Figure out if it crosses the X boundary
					q = tx;
					if(dx < 0)
						q += _tileWidth;
					rx = q;
					ry = ly + stepY*((q-lx)/stepX);
					if((ry > ty) && (ry < ty + _tileHeight))
					{
						Result.X = rx;
						Result.Y = ry;
						return true;
					}
					
					//Else, figure out if it crosses the Y boundary
					q = ty;
					if(dy < 0)
						q += _tileHeight;
					rx = lx + stepX*((q-ly)/stepY);
					ry = q;
					if((rx > tx) && (rx < tx + _tileWidth))
					{
						Result.X = rx;
						Result.Y = ry;
						return true;
					}
					return false;
				}
				i++;
			}
			return false;
		}

		/**
		 * Converts a one-dimensional array of tile data to a comma-separated string.
		 * 
		 * @param	Data		An array full of integer tile references.
		 * @param	Width		The number of tiles in each row.
		 * 
		 * @return	A comma-separated string containing the level data in a <code>FlxTilemap</code>-friendly format.
		 */
		static public string arrayToCSV(int[] Data, int Width)
		{
			int r = 0;
			int c;
			string csv = "";
			int Height = Data.Length / Width;
			while(r < Height)
			{
				c = 0;
				while(c < Width)
				{
					if(c == 0)
					{
						if(r == 0)
							csv += Data[0];
						else
							csv += "\n"+Data[r*Width];
					}
					else
						csv += ", "+Data[r*Width+c];
					c++;
				}
				r++;
			}
			return csv;
		}

        /**
         * Converts a <code>BitmapData</code> object to a comma-separated string.
         * Black pixels are flagged as 'solid' by default,
         * non-black pixels are set as non-colliding.
         * Black pixels must be PURE BLACK.
         * 
         * @param	bitmapData	A Texture2D, preferably black and white.
         * @param	Invert		Load white pixels as solid instead.
         * 
         * @return	A comma-separated string containing the level data in a <code>FlxTilemap</code>-friendly format.
         */
        static public string bitmapToCSV(Texture2D bitmapData)
        {
            return bitmapToCSV(bitmapData, false);
        }
		static public string bitmapToCSV(Texture2D bitmapData, bool Invert)
		{
			//Walk image and export pixel values
			int r = 0;
			int c;
			uint p;
			string csv = "";
			int w = bitmapData.Width;
			int h = bitmapData.Height;
            uint[] _bitData = new uint[1];

            bitmapData.GetData<uint>(_bitData);

			while(r < h)
			{
				c = 0;
				while(c < w)
				{
					//Decide if this pixel/tile is solid (1) or not (0)
					p = _bitData[(r * w) + c];
					if((Invert && (p > 0)) || (!Invert && (p == 0)))
						p = 1;
					else
						p = 0;
					
					//Write the result to the string
					if(c == 0)
					{
						if(r == 0)
							csv += p;
						else
							csv += "\n"+p;
					}
					else
						csv += ", "+p;
					c++;
				}
				r++;
			}
			return csv;
		}

		/**
		 * An internal function used by the binary auto-tilers.
		 * 
		 * @param	Index		The index of the tile you want to analyze.
		 */
		protected void autoTile(int Index)
		{
			if(_data[Index] == 0) return;
			_data[Index] = 0;
			if((Index-widthInTiles < 0) || (_data[Index-widthInTiles] > 0)) 		//UP
				_data[Index] += 1;
			if((Index%widthInTiles >= widthInTiles-1) || (_data[Index+1] > 0)) 		//RIGHT
				_data[Index] += 2;
			if((Index+widthInTiles >= totalTiles) || (_data[Index+widthInTiles] > 0)) //DOWN
				_data[Index] += 4;
			if((Index%widthInTiles <= 0) || (_data[Index-1] > 0)) 					//LEFT
				_data[Index] += 8;
			if((auto == ALT) && (_data[Index] == 15))	//The alternate algo checks for interior corners
			{
				if((Index%widthInTiles > 0) && (Index+widthInTiles < totalTiles) && (_data[Index+widthInTiles-1] <= 0))
					_data[Index] = 1;		//BOTTOM LEFT OPEN
				if((Index%widthInTiles > 0) && (Index-widthInTiles >= 0) && (_data[Index-widthInTiles-1] <= 0))
					_data[Index] = 2;		//TOP LEFT OPEN
				if((Index%widthInTiles < widthInTiles-1) && (Index-widthInTiles >= 0) && (_data[Index-widthInTiles+1] <= 0))
					_data[Index] = 4;		//TOP RIGHT OPEN
				if((Index%widthInTiles < widthInTiles-1) && (Index+widthInTiles < totalTiles) && (_data[Index+widthInTiles+1] <= 0))
					_data[Index] = 8; 		//BOTTOM RIGHT OPEN
			}
			_data[Index] += 1;
		}
		
		/**
		 * Internal function used in setTileByIndex() and the constructor to update the map.
		 * 
		 * @param	Index		The index of the tile you want to update.
		 */
		protected void updateTile(int Index)
		{
			if(_data[Index] < drawIndex)
			{
				_rects[Index] = Rectangle.Empty;
				return;
			}
			int rx = (_data[Index]-startingIndex)*_tileWidth;
			int ry = 0;
			if(rx >= _tileBitmap.Width)
			{
                ry = (int)(rx / _tileBitmap.Width) * _tileHeight;
                rx %= _tileBitmap.Width;
			}
			_rects[Index] = (new Rectangle(rx,ry,_tileWidth,_tileHeight));
		}

    }
}
