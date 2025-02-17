using UnityEngine;

namespace DirePixel
{
    [CreateAssetMenu(fileName = "New Sprite Settings", menuName = "Sprite Importer/Settings")]
    public class SpriteSettings : ScriptableObject
    {
        #region Fields & Properties

        public string TargetPath = SpriteImporter.DEFAULT_TEXTURE_PATH;
        public int PixelsPerUnit = SpriteImporter.DEFAULT_PIXELS_PER_UNIT;
        [Space]
        public bool IsSpritesheet = true;
        public Vector2Int SpriteSize = SpriteImporter.DEFAULT_SPRITE_SIZE;
        public Vector2 SpritePivotPoint = SpriteImporter.DEFAULT_SPRITE_PIVOT;

        #endregion
    }
}