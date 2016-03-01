'use strict';
app.controller('signupController', ['$scope', '$location', '$timeout', 'authService', function ($scope, $location, $timeout, authService) {

    $scope.savedSuccessfully = false;
    $scope.message = "";

    $scope.registration = {
        userName: "",
        password: "",
        confirmPassword: ""
    };

    $scope.signUp = function () {
        authService.saveRegistration($scope.registration).then(
            function (response) {
                if (response.status === 202) {
                    $scope.savedSuccessfully = true;
                    $scope.message = "User has been registered successfully, you will be redicted to Login page in 2 seconds.";
                    $timeout(function () {
                        $location.path('/login');
                    }, 2000);
                } else {
                    $scope.savedSuccessfully = true;
                    $scope.message = "User has been registered successfully, you will be redicted to User Options page in 2 seconds.";
                    $timeout(function () {
                        $location.path('/options');
                    }, 2000);
                }
            },
            function (response) {
                var errors = [];
                for (var key in response.data.modelState) {
                    for (var i = 0; i < response.data.modelState[key].length; i++) {
                        errors.push(response.data.modelState[key][i]);
                    }
                }
                $scope.message = "Failed to register user due to: " + errors.join(' ');
            }
        );
    };
}]);