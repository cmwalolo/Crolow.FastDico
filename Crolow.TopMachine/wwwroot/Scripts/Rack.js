class Rack {
    constructor(gameController) {
        this.controller = gameController;
        this.rack =[];
    }

    ClearRack() {
        $("#rack").find("table tr").html("");
        this.rack =[];
    }

    Init() {
        this.ClearRack();
        var t = $("#rack").find("table tr");
        var count = this.controller.config.SelectedRound.Rack.Tiles.length;
        for (var x = 0; x < count; x++) {
            var letter = this.controller.config.SelectedRound.Rack.Tiles[x];
            var char = this.controller.letterConfig.GetRackLetter(letter, false);

            const tileCell = $("<td>")
                .text(char) // Add the letter as text content
                .data("letter", letter);
            this.rack.push(letter);
            $(t).append(tileCell);
        }
    }
    Add(letter)
    {
        var t = $("#rack").find("table tr");
        var count = this.controller.config.SelectedRound.Rack.Tiles.length;
        var char = this.controller.letterConfig.GetRackLetter(letter, false);
        const tileCell = $("<td>")
            .text(char) // Add the letter as text content
            .data("letter", letter);
        this.rack.push(letter);
        $(t).append(tileCell);
    } 

    RemoveLetter(letter, isJoker)
    {
        //var t = $("#rack").find("table tr");
        var rack = $("#rack").find("table tr td");
        for(var x = 0; x < rack.length; x++)
        {
            var r = rack[x];
            var d = $(r).data("letter");
            if ((d.Letter == letter && !d.IsJoker) 
            || (d.IsJoker && isJoker))
            {
                rack = rack.filter(item => item !== d);
                r.remove();
                return d; 
            }
        }

        return null;
    }

}