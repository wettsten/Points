(function() {
    'use strict';

    angular
        .module('checkpoint')
        .directive('socialLogin', socialLogin);
    
    function socialLogin() {
        var directive = {
            restrict: 'EA',
            scope: {},
            templateUrl: '/app/areas/login/socialLogin.html',
            controller: 'socialLoginController',
            controllerAs: 'slVm',
            bindToController: true
        };
        return directive;
    }

    angular
        .module('checkpoint')
        .controller('socialLoginController', socialLoginController);

    socialLoginController.$inject = ['$scope', 'cpSettings', 'authService', 'authDataService', '$location'];

    function socialLoginController($scope, cpSettings, authService, authDataService, $location) {
        /* jshint validthis:true */
        var slVm = this;

        slVm.authExternalProvider = authExternalProvider;
        slVm.authCompletedCB = authCompletedCB;

        activate();

        function activate() { }

        function authExternalProvider (provider) {
            var redirectUri = location.protocol + '//' + location.host + '/authcomplete.html';

            var externalProviderUrl = cpSettings.apiServiceBaseUri + 'api/Account/ExternalLogin?provider=' + provider
                                                                        + '&response_type=token&client_id=' + cpSettings.clientId
                                                                        + '&redirect_uri=' + redirectUri;
            window.$windowScope = $scope;
            var oauthWindow = window.open(externalProviderUrl, 'Authenticate Account', 'location=0,status=0,width=600,height=750');
        }

        function authCompletedCB (fragment) {
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
                    authService
                        .obtainAccessToken(externalData)
                        .then(obtainTokenSuccess, obtainTokenError);
                }
            });
        }

        function obtainTokenSuccess(response) {
            $location.path('/active');
        }

        function obtainTokenError(err) {
            $scope.message = err.error_description;
        }
    }

})();