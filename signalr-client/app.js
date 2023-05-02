$(function () {
  // result div hiding
  $("#resultDiv").hide();

  //alert div
  $("#alertContent").hide();

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
    li.valContent = `${message}`;
    document.getElementById("messageList").appendChild(li);
    $("#inputMessage").val("");
    $("#inputMessage").text("");
  });

  // client giriş yaptığından tüm kullanıcılara bildiriliyor
  connection.on("UserJoined", (connectionId) => {
    $("#alertContent").html(`${connectionId} bağlandı.`);
    $("#alertContent").show();

    setTimeout(function () {
      $("#alertContent").hide();
    }, 3000);
  });

  // client giriş yaptığından tüm kullanıcılara bildiriliyor
  connection.on("UserLeaved", (connectionId) => {
    $("#alertContent").html(`${connectionId} ayrıldı.`);
    $("#alertContent").show();

    setTimeout(function () {
      $("#alertContent").hide();
    }, 3000);
  });

  // sunucudan client listesi alınıyor
  connection.on("clients", (clientsData) => {
    $("#ClientList").empty();
    $.each(clientsData, function (index, item) {
      $("#ClientList").append(`<li id="${index}">${item}</li>`);
    });
  });

  $("#inputMessage").keypress(function (e) {
    if (e.which == 13) {
      send();
      return false;
    }
  });

  //button tıklandığında
  $("#btnSend").click(function (e) {
    e.preventDefault();
    send();
  });

  function send() {
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
  }
});
