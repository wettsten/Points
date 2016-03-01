'use strict';
app.controller('optionsController', ['$scope', 'authService', 'resourceService', function ($scope, authService, resourceService) {

    $scope.originalUser = {};
    $scope.user = {};
    $scope.days = [];
    $scope.hours = [];
    $scope.hoursPrior = [];
    var offset = new Date().getTimezoneOffset() / 60;
    var dataLoaded = false,
        userLoaded = false;

    var convertToLocal = function (user) {
        if (user.weekStartHour.id - offset < 0) {
            user.weekStartHour = $scope.hours[user.weekStartHour.id - offset + 24];
            user.weekStartDay = $scope.days[$scope.days.indexOf(user.weekStartDay) - 1];
        } else if (user.weekStartHour.id - offset > 23) {
            user.weekStartHour = $scope.hours[user.weekStartHour.id - offset - 24];
            user.weekStartDay = $scope.days[$scope.days.indexOf(user.weekStartDay) + 1];
        } else {
            user.weekStartHour = $scope.hours[user.weekStartHour.id - offset];
        }
        return user;
    };

    var convertToUtc = function (user) {
        if (user.weekStartHour.id + offset < 0) {
            user.weekStartHour = $scope.hours[user.weekStartHour.id + offset + 24];
            user.weekStartDay = $scope.days[$scope.days.indexOf(user.weekStartDay) - 1];
        } else if (user.weekStartHour.id + offset > 23) {
            user.weekStartHour = $scope.hours[user.weekStartHour.id + offset - 24];
            user.weekStartDay = $scope.days[$scope.days.indexOf(user.weekStartDay) + 1];
        } else {
            user.weekStartHour = $scope.hours[user.weekStartHour.id + offset];
        }
        return user;
    };

    var initData = function () {
        if (dataLoaded === true && userLoaded === true) {
            $scope.user = convertToLocal(angular.copy($scope.originalUser));
            $scope.user.notifyWeekStarting = $scope.hoursPrior[$scope.user.notifyWeekStarting.id];
            $scope.user.notifyWeekEnding = $scope.hoursPrior[$scope.user.notifyWeekEnding.id];
            $scope.originalUser = angular.copy($scope.user);
        }
    };

    resourceService.get('users/data', function (data) {
        $scope.days = data.days;
        $scope.hours = data.hours;
        $scope.hoursPrior = data.hoursPrior;
        dataLoaded = true;
        initData();
    });

    var loadData = function() {
        resourceService.get('users', function (data) {
            $scope.originalUser = data[0];
            userLoaded = true;
            initData();
        });
    };

    $scope.areNoChanges = function() {
        return angular.equals($scope.user, $scope.originalUser);
    };

    $scope.cancelChanges = function() {
        $scope.user = angular.copy($scope.originalUser);
    };

    $scope.saveChanges = function () {
        if ($scope.validateEmail()) {
            var editUser = convertToUtc(angular.copy($scope.user));
            resourceService.edit('users', editUser).then(
                function() {
                    resourceService.get('users');
                    $scope.addSuccess('Options successfully updated');
                },
                function(err) {
                    $scope.addError(err.data.message);
                });
        }
    };

    $scope.validateEmail = function() {
        if (($scope.user.notifyWeekStarting.id > 0 || $scope.user.notifyWeekEnding.id > 0 || $scope.user.weekSummaryEmail === true) && !$scope.user.email) {
            $scope.addError('Email is required for notifications');
            return false;
        }
        return true;
    };

    loadData();
}]);