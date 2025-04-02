const connection = new signalR.HubConnectionBuilder()
    .withUrl("/newshub")
    .build();

connection.start().then(() => {
    console.log("SignalR connected to /newshub");
}).catch(err => console.error(err.toString()));

// Ví dụ lắng nghe sự kiện từ server
connection.on("ReceiveUpdate", function () {
    // Có thể gọi reload hoặc cập nhật DOM
    location.reload();
});