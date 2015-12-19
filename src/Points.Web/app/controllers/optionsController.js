'use strict';
app.controller('optionsController', ['$scope', 'authService', 'usersService', '$timeout', '$q', function ($scope, authService, usersService, $timeout, $q) {

    $scope.originalUser = {};
    $scope.user = {};
    $scope.alerts = [];
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

    $scope.loadUser = usersService.getUserByName(authService.authentication.userName).then(
            function (response) {
                return response.data;
            });

    $scope.loadData = function () {
        $q.all([$scope.loadUser]).then(
            function (data) {
                $scope.user = data[0];
                $scope.convertToLocal();
                $scope.user.startHour = $scope.hours[$scope.user.offsetHour];
                $scope.originalUser = angular.copy($scope.user);
            });
    };

    $scope.convertToLocal = function () {
        var offset = new Date().getTimezoneOffset() / 60;
        if ($scope.user.weekStartHour - offset < 0) {
            $scope.user.offsetHour = $scope.user.weekStartHour - offset + 24;
            $scope.user.offsetDay = $scope.days[$scope.days.indexOf($scope.user.weekStartDay) - 1];
        } else if ($scope.user.weekStartHour - offset > 23) {
            $scope.user.offsetHour = $scope.user.weekStartHour - offset - 24;
            $scope.user.offsetDay = $scope.days[$scope.days.indexOf($scope.user.weekStartDay) + 1];
        } else {
            $scope.user.offsetHour = $scope.user.weekStartHour - offset;
            $scope.user.offsetDay = $scope.user.weekStartDay;
        }
    };

    $scope.convertToUtc = function () {
        var offset = new Date().getTimezoneOffset() / 60;
        if ($scope.user.offsetHour + offset < 0) {
            $scope.user.weekStartHour = $scope.user.offsetHour + offset + 24;
            $scope.user.weekStartDay = $scope.days[$scope.days.indexOf($scope.user.offsetDay) - 1];
        } else if ($scope.user.offsetHour + offset > 23) {
            $scope.user.weekStartHour = $scope.user.offsetHour + offset - 24;
            $scope.user.weekStartDay = $scope.days[$scope.days.indexOf($scope.user.offsetDay) + 1];
        } else {
            $scope.user.weekStartHour = $scope.user.offsetHour + offset;
            $scope.user.weekStartDay = $scope.user.offsetDay;
        }
    };

    $scope.areNoChanges = function() {
        return angular.equals($scope.user, $scope.originalUser);
    };

    $scope.cancelChanges = function() {
        $scope.user = angular.copy($scope.originalUser);
    };

    $scope.saveChanges = function () {
        if ($scope.validateEmail()) {
            $scope.user.offsetHour = $scope.user.startHour.hour;
            $scope.convertToUtc();
            usersService.editUser($scope.user).then(
                function(response) {
                    $scope.originalUser = angular.copy($scope.user);
                    $scope.addAlert('success', 'Options successfully updated');
                },
                function(err) {
                    $scope.addAlert('danger', err.data.message);
                });
        }
    };

    $scope.validateEmail = function() {
        if (($scope.user.notifyWeekStarting || $scope.user.notifyWeekEnding) && !$scope.user.email) {
            $scope.addAlert('danger', 'Email is required if notifications are desired');
            return false;
        }
        return true;
    };

    $scope.addAlert = function (type, msg) {
        var alert = { type: type, msg: msg };
        $scope.alerts.push(alert);
        $timeout(function () {
            if ($scope.alerts.indexOf(alert) > -1) {
                $scope.alerts.splice($scope.alerts.indexOf(alert), 1);
            }
        }, 5000);
    };

    $scope.closeAlert = function (alert) {
        if ($scope.alerts.indexOf(alert) > -1) {
            $scope.alerts.splice($scope.alerts.indexOf(alert), 1);
        }
    };

    $scope.loadData();
}]);