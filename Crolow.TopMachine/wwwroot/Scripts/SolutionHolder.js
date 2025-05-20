class SolutionHolder {
    constructor(gameController) {
        this.controller = gameController;
        this.solution = [];
        this.solutionCells = [];
        this.roundPosition = null;
        this.solutionHolder = $("#solutions")
        this.totalPoints = 0;
    }

    GetPosition() {
        if (this.roundPosition.d == 0) {
            return String.fromCharCode(64 + this.roundPosition.y) + this.roundPosition.x;
        } else {
            return this.roundPosition.x + String.fromCharCode(64 + this.roundPosition.y);
        }
    }

    GetWord() {
        var word = "";
        for (var x = 0; x < this.solution.length; x++) {
            word += this.controller.letterConfig.GetRackLetter(this.solution[x].CurrentLetter, true);
        }
        return word;
    }

    SetSolution(validated, isValid) {
        var y = validated ? 1 : 3;

        //var r = $("#solutions").find("tr:eq(" + y + ")")
        var r = $("table#solutions tr:eq(" + y + ")")
        if (isValid) {
            r.find("td:eq(" + 0 + ")").text(this.GetPosition());
            r.find("td:eq(" + 1 + ")").text(this.GetWord());
            r.find("td:eq(" + 2 + ")").text(this.totalPoints);
        } else {
            r.find("td:eq(" + 0 + ")").text(this.GetPosition());
            r.find("td:eq(" + 1 + ")").text(this.GetWord());
            r.find("td:eq(" + 2 + ")").text(0);
        }
    }

    CheckSolution() {
        var isValid = true;
        var positionValid = false;

        if (this.roundPosition == null || this.solution.length == 0) {
            return;
        }

        var mh = (this.controller.grid.board.SizeH - 1) / 2;
        var mv = (this.controller.grid.board.SizeV - 1) / 2;

        var position = { ... this.roundPosition };

        var wm = 1;
        var pivotTotal = 0;
        var total = 0;
        var tilesFromRack = 0;
        var points = 0;

        for (var x = 0; x < this.solution.length; x++) {
            var sq = this.solution[x];
            var cell = this.solutionCells[x];
            var cx = cell.cellIndex;
            var cy = cell.parentNode.rowIndex;

            if (this.controller.config.Round == 0 && cx == mh && cy == mv) {
                positionValid = true;
            }

            if (cx != position.x || cy != position.y) {
                isValid = false;
                break;
            }

            if (sq.Status == 0) {
                tilesFromRack++;
                wm *= sq.WordMultiplier;
                if (sq.PivotPoints[position.d] > 0) {
                    pivotTotal += (sq.Points[position.d] * sq.LetterMultiplier * sq.WordMultiplier) + sq.PivotPoints * sq.WordMultiplier;
                }
                points += sq.CurrentLetter.Points * sq.LetterMultiplier;
            } else {
                points += sq.CurrentLetter.Points;
            }

            if (position.d == 0)
                position.x++;
            else
                position.y++;
        }

        if (isValid && positionValid && tilesFromRack > 0) {
            this.totalPoints = points * wm + pivotTotal;
            this.totalPoints += this.controller.config.GameConfig.Bonus[tilesFromRack - 1];
            this.SetSolution(false, true);
        } else {
            this.SetSolution(false, false);
        }
    }

    Validate() {
        var s = {
            Squares: this.solution,
            Position: this.roundPosition,
            Points: this.totalPoints,
            FinalRound: true
        };
        sendPlaygroundController("Validate", s);
    }

    ClearSolution() {
        for (var x = 0; x < this.solution.length; x++) {
            var sq = this.solution[x];
            if (sq.Status != 1) {
                this.controller.rack.Add(sq.CurrentLetter);
            }
        }
        this.solution = [];
        this.solutionCells = [];
    }

    PlayLetter(cell, letter, joker, doOnlyFromBoard) {
        var byteLetter = letter == null ? "" : this.controller.letterConfig.GetLetterByte(letter.toUpperCase());
        var sq = $(cell).data("square");

        if (sq.Status == -1 && !doOnlyFromBoard) {
            var l = this.controller.rack.RemoveLetter(byteLetter, joker);
            if (l != null) {
                if (l.IsJoker) {
                    l.Letter = byteLetter;
                }

                sq.CurrentLetter = l;
                sq.Status = 0;
                $(cell).data("square", sq);
                this.controller.grid.SetLetter(cell)
            } else {
                return;
            }
        } else if (sq.Status != 1) {
            return;
        }

        var position = { ... this.controller.grid.GetPosition() };
        if (this.roundPosition == null) {
            this.roundPosition = position;
        }

        if (this.solution.length == 0 ||
            position.x < this.roundPosition.x ||
            position.y < this.roundPosition.y
        ) {
            this.roundPosition = position;
            this.solutionCells.unshift(cell);
            this.solution.unshift(sq);

            var newCell = this.controller.grid.MoveBackward();
            if (newCell != null) {
                this.PlayLetter(newCell, null, null, true);
            }
        } else {
            this.solution.push(sq);
            this.solutionCells.push(cell);
        }

        this.controller.grid.ResetPosition(null, position);
        this.controller.grid.MoveForward();
        this.controller.grid.ResetPosition(null, null);
        if (newCell != null) {
            this.PlayLetter(newCell, null, null, true);
        }
    }

    RemoveLastLetter() {
        var didFirst = false;
        var count = 0;
        while (this.solution.length > 0) {
            var c = this.solution.length - 1;
            var sq = this.solution[c];
            if (!didFirst && sq.Status != 1) {
                this.controller.rack.Add(sq.CurrentLetter);
                this.controller.grid.RemoveLetter(this.solutionCells[c])
                didFirst = true;
                count++;
            }
            else if (didFirst && sq.Status == 1) {
                count++;
            } else {
                break;
            }
            this.solution.pop();
            this.solutionCells.pop(sq);

        }

        while (count > 0) {
            this.controller.grid.MoveBackward();
            count--;
        }

        this.controller.grid.ResetPosition(null, null);
    }
}