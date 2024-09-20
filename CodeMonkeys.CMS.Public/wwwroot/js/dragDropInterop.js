console.log("dragDropInterop.js loaded");

window.dragDropInterop = {
    setData: function (event, key, data) {
        event.dataTransfer.setData(key, data);
    },
    getData: function (event, key) {
        return event.dataTransfer.getData(key);
    },
    preventDefault: function (event) {
        event.preventDefault();
    }
};

