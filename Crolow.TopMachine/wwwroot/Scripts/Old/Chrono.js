ChronoClass = function (chronoId, maxTime) {
    this.timerCount = null;
    this.timeStart = null;
    // SPAN qui contient le chrono a afficher
    this.chronoId = chronoId;
    // Le temps maximal a afficher
    this.maxTime = maxTime;
}

ChronoClass.prototype = {
    /*
    // Affiche l'état du chrono 
    */
    showTimer: function () {
        
        if (this.timerCount) {
            clearTimeout(this.timerCount);
            clockID = 0;
        }

        if (!this.timeStart) {
            this.timeStart = new Date();
        }

        var t = this.getCurrent();
        var mp = t.minutes;
        var sp = t.seconds;

        var total = mp * 60 + sp;
        var end = false;

        // On regarde si on dépasse pas le temps maximal
        if (total > this.maxTime) {
            mp = Math.floor(this.maxTime / 60);
            sp = this.maxTime % 60;
            end = true;
        }

        // On formate MM:SS
        if (mp < 10) {
            mp = "0" + mp;
        }

        if (sp < 10) {
            sp = "0" + sp;
        }

        $(this.chronoId).text(mp + ":" + sp);

        // On crée un appel récursif dans 1 seconde 
        if (!end) {
            var self = this;
            this.timerCount = setTimeout(function () { self.showTimer(); }, 1000);
        }
    },
    // on démarre le chrono
    start: function (maxTime) {
        if (maxTime != 0) {
            this.maxTime = maxTime;
        }
        var self = this;
        this.timeStart = new Date();
        $(this.chronoId).text("00:00");
        this.timerCount = setTimeout(function () { self.showTimer(); }, 1000);
    },
    // On calcule le temps parcouru actuel
    // et on retourne un structure
    // minutes,seconds, milli
    getCurrent: function () {
        var timeend = new Date();
        var timedifference = timeend.getTime() - this.timeStart.getTime();
        timeend.setTime(timedifference);
        var mp = timeend.getMinutes();
        var sp = timeend.getSeconds();
        var millisp = timeend.getMilliseconds();


        return { minutes: mp, seconds: sp, milli: millisp };
    },
    // on arrête le chrono
    stop: function () {
        if (this.timerCount) {
            clearTimeout(this.timerCount);
        }
        this.timeStart = 0;
    },
    // On remet le chrono à zéro
    reset: function () {
        this.timeStart = 0;
        $(this.chronoId).text("00:00");
    }
}
