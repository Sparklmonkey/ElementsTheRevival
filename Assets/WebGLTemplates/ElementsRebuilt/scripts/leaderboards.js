var playerList;

function getAllPlayerData(filter) {
  fetch("http://localhost:5158/api/user-data/leaderboard", {
  method: "POST",
  body: JSON.stringify({
    filter: filter
  }),
  headers: {
    "Content-type": "application/json; charset=UTF-8"
  }
}).then((response) => response.json())
  .then((json) => setupTableWithPlayerData(json));
}

function setupTableWithPlayerData(data) {
  var table = document.getElementById("detail-table");
  var tableContents = "<table class='detail-table'><tr> <th class='detail-header'>Username</th> <th class='detail-header'>Player Score</th> <th class='detail-header'>Overall Wins</th> <th class='detail-header'>Overall Loses</th> <th class='detail-header'>Total Cards</th> <th class='detail-header'>Electrum</th> </tr>"

  data.leaderboardList.forEach((player) => {

  tableContents += "<tr> <td class='detail-cell'>" + player.username + "</td> <td class='detail-cell'>" + player.playerScore + "</td> <td class='detail-cell'>" + player.overallWins + "</td> <td class='detail-cell'>" + player.overallLoses + "</td> <td class='detail-cell'>" + player.cardCount + "</td> <td class='detail-cell'>" + player.electrum + "</td> </tr>"
  })
    tableContents += "</table>"
    table.innerHTML = tableContents;
    console.log(tableContents);
}
