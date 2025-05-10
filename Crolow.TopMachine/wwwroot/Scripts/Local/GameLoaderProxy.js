GameLoaderLocalProxy = function (gameState) {
    this.minNum = 1;
    this.maxNum = 60;
    this.client = null;

    this.gameId = null;
    this.lastRound = 0;
    this.lastComment = 0;
}

GameLoaderLocalProxy.prototype = {
    init: function (client, params) {
        this.gameId = null;
        this.lastRound = 0;
        this.lastComment = 0;

        if (!params.gameId || params.gameId == "0") {
            this.gameId = Math.floor(Math.random() * (this.maxNum) + 1);
        } else {
            this.gameId = gameId;
        }

        this.client = client;
        params.config = new GameParameters();

        var msg = new Message('proxy_init', 0, '', params);
        this.client.proxyResponse(msg);
    },
    loadGame: function (params) {

        var self = this;

        $.ajax({
            type: "GET",
            url: "Games/" + self.gameId + ".xml",
            dataType: "script",
            cache: false,
            success: function (msg) {
                // On désérialise le fichier XML en un objet
                var json = $.xml2json(msg);

                var gameConfig = { rounds: new Object(), comments: new Array() };

                gameConfig.rounds.totalRounds = -1;
                gameConfig.rounds.rounds = json.Rounds.GeneratedGameRounds.splice(self.lastRound, self.lastRound + 10)

                if (this.lastRound > json.Rounds.GeneratedGameRounds.length) {
                    gameConfig.rounds.totalRounds = json.Rounds.GeneratedGameRounds.length;
                }




                if (json.Comments) {
                    gameConfig.comments = json.Rounds.GeneratedGameRounds.splice(self.lastComment, self.lastComment + 10)
                } else {
                    gameConfig.comments = new Array();
                }

                self.lastRound += gameConfig.rounds.rounds.length;
                self.lastComment += gameConfig.comments.length;


                if (params.firstLoad) {
                    gameConfig.lettersOnRack = json.MaxLettres;
                    gameConfig.lettersPlayed = json.MinLettres;
                    gameConfig.isJoker = json.IsJoker;
                    gameConfig.chrono = json.Temps;
                }

                params.gameConfig = gameConfig;
                self.client.proxyResponse(Message('proxy_gameLoad', 0, '', params));

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                self.client.proxyResponse(Message('proxy_gameLoad', 0, textStatus, params));
            }
        });
    },
    /*
    params : type, pageIndex, pageSize, sorting
    */
    getRankings: function (params) {

        rankings = new Object();
        rankings.fields = new Array("Pos", "Player", "Country", "Club", "Pts", "Neg", "Pc");
        rankings.headers = new Array("Pos", "Joueur", "Pays", "Club", "Pts", "Neg", "Pc");
        rankings.list = new Array();

        for (var pl = 0; pl < 10; pl++) {
            rankings.list.push(
                {
                    Pos: pl,
                    Player: "Laurent " + pl,
                    Country: "BEL",
                    Club: "Yod",
                    Pts: 1000,
                    Neg: 0,
                    Pc: 98.99
                }
            );
        }

        params.rankings = rankings;
        self.client.proxyResponse(Message('proxy_rankingLoad', 0, '', params));

    }


}