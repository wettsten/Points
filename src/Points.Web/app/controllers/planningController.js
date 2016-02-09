'use strict';
app.controller('planningController', ['$scope', 'resourceService', '$timeout', function ($scope, resourceService, $timeout) {

    $scope.cats = [];
    $scope.alerts = [];
    $scope.taskInEdit = { id: '' };

    var setupCats = function() {
        for (var i = 0; i < $scope.cats.length; i++) {
            $scope.cats[i].isOpen = i === 0;
        }
    };

    var loadCats = function () {
        $scope.cats = resourceService.get('planningtasks');
        setupCats();
        $timeout(function () {
            if ($scope.cats.length === 0) {
                $scope.addAlert('warning', 'No planning tasks found');
            }
        }, 1000);
    };

    resourceService.registerForUpdates('planningtasks', function (data) {
        $scope.cats = data;
        setupCats();
    });

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

    loadCats();
}]);