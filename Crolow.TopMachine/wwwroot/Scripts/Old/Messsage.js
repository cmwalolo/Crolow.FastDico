/* Messages used

    "proxy_init"
    "proxy_gameLoad"
    "proxy_rankingLoad"

*/
function Message(type, code, message, params) {
    var o = { typeMessage: type, code: code, message: message, params: params };
    return o;

}