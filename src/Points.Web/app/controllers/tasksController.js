'use strict';
app.controller('tasksController', ['$scope', 'resourceService', 'filterFactory', '$timeout', function ($scope, resourceService, filterFactory, $timeout) {

    $scope.tasks = [];
    $scope.alerts = [];
    $scope.taskInEdit = { id: '' };
    $scope.taskFilter = filterFactory.getTaskFilter();

    filterFactory.subscribe($scope, 'taskFilter', function taskFilterChanged() {
        $scope.taskFilter = filterFactory.getTaskFilter();
    });

    var loadTasks = function () {
        $scope.tasks = resourceService.get('tasks');
        $timeout(function() {
            if ($scope.tasks.length === 0) {
                $scope.addAlert('warning', 'No tasks found');
            }
        }, 1000);
    };

    resourceService.registerForUpdates('tasks', function (data) {
        $scope.tasks = data;
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

    loadTasks();
}]);