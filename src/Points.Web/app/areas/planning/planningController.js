'use strict';
app.controller('planningController', ['$scope', 'resourceService', '$timeout', 'modalService', function ($scope, resourceService, $timeout, modalService) {

    $scope.tasks = [];
    $scope.availableTasks = false;
    $scope.alerts = [];
    $scope.taskInEdit = { id: '' };

    var setupCats = function() {
        for (var i = 0; i < $scope.tasks.length; i++) {
            $scope.tasks[i].isOpen = i === 0;
        }
    };

    var loadCats = function () {
        resourceService.get('planningtasks');
        resourceService.get('availabletasks');
    };

    resourceService.registerForUpdates('planningtasks', function (data) {
        $scope.tasks = data;
        if ($scope.tasks.length === 0) {
            $scope.addAlert('warning', 'No planning tasks found');
        }
        setupCats();
    });

    resourceService.registerForUpdates('availabletasks', function (data) {
        $scope.availableTasks = data.length > 0;
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

    $scope.addTask = function () {
        modalService.newModal('newPlanningTask', null, 'lg',
            function () {
                $scope.addAlert({ type: 'success', msg: 'Task successfully added' });
            }
        );
    };

    loadCats();
}]);