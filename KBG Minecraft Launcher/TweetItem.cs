using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KBG_Minecraft_Launcher
{
    class TweetItem
    {
        private string _headline;
        private string _text;
        private string _time;

        public string Headline
        {
            get {return _headline;}
        }

        public string Text
        {
            get {return _text;}
        }

        public string Time
        {
            get {return _time;}
        }

        public TweetItem()
        {}

        public TweetItem (string headline, string text, string time)
        {
            _headline = headline;
            _text = text;
            _time = time;
        }
    }
}
