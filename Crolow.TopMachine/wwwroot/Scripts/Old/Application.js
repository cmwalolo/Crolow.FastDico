

var gameController = null;
var myLayout = null; // a var is required because this page utilizes: myLayout.allowOverflow() method
var gameLayout = null;
var contentLayout = null;


$(document).ready(function () {
    // On crée les différents layouts de l'écran
    // Le layout principal. Consiste a l'en-tête, l'espace de jeu (Grille + Chevalet), et 
    // la partie droite
    myLayout = $('body').layout({
        resizeNestedLayout: true,
        west__showOverflowOnHover: true
		        , north__resizable: false	// OVERRIDE the pane-default of 'resizable=true'
		        , north__spacing_open: 0		// no resizer-bar when open (zero height)
		        , north__spacing_closed: 20
		        , south__resizable: false	// OVERRIDE the pane-default of 'resizable=true'
		        , south__spacing_open: 0		// no resizer-bar when open (zero height)
		        , south__spacing_closed: 20
                , center__minWidth: 500
                , east_resizable: true
		        , east__minSize: 500
		        , east__maxSize: Math.floor(screen.availWidth / 2) // 1/2 screen width
                , east__onresize: function () {
                    if (gameController != null) gameController.grid.resizeGrid();
                }
            });
    });

    $(document).ready(function () {


    // L'espace de jeu est divisé en 2 panneaux : La grille et le chevalet
     gameLayout = $('#playGround.ui-layout-center').layout({
        center__paneSelector: "#grid",
        south__paneSelector: "#rack",
        south__resizable: false,
        south__spacing_open: 0,
        south__minSize: 50,
        center__onresize: function () {
            if (gameController != null) gameController.grid.resizeGrid();
        }

    });

    // La partie de droite est divisé en 2 panneaux : Un qui contient le déroulement de la partie, 
    // Et l'autre les informations au coup par coup contenus dans des onglets
    contentLayout = $('div.ui-layout-east').layout({
        north__paneSelector: "#nav",
        center__paneSelector: "#tabs",
        north__resizable: false,
        north__spacing_open: 0,
        north__spacing_closed: 20,
        center__onresize: function () {
            if (gameController != null) gameController.grid.resizeGrid();
        }

    });

    // On crée les onglets de la partie droit de l'écran avec JQuery.UI.Tabs
    $("#tabs").tabs();

    // On crée un évenement lorsque la fenêtre du brower est redimensionnée
    // cela marche plus ou moins bine sauf quand on passe de la taille maximale de la fenête
    // a l'ancienne taille. Si vous regardez les layouts il y a aussi du code pour le redimensionnement
    // Mais le plugin n'est pas encore tout à fait au point.

    $(window).onresize = function () {
        if (gameController != null) {
            gameLayout.resizeAll();
            gameController.grid.resizeGrid();
        }
    };

    // On crée le controleur de jeu et on l'initialise
    // On peut imaginer que tous les fichiers HTML et JS soient cachables
    // Les paramètres de la partie se trouvant dans le querystring de l'url
    // On optimise le chargement du serveur, et seul les appels 
    // aux services ont un résultat dynamique...
    var game = getQuerystring("gameid", null);
    var mode = parseInt(getQuerystring("mode", 0));

    gameController = new GameControllerClass(mode, game);
    gameController.init();

    $(function () {
        // a workaround for a flaw in the demo system (http://dev.jqueryui.com/ticket/4375), ignore!
        $("#dialog:ui-dialog").dialog("destroy");

        $("#dialog-modal").dialog({
            height: 140,
            modal: true
        });
    });


});


function getQuerystring(key, def) {
    if (def == null) def = "";
    key = key.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regex = new RegExp("[\\?&]" + key + "=([^&#]*)");
    var qs = regex.exec(window.location.href);
    if (qs == null)
        return def;
    else
        return qs[1];
}

