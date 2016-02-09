'use strict';
app.controller('activeController', ['$scope', 'resourceService', '$timeout', function ($scope, resourceService, $timeout) {

    $scope.tasks = [];
    $scope.cats = [];
    $scope.alerts = [];
    $scope.filteredCats = [];
    $scope.taskInEdit = { id: '' };

    var setupCats = function () {
        for (var i = 0; i < $scope.cats.length; i++) {
            $scope.cats[i].isOpen = i === 0;
            for (var j = 0; j < $scope.cats[i].tasks.length; j++) {
                $scope.cats[i].tasks[j].isOpen = true;
                $scope.cats[i].tasks[j].details = { isOpen: false };
            }
        }
    };

    var loadCats = function () {
        $scope.cats = resourceService.get('activetasks');
        setupCats();
        $timeout(function () {
            if ($scope.cats.length === 0) {
                $scope.addAlert('warning', 'No active tasks found');
            }
        }, 1000);
    };

    resourceService.registerForUpdates('activetasks', function (data) {
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