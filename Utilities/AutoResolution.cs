using UnityEngine;

namespace UI
{
    public class AutoResolution : MonoBehaviour
    {
        [SerializeField] private RectTransform topLetterBox;
        [SerializeField] private RectTransform bottomLetterBox;

        public static float letterBoxHeight { get; private set; }

        private void Start()
        {
            SetResolution();
            SetLetterBox();
        }

        private void SetResolution()
        {
            var cam = Camera.main;
            if (cam == null) return;
            var rect = cam.rect;

            var scaleHeight = (float)Screen.width / Screen.height * (9f / 16f);

            if (scaleHeight < 1)
            {
                rect.height = scaleHeight;
                rect.y = (1f - scaleHeight) / 2f;
            }

            cam.rect = rect;
        }

        private void SetLetterBox()
        {
            var rectTransform = GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(1, 1);
            rectTransform.pivot = Vector2.one * 0.5f;
            var thisRect = rectTransform.rect;
            thisRect.width = 0;
            thisRect.height = 0;
            rectTransform.sizeDelta = thisRect.size;
            
            topLetterBox.anchorMin = Vector2.zero;
            topLetterBox.anchorMax = Vector2.one;
            topLetterBox.pivot = Vector2.one * 0.5f;

            bottomLetterBox.anchorMin = Vector2.zero;
            bottomLetterBox.anchorMax = Vector2.one;
            bottomLetterBox.pivot = Vector2.one * 0.5f;

            var topRect = topLetterBox.rect;
            topRect.width = 0;
            topRect.height = 0;
            topLetterBox.sizeDelta = topRect.size;
            
            var bottomRect = bottomLetterBox.rect;
            bottomRect.width = 0;
            bottomRect.height = 0;
            bottomLetterBox.sizeDelta = bottomRect.size;

            var uiHeight = bottomLetterBox.rect.height - 1080;
            letterBoxHeight = uiHeight / 2;

            topLetterBox.anchorMin = new Vector2(0, 1);
            topLetterBox.anchorMax = Vector2.one;
            topLetterBox.pivot = new Vector2(0.5f, 1);
            topLetterBox.sizeDelta = new Vector2(Screen.width, letterBoxHeight);

            bottomLetterBox.anchorMin = Vector2.zero;
            bottomLetterBox.anchorMax = new Vector2(1, 0);
            bottomLetterBox.pivot = new Vector2(0.5f, 0);
            bottomLetterBox.sizeDelta = new Vector2(Screen.width, letterBoxHeight);

            if (topLetterBox.sizeDelta.y >= 1) return;

            Destroy(topLetterBox.gameObject);
            Destroy(bottomLetterBox.gameObject);
        }
    }
}