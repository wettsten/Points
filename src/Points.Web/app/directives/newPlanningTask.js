'use strict';
app.directive('newPlanningTask', function () {
    return {
        scope: {
            addAlert: '&'
        },
        templateUrl: '/app/views/directives/newPlanningTask.html',
        replace: true,
        controller: 'newPlanningTaskController'
    };
}).controller('newPlanningTaskController', [
    '$scope', 'tasksService', 'catsService', 'planningTasksService', function ($scope, tasksService, catsService, planningTasksService) {
        
    $scope.addTaskData = {};
    $scope.cats = [];
    $scope.tasks = [];
    $scope.filteredTasks = [];
    $scope.planningTasks = [];
    $scope.enums = {};

    $scope.loadCats = function () {
        catsService.getCats().then(
            function (results) {
                $scope.cats = results.data;
                $scope.getEnums();
            }, function (err) {
                $scope.addAlert({ type: 'danger', msg: err.statusMessage });
        });
    };

    $scope.getEnums = function () {
        planningTasksService.getEnums().then(
            function (results) {
                $scope.enums = results.data;
                $scope.loadTasks();
            }, function (err) {
                $scope.addAlert({ type: 'danger', msg: err.statusText });
        });
    };

    $scope.loadTasks = function () {
        tasksService.getTasks().then(
            function (results) {
                $scope.tasks = results.data;
                $scope.loadPlanningTasks();
            }, function (err) {
                $scope.addAlert({ type: 'danger', msg: err.statusText });
        });
    };

    $scope.loadPlanningTasks = function () {
        planningTasksService.getTasks().then(
            function (results) {
                $scope.planningTasks = results.data;
                $scope.resetAddData();
            }, function (err) {
                $scope.resetAddData();
            });
    };

    $scope.filterTasks = function () {
        $scope.filteredTasks = $scope.tasks.filter(
            function (task) {
                for (var i = 0; i < $scope.planningTasks.length; i++) {
                    if ($scope.planningTasks[i].task.id === task.id) {
                        return false;
                    }
                }
                return true;
            });
        $scope.filteredTasks = $scope.filteredTasks.filter(
            function (task) {
                return task.category.id === $scope.addTaskData.cat.id;
            });
        $scope.addTaskData.task = $scope.filteredTasks[0];
    };

    $scope.resetAddData = function () {
        $scope.addTaskData.cat = $scope.cats[0];
        $scope.addTaskData.duration = {value:0};
        $scope.addTaskData.frequency = {value:1};
        $scope.addTaskData.duration.type = $scope.enums.dTypes[0];
        $scope.addTaskData.duration.unit = $scope.enums.dUnits[0];
        $scope.addTaskData.frequency.type = $scope.enums.fTypes[0];
        $scope.addTaskData.frequency.unit = $scope.enums.fUnits[0];
        $scope.filterTasks();
    };

    $scope.showAddDuration = function () {
        if (!$scope.addTaskData.duration.type) {
            return false;
        }
        return $scope.addTaskData.duration.type.id !== 'None';
    };

    $scope.showAddFrequency = function () {
        if (!$scope.addTaskData.frequency.type) {
            return false;
        }
        return $scope.addTaskData.frequency.type.id !== 'Once';
    };

    $scope.addTask = function () {
        var aTask = angular.copy($scope.addTaskData);
        aTask.name = $scope.addTaskData.task.id;
        aTask.taskId = $scope.addTaskData.task.id;
        aTask.duration.type = $scope.addTaskData.duration.type.id;
        aTask.duration.unit = $scope.addTaskData.duration.unit.id;
        aTask.frequency.type = $scope.addTaskData.frequency.type.id;
        aTask.frequency.unit = $scope.addTaskData.frequency.unit.id;
        aTask.isPrivate = true;
        planningTasksService.addTask(aTask).then(
            function (response) {
                $scope.$emit('refreshTasks');
                $scope.resetAddData();
                $scope.addAlert({ type: 'success', msg: 'Task successfully added' });
            },
            function (err) {
                $scope.addAlert({ type: 'danger', msg: err.data.message });
         });
    };

    $scope.loadCats();
}]);