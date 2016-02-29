'use strict';
app.directive('socialLogin', function() {
    return {
        scope: {
        },
        templateUrl: '/app/views/partials/socialLogin.html',
        replace: true,
        controller: 'socialLoginController'
    };
}).controller('socialLoginController', ['$scope', 'ngAuthSettings', 'authService', 'authDataService', '$location', function ($scope, ngAuthSettings, authService, authDataService, $location) {
    
    $scope.authExternalProvider = function (provider) {
        var redirectUri = location.protocol + '//' + location.host + '/authcomplete.html';

        var externalProviderUrl = ngAuthSettings.apiServiceBaseUri + "api/Account/ExternalLogin?provider=" + provider
                                                                    + "&response_type=token&client_id=" + ngAuthSettings.clientId
                                                                    + "&redirect_uri=" + redirectUri;
        window.$windowScope = $scope;
        var oauthWindow = window.open(externalProviderUrl, "Authenticate Account", "location=0,status=0,width=600,height=750");
    };

    $scope.authCompletedCB = function (fragment) {
        $scope.$apply(function () {
            if (fragment.haslocalaccount == 'False') {
                authService.logOut();
                authDataService.externalAuthData = {
                    provider: fragment.provider,
                    userName: fragment.external_user_name,
                    externalAccessToken: fragment.external_access_token
                };
                $location.path('/associate');
            }
            else {
                //Obtain access token and redirect to active week
                var externalData = { provider: fragment.provider, externalAccessToken: fragment.external_access_token };
                authService.obtainAccessToken(externalData).then(
                    function (response) {
                        $location.path('/active');
                    },
                    function (err) {
                        $scope.message = err.error_description;
                    }
                );
            }
        });
    }
}]);