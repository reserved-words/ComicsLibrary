//var config = "";
//var mgr = null;
//var appUrl = window.location.protocol + "//" + window.location.host + $("#appBaseUrl").data("stuff-url");

//function authorize(config) {

//    var authConfig = {
//        authority: config.authUrl,
//        client_id: config.authClientId,
//        redirect_uri: appUrl + "home/callback",
//        response_type: config.authResponseType,
//        scope: config.authScope,
//        post_logout_redirect_uri: appUrl
//    };

//    mgr = new Oidc.UserManager(authConfig);

//    mgr.getUser().then(function (user) {
//        if (user) {
//            ko.applyBindings(index);
//            index.loading(false);
//            index.menuClick(index.pages[0], null);
//        }
//        else {
//            mgr.signinRedirect();
//        }
//    });
//}

//function login() {
//    mgr.signinRedirect();
//}

//function callback() {
//    new Oidc.UserManager({ response_mode: "query" }).signinRedirectCallback().then(function () {
//        window.location = appUrl;
//    }).catch(function (e) {
//        console.error(e);
//    });
//}

//function login() {
//    mgr.signinRedirect();
//}

//function logout() {
//    mgr.signoutRedirect();
//}

//$(function () {
//    var rawFile = new XMLHttpRequest();
//    rawFile.open("GET", appUrl + "authSettings.json", false);
//    rawFile.onreadystatechange = function () {
//        if (rawFile.readyState === 4) {
//            if (rawFile.status === 200 || rawFile.status == 0) {
//                config = JSON.parse(rawFile.responseText);
//                authorize(config);
//            }
//        }
//    }
//    rawFile.send(null);
//});