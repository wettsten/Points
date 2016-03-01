'use strict';
app.controller('associateController', ['$scope', '$location','$timeout','authService', function ($scope, $location,$timeout, authService) {

    $scope.savedSuccessfully = false;
    $scope.message = "";

    $scope.registerData = {
        userName: authService.externalAuthData.userName,
        provider: authService.externalAuthData.provider,
        externalAccessToken: authService.externalAuthData.externalAccessToken
    };

    $scope.registerExternal = function () {
        authService.registerExternal($scope.registerData).then(
            function (response) {
                $scope.savedSuccessfully = true;
                $scope.message = "User has been registered successfully, you will be redicted to User Options page in 2 seconds.";
                $timeout(function () {
                    $location.path('/options');
                }, 2000);
            },
            function (response) {
                var errors = [];
                for (var key in response.modelState) {
                    errors.push(response.modelState[key]);
                }
                $scope.message = "Failed to register user due to:" + errors.join(' ');
            }
        );
    };
}]);