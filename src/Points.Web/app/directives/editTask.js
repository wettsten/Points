'use strict';
app.directive('editTask', function () {
    return {
        scope: {
            task: '=theTask',
            taskInEdit: '=',
            addAlert: '&'
        },
        templateUrl: '/app/views/directives/editTask.html',
        replace: true,
        controller: 'editTaskController'
    };
}).controller('editTaskController', ['$scope', 'resourceService', '$uibModal', function ($scope, resourceService, $uibModal) {

    $scope.cats = [];
    $scope.editTask = {};

    $scope.isInEditMode = function () {
        return $scope.taskInEdit.id === $scope.task.id;
    };

    $scope.$watch('taskInEdit.id', function () {
        if ($scope.taskInEdit.id !== '' && $scope.taskInEdit.id !== $scope.task.id) {
            $scope.editTask = {};
        }
    });

    $scope.clearEditData = function () {
        $scope.editTask = {};
        $scope.taskInEdit.id = '';
    };

    var loadCats = function () {
        $scope.cats = resourceService.get('categories');
    };

    resourceService.registerForUpdates('categories', function (data) {
        $scope.cats = data;
    });

    $scope.startEdit = function () {
        $scope.editTask = angular.copy($scope.task);
        $scope.taskInEdit.id = $scope.task.id;
    };

    $scope.saveEdit = function () {
        resourceService.edit('tasks',$scope.editTask).then(
            function (response) {
                $scope.clearEditData();
                $scope.addAlert({ type: 'success', msg: 'Task successfully updated' });
            },
            function (err) {
                $scope.addAlert({ type: 'danger', msg: err.data.message });
            }
        );
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

        modalInstance.result.then(function (result) {
            if (result !== 'cancel') {
                resourceService.delete('tasks',$scope.task.id).then(
                    function (response) {
                        $scope.addAlert({ type: 'success', msg: 'Task successfully deleted' });
                    },
                    function (err) {
                        $scope.addAlert({ type: 'danger', msg: err.data.message });
                    }
                );
            }
        });
    };

    loadCats();
}]);