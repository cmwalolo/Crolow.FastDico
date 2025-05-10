GameParameters = function () {
    this.loaded = 0;
    this.currentRound = 0;

    this.rounds = null;
    this.playerRounds = null; 

    this.gridConfig =
    {
        // Taille de la grille horizontale et verticale
        sizeH: 15,
        sizeV: 15,
        // Affichage des coordonnées, dans le scrabble francophone : A ... Z pour les lignes, 1....15 pour les colonnes
        coordH: new Array('A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O' /*, 'P', 'Q', 'R', 'S', 'T', 'U'*/),
        coordV: new Array('1', '2', '3', '4', '5', '6', '7', '8', '9', '10', '11', '12', '13', '14', '15' /*, '16', '17', '18', '19', '20', '21'*/),
        // Tableau de x*3 éléments : dont la suite correspond à la coordonnée de la grille X-Y, et un multiplicateur 
        // -2, -3 : Pour doubler ou tripler la letter
        // 2; 3   : Pour doubler ou tripler le mot
        // Par défaut chaque case n'a aucun multiplicateur...
        multipliers: new Array(
                0, 0, 3,
                0, 7, 3,
                0, 14, 3,
                7, 0, 3,
                7, 14, 3,
                14, 0, 3,
                14, 7, 3,
                14, 14, 3,
                1, 1, 2,
                2, 2, 2,
                3, 3, 2,
                4, 4, 2,
                7, 7, 2,
                1, 13, 2,
                2, 12, 2,
                3, 11, 2,
                4, 10, 2,
                13, 1, 2,
                12, 2, 2,
                11, 3, 2,
                10, 4, 2,
                13, 13, 2,
                12, 12, 2,
                11, 11, 2,
                10, 10, 2,
                0, 3, -2,
                0, 11, -2,
                2, 6, -2,
                2, 8, -2,
                3, 0, -2,
                3, 7, -2,
                3, 14, -2,
                6, 2, -2,
                6, 6, -2,
                6, 8, -2,
                6, 12, -2,
                7, 3, -2,
                7, 11, -2,
                14, 3, -2,
                14, 11, -2,
                12, 6, -2,
                12, 8, -2,
                11, 0, -2,
                11, 7, -2,
                11, 14, -2,
                8, 2, -2,
                8, 6, -2,
                8, 8, -2,
                8, 12, -2,
                7, 3, -2,
                7, 11, -2,
                1, 5, -3,
                   1, 9, -3,
                   5, 1, -3,
                   5, 5, -3,
                   5, 9, -3,
                   5, 13, -3,
                   13, 5, -3,
                   13, 9, -3,
                   9, 1, -3,
                   9, 5, -3,
                   9, 9, -3,
                   9, 13, -3)

    };


    // Cette propriété contiendra la configuration de la partie
    // lettersOnRack : Lettres dans le tirage
    // lettersPlayed : Nombre de lettres que l'on peut jouer
    // letters
    this.gameConfig = {
        lettersOnRack: 7,
        lettersPlayed: 7,
        isJoker: false,
        chrono: 90,
        // Un tableau d'objets avec 3 propriété : Lettre, Points, Total 
        // ------------
        // Exemple d'utilisation : 
        //         this.letters = new Array(
        //        { Letter: '?', Points: 0, Total: 2 },
        //        { Letter: 'A', Points: 1, Total: 9 },
        //        ); 
        // 
        // Le joker doit toujours être la première lettre.
        letters: new Array(
            { letter: '?', points: 0, total: 2 },
            { letter: 'A', points: 1, total: 9 },
            { letter: 'B', points: 3, total: 2 },
            { letter: 'C', points: 3, total: 2 },
            { letter: 'D', points: 2, total: 3 },
            { letter: 'E', points: 1, total: 15 },
            { letter: 'F', points: 4, total: 2 },
            { letter: 'G', points: 2, total: 2 },
            { letter: 'H', points: 4, total: 2 },
            { letter: 'I', points: 1, total: 8 },
            { letter: 'J', points: 8, total: 1 },
            { letter: 'K', points: 10, total: 1 },
            { letter: 'L', points: 1, total: 5 },
            { letter: 'M', points: 2, total: 3 },
            { letter: 'N', points: 1, total: 6 },
            { letter: 'O', points: 1, total: 6 },
            { letter: 'P', points: 3, total: 2 },
            { letter: 'Q', points: 8, total: 1 },
            { letter: 'R', points: 1, total: 6 },
            { letter: 'S', points: 1, total: 6 },
            { letter: 'T', points: 1, total: 6 },
            { letter: 'U', points: 1, total: 6 },
            { letter: 'V', points: 4, total: 2 },
            { letter: 'W', points: 10, total: 1 },
            { letter: 'X', points: 10, total: 1 },
            { letter: 'Y', points: 10, total: 1 },
            { letter: 'Z', points: 10, total: 1 }

        )
    };
}
