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
      document.getElementById("xpReq0").innerHTML = data[0].xpLevel;
      document.getElementById("raidReward0").innerHTML = data[0].xpReward;
      document.getElementById("raidBoss1").innerHTML = data[1].name;
      document.getElementById("sessionId1").innerHTML = data[1].date;
      document.getElementById("xpReq1").innerHTML = data[1].xpLevel;
      document.getElementById("raidReward1").innerHTML = data[1].xpReward;
      document.getElementById("raidBoss2").innerHTML = data[2].name;
      document.getElementById("sessionId2").innerHTML = data[2].date;
      document.getElementById("xpReq2").innerHTML = data[2].xpLevel;
      document.getElementById("raidReward2").innerHTML = data[2].xpReward;
      document.getElementById("raidBoss3").innerHTML = data[3].name;
      document.getElementById("sessionId3").innerHTML = data[3].date;
      document.getElementById("xpReq3").innerHTML = data[3].xpLevel;
      document.getElementById("raidReward3").innerHTML = data[3].xpReward;
      document.getElementById("raidBoss4").innerHTML = data[4].name;
      document.getElementById("sessionId4").innerHTML = data[4].date;
      document.getElementById("xpReq4").innerHTML = data[4].xpLevel;
      document.getElementById("raidReward4").innerHTML = data[4].xpReward;
      document.getElementById("raidBoss5").innerHTML = data[5].name;
      document.getElementById("sessionId5").innerHTML = data[5].date;
      document.getElementById("xpReq5").innerHTML = data[5].xpLevel;
      document.getElementById("raidReward5").innerHTML = data[5].xpReward;
      document.getElementById("raidBoss6").innerHTML = data[6].name;
      document.getElementById("sessionId6").innerHTML = data[6].date;
      document.getElementById("xpReq6").innerHTML = data[6].xpLevel;
      document.getElementById("raidReward6").innerHTML = data[6].xpReward;
      document.getElementById("raidBoss7").innerHTML = data[7].name;
      document.getElementById("sessionId7").innerHTML = data[7].date;
      document.getElementById("xpReq7").innerHTML = data[7].xpLevel;
      document.getElementById("raidReward7").innerHTML = data[7].xpReward;
      document.getElementById("raidBoss8").innerHTML = data[8].name;
      document.getElementById("sessionId8").innerHTML = data[8].date;
      document.getElementById("xpReq8").innerHTML = data[8].xpLevel;
      document.getElementById("raidReward8").innerHTML = data[8].xpReward;
      document.getElementById("raidBoss9").innerHTML = data[9].name;
      document.getElementById("sessionId9").innerHTML = data[9].date;
      document.getElementById("xpReq9").innerHTML = data[9].xpLevel;
      document.getElementById("raidReward9").innerHTML = data[9].xpReward;
    }
  }
);
