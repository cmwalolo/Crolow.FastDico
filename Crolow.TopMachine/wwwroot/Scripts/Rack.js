class Rack {
    constructor(gameController) {
        this.controller = gameController;
    }

    clearRack() {
        $("#rack").find("table tr").html("");
    }

    init() {
        this.clearRack();
        var t = $("#rack").find("table tr");
        var count = this.controller.config.SelectedRound.Rack.Tiles.length;
        for (var x = 0; x < count; x++) {
            var letter = this.controller.config.SelectedRound.Rack.Tiles[x];
            var char = this.controller.letterConfig.GetRackLetter(letter);

            const tileCell = $("<td>")
                .text(char) // Add the letter as text content
                .data("letter", letter);

            $(t).append(tileCell);
        }
    }

}