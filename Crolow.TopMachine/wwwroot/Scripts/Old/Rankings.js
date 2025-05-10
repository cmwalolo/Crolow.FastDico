RankingClass = function (gameController, gameState) {
    this.gameController = gameController; 
    this.gameState = gameState;
}

RankingClass.prototype =
{
    init: function () {
        var self = this;
        $("#tabRankings").tabs();
        $("#btnRankingDetail").click(function () { self.gameController.loadRanking(1, "#listRankingDetail"); });
        $("#btnRankingGame").click(function () { self.gameController.loadRanking(2, "#listRankingGame"); });
        $("#btnRankingGlobal").click(function () { self.gameController.loadRanking(3, "#listRankingGlobal"); });
    },
    setRanking: function (result) {
        var type = result.params.type;
        var listId = result.params.listId;
        var table = $(listId);
        table.html("");

        var row = "<tr>";
        for (var x = 0; x < rank.headers.length; x++) {
            row += "<th>" + rank.headers[x] + "</th>";
        }
        row += "</tr>";
        table.append(row);

        for (var i = 0; i < rank.list.length; i++) {
            var o = rank.list[i];
            var row = "<tr>";
            for (var x = 0; x < rank.fields.length; x++) {
                row += "<td>" + o[rank.fields[x]] + "</td>";
            }
            row += "</tr>";
            table.append(row);
        }
    }
}
