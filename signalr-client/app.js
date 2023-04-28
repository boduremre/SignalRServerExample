$(function () {
  // result div hiding
  $("#resultDiv").hide();

  // connection ayarlanıyor
  const connection = new signalR.HubConnectionBuilder()
    .withUrl("https://localhost:7084/myhub/", {
      skipNegotiation: true,
      transport: signalR.HttpTransportType.WebSockets,
    })
    .withAutomaticReconnect()
    .configureLogging(signalR.LogLevel.Information)
    .build();

  // connection başlatılıyor.
  connection.start();

  // sunucudan cevap alınıyor
  connection.on("ReceiveMessage", (message) => {
    const li = document.createElement("li");
    li.textContent = `${message}`;
    document.getElementById("messageList").appendChild(li);
    $("#inputMessage").val("");
    $("#inputMessage").text("");
});

  //button tıklandığında
  $("#btnSend").click(function (e) {
    e.preventDefault();

    $("#inputMessage").removeClass("is-invalid");

    let message = $("#inputMessage").val();

    if (message === null || message === "") {
      $("#inputMessage").addClass("is-invalid");
    } else {
      connection
        .invoke("SendMessageAsync", message)
        .catch((error) =>
          console.log(`Mesaj gönderilirken hata meydana geldi! HATA: ${error}`)
        );
    }
  });
});
