mergeInto(LibraryManager.library, {
  CreateNFT: function (docId) {
    window.dispatchReactUnityEvent(
      "CreateNFT",
      Pointer_stringify(docId)
    );
  },
  CreateWorker: function (index) {
    window.dispatchReactUnityEvent(
      "CreateWorker",
      index
    );
  },
});