

/* Cette classe est le contrôleur de l'application 
Il contient tous les éléments nécessaires pour piloter l'application et ses
différents éléments
*/
class GameController {
    constructor() {
        this.InitGame = this.InitGame.bind(this);
        this.InitializeRound = this.InitializeRound.bind(this);

        window.messageService.addListener("InitializeGame", this.InitGame);
        window.messageService.addListener("InitializeRound", this.InitializeRound);

        this.config = null;
        this.grid = null;
        this.rack = null;
        this.letterConfig = null;
    }

    InitializeRound(configuration) {
        this.config = configuration;
        this.rack = new Rack(this);
        this.rack.init();
    }

    InitGame(configuration) {
        this.config = configuration;

        this.letterConfig = new LetterConfig(this);
        // On crée la grille
        this.grid = new Grid(this);
        this.grid.init(true);
    }
};

window.messageService = new MessageService();
window.gameController = new GameController();
