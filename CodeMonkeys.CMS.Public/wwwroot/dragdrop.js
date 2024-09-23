window.dragDropInterop = {
    setData: function (event, data) {
        event.dataTransfer.setData("text", data);
        console.log("Set data:", data); // Lägg till logg för att verifiera
    },
    getData: function (event) {
        const data = event.dataTransfer.getData("text");
        console.log("Get data:", data); // Lägg till logg för att verifiera
        return data;
    }
};
