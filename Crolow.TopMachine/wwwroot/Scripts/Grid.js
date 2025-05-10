
class Grid {
    constructor(gameController) {
        this.controller = gameController;
    }

    removeHandlers() {
        var table = $(this.playGroundid).find("#grid table");
        table.find("td").unbind('click');
    }

    init(clickable) {
        this.board = this.controller.config.Board.CurrentBoard[0];
        this.removeHandlers();
        this.clickable = clickable;
        this.drawGrid();
        this.scaleGrid();

        const element = document.querySelector("#grid");

        var grid = this;
        // Create a ResizeObserver
        const resizeObserver = new ResizeObserver(entries => {
            for (let entry of entries) {
                const { width, height } = entry.contentRect; // Get new dimensions
                console.log(`Element resized to: ${width}px x ${height}px`);
                grid.scaleGrid();
            }
        });

        // Start observing the element
        resizeObserver.observe(element);
    }

    drawGrid() {
        // On réinitialise la grille en créant une nouvelle table!
        var grid = $("#grid");
        grid.html("");
        grid.append("<table cellpadding='0' cellspacing='0' border='0'><tbody></tbody></table>");
        var table = grid.find("tbody");

        for (var x = 0; x < this.board.SizeV; x++) {
            table.append("<tr></tr>");
        }

        for (var x = 0; x < this.board.SizeH; x++) {
            table.find("tr").append("<td></td>");
        }

        // on assigne l'objet a une variable pour y accéder dans les fonctions dynamiques suivantes
        var self = this;

        // la class par défaut de chaque case est "cell", on y ajoute un espace fixe.
        table.find("td").addClass("cell");
        table.find("td").html("");

        // On assigne la classe border aux cellules affichant les coordonnées
        // On profite des capacités de jquery pour faire des requêtes sur le dom
        // pour effectuer la tâche

        for (var x = 1; x < this.board.SizeH - 1; x++) {
            for (var y = 0; y < this.board.SizeV; y += this.board.SizeV - 1) {
                table.find("tr:eq(" + y + ")").find("td:eq(" + x + ")")
                    .addClass("border")
                    .text(x);
            }
        }

        for (var x = 0; x < this.board.SizeH; x += this.board.SizeH - 1) {
            for (var y = 1; y < this.board.SizeV - 1; y++) {
                table.find("tr:eq(" + y + ")").find("td:eq(" + x + ")")
                    .addClass("border")
                    .text(String.fromCharCode(64 + y));
            }
        }

        // Cette boucle va définir la classe supplémentaire à utiliser pour les cases multiplicatrices
        // la classe est "cell"+ le multiplicateur défini.
        for (var x = 1; x < this.board.SizeH - 1; x++) {
            for (var y = 1; y < this.board.SizeV - 1; y++) {
                var lmul = -this.board.Grid[x][y].LetterMultiplier;
                var vmul = this.board.Grid[x][y].WordMultiplier;
                var multiplier = lmul < -1 ? lmul : vmul > 1 ? vmul : 0;
                var className = "cell" + multiplier;
                table.find("tr:eq(" + y + ")").find("td:eq(" + x + ")")
                    .addClass(className)
                    .attr("multiplier", multiplier);
            }
        }

        // Si on est en mode jeu, alors il est possible de cliquer sur la grille
        // LE click est renvoyé au controleur de jeu.
        if (this.clickable) {
            table.find("td").click(
                function () {
                    //self.gameController.clickGrid(this.parentNode.rowIndex, this.cellIndex);
                }
            );
        }

        // On définit la taille des cases
        //this.resizeGrid();

    }
    /*
    Est appelé à l'initialisation et par les événements de redimensionnement de la fenêtre ou 
    du layout manager.
    */
    resizeGrid() {
        // La taille de la grille est dynamique et est dépendante du container principal.
        // Si pour une raison ou une autre on change la présentation, ou le container
        // est redimensionné on peut redéfinir la taille des cases.
        var grid = $("#grid");
        var table = grid.find("table");

        // On calcule donc la taille du div afin de calculer la taille des cases
        var size = grid.width();
        var h = grid.height();

        // On prend la taille la plus petite et on calcule la taille minimal d'une case
        if (size > h) size = h;

        size = Math.round(size / (this.config.SizeH + 2));

        table.find("td").width(size - 2);
        table.find("td").height(size - 2);

        // On change la taille de la police à 60% de la taille de la case pour toutes 
        // Les cases de jeu => la class commence par cell.
        this.currentFontSize = (Math.round(size * 60 / 100)) + "px";
        table.find("td[class^='cell']").css('font-size', this.currentFontSize);

        // Comme la taille de la grille ne correspond pas forcément à la taille de son parent
        // Nous recentrons la grille dans celui-ci        
        var table = grid.find("table");
        var left = Math.round((grid.width() - table.width()) / 2);
        table.css("margin-left", left + "px");

        var top = Math.round((grid.height() - table.height()) / 2);
        table.css("margin-top", top + "px");

    }
    // Réinitialise la grille en vidant toutes les lettres 
    clearGrid() {
        var table = $("#grid table tbody");
        for (var x = 1; x <= this.config.SizeH; x++) {
            for (var y = 1; y <= this.config.SizeV; y++) {
                // Une particularité de Jquery est de pouvoir appeler
                // des fonctions à la queue. 
                // Un exemple ici : table.find.find -> Sélectionne des éléments du DOM
                // removeClass - removeclass - text seront exécutés sur la sélection courante.
                table.find("tr:eq(" + y + ")").find("td:eq(" + x + ")")
                    .removeClass("tile")
                    .removeClass("tileJoker")
                    .text("");
            }
        }

    }

    scaleGrid() {
        const gridElement = document.querySelector('#grid table');
        const parentElement = document.querySelector('#grid');

        if (gridElement && parentElement) {
            // Get the parent's dimensions
            var parentWidth = parentElement.offsetWidth;
            var parentHeight = parentElement.offsetHeight;

            // Get the grid's natural dimensions
            var gridWidth = gridElement.offsetWidth;
            var gridHeight = gridElement.offsetHeight;

            // Calculate the scale factor to fit the grid within the parent
            const scaleX = parentWidth / gridWidth;
            const scaleY = parentHeight / gridHeight;

            // Choose the smaller scale to fit within both dimensions
            const scale = Math.min(scaleX, scaleY);

            // Apply the scale
            gridElement.style.transform = `scale(${scale})`;
            gridElement.style.transformOrigin = 'top left'; // Ensure scaling starts from the top-left corner

            parentWidth = parentElement.offsetWidth;
            parentHeight = parentElement.offsetHeight;
            // Get the grid's natural dimensions
            gridWidth = gridElement.offsetWidth * scale;
            gridHeight = gridElement.offsetHeight * scale;
            // Optional: Center the grid if there's extra space
            gridElement.style.marginLeft = `${(parentWidth - gridWidth) / 2}px`;
            gridElement.style.marginTop = `${(parentHeight - gridHeight) / 2}px`;
        }
    }

}