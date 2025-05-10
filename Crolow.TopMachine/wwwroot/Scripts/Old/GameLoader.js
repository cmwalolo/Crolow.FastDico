GameLoader = function (gameController, gameState) {
    this.gameController = gameController; 
    this.proxy = null;
    this.gameState = gameState;
    this.minNum = 1;
    this.maxNum = 60;
    this.config = null;
}

GameLoader.prototype = {
    init: function (params) {
        this.proxy = new GameLoaderLocalProxy(this.gameState);
        this.proxy.init(this, params);
    },
    proxyResponse: function (result) {
        if (result.code != 0) {
            gameController.error(result);
            return;
        }

        switch (result.typeMessage) {
            case "proxy_init":
                this.config = result.params.config;
                break;
            case "proxy_gameLoad":
                // Si on a chargé la première on copie la config 
                // Sinon on copie les nouveaux tours et les nouveaux commentaires
                if (result.params.firstLoad) {
                    this.config.gameConfig = result.params.gameConfig;
                } else {
                    this.config.gameConfig.rounds.totalRounds = result.params.gameConfig.rounds.totalRounds;
                    this.config.gameConfig.rounds.rounds.push(result.params.gameConfig.rounds.rounds);
                    this.config.gameConfig.comments.push(result.params.gameConfig.comments);
                }
                break;
            case "proxy_rankingLoad":
                break;
        }

        gameController.success(result);
    },
    loadGame: function (firstLoad) {
        this.proxy.loadGame({ firstLoad: firstLoad });
    },
    // On retourne le nombre de coups chargés 
    getTotalRounds: function () {
        return this.config.gameConfig.rounds.rounds.length;
    },
    // On retourne un coup donné
    getRound: function (round) {
        return this.config.gameConfig.rounds.rounds[round];
    },
    // On retourne un table de tous les coups chargés
    getAllRounds: function () {
        return this.config.gameConfig.rounds.rounds
    },
    // On charge les commentaires d'un tour et d'une certaine position
    // 0 : Au début du coup
    // 1 : Pendant le coup
    // 2 : A la fin du coup
    getComments: function (round, position) {
        var comments = new Array();
        if (this.config.gameConfig.comments != null) {
            for (var x = 0; x < this.config.gameConfig.comments.length; x++) {
                var o = this.config.gameConfig.comments[x];
                if (parseInt(o.Round) == round + 1 && parseInt(o.Position) == position) {
                    comments.push(o);
                }
            }
        }

        return comments;
    },
    // Retourne le total des points à un coup donné.
    getTotal: function (round) {
        var total = 0;
        for (var x = 0; x < round; x++) {
            total += parseInt(this.config.gameConfig.rounds.rounds[x].Points);
        }

        return total;
    },
    // Récupère les classements
    // Renvoie toujours la même chose en mode local
    getRankings: function (params) {
        this.proxy.getRankings({ type: params.type, pageIndex: params.pageIndex, pageSize: params.pageSize, sorting: params.sorting });
    }


}