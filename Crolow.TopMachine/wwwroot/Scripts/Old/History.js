HistoryClass = function (gameState, historyId, eventRoundRequest) {
    this.gameState = gameState;
    this.historyId = historyId;
    this.eventRoundRequest = eventRoundRequest;
}

HistoryClass.prototype = {
    init: function () {
        // On recrée une nouvelle table avec les en-tetes
        this.clearHistory();
    },
    clearHistory: function () {
        var h = $(this.historyId);
        $(h).html("<table cellspacing='0' cellpadding='0' class='list'><tr></tr></table>");
        var tr = $(h).find("table tr");

        $(tr).append("<th>#</th><th>Tirage</th><th>Mot joué</th><th>Position</th><th>Points</th><th>Total</th>");
    },
    setHistory: function (rounds) {
        this.clearHistory();
        var t = $(this.historyId).find("table");
        /*
        <GeneratedGameRounds>
        <Turn>1</Tudrn>
        <Word>eXpOSAS</Word>
        <Direction>H3</Direction>
        <Points>98</Points>
        <Tirage>-??AOSSX</Tirage>
        </GeneratedGameRounds>
        */
        var total = 0;
        for (var x = 0; x < rounds.length; x++) {
            var r = rounds[x];
            total += parseInt(r.Points);
            $(t).append("<tr><td class='link'><a href='#' hid='" + x + "'>" + r.Turn + "</a></td><td>" + r.Tirage + "</td><td>" + r.Word + "</td><td>"
                    + r.Direction + "</td><td>" + r.Points + "</td><td>" + total + "</td></tr>");

        }

        var self = this;
        $(t).find("td.link a").click(
            function (eventObject) {
                if (self.eventRoundRequest) {
                    self.eventRoundRequest($(this).attr("hid"));
                }
            }
        );

    }

}