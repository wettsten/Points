'use strict';
app.directive('editTask', function () {
    return {
        scope: {
            task: '=theTask',
            cats: '='
        },
        templateUrl: '/app/views/directives/editTask.html',
        replace: true,
        controller: 'editTaskController'
    };
}).controller('editTaskController', ['$scope', 'tasksService', 'authService', '$uibModal', function ($scope, tasksService, authService, $uibModal) {

   $scope.editTask = {};

    $scope.isInEditMode = function () {
        return $scope.$parent.$parent.editTaskId === $scope.task.id;
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

    $scope.startEdit = function () {
        for (var i = 0; i < $scope.$parent.tasks.length; i++) {
            if ($scope.$parent.tasks[i].id === $scope.task.id) {
                $scope.editTask = angular.copy($scope.$parent.tasks[i]);
                $scope.$parent.$parent.editTaskId = $scope.task.id;
                $scope.editForm.$show();
                break;
            }
        }
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