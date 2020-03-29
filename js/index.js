const container = document.createElement("div");
container.setAttribute("class", "container");

// var request = new XMLHttpRequest();
var getJSON = function(url, callback) {
  var xhr = new XMLHttpRequest();
  xhr.open("GET", url, true);
  xhr.responseType = "json";
  xhr.onload = function() {
    var status = xhr.status;
    if (status === 200) {
      callback(null, xhr.response);
    } else {
      callback(status, xhr.response);
    }
  };
  xhr.send();
};

const json = getJSON(
  "https://telequestbackend.azurewebsites.net/api/Character/GetLeaderbaord",
  function(err, data) {
    if (err !== null) {
      console.log("Something went wrong: " + err);
    } else {
      document.getElementById("username0").innerHTML = data[0].username;
      document.getElementById("xp0").innerHTML = data[0].expPoints;
      document.getElementById("username1").innerHTML = data[1].username;
      document.getElementById("xp1").innerHTML = data[1].expPoints;
      document.getElementById("username2").innerHTML = data[2].username;
      document.getElementById("xp2").innerHTML = data[2].expPoints;
      document.getElementById("username3").innerHTML = data[3].username;
      document.getElementById("xp3").innerHTML = data[3].expPoints;
      document.getElementById("username4").innerHTML = data[4].username;
      document.getElementById("xp4").innerHTML = data[4].expPoints;
      document.getElementById("username5").innerHTML = data[5].username;
      document.getElementById("xp5").innerHTML = data[5].expPoints;
      document.getElementById("username6").innerHTML = data[6].username;
      document.getElementById("xp6").innerHTML = data[6].expPoints;
      document.getElementById("username7").innerHTML = data[7].username;
      document.getElementById("xp7").innerHTML = data[7].expPoints;
      document.getElementById("username8").innerHTML = data[8].username;
      document.getElementById("xp8").innerHTML = data[8].expPoints;
      document.getElementById("username9").innerHTML = data[9].username;
      document.getElementById("xp9").innerHTML = data[9].expPoints;
    }
  }
);

var getJSON = function(url, callback) {
  var xhr = new XMLHttpRequest();
  xhr.open("GET", url, true);
  xhr.responseType = "json";
  xhr.onload = function() {
    var status = xhr.status;
    if (status === 200) {
      callback(null, xhr.response);
    } else {
      callback(status, xhr.response);
    }
  };
  xhr.send();
};

const raids = getJSON(
  "https://telequestbackend.azurewebsites.net/api/Raid/GetRaids",
  function(err, data) {
    if (err !== null) {
      console.log("Something went wrong: " + err);
    } else {
      document.getElementById("raidBoss0").innerHTML = data[0].name;
      document.getElementById("sessionId0").innerHTML = data[0].date;
      document.getElementById("raidReward0").innerHTML = data[0].xpReward;
      document.getElementById("raidBoss1").innerHTML = data[1].name;
      document.getElementById("sessionId1").innerHTML = data[1].date;
      document.getElementById("raidReward1").innerHTML = data[1].xpReward;
    }
  }
);
