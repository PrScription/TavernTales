using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace DirePixel
{
    public class SpriteImporter : AssetPostprocessor
    {
        #region Fields & Properties

        public const string HOME_PATH = "Importer/";
        public const bool IS_VERBOSE = false;

        public const string DEFAULT_TEXTURE_PATH = "Importer/Sprites/";
        public const int DEFAULT_PIXELS_PER_UNIT = 16;
        public const bool DEFAULT_KEEP_EMPTY_RECTS = false;
        public static readonly Vector2Int DEFAULT_TEXTURE_SIZE = new Vector2Int(128, 128);
        public static readonly Vector2Int DEFAULT_SPRITE_SIZE = new Vector2Int(16, 16);
        public static readonly Vector2 DEFAULT_SPRITE_PIVOT = new Vector2(0.5f, 0.5f);

        #endregion

        #region Preprocessing

        private void OnPreprocessTexture()
        {
            if (!assetPath.Contains(HOME_PATH))
            {
                return;
            }

            // Assign the texture importer
            TextureImporter importer = (TextureImporter)assetImporter;

            // Get all of the settings files located in the settings path.
            SpriteSettings[] settingsFiles = Resources.LoadAll<SpriteSettings>(HOME_PATH);

            if(settingsFiles == null || settingsFiles.Length == 0)
            {
                return;
            }

            // Get the texture dimensions of the texture being imported
            int width, height;
            importer.GetSourceTextureWidthAndHeight(out width, out height);
            var size = new Vector2Int(width, height);

            SpriteSettings currentSettings = null;
            // Try to find a settings file that matches our current file size and location
            foreach (SpriteSettings settings in settingsFiles)
            {
                if(importer.assetPath.Contains(settings.TargetPath))
                {
                    currentSettings = settings;
                    break;
                }
            }

            // If no settings were found, we will print a message to the console and not continue.
            if(currentSettings == null)
            {
                if (IS_VERBOSE)
                {
#pragma warning disable CS0162 // Unreachable code detected
                    Debug.LogWarning("[Sprite Importer] Valid settings file not found for '" + Path.GetFileNameWithoutExtension(assetPath) + "', ignoring...");
#pragma warning restore CS0162 // Unreachable code detected
                }

                return;
            }
            
            // Make sure the imported texture type is set to Sprite
            if (importer.textureType != TextureImporterType.Sprite)
            {
                importer.textureType = TextureImporterType.Sprite;
            }

            // Apply pixel perfect settings while still allowing the user to configure their own alternate settings such as read/write.
            importer.spritePixelsPerUnit = currentSettings.PixelsPerUnit;
            importer.mipmapEnabled = false;
            importer.wrapMode = TextureWrapMode.Clamp;
            importer.filterMode = FilterMode.Point;
            importer.textureCompression = TextureImporterCompression.Uncompressed;

            // Get the max size dimension of the texture.
            int maxSize = size.x;
            if(size.y > maxSize)
            {
                maxSize = size.y;
            }
            // We'll set max size to be only as large as it needs to be.
            if (maxSize <= 32)
            {
                importer.maxTextureSize = 32;
            }
            else if(maxSize <= 64)
            {
                importer.maxTextureSize = 64;
            }
            else if(maxSize <= 128)
            {
                importer.maxTextureSize = 128;
            }
            else if(maxSize <= 256)
            {
                importer.maxTextureSize = 256;
            }
            else if(maxSize <= 512)
            {
                importer.maxTextureSize = 512;
            }
            else if(maxSize <= 1024)
            {
                importer.maxTextureSize = 1024;
            }
            else if(maxSize <= 2048)
            {
                importer.maxTextureSize = 2048;
            }
            else if(maxSize <= 4096)
            {
                importer.maxTextureSize = 4096;
            }
            else if(maxSize <= 8192)
            {
                importer.maxTextureSize = 8192;
            }
            else
            {
                importer.maxTextureSize = 16384;
            }

            // We've finished applying pixel perfect settings. Lastly, we'll check if this is a spritesheet.
            if (currentSettings.IsSpritesheet)
            {
                importer.spriteImportMode = SpriteImportMode.Multiple;
                string fileName = Path.GetFileNameWithoutExtension(assetPath);
                // Slice sprites
                importer.spritesheet = CreateSpriteMetaData(size, currentSettings.SpriteSize, currentSettings.SpritePivotPoint, fileName);
            }
            else
            {
                importer.spriteImportMode = SpriteImportMode.Single;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Returns an array of SpriteMetaData sliced to specifications.
        /// </summary>
        /// <param name="textureSize">The size of the texture being sliced.</param>
        /// <param name="spriteSize">The desired size of sprites.</param>
        /// <param name="pivot">The desired pivot point for sprites.</param>
        /// <returns></returns>
        private SpriteMetaData[] CreateSpriteMetaData(Vector2Int textureSize, Vector2Int spriteSize, Vector2 pivot, string fileName)
        {
            List<SpriteMetaData> metaDatas = new List<SpriteMetaData>();
            int spriteCount = 0;
            int rowCount = textureSize.y / spriteSize.y;
            int colCount = textureSize.x / spriteSize.x;

            // Iterate through the sprite sheet and create SpriteMetaData for each sprite, which sets sprite X, Y position within the sheet, the size bounds
            // and the pivot point.
            for (int row = rowCount - 1; row >= 0; --row)
            {
                for (int col = 0; col < colCount; ++col)
                {
                    SpriteMetaData metaData = new SpriteMetaData()
                    {
                        rect = new Rect(col * spriteSize.x, row * spriteSize.y, spriteSize.x, spriteSize.y),
                        name = $"{fileName}_{spriteCount}",
                        alignment = (int)SpriteAlignment.Custom,
                        pivot = pivot
                    };

                    metaDatas.Add(metaData);

                    ++spriteCount;
                }
            }

            return metaDatas.ToArray();
        }

        #endregion
    }
}