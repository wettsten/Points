'use strict';
app.controller('newTaskController', [
    '$scope', 'tasksService', 'catsService', 'authService', function($scope, tasksService, catsService, authService) {
        
    $scope.selectedAdd = {};
    $scope.addTaskData = {
        duration: {},
        frequency: {}
    };

    $scope.showAddDuration = function () {
        if (!$scope.selectedAdd.dType) {
            return false;
        }
        return $scope.selectedAdd.dType.id !== 'None';
    };

    $scope.showAddFrequency = function () {
        if (!$scope.selectedAdd.fType) {
            return false;
        }
        return $scope.selectedAdd.fType.id !== 'Once';
    };

    $scope.clearAddData = function () {
        $scope.addTaskData = {
            duration: {
                value: 0
            },
            frequency: {
                value: 0
            }
        };
    };

    $scope.addTask = function () {
        $scope.addTaskData.categoryId = $scope.selectedAdd.cat.id;
        $scope.addTaskData.duration.type = $scope.selectedAdd.dType.id;
        $scope.addTaskData.duration.unit = $scope.selectedAdd.dUnit.id;
        $scope.addTaskData.frequency.type = $scope.selectedAdd.fType.id;
        $scope.addTaskData.frequency.unit = $scope.selectedAdd.fUnit.id;
        $scope.addTaskData.userId = authService.authentication.userId;
        tasksService.addTask($scope.addTaskData).then(function (response) {
            $scope.loadTasks();
                $scope.clearAddData();
            },
         function (err) {
             $scope.$parent.message = err.data.message;
         });
    };

    $scope.$on('catsLoaded', function (event,cats) {
        $scope.selectedAdd.cat = cats[0];
    });

    $scope.$on('enumsLoaded', function (event,enums) {
        $scope.selectedAdd.dType = enums.dTypes[0];
        $scope.selectedAdd.dUnit = enums.dUnits[0];
        $scope.selectedAdd.fType = enums.fTypes[0];
        $scope.selectedAdd.fUnit = enums.fUnits[0];
    });

    $scope.clearAddData();
}]);