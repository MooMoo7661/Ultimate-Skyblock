using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateSkyblock.Content.UI.Guidebook
{
    public class Page
    {
        private readonly string _title;
        private readonly string _text;
        private readonly string _wiki;

        public Page(string title, string text, string wiki = null)
        {
            _title = title;
            _text = text;
            _wiki = wiki;
        }

        public string Title { get { return _title; } }
        public string Text { get { return _text; } }
        public string Wiki { get { return _wiki; } }
    }
}
