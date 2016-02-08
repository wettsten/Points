'use strict';
app.directive('editPlanningTask', function () {
    return {
        scope: {
            task: '=theTask',
            taskInEdit: '=',
            addAlert: '&'
        },
        templateUrl: '/app/views/directives/editPlanningTask.html',
        replace: true,
        controller: 'editPlanningTaskController'
    };
}).controller('editPlanningTaskController', ['$scope', 'resourceService', '$uibModal', function ($scope, resourceService, $uibModal) {

    $scope.editTask = {};
    $scope.enums = {};

    var loadEnums = function () {
        $scope.enums = resourceService.get('enums');
    };

    resourceService.registerForUpdates('enums', function (data) {
        $scope.enums = data;
    });

    $scope.isInEditMode = function () {
        return $scope.taskInEdit.id === $scope.task.id;
    };

    $scope.$watch('taskInEdit.id', function () {
        if ($scope.taskInEdit.id !== '' && $scope.taskInEdit.id !== $scope.task.id) {
            $scope.editTask = {};
        }
    });

    $scope.ignoreDurationValueAndUnit = function () {
        if ($scope.isInEditMode()) {
            return $scope.editTask.duration.type.id === 'None';
        } else {
            return $scope.task.duration.type.id === 'None';
        }
    };

    $scope.ignoreFrequencyValueAndUnit = function () {
        if ($scope.isInEditMode()) {
            return $scope.editTask.frequency.type.id === 'Once';
        } else {
            return $scope.task.frequency.type.id === 'Once';
        }
    };

    $scope.clearEditData = function () {
        $scope.editTask = {};
        $scope.taskInEdit.id = '';
    };

    $scope.startEdit = function () {
        $scope.editTask = angular.copy($scope.task);
        $scope.taskInEdit.id = $scope.task.id;
    };

    $scope.disableEditSave = function () {
        if ($scope.isInEditMode()) {
            if ($scope.editTask.duration.type.id !== 'None') {
                if (!$scope.editTask.duration.value || $scope.editTask.duration.value < 1) {
                    return true;
                }
            }
            if ($scope.editTask.frequency.type.id !== 'Once') {
                if (!$scope.editTask.frequency.value || $scope.editTask.frequency.value < 1) {
                    return true;
                }
            }
        }
        return false;
    };

    $scope.saveEdit = function () {
        if ($scope.disableEditSave()) {
            return;
        }
        var eTask = angular.copy($scope.editTask);
        eTask.duration.type = $scope.editTask.duration.type.id;
        eTask.duration.unit = $scope.editTask.duration.unit.id;
        eTask.frequency.type = $scope.editTask.frequency.type.id;
        eTask.frequency.unit = $scope.editTask.frequency.unit.id;
        eTask.taskId = $scope.editTask.task.id;
        resourceService.edit('planningtasks',eTask).then(
            function (response) {
                $scope.clearEditData();
                $scope.$emit('refreshTasks');
                $scope.addAlert({ type: 'success', msg: 'Task successfully updated' });
            },
            function (err) {
                $scope.addAlert({ type: 'danger', msg: err.data.message });
        });
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
                        name: $scope.task.task.name,
                        id: $scope.task.id
                    };
                }
            }
        });

        modalInstance.result.then(
            function (result) {
                if (result !== 'cancel') {
                    resourceService.delete('planningtasks',$scope.task.id).then(
                        function (response) {
                            $scope.$emit('refreshTasks');
                            $scope.addAlert({ type: 'success', msg: 'Task successfully deleted' });
                        },
                        function (err) {
                            $scope.addAlert({ type: 'danger', msg: err.data.message });
                        });
                }
        });
    };

    loadEnums();
}]);