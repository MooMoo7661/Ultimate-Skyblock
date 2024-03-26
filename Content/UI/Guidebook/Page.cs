namespace UltimateSkyblock.Content.UI.Guidebook
{
    public class Page
    {
        private readonly string _title;
        private readonly string _text;
        private readonly string _wiki;
        private readonly List<UIImage> _images;

        public string Title { get { return _title; } }
        public string Text { get { return _text; } }
        public string Wiki { get { return _wiki; } }
        public List<UIImage> ListedImages { get { return _images; } }

        public Page(string title, string text, string wiki = null, List<UIImage> images = null)
        {
            _title = title;
            _text = text;
            _wiki = wiki;
            _images = images;
        }

        public Page()
        {

        }

        /// <summary>
        /// Lazy method I'm using to make code simpler when creating new UIImages.
        /// </summary>
        public UIImage Image(Asset<Texture2D> texture, float HAlign, float VAlign, float scale = 1f, bool ScaleToFit = false)
        {
            UIImage img = new UIImage(texture);
            img.HAlign = HAlign;
            img.VAlign = VAlign;
            img.ImageScale = scale;
            img.ScaleToFit = ScaleToFit;

            return img;
        }
    }
}
