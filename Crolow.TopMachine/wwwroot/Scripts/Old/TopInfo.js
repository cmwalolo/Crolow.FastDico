TopInfoClass = function (gameController, gameState, topInfoId) {
    this.topInfoId = topInfoId;
    this.gameState = gameState;
    this.gameController = gameController;
}

TopInfoClass.prototype = {
    init: function () {
        // On est dans un mode de jeu
        // alors on affiche le top du joueur 
        if (this.mode > 0) {
            $("#posJoueurRow").show();
        } else {
            $("#posJoueurRow").hide();
        }
    },
    setSolution: function () {
        $("#posTopRow li:nth-child(2)").html("<span>" + this.gameState.currentSolution.Word + "</span");
        $("#posTopRow li:nth-child(3)").html("<span>" + this.gameState.currentSolution.Direction + "</span>");
        $("#posTopRow li:nth-child(4)").html("<span>" + this.gameState.currentSolution.Points + "</span>");

        if (this.mode > 0) {
            $("#posJoueurRow li:nth-child(2)").html("<span>" + this.gameState.currentPlayerSolution.Word + "</span");
            $("#posJoueurRow li:nth-child(3)").html("<span>" + this.gameState.currentPlayerSolution.Direction + "</span>");
            $("#posJoueurRow li:nth-child(4)").html("<span>" + this.gameState.currentPlayerSolution.Points + "</span>");
        }
    }
}