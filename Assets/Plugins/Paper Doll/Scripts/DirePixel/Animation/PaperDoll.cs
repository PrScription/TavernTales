using UnityEngine;

namespace DirePixel.Animation
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class PaperDoll : MonoBehaviour
    {
        #region Fields & Properties

        /// <summary>
        /// The replacement spritesheet to use.
        /// </summary>
        [SerializeField]
        [Tooltip("The replacement spritesheet to use.")]
        protected Texture2D ReplacementTexture;

        /// <summary>
        /// The path in Resources where the replacement texture is located. (I recommend you put all files for this layer in this same folder.
        /// </summary>
        [SerializeField]
        [Tooltip("The path in Resources where the replacement texture is located. (I recommend you put all files for this layer in this same folder.)")]
        protected string TexturePath = string.Empty;

        /// <summary>
        /// Set to true if this is a child sprite, like an outfit, eye color, etc.
        /// </summary>
        [SerializeField]
        [Tooltip("Set to true if this is a child sprite, like an outfit, eye color, etc.")]
        protected bool IsChild = false;

        [SerializeField]
        [Tooltip("Set to true if this child should sync to the parent renderer's flip settings.")]
        protected bool SyncRenderer = true;

        /// <summary>
        /// Spritesheet array containing all sprites within the ReplacementTexture.
        /// </summary>
        protected Sprite[] Spritesheet;
        /// <summary>
        /// This object's sprite renderer.
        /// </summary>
        protected SpriteRenderer Renderer;
        /// <summary>
        /// Name of the current frame.  Used to find the current frame's number.
        /// </summary>
        protected string AnimationFrameName;
        /// <summary>
        /// The index of the current frame. Used to set index of new frame.
        /// </summary>
        protected int AnimationFrameIndex = 0;
        /// <summary>
        /// The parent object's Paper Doll component.  Used only by child objects.
        /// </summary>
        protected PaperDoll ParentDoll;

        #endregion

        #region Monobehavior Callbacks

        private void Awake()
        {
            Renderer = gameObject.GetComponent<SpriteRenderer>();
            Spritesheet = Resources.LoadAll<Sprite>(TexturePath + ReplacementTexture.name);
        }

        protected virtual void Start()
        {
            if (IsChild)
            {
                ParentDoll = transform.parent.GetComponent<PaperDoll>();
            }
        }

        protected virtual void LateUpdate()
        {
            if (IsChild == true && ParentDoll == null)
            {
                Debug.LogError("Couldn't find Paper Doll component in parent.");
                enabled = false;
                return;
            }

            if (ReplacementTexture != null && Renderer != null && Spritesheet.Length > 0 && Renderer.sprite != null)
            {
                AnimationFrameName = Renderer.sprite.name;

                if (IsChild == false)
                {
                    int.TryParse(AnimationFrameName.Substring(AnimationFrameName.LastIndexOf('_') + 1), out AnimationFrameIndex);
                }
                else
                {
                    AnimationFrameIndex = ParentDoll.GetParentFrameIndex();
                    if(SyncRenderer)
                    {
                        Renderer.flipX = ParentDoll.Renderer.flipX;
                        Renderer.flipY = ParentDoll.Renderer.flipY;
                    }
                }

                Renderer.sprite = Spritesheet[AnimationFrameIndex];
            }
            else if (ReplacementTexture == null)
            {
                Debug.LogWarning("New Sprite has not been set. Drag and drop your spritesheet texture to the New Sprite Sheet field.", gameObject);
                this.enabled = false;
            }
            else if (Renderer == null)
            {
                Debug.LogError("Sprite Renderer not found on this object.", gameObject);
                this.enabled = false;
            }
            else if (Spritesheet.Length <= 0)
            {
                Debug.LogWarning(gameObject.name + " found no sprites in the spritesheet or the spritesheet was not found.", gameObject);
                this.enabled = false;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// This feature is used by child objects to get the frameIndex from the parent base sprite's Paper Doll component
        /// </summary>
        /// <returns></returns>
        public virtual int GetParentFrameIndex()
        {
            if (IsChild == false)
            {
                return AnimationFrameIndex;
            }
            else
            {
                Debug.LogError("wtf");
                return 0;
            }
        }

        /// <summary>
        /// Use this to swap between textures (i.e. hair style change, clothing change, etc.) Only add a texture path if it is different than the current, otherwise leave it alone.
        /// </summary>
        /// <param name="texture"></param>
        public virtual void SetTexture(Texture2D texture, string texturePath = "")
        {
            ReplacementTexture = texture;
            if(!texturePath.Equals(""))
            {
                TexturePath = texturePath;
            }

            Spritesheet = Resources.LoadAll<Sprite>(TexturePath + ReplacementTexture.name);
        }

        #endregion
    }
}