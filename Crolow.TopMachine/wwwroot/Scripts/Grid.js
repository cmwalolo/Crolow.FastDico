
class Grid {
    constructor(gameController) {
        this.controller = gameController;
        this.currentCell = null;
    }

    RemoveHandlers() {
        var table = $(this.playGroundid).find("#grid table");
        table.find("td").unbind('click');
    }

    Init(clickable) {
        this.board = this.controller.config.Board.CurrentBoard[0];
        this.boards = this.controller.config.Board.CurrentBoard;
        this.RemoveHandlers();
        this.clickable = clickable;
        this.DrawGrid();
        this.ScaleGrid();

        const element = document.querySelector("#grid");

        var grid = this;
        // Create a ResizeObserver
        const resizeObserver = new ResizeObserver(entries => {
            for (let entry of entries) {
                const { width, height } = entry.contentRect; // Get new dimensions
                console.log(`Element resized to: ${width}px x ${height}px`);
                grid.ScaleGrid();
            }
        });

        // Start observing the element
        resizeObserver.observe(element);
    }

    DrawGrid() {
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
                var sq = this.board.Grid[y][x];
                var lmul = -sq.LetterMultiplier;
                var vmul = sq.WordMultiplier;
                var multiplier = lmul < -1 ? lmul : vmul > 1 ? vmul : 0;
                var className = "cell" + multiplier;
                var el = table.find("tr:eq(" + y + ")").find("td:eq(" + x + ")")
                    .addClass(className)
                    .attr("multiplier", multiplier)
                    .data("square", sq);

                if (sq.Status != -1) {
                    var c = "tile";
                    if (sq.CurrentLetter.IsJoker) {
                        c += "tile tileJoker";
                    }
                    el.addClass(c).text(this.controller.letterConfig.GetRackLetter(sq.CurrentLetter, true));
                }
            }
        }

        // Si on est en mode jeu, alors il est possible de cliquer sur la grille
        // LE click est renvoyé au controleur de jeu.
        var self = this;
        if (this.clickable) {
            table.find("td").click(
                function () {
                    var sq = $(this).data("square");
                    if (sq && sq.Status != 1) {
                        var pos = self.ClickGrid(this.parentNode.rowIndex, this.cellIndex);
                        self.ResetPosition(this, pos);
                    }
                }
            );
        }
    }

    UpdateGrid() {
        this.board = this.controller.config.Board.CurrentBoard[0];
        this.boards = this.controller.config.Board.CurrentBoard;

        var grid = $("#grid");
        var table = grid.find("tbody");
        for (var x = 1; x < this.board.SizeH - 1; x++) {
            for (var y = 1; y < this.board.SizeV - 1; y++) {
                var sq = this.board.Grid[y][x];
                var lmul = -sq.LetterMultiplier;
                var vmul = sq.WordMultiplier;
                var multiplier = lmul < -1 ? lmul : vmul > 1 ? vmul : 0;
                var className = "cell" + multiplier;
                var el = table.find("tr:eq(" + y + ")").find("td:eq(" + x + ")")
                    .addClass(className)
                    .attr("multiplier", multiplier)
                    .data("square", sq);

                if (sq.Status != -1) {
                    var c = "tile";
                    if (sq.CurrentLetter.IsJoker) {
                        c += "tile tileJoker";
                    }
                    el.addClass(c).text(this.controller.letterConfig.GetRackLetter(sq.CurrentLetter, true));
                }
            }
        }
    }

    MoveGrid(h, v, changeDirection) {
        var o = this.currentPosition;
        var n = { x: o.x + h, y: o.y + v, d: o.d };
        n.x = Math.max(1, n.x);
        n.y = Math.max(1, n.y);
        n.x = Math.min(this.board.SizeH - 2, n.x);
        n.y = Math.min(this.board.SizeV - 2, n.y);

        if (changeDirection) {
            n.d = n.d == 1 ? 0 : 1;
        }
        this.currentPosition = n;
        this.ResetPosition(null, this.currentPosition);
    }

    GetPosition() {
        return this.currentPosition;
    }

    ClickGrid(row, col) {
        if (this.currentPosition != null && this.currentPosition.x == col && this.currentPosition.y == row) {
            this.currentPosition.d = this.currentPosition.d == 1 ? 0 : 1;
        } else {
            this.currentPosition = { x: col, y: row, d: 0 };
        }
        return this.currentPosition;
    }

    ResetPosition(el, pos) {
        if (pos == null) {
            pos = this.currentPosition;
        } else {
            this.currentPosition = { ...pos };
        }

        if (el == null) {
            el = $("#grid table tr:eq(" + pos.y + ")").find("td:eq(" + pos.x + ")")[0]
        }

        this.currentCell = el;

        $("#grid table td").removeClass("arrow-right");
        $("#grid table td").removeClass("arrow-down");
        $(el).addClass(pos.d == 0 ? "arrow-right" : "arrow-down");
    }

    // Réinitialise la grille en vidant toutes les lettres 
    ClearGrid() {
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

    GetCurrentCell() {
        return this.currentCell;
    }

    GetCell(x, y) {
        var table = $("#grid table tbody");
        var cell = table.find("tr:eq(" + y + ")").find("td:eq(" + x + ")")
        return cell;
    }

    MoveBackward() {
        var p = this.currentPosition;
        if (p.x > 1 && p.d == 0) {
            p.x--;
        } else if (p.y > 1 && p.d == 1) {
            p.y--;
        } else {
            return null;
        }

        return this.currentCell;
    }

    MoveForward() {
        var p = this.currentPosition;
        var maxH = this.board.SizeH - 2;
        var maxV = this.board.SizeV - 2;

        if ((p.x < maxH && p.d == 0)) {
            p.x++;
        } else if ((p.y < maxV && p.d == 1)) {
            p.y++;
        } else {
            return null;
        }
        return this.currentCell;
    }

    SetLetter(cell, text) {
        var sq = $(cell).data("square");
        var c = "tile";
        if (sq.CurrentLetter.IsJoker) {
            c += "tile tileJoker";
        }
        $(cell).data("square", sq)
            .addClass(c)
            .text(this.controller.letterConfig.GetRackLetter(sq.CurrentLetter, true));
    }

    RemoveLetter(cell) {
        var sq = $(cell).data("square");
        sq.CurrentLetter = null;
        sq.Status = -1;
        $(cell).removeClass("tile")
            .removeClass("tileJoker")
            .data("square", sq).text("");
    }

    ScaleGrid() {
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