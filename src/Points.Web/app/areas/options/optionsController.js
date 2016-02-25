'use strict';
app.controller('optionsController', ['$scope', 'authService', 'resourceService', function ($scope, authService, resourceService) {

    $scope.originalUser = {};
    $scope.user = {};
    $scope.days = [];
    $scope.hours = [];
    $scope.hoursPrior = [];
    var offset = new Date().getTimezoneOffset() / 60;

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

    resourceService.get('users/days', function (data) {
        $scope.days = data;
    });
    resourceService.get('users/hours', function (data) {
        $scope.hours = data;
    });
    resourceService.get('users/hoursprior', function (data) {
        $scope.hoursPrior = data;
    });
    var loadData = function() {
        resourceService.get('users', function (data) {
            $scope.user = convertToLocal(angular.copy(data[0]));
            $scope.user.notifyWeekStarting = $scope.hoursPrior[$scope.user.notifyWeekStarting.id];
            $scope.user.notifyWeekEnding = $scope.hoursPrior[$scope.user.notifyWeekEnding.id];
            $scope.originalUser = angular.copy($scope.user);
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
            var editUser = angular.copy($scope.user);
            editUser = convertToUtc(editUser);
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