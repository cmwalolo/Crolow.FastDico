RoundControlClass = function (gameController, gameState, roundControlId) {
    this.gameState = gameState;
    this.roundControlId = roundControlId;
    this.gameController = gameController;
}

RoundControlClass.prototype = {
    init: function () {
        // Mode = 2 => Exclusivement jouer ...
        if (this.mode == 2) {
            $("#nextRound").hide();
            $("#previousRound").hide();
        } else {
            $("#nextRound").show();
            $("#previousRound").show();
        }

        this.removeHandlers();
    },
    setRoundPosition: function () {
        $("#lblRound").text("Coup: " + (this.gameState.currentRound + 1));
        $("#lblScore").text("Score: " + this.gameState.gameLoader.getTotal(this.gameState.currentRound));

    },
    removeHandlers: function () {

        $('#nextRound').unbind('click').addClass('disabled');
        $('#previousRound').unbind('click').addClass('disabled');
        $('#getSolution').unbind('click').addClass('disabled');
    },
    addHandlers: function (endRound) {
        this.removeHandlers();
        // On assigne un événement sur le bouton "ViewGame"
        var self = this;

        if (self.gameState.mode == 0) {
            $("#nextRound").click(function () { self.gameController.nextRound(); }).removeClass('disabled');
            $("#previousRound").click(function () { self.gameController.previousRound(); }).removeClass('disabled');
            $('#getSolution').click(function () { self.gameController.setSolution(); }).removeClass('disabled');
        } else {
            if (endRound) {
                $("#nextRound").click(function () { self.gameController.nextRound(); }).removeClass('disabled');
                $("#previousRound").click(function () { self.gameController.previousRound(); }).removeClass('disabled');
            } else {
                $("#getSolution").click(function () { self.gameController.setSolution(); }).removeClass('disabled');
            }
        }
    }
}