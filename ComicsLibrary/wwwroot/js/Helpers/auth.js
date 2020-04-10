var mgr = null;

function authorize(onAuthorized) {

    $.get(app.authSettingsUrl(), function (data) {

        mgr = new Oidc.UserManager({
            authority: data.url,
            client_id: data.clientId,
            redirect_uri: app.callbackUrl(),
            response_type: data.responseType,
            scope: data.scope,
            post_logout_redirect_uri: app.baseUrl
        });

        mgr.getUser()
            .then(function (user) {
                if (user) {
                    onAuthorized(user);
                }
                else {
                    mgr.signinRedirect()
                        .catch(function (e) {
                            console.error(e);
                            index.loading(false);
                            alert("Failed to load sign-in page. Please try again.");
                        });
                }
            })
            .catch(function (e) {
                console.error(e);
            });
    });
}

function callback() {
    new Oidc.UserManager({ response_mode: "query" })
        .signinRedirectCallback()
        .then(function () {
            window.location = app.baseUrl;
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