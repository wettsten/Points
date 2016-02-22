'use strict';
app.controller('tasksController', ['$scope', 'resourceService', 'filterFactory', '$timeout', function ($scope, resourceService, filterFactory, $timeout) {

    $scope.tasks = [];
    $scope.alerts = [];
    $scope.taskInEdit = { id: '' };
    $scope.taskFilter = filterFactory.getTaskFilter();

    filterFactory.subscribe($scope, 'taskFilter', function taskFilterChanged() {
        $scope.taskFilter = filterFactory.getTaskFilter();
    });

    $scope.loadTasks = function () {
        resourceService.get('tasks');
    };

    resourceService.subscribe('tasks', function (data) {
        $scope.tasks = data;
        if ($scope.tasks.length === 0) {
            $scope.addWarning('No tasks found');
        }
    });

    $scope.loadTasks();
}]);