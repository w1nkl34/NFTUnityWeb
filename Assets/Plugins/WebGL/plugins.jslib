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
  UpgradeBuilding: function (buildingName) {
    window.dispatchReactUnityEvent(
      "UpgradeBuilding",
      Pointer_stringify(buildingName)
    );
  },
  SellNFT: function (docId,price) {
    window.dispatchReactUnityEvent(
      "SellNFT",
      Pointer_stringify(docId),
      price
    );
  },
  DestroyWorker: function (docId) {
    window.dispatchReactUnityEvent(
      "DestroyWorker",
      Pointer_stringify(docId)
    );
  },
});