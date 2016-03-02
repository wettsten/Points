'use strict';
app.directive('alertBox', function () {
    return {
        scope: false,
        templateUrl: '/app/views/directives/alertBox.html',
        controller: "alertBoxController"
    };
}).controller('alertBoxController', ['$scope', '$timeout', function ($scope, $timeout) {

    $scope.alerts = [];

    var addAlert = function (type, msg) {
        var alert = { type: type, msg: msg };
        $scope.alerts.push(alert);
        $timeout(function () {
            if ($scope.alerts.indexOf(alert) > -1) {
                $scope.alerts.splice($scope.alerts.indexOf(alert), 1);
            }
        }, 5000);
    };

    $scope.addSuccess = function (msg) {
        addAlert('success', msg);
    }

    $scope.addWarning = function (msg) {
        addAlert('warning', msg);
    }

    $scope.addError = function (msg) {
        addAlert('danger', msg);
    }

    $scope.closeAlert = function (alert) {
        if ($scope.alerts.indexOf(alert) > -1) {
            $scope.alerts.splice($scope.alerts.indexOf(alert), 1);
        }
    };
}]);