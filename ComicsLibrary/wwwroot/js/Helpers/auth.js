var config = "";
var mgr = null;
var appUrl = $("#appBaseUrl").data("stuff-url");

function authorize2(onAuthorized) {

    var authConfig = {
        authority: config.authUrl,
        client_id: config.authClientId,
        redirect_uri: appUrl + "home/callback",
        response_type: config.authResponseType,
        scope: config.authScope,
        post_logout_redirect_uri: appUrl
    };

    mgr = new Oidc.UserManager(authConfig);

    mgr.getUser().then(function (user) {
        if (user) {
            onAuthorized();
        }
        else {
            mgr.signinRedirect();
        }
    });
}

function callback() {
    new Oidc.UserManager({ response_mode: "query" }).signinRedirectCallback().then(function () {
        console.log(appUrl);
        window.location = appUrl;
    }).catch(function (e) {
        console.error(e);
    });
}

//function login() {
//    mgr.signinRedirect();
//}

//function logout() {
//    mgr.signoutRedirect();
//}

function authorize(onAuthorized) {
    settingsPath = appUrl + "authSettings.json";
    AJAX.getContent(settingsPath, function (content) {
        config = content;
        authorize2(onAuthorized);
    });
}