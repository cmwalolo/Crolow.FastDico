class LetterConfig {
    constructor(gameController) {
        this.controller = gameController;
        this.letterConfig = gameController.config.LetterConfig;
    }
    GetWord(letters) {
        var word = "";
        letters.forEach(letter => {
            result = this.letterConfig.Letters.find(item => item.Letter === letter.Letter);
            if (result != undefined) {
                word += letter.IsJoker ? result.Char.toLowerCase() : result.Char;
            }
        });
        return word;
    }

    GetRackLetter(letter) {
        // Process each letter
        if (letter.IsJoker) return "?";
        const result = this.letterConfig.Letters.find(item => item.Letter === letter.Letter);
        if (result != undefined) {
            return result.Char;
        }
        return "";
    }

}