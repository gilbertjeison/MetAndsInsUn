import Oidc from 'oidc-client';


var authority_dev = 'http://localhost:5093/';
var authority_loc = 'http://localhost/identity';
var authority_prod = 'http://appmtto7-001-site4.itempurl.com/';

var redirect_dev = 'http://localhost:5000/callback';
var redirect_loc = 'http://localhost/metins/callback';
var redirect_prod = 'http://metinsman.digitoolsunilever.com/callback';

var post_logout_dev = 'http://localhost:5000/main';
var post_logout_loc = 'http://localhost/metins/main';
var post_logout_prod = 'http://metinsman.digitoolsunilever.com/main';

var silent_redirect_dev = 'http://localhost:5000/static/silent-renew.html';
var silent_redirect_loc = 'http://localhost/metins/static/silent-renew.html';
var silent_redirect_prod = 'http://metinsman.digitoolsunilever.com/silent-renew.html';

// var mgr = new Oidc.UserManager({
//     authority: authority_prod,
//     client_id: 'js',
//     redirect_uri: redirect_prod,
//     response_type: 'id_token token',
//     scope: 'openid profile api1',
//     post_logout_redirect_uri: post_logout_prod,
//     userStore: new Oidc.WebStorageStateStore({store: window.localStorage}),
//     automaticSilentRenew: true,
//     silent_redirect_uri: silent_redirect_prod,
//     accessTokenExpiringNotificationTime: 100,
// });

var mgr = new Oidc.UserManager({
    authority: authority_loc,
    client_id: 'js',
    redirect_uri: redirect_loc,
    response_type: 'id_token token',
    scope: 'openid profile api1',
    post_logout_redirect_uri: post_logout_loc,
    userStore: new Oidc.WebStorageStateStore({store: window.localStorage}),
    automaticSilentRenew: true,
    silent_redirect_uri: silent_redirect_loc,
    accessTokenExpiringNotificationTime: 100,
});

// var mgr = new Oidc.UserManager({
//     authority: authority_dev,
//     client_id: 'js',
//     redirect_uri: redirect_dev,
//     response_type: 'id_token token',
//     scope: 'openid profile api1',
//     post_logout_redirect_uri: post_logout_dev,
//     userStore: new Oidc.WebStorageStateStore({store: window.localStorage}),
//     automaticSilentRenew: true,
//     silent_redirect_uri: silent_redirect_dev,
//     accessTokenExpiringNotificationTime: 100,
// });

// Oidc.Log.logger = console;
// Oidc.Log.level = Oidc.Log.INFO;

export default mgr;