
/*
Cette classe contient l'état de la partie en cours
*/
GameStateClass = function (mode, gameId) {
    this.mode = mode;
    this.gameId = gameId;

    this.currentSolution = null;
    this.currentPlayerSolution = null;

    // Est-ce que la partie est chargée intégralement ?
    // Pour une partie en direct, les coups seront chargés
    // Au coup par coup.
    this.partialLoad = 0;

    // Le coup actuel
    this.currentRound = 0;

    // Le chargeur de partie
    this.gameLoader = null;

    // un tableau avec les multiplicateurs
    this.gridMultipliers = new Array();

    // Etat de la grille
    // 0 : vide
    // 1 : lettre jouée
    // 2 : lettre jouée temporairement
    // 4 : joker
    this.gridCellStates = new Array(); 
    this.gridCellLetters = new Array(); 

    // Un tableau en 3 dimensions
    // qui contiendra les points des prolongements
    // horizontaux et verticaux
    // Cela nous permet de ne pas recalculer a chaque fois ceux-ci 
    // lorsque l'on est en mode jeu
    this.gridCellCalc = new Array(); 

    // Etat du chevalet 
    // 0 : vide
    // 1 : sur le chevalet
    // 2 : sur la grille
    this.rackState =  new Array(); 
    this.rackLetters = new Array();

    // Ce dictionnaire peut servir a éviter de devoir vérifier
    // la validité des mots sur le serveur à chaque validation
    // Une fois qu'un mot est validé ce dictionnaire est mis à jour.
    this.dictionary = new Object();
}

GameStateClass.prototype =
{
    init : function()
    {
        this.createMultipliers(); 
        this.createGridState(); 
    },
    setRack: function (letters) {
        var rack = ""; 
        for (var x = 0; x < letters.length; x++) {
            var l = letters[x];
            if (l != '?' && l != '+' && l != '-') {
                rack += letters[x]; 
            }
        }

        this.rackState = new Array(rack.length); 
        this.rackLetters = new Array(rack.length); 

        for (var x = 0; x < rack.length; x++)
        {
            this.rackState[x] = 0;
            this.rackLetters[x] = rack[x]; 
        }
    }, 
    createGridState : function()
    {
        var config = this.gameLoader.config.gridConfig;

        var sizeV = config.sizeV; 
        var sizeH = config.sizeH;

        this.gridCellCalc = new Array(2)
        for (var i = 0; i < 2; i++)
        {
            this.gridCellStates[i] = new Array(sizeV+2)
            for (var j = 0; j < sizeV+2; j++)
            {
                this.gridCellStates[i][j] = new Array(sizeH+2)
                
            }
       }

        this.gridCellStates = new Array(config.sizeV+2)
        for (var i = 0; i < sizeV+2; i++)
            this.gridCellStates[i] = new Array(sizeH+2)

        this.gridCellLetters = new Array(sizeV+2)
        for (var i = 0; i < sizeV+2; i++)
            this.gridCellLetters[i] = new Array(sizeH+2)

        for (var x = 0; x < sizeV+2; x++) {
            for (var y = 0; y < sizeH+2; y++) {
                if (x == 0 || y == 0 || x == sizeV+1 || y == sizeH+1)
                {
                    // CE sont les bords de la grille 
                    // Cela evite beaucoup de tests pour verifier qu'on reste dans les limites
                    // de celle-ci
                    this.gridCellStates[y][x] = 0; 
                    this.gridCellLetters[y][x] = -2;
                } else
                {
                    // Sinon on met a -1 
                    this.gridCellStates[y][x] = 0; 
                    this.gridCellLetters[y][x] = -1;
                } 
            }
        }

    }, 
    createMultipliers: function () {
        var config = this.gameLoader.config.gridConfig;
        this.gridMultipliers = new Array(config.sizeV+2)
        for (i = 0; i < config.sizeV+2; i++)
            this.gridMultipliers[i] = new Array(config.sizeH+2)

        // Cette boucle va définir la classe supplémentaire à utiliser pour les cases multiplicatrices
        // la classe est "cell"+ le multiplicateur défini.
        for (var x = 0; x < this.config.sizeH+2; x++) {
            for (var y = 0; y < this.config.sizeV+2; y++) {
                var multiplier = this.gridGetCellColor(x - 1, y - 1);
                this.gridMultipliers[y][x] = multiplier; 
            }
        }
    },
    gridGetCellColor: function (x, y) {
        var config = this.gameLoader.config.gridConfig;
        var array = config.multipliers;
        for (var xx = 0; xx < array.length; xx += 3) {
            if (array[xx] == x && array[xx + 1] == y)
                return array[xx + 2];
        }
        return 0;
    }
}