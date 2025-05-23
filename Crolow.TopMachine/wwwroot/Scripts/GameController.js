

/* Cette classe est le contrôleur de l'application 
Il contient tous les éléments nécessaires pour piloter l'application et ses
différents éléments
*/
class GameController {
    constructor() {
        this.InitGame = this.InitGame.bind(this);
        this.InitializeRound = this.InitializeRound.bind(this);
        this.SetRound = this.SetRound.bind(this);

        window.messageService.addListener("InitializeGame", this.InitGame);
        window.messageService.addListener("InitializeRound", this.InitializeRound);
        window.messageService.addListener("SetRound", this.SetRound);

        var self = this;
        document.addEventListener("keydown", function (event) {
            self.HandleKeyPress(event);
        });

        this.config = null;
        this.grid = null;
        this.rack = null;
        this.letterConfig = null;
    }


    InitGame(configuration) {
        this.config = configuration;
        this.letterConfig = new LetterConfig(this);
        // On crée la grille
        this.grid = new Grid(this);
        this.grid.Init(true);
        var self = this;
    }

    InitializeRound(configuration) {
        this.config = configuration;
        this.rack = new Rack(this);
        this.rack.Init();
        this.grid.Init(true);
        this.grid.UpdateGrid();
        this.solutionHolder = new SolutionHolder(this);
        this.solutionHolder.SetSolution(false, false);
    }

    SetRound(objects) {
        // this.grid.SetRound(objects[0]);
        // this.grid.SetRack(objects[1]);
    }

    HandleKeyPress(event) {
        // Read the pressed key
        const key = event.key; // Character or key name (e.g., "a", "Enter", "Backspace")
        const shiftPressed = event.shiftKey; // True if Shift is held down

        // Handle specific keys
        switch (key) {
            case "Enter":
                this.Validate();
                break;
            case "Backspace":
                this.RemoveLast();
                break;
            case "ArrowUp":
                this.grid.MoveGrid(0, -1, false);
                break;
            case "ArrowDown":
                this.grid.MoveGrid(0, 1, false);
                break;
            case "ArrowLeft":
                this.grid.MoveGrid(-1, 0, false);
                break;
            case "ArrowRight":
                this.grid.MoveGrid(1, 0, false);
                break;
            case "Home":
                this.grid.MoveGrid(-5, 0, false);
                break;
            case "End":
                this.grid.MoveGrid(5, 0, false);
                break;
            case "PageUp":
                this.grid.MoveGrid(0, -5, false);
                break;
            case "PageDown":
                this.grid.MoveGrid(0, 5, false);
                break;
            case " ":
                this.grid.MoveGrid(0, 0, true);
                break;
            default:
                if (key != "Shift") {
                    this.Play(key.toUpperCase(), shiftPressed);
                    console.log(`Key pressed: ${key}`);
                }
                break;
        }
    }

    Play(key, shifted) {
        var cell = this.grid.GetCurrentCell();
        this.solutionHolder.PlayLetter(cell, key, shifted, false);
        this.solutionHolder.CheckSolution();
    }

    Validate() {
        this.solutionHolder.Validate();
    }

    RemoveLast() {
        this.solutionHolder.RemoveLastLetter();
        this.solutionHolder.CheckSolution();
    }
};

window.messageService = new MessageService();
window.gameController = new GameController();

window.playGroundComponentRef = null;

window.setPlaygroundComponent = (ref) => {
    window.playGroundComponentRef = ref;
};

function sendPlaygroundController(fn, parameters) {
    if (window.playGroundComponentRef) {
        return window.playGroundComponentRef.invokeMethodAsync(fn, parameters);
            //.then(() => console.log('Instance method called!'))
            ///.catch(err => console.error(err));
    }
}
