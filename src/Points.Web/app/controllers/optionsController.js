'use strict';
app.controller('optionsController', ['$scope', 'authService', 'usersService', function ($scope, authService, usersService) {

    $scope.originalUser = {};
    $scope.user = {};
    $scope.message = '';
    $scope.days = [
        'Sunday',
        'Monday',
        'Tuesday',
        'Wednesday',
        'Thursday',
        'Friday',
        'Saturday'
    ];
    $scope.hours = [
        {
            text: '12 am',
            hour: 0
        },
        {
            text: '1 am',
            hour: 1
        },
        {
            text: '2 am',
            hour: 2
        },
        {
            text: '3 am',
            hour: 3
        },
        {
            text: '4 am',
            hour: 4
        },
        {
            text: '5 am',
            hour: 5
        },
        {
            text: '6 am',
            hour: 6
        },
        {
            text: '7 am',
            hour: 7
        },
        {
            text: '8 am',
            hour: 8
        },
        {
            text: '9 am',
            hour: 9
        },
        {
            text: '10 am',
            hour: 10
        },
        {
            text: '11 am',
            hour: 11
        },
        {
            text: '12 pm',
            hour: 12
        },
        {
            text: '1 pm',
            hour: 13
        },
        {
            text: '2 pm',
            hour: 14
        },
        {
            text: '3 pm',
            hour: 15
        },
        {
            text: '4 pm',
            hour: 16
        },
        {
            text: '5 pm',
            hour: 17
        },
        {
            text: '6 pm',
            hour: 18
        },
        {
            text: '7 pm',
            hour: 19
        },
        {
            text: '8 pm',
            hour: 20
        },
        {
            text: '9 pm',
            hour: 21
        },
        {
            text: '10 pm',
            hour: 22
        },
        {
            text: '11 pm',
            hour: 23
        }
    ];

    $scope.loadUser = function() {
        usersService.getUserByName(authService.authentication.userName).then(
            function (response) {
                $scope.user = response.data[0];
                $scope.lookupHour();
                $scope.originalUser = angular.copy($scope.user);
            },
            function(err) {
                $scope.message = err.data.message;
            });
    };

    $scope.lookupHour = function() {
        for (var i = 0; i < $scope.hours.length; i++) {
            if ($scope.user.weekStartHour === $scope.hours[i].hour) {
                $scope.user.startHour = $scope.hours[i];
                break;
            }
        }
    };

    $scope.areNoChanges = function() {
        return angular.equals($scope.user, $scope.originalUser);
    };

    $scope.cancelChanges = function() {
        $scope.user = angular.copy($scope.originalUser);
    };

    $scope.saveChanges = function () {
        $scope.user.weekStartHour = $scope.user.startHour.hour;
        usersService.editUser($scope.user).then(
                function (response) {
                    $scope.originalUser = angular.copy($scope.user);
                },
                function (err) {
                    $scope.message = err.data.message;
                });
    };

    $scope.loadUser();
}]);