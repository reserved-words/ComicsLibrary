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

    console.log(JSON.stringify(authConfig));

    mgr = new Oidc.UserManager(authConfig);

    mgr.getUser().then(function (user) {
        if (user) {
            alert("User logged in");
        }
        else {
            alert("User not logged in");
            mgr.signinRedirect();
        }
    });
}

function login() {
    mgr.signinRedirect();
}

function callback() {
    new Oidc.UserManager({ response_mode: "query" }).signinRedirectCallback().then(function () {
        window.location = window.applicationBaseUrl;
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

function api() {
    //mgr.getUser().then(function (user) {
    //    var url = "http://localhost:58281/identity";

    //    var xhr = new XMLHttpRequest();
    //    xhr.open("GET", url);
    //    xhr.onload = function () {
    //        log(xhr.status, JSON.parse(xhr.responseText));
    //    }
    //    xhr.setRequestHeader("Authorization", "Bearer " + user.access_token);
    //    xhr.send();
    //});
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