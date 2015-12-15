'use strict';
app.directive('planningTask', function () {
    return {
        scope: {
            task: '=theTask',
            taskInEdit: '='
        },
        templateUrl: '/app/views/directives/planningTask.html',
        replace: true,
        controller: 'planningTaskController'
    };
}).controller('planningTaskController', ['$scope', 'planningTasksService', 'authService', '$uibModal', function ($scope, planningTasksService, authService, $uibModal) {

    $scope.editTask = {};
    $scope.enums = {};

    $scope.getEnums = function () {
        planningTasksService.getEnums().then(
            function (results) {
                $scope.enums = results.data;
                $scope.lookupDType();
                $scope.lookupDUnit();
                $scope.lookupFType();
                $scope.lookupFUnit();
            }, function (error) {
                //$scope.message = 'Error loading data';
            });
    };

    $scope.isInEditMode = function () {
        return $scope.taskInEdit.id === $scope.task.id;
    };

    $scope.$watch('taskInEdit.id', function () {
        if ($scope.taskInEdit.id !== '' && $scope.taskInEdit.id !== $scope.task.id) {
            $scope.editTask = {};
            $scope.editForm.$hide();
        }
    });

    $scope.dChanged = function(data) {
        if (data.id === 'None') {
            $scope.editForm.$editables[1].hide();
            $scope.editForm.$editables[2].hide();
        } else if ($scope.editTask.dType.id === 'None') {
            $scope.editForm.$editables[1].show();
            $scope.editForm.$editables[2].show();
        }
        $scope.editTask.dType = data;
    };

    $scope.fChanged = function (data) {
        if (data.id === 'Once') {
            $scope.editForm.$editables[4].hide();
            $scope.editForm.$editables[5].hide();
        } else if ($scope.editTask.fType.id === 'Once') {
            $scope.editForm.$editables[4].show();
            $scope.editForm.$editables[5].show();
        }
        $scope.editTask.fType = data;
    };

    $scope.ignoreDurationValueAndUnit = function () {
        if ($scope.isInEditMode()) {
            return $scope.editTask.dType.id === 'None';
        } else {
            return $scope.task.duration.type === 'None';
        }
    };

    $scope.ignoreFrequencyValueAndUnit = function () {
        if ($scope.isInEditMode()) {
            return $scope.editTask.fType.id === 'Once';
        } else {
            return $scope.task.frequency.type === 'Once';
        }
    };

    $scope.clearEditData = function () {
        $scope.editTask = {};
        $scope.taskInEdit.id = '';
        $scope.editForm.$hide();
    };

    $scope.startEdit = function () {
        $scope.editTask = angular.copy($scope.task);
        $scope.taskInEdit.id = $scope.task.id;
        $scope.editForm.$show();
        $scope.dChanged($scope.editTask.dType);
        $scope.fChanged($scope.editTask.fType);
    };

    $scope.saveEdit = function () {
        $scope.editForm.$submit();
        if ($scope.editForm.$dirty) {
            $scope.editForm.$show();
            return;
        }
        $scope.editTask.duration.type = $scope.editTask.dType.id;
        $scope.editTask.duration.unit = $scope.editTask.dUnit.id;
        $scope.editTask.frequency.type = $scope.editTask.fType.id;
        $scope.editTask.frequency.unit = $scope.editTask.fUnit.id;
        $scope.editTask.userId = authService.authentication.userId;
        planningTasksService.editTask($scope.editTask).then(
            function (response) {
                $scope.clearEditData();
                //$scope.$parent.loadTasks();
            },
            function (err) {
                //$scope.$parent.$parent.$parent.message = err.data.message;
                $scope.editForm.$show();
        });
    };

    $scope.validateName = function (data) {
        if (!data) {
            $scope.editForm.$setDirty();
            return "Name is required!";
        }
        $scope.editForm.$setPristine();
    };

    $scope.lookupDType = function () {
        for (var i = 0; i < $scope.enums.dTypes.length; i++) {
            if ($scope.enums.dTypes[i].id === $scope.task.duration.type) {
                $scope.task.dType = $scope.enums.dTypes[i];
                break;
            }
        }
    };

    $scope.lookupDUnit = function () {
        for (var i = 0; i < $scope.enums.dUnits.length; i++) {
            if ($scope.enums.dUnits[i].id === $scope.task.duration.unit) {
                $scope.task.dUnit = $scope.enums.dUnits[i];
                break;
            }
        }
    };

    $scope.lookupFType = function () {
        for (var i = 0; i < $scope.enums.fTypes.length; i++) {
            if ($scope.enums.fTypes[i].id === $scope.task.frequency.type) {
                $scope.task.fType = $scope.enums.fTypes[i];
                break;
            }
        }
    };

    $scope.lookupFUnit = function () {
        for (var i = 0; i < $scope.enums.fUnits.length; i++) {
            if ($scope.enums.fUnits[i].id === $scope.task.frequency.unit) {
                $scope.task.fUnit = $scope.enums.fUnits[i];
                break;
            }
        }
    };

    $scope.delete = function () {
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: '/app/views/partials/confirmDelete.html',
            controller: 'confirmDeleteController',
            size: 'sm',
            resolve: {
                item: function () {
                    return {
                        name: $scope.task.name,
                        id: $scope.task.id
                    };
                }
            }
        });

        modalInstance.result.then(
            function (result) {
                if (result !== 'cancel') {
                    planningTasksService.deleteTask($scope.task.id).then(
                        function (response) {
                            $scope.$parent.loadTasks();
                        },
                        function (err) {
                            $scope.$parent.$parent.$parent.message = err.data.message;
                        });
                }
        });
    };

    $scope.getEnums();
}]);