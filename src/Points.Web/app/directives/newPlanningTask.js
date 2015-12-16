'use strict';
app.directive('newPlanningTask', function () {
    return {
        scope: {
            planningTasks: '=tasks'
        },
        templateUrl: '/app/views/directives/newPlanningTask.html',
        replace: true,
        controller: 'newPlanningTaskController'
    };
}).controller('newPlanningTaskController', [
    '$scope', 'tasksService', 'catsService', 'planningTasksService', 'authService', function ($scope, tasksService, catsService, planningTasksService, authService) {
        
    $scope.addTaskData = {};
    $scope.cats = [];
    $scope.tasks = [];
    $scope.filteredTasks = [];
    $scope.enums = {};

    $scope.loadCats = function () {
        catsService.getCatsByUser(authService.authentication.userId).then(
            function (results) {
                $scope.cats = results.data;
                $scope.getEnums();
            }, function (err) {
                $scope.addAlert({ type: 'danger', msg: err.data.message });
        });
    };

    $scope.getEnums = function () {
        planningTasksService.getEnums().then(
            function (results) {
                $scope.enums = results.data;
                $scope.loadTasks();
            }, function (err) {
                $scope.addAlert({ type: 'danger', msg: err.data.message });
        });
    };

    $scope.loadTasks = function () {
        tasksService.getTasksByUser(authService.authentication.userId).then(
            function (results) {
                $scope.tasks = results.data.filter(
                    function (task) {
                        for (var i = 0; i < $scope.planningTasks.length; i++) {
                            if ($scope.planningTasks[i].name === task.name) {
                                return false;
                            }
                        }
                        return true;
                    });
                $scope.resetAddData();
            }, function (err) {
                $scope.addAlert({ type: 'danger', msg: err.data.message });
        });
    };

    $scope.filterTasks = function() {
        $scope.filteredTasks = $scope.tasks.filter(
            function (task) {
                return task.categoryId === $scope.addTaskData.cat.id;
        });
        $scope.addTaskData.task = $scope.filteredTasks[0];
    };

    $scope.resetAddData = function () {
        $scope.addTaskData.duration = {value:0};
        $scope.addTaskData.frequency = {value:1};
        $scope.addTaskData.cat = $scope.cats[0];
        $scope.addTaskData.dType = $scope.enums.dTypes[0];
        $scope.addTaskData.dUnit = $scope.enums.dUnits[0];
        $scope.addTaskData.fType = $scope.enums.fTypes[0];
        $scope.addTaskData.fUnit = $scope.enums.fUnits[0];
        $scope.filterTasks();
    };

    $scope.showAddDuration = function () {
        if (!$scope.addTaskData.dType) {
            return false;
        }
        return $scope.addTaskData.dType.id !== 'None';
    };

    $scope.showAddFrequency = function () {
        if (!$scope.addTaskData.fType) {
            return false;
        }
        return $scope.addTaskData.fType.id !== 'Once';
    };

    $scope.addTask = function () {
        $scope.addTaskData.name = $scope.addTaskData.task.name;
        $scope.addTaskData.categoryId = $scope.addTaskData.cat.id;
        $scope.addTaskData.duration.type = $scope.addTaskData.dType.id;
        $scope.addTaskData.duration.unit = $scope.addTaskData.dUnit.id;
        $scope.addTaskData.frequency.type = $scope.addTaskData.fType.id;
        $scope.addTaskData.frequency.unit = $scope.addTaskData.fUnit.id;
        $scope.addTaskData.isPrivate = true;
        $scope.addTaskData.userId = authService.authentication.userId;
        planningTasksService.addTask($scope.addTaskData).then(
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