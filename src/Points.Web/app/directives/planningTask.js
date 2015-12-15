'use strict';
app.directive('planningTask', function () {
    return {
        scope: {
            task: '=theTask'
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
        return false;//$scope.$parent.$parent.editTaskId === $scope.task.id;
    };

    $scope.hideEditDuration = function () {
        return $scope.editTask.dType.id !== 'None' || $scope.editTask.dType.id === '';
    };

    $scope.hideEditFrequency = function () {
        return $scope.editTask.fType.id !== 'Once' || $scope.editTask.fType.id === '';
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

    $scope.isSomeoneElseInEditMode = function () {
        return $scope.$parent.$parent.editTaskId !== '' && $scope.$parent.$parent.editTaskId !== $scope.task.id;
    };

    $scope.clearEditData = function () {
        $scope.editTask = {};
        $scope.$parent.$parent.editTaskId = '';
        $scope.editForm.$hide();
    };

    $scope.saveEdit = function () {
        $scope.editForm.$submit();
        if ($scope.editForm.$dirty) {
            $scope.editForm.$show();
            return;
        }
        $scope.editTask.categoryId = $scope.editTask.category.id;
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

    $scope.startEdit = function () {
        for (var i = 0; i < $scope.$parent.tasks.length; i++) {
            if ($scope.$parent.tasks[i].id === $scope.task.id) {
                $scope.editTask = angular.copy($scope.$parent.tasks[i]);
                //$scope.$parent.$parent.editTaskId = $scope.task.id;
                $scope.editForm.$show();
                break;
            }
        }
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