console.log("dragDropInterop.js loaded");

window.dragDropInterop = {
    setData: function (data) {
        event.dataTransfer.setData("text", data);
    },
    getData: function () {
        return event.dataTransfer.getData("text");
    },
    preventDefault: function () {
        event.preventDefault();
    }
};
