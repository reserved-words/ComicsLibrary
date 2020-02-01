var config = "";
var mgr = null;

function authorize(config) {

    var authConfig = {
        authority: config.authUrl,
        client_id: config.authClientId,
        redirect_uri: "http://localhost:54865/home/callback",
        response_type: config.authResponseType,
        scope: config.authScope,
        post_logout_redirect_uri: "http://localhost:54865/"
    };

    mgr = new Oidc.UserManager(authConfig);

    mgr.getUser().then(function (user) {
        if (user) {
            ko.applyBindings(index);
            index.loading(false);
            index.menuClick(index.menuItems[0], null);
        }
        else {
            mgr.signinRedirect();
        }
    });
}

function login() {
    mgr.signinRedirect();
}

function callback() {
    new Oidc.UserManager({ response_mode: "query" }).signinRedirectCallback().then(function () {
        window.location = "http://localhost:54865/";
    }).catch(function (e) {
        console.error(e);
    });
}

//function logout() {
//    mgr.signoutRedirect();

//    mgr.getUser().then(function (user) {
//        if (!user) {
//        }
//        else {
//            mainViewModel.loggedIn(false);
//        }
//    });
//}

//document.getElementById("login").addEventListener("click", login, false);
//document.getElementById("api").addEventListener("click", api, false);
//document.getElementById("logout").addEventListener("click", logout, false);



function login() {
    mgr.signinRedirect();
}

function logout() {
    mgr.signoutRedirect();
}

$(function () {
    //var rawFile = new XMLHttpRequest();
    //rawFile.open("GET", window.applicationBaseUrl + "authSettings.json", false);
    //rawFile.onreadystatechange = function () {
    //    if (rawFile.readyState === 4) {
    //        if (rawFile.status === 200 || rawFile.status == 0) {
    //            config = JSON.parse(rawFile.responseText);
    //            authorize(config);
    //        }
    //    }
    //}
    //rawFile.send(null);

    var config = {
        authUrl: "http://localhost:5000",
        authClientId: "ComicsLibrary",
        authResponseType: "code",
        authScope: "ComicsLibraryApi"
    };

    authorize(config);
});