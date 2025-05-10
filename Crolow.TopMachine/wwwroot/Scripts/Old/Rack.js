RackClass = function (controller) {
    this.controller = controller;
    // Contenant HTML de la grille
    this.rackId = rackId;
}

RackClass.prototype = {
    init: function () {
        ;
    },
    clearRack: function () {
        $(this.rackId).find("table tr").html("");
    },
    setRack: function (letters) {
        this.clearRack();
        var t = $(this.rackId).find("table tr");
        for (var x = 0; x < letters.length; x++) {
            var l = letters[x];
            if (l != '?' && l != '+' && l != '-') {
                $(t).append("<td>" + letters[x] + "</td>");
            }
        }
    }

}