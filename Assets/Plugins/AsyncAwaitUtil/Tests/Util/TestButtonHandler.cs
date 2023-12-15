using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace UnityAsyncAwaitUtil
{
    public class TestButtonHandler
    {
        private readonly Settings _settings;

        private int _buttonVCount;
        private int _buttonHCount;

        public TestButtonHandler(Settings settings)
        {
            _settings = settings;
        }

        public void Restart()
        {
            _buttonVCount = 0;
            _buttonHCount = 0;
        }

        public bool Display(string text)
        {
            if (_buttonVCount > _settings.numPerColumn)
            {
                _buttonHCount++;
                _buttonVCount = 0;
            }

            var result = GUI.Button(
                new Rect(
                    _settings.horizontalMargin + _buttonHCount * (_settings.buttonWidth + _settings.horizontalSpacing),
                    _settings.verticalMargin + _buttonVCount * (_settings.buttonHeight + _settings.verticalSpacing),
                    _settings.buttonWidth, _settings.buttonHeight), text);

            _buttonVCount++;

            return result;
        }

        [Serializable]
        public class Settings
        {
            [FormerlySerializedAs("NumPerColumn")] public int numPerColumn = 6;
            [FormerlySerializedAs("VerticalMargin")] public float verticalMargin = 50;
            [FormerlySerializedAs("VerticalSpacing")] public float verticalSpacing = 50;
            [FormerlySerializedAs("HorizontalSpacing")] public float horizontalSpacing = 50;
            [FormerlySerializedAs("HorizontalMargin")] public float horizontalMargin = 50;
            [FormerlySerializedAs("ButtonWidth")] public float buttonWidth = 50;
            [FormerlySerializedAs("ButtonHeight")] public float buttonHeight = 50;
        }
    }
}
