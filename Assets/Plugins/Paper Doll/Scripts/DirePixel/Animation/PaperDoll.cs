using UnityEngine;

namespace DirePixel.Animation
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class PaperDoll : MonoBehaviour
    {
        #region Fields & Properties

        [SerializeField]
        [Tooltip("The replacement spritesheet to use.")]
        protected Texture2D ReplacementTexture;

        [SerializeField]
        [Tooltip("The path in Resources where the replacement texture is located. (I recommend you put all files for this layer in this same folder.)")]
        protected string TexturePath = string.Empty;

        [SerializeField]
        [Tooltip("Set to true if this is a child sprite (i.e., an item) that should sync to the parent's animation frame.")]
        protected bool IsChild = false;

        [SerializeField]
        [Tooltip("Set to true if this child should sync to the parent renderer's flip settings.")]
        protected bool SyncRenderer = true;

        // New option: if false, the base object's sprite is NOT overridden.
        [SerializeField]
        [Tooltip("If false, the PaperDoll will not override the base sprite. Leave this off for your main Animator-controlled object.")]
        protected bool OverrideBaseSprite = false;

        protected Sprite[] Spritesheet;
        protected SpriteRenderer Renderer;
        protected string AnimationFrameName;
        protected int AnimationFrameIndex = 0;
        protected PaperDoll ParentDoll;

        #endregion

        #region Monobehaviour Callbacks

        private void Awake()
        {
            Renderer = GetComponent<SpriteRenderer>();
            // Load sprites from the given texture path and texture name.
            Spritesheet = Resources.LoadAll<Sprite>(TexturePath + ReplacementTexture.name);
        }

        protected virtual void Start()
        {
            if (IsChild)
            {
                ParentDoll = transform.parent.GetComponent<PaperDoll>();
                if (ParentDoll == null)
                {
                    Debug.LogError("Couldn't find a PaperDoll component on the parent.", gameObject);
                    enabled = false;
                }
            }
        }

        protected virtual void LateUpdate()
        {
            // If this is NOT a child and we're not set to override the base sprite,
            // then allow the Animator (blend tree) to control the sprite.
            if (!IsChild && !OverrideBaseSprite)
                return;

            if (ReplacementTexture != null && Renderer != null && Spritesheet.Length > 0 && Renderer.sprite != null)
            {
                // Get the current animation frame name from the Animator-driven sprite.
                AnimationFrameName = Renderer.sprite.name;
                if (!IsChild)
                {
                    // For base objects (when overriding is desired), extract the frame index from the name.
                    int.TryParse(AnimationFrameName.Substring(AnimationFrameName.LastIndexOf('_') + 1), out AnimationFrameIndex);
                }
                else
                {
                    // For child objects, get the frame index from the parent's PaperDoll component.
                    AnimationFrameIndex = ParentDoll.GetParentFrameIndex();
                    if (SyncRenderer)
                    {
                        Renderer.flipX = ParentDoll.Renderer.flipX;
                        Renderer.flipY = ParentDoll.Renderer.flipY;
                    }
                }
                // Set this object's sprite from the spritesheet.
                Renderer.sprite = Spritesheet[AnimationFrameIndex];
            }
            else if (ReplacementTexture == null)
            {
                Debug.LogWarning("Replacement Texture not set. Drag and drop your spritesheet texture.", gameObject);
                enabled = false;
            }
            else if (Renderer == null)
            {
                Debug.LogError("Sprite Renderer not found.", gameObject);
                enabled = false;
            }
            else if (Spritesheet.Length <= 0)
            {
                Debug.LogWarning(gameObject.name + " found no sprites in the spritesheet or the spritesheet was not found.", gameObject);
                enabled = false;
            }
        }

        #endregion

        #region Public Methods

        public virtual int GetParentFrameIndex()
        {
            if (!IsChild)
            {
                return AnimationFrameIndex;
            }
            else
            {
                Debug.LogError("Called GetParentFrameIndex on a child PaperDoll. This should only be called on the base PaperDoll.");
                return 0;
            }
        }

        public virtual void SetTexture(Texture2D texture, string texturePath = "")
        {
            ReplacementTexture = texture;
            if (!string.IsNullOrEmpty(texturePath))
            {
                TexturePath = texturePath;
            }
            Spritesheet = Resources.LoadAll<Sprite>(TexturePath + ReplacementTexture.name);
        }

        #endregion
    }
}
