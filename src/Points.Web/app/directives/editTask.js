'use strict';
app.directive('editTask', function () {
    return {
        scope: {
            task: '=theTask',
            cats: '=',
            taskInEdit: '='
        },
        templateUrl: '/app/views/directives/editTask.html',
        replace: true,
        controller: 'editTaskController'
    };
}).controller('editTaskController', ['$scope', 'tasksService', 'authService', '$uibModal', function ($scope, tasksService, authService, $uibModal) {

   $scope.editTask = {};

    $scope.isInEditMode = function () {
        return $scope.taskInEdit.id === $scope.task.id;
    };

    $scope.$watch('taskInEdit.id', function () {
        if ($scope.taskInEdit.id !== '' && $scope.taskInEdit.id !== $scope.task.id) {
            $scope.editTask = {};
            $scope.editForm.$hide();
        }
    });

    $scope.clearEditData = function () {
        $scope.editTask = {};
        $scope.taskInEdit.id = '';
        $scope.editForm.$hide();
    };

    $scope.startEdit = function () {
        $scope.editTask = angular.copy($scope.task);
        $scope.taskInEdit.id = $scope.task.id;
        $scope.editForm.$show();
    };

    $scope.saveEdit = function () {
        $scope.editForm.$submit();
        if ($scope.editForm.$dirty) {
            $scope.editForm.$show();
            return;
        }
        $scope.editTask.categoryId = $scope.editTask.category.id;
        $scope.editTask.userId = authService.authentication.userId;
        tasksService.editTask($scope.editTask).then(function (response) {
                $scope.clearEditData();
            $scope.$parent.loadTasks();
        },
         function (err) {
             $scope.$parent.$parent.$parent.message = err.data.message;
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
                tasksService.deleteTask($scope.task.id).then(
                    function (response) {
                        $scope.$parent.loadTasks();
                    },
                    function (err) {
                        $scope.$parent.$parent.$parent.message = err.data.message;
                    });
            }
        });
    };
}]);