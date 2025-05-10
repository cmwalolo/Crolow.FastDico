CommentsClass = function (gameState, commentsId) {
    this.gameState = gameState;
    this.commentsId = commentsId;
}

CommentsClass.prototype = {
    init: function () {
        //  this.clearComments();
    },
    clearComments: function () {
        $(this.commentsId + ' .exo').unbind('click');
        var h = $(this.commentsId);

        $(h).html("");

    },
    setComments: function (clear, comments) {
        // si c'est le début d'un coup on devrait effacer les commentaires précédents
        if (clear) {
            this.clearComments();
        }

        var t = $(this.commentsId);
        var total = 0;

        // On rajoute tous les commentaires
        for (var x = 0; x < comments.length; x++) {
            var r = comments[x];
            var el = $("<div style='display:none' class='comment commentInfo_" + r.Type + "'>" + r.Text + "</div>");
            $(t).append(el);
            $(el).animate({ "height": "toggle", "opacity": "toggle" }, "slow");


            // S'il existe une réponse à un commentaire... Il s'affiche seulement si un lien est cliqué dans le commentaire.
            // La réponse est ajoutée a la table, mais donc cachée.
            if (r.Popup != null) {

                var popup = $("<div style='display:none' class='comment commentInfo_3'>" + r.Popup + "</div>");
                $(t).append(popup);
                $(el).find(".exo").click(function () {
                    $(popup).animate({ "height": "toggle", "opacity": "toggle" }, "slow");
                });
            }
        }

    }

}