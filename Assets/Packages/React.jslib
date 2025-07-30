mergeInto(LibraryManager.library, {
  UpdateScreenName: function (screenName) {
    window.dispatchReactUnityEvent("UpdateScreenName", UTF8ToString(screenName));
  },
  UpdateAiTurnCount: function (turnCount) {
    window.dispatchReactUnityEvent("UpdateAiTurnCount", turnCount);
  },
  UpdateAiName: function (aiName) {
    window.dispatchReactUnityEvent("UpdateAiName", UTF8ToString(aiName));
  },
  });